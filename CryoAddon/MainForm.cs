using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CryoAddon
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            txtIniFile.Text = Path.Combine(Application.StartupPath, "settings.ini");
        }

        private void btnBrowseIni_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "INI files (*.ini)|*.ini|All files (*.*)|*.*";
            dialog.Title = "Select settings.ini";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtIniFile.Text = dialog.FileName;
            }
        }

        private async void btnLoadFiles_Click(object sender, EventArgs e)
        {
            dgvSourceFiles.Rows.Clear();
            txtLog.Clear();
            lblRepositoryPath.Text = "-";

            string iniPath = txtIniFile.Text.Trim();

            if (!File.Exists(iniPath))
            {
                Log("ERROR: settings.ini file not found.");
                return;
            }

            string repositoryUrl = ReadValueFromIni(iniPath, "Common", "RepositoryUrl");

            if (string.IsNullOrWhiteSpace(repositoryUrl))
            {
                Log("ERROR: RepositoryUrl not found in settings.ini.");
                return;
            }

            lblRepositoryPath.Text = repositoryUrl;
            Log("GitHub repository: " + repositoryUrl);

            try
            {
                await LoadFilesFromGitHub(repositoryUrl);
            }
            catch (Exception ex)
            {
                Log("ERROR: " + ex.Message);
            }
        }

        private async Task LoadFilesFromGitHub(string repositoryUrl)
        {
            GitHubRepositoryInfo repo = ParseGitHubRepositoryUrl(repositoryUrl);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("TiaSourceUpdater");

                Log("Connection status: Connecting to GitHub...");

                string repoApiUrl = "https://api.github.com/repos/" + repo.Owner + "/" + repo.Name;
                string repoJson = await client.GetStringAsync(repoApiUrl);

                Log("Connection status: OK");

                string defaultBranch = GetJsonValue(repoJson, "default_branch");
                string lastUpdatedTime = GetJsonValue(repoJson, "updated_at");

                if (string.IsNullOrWhiteSpace(defaultBranch))
                {
                    defaultBranch = "main";
                }

                Log("Repository URL: " + repositoryUrl);
                Log("Repository owner: " + repo.Owner);
                Log("Repository name: " + repo.Name);
                Log("Default branch: " + defaultBranch);

                if (!string.IsNullOrWhiteSpace(lastUpdatedTime))
                {
                    Log("Last updated time: " + lastUpdatedTime);
                }

                string commitApiUrl = repoApiUrl + "/commits/" + defaultBranch;
                string commitJson = await client.GetStringAsync(commitApiUrl);

                string latestCommitSha = GetJsonValue(commitJson, "sha");
                string latestCommitMessage = GetNestedCommitMessage(commitJson);
                string latestCommitAuthor = GetNestedCommitAuthor(commitJson);

                if (!string.IsNullOrWhiteSpace(latestCommitSha))
                {
                    Log("Latest commit SHA: " + latestCommitSha);
                }

                if (!string.IsNullOrWhiteSpace(latestCommitMessage))
                {
                    Log("Latest commit message: " + latestCommitMessage);
                }

                if (!string.IsNullOrWhiteSpace(latestCommitAuthor))
                {
                    Log("Latest commit author: " + latestCommitAuthor);
                }

                string treeApiUrl = repoApiUrl + "/git/trees/" + defaultBranch + "?recursive=1";
                string treeJson = await client.GetStringAsync(treeApiUrl);

                MatchCollection matches = Regex.Matches(
                    treeJson,
                    "\"path\"\\s*:\\s*\"([^\"]+)\"",
                    RegexOptions.IgnoreCase
                );

                int totalCount = 0;
                int sclCount = 0;
                int dbCount = 0;
                int udtCount = 0;

                Log("File paths found:");

                foreach (Match match in matches)
                {
                    string filePath = match.Groups[1].Value.Replace("\\/", "/");
                    string extension = Path.GetExtension(filePath).ToLower();

                    if (extension == ".db" || extension == ".scl" || extension == ".udt")
                    {
                        string rawUrl =
                            "https://raw.githubusercontent.com/" +
                            repo.Owner + "/" +
                            repo.Name + "/" +
                            defaultBranch + "/" +
                            filePath;

                        dgvSourceFiles.Rows.Add(
                            false,
                            Path.GetFileName(filePath),
                            extension,
                            rawUrl
                        );

                        Log("  " + filePath);

                        totalCount++;

                        if (extension == ".scl")
                        {
                            sclCount++;
                        }
                        else if (extension == ".db")
                        {
                            dbCount++;
                        }
                        else if (extension == ".udt")
                        {
                            udtCount++;
                        }
                    }
                }

                Log("Number of files found: " + totalCount);
                Log("Number of .scl files: " + sclCount);
                Log("Number of .db files: " + dbCount);
                Log("Number of .udt files: " + udtCount);
            }
        }

        private GitHubRepositoryInfo ParseGitHubRepositoryUrl(string repositoryUrl)
        {
            Uri uri = new Uri(repositoryUrl);

            string[] parts = uri.AbsolutePath.Trim('/').Split('/');

            if (parts.Length < 2)
            {
                throw new Exception("Invalid GitHub repository URL.");
            }

            return new GitHubRepositoryInfo
            {
                Owner = parts[0],
                Name = parts[1].Replace(".git", "")
            };
        }

        private string ReadValueFromIni(string iniPath, string sectionName, string keyName)
        {
            bool insideSection = false;

            foreach (string rawLine in File.ReadAllLines(iniPath))
            {
                string line = rawLine.Trim();

                if (line == "" || line.StartsWith(";") || line.StartsWith("#"))
                {
                    continue;
                }

                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    string currentSection = line.Trim('[', ']');
                    insideSection = currentSection.Equals(sectionName, StringComparison.OrdinalIgnoreCase);
                    continue;
                }

                if (insideSection && line.StartsWith(keyName + "=", StringComparison.OrdinalIgnoreCase))
                {
                    return line.Substring((keyName + "=").Length).Trim();
                }
            }

            return "";
        }

        private string GetJsonValue(string json, string key)
        {
            Match match = Regex.Match(
                json,
                "\"" + key + "\"\\s*:\\s*\"([^\"]+)\"",
                RegexOptions.IgnoreCase
            );

            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return "";
        }

        private string GetNestedCommitMessage(string json)
        {
            Match match = Regex.Match(
                json,
                "\"message\"\\s*:\\s*\"([^\"]+)\"",
                RegexOptions.IgnoreCase
            );

            if (match.Success)
            {
                return match.Groups[1].Value.Replace("\\n", " ");
            }

            return "";
        }

        private string GetNestedCommitAuthor(string json)
        {
            Match match = Regex.Match(
                json,
                "\"author\"\\s*:\\s*\\{[^\\}]*\"name\"\\s*:\\s*\"([^\"]+)\"",
                RegexOptions.IgnoreCase
            );

            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return "";
        }

        private void Log(string message)
        {
            txtLog.AppendText(DateTime.Now.ToString("HH:mm:ss") + "  " + message + Environment.NewLine);
        }
    }

    public class GitHubRepositoryInfo
    {
        public string Owner { get; set; }
        public string Name { get; set; }
    }
}