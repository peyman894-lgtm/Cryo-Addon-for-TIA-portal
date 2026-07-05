using Siemens.Engineering;
using Siemens.Engineering.HW;
using Siemens.Engineering.HW.Features;
using Siemens.Engineering.SW;
using Siemens.Engineering.SW.ExternalSources;
using Siemens.Engineering.Compiler;
using System;
using System.Collections.Generic;
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
        private List<TiaPortalProcess> tiaProcesses = new List<TiaPortalProcess>();
        private TiaPortal selectedTiaPortal;
        private Project selectedProject;
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

        private void btnNext_Click(object sender, EventArgs e)
        {
            tabMain.SelectedTab = tabTia;
            Log("Please open the TIA Portal project.");
        }

        private void btnLoadTiaInstances_Click(object sender, EventArgs e)
        {
            dgvTiaInstances.Rows.Clear();
            tiaProcesses.Clear();

            try
            {
                tiaProcesses = TiaPortal.GetProcesses().ToList();

                for (int i = 0; i < tiaProcesses.Count; i++)
                {
                    TiaPortalProcess process = tiaProcesses[i];

                    dgvTiaInstances.Rows.Add(
                        false,
                        i,
                        process.Id,
                        process.ProjectPath
                    );
                }

                Log("Found " + tiaProcesses.Count + " opened TIA Portal instance(s).");
            }
            catch (Exception ex)
            {
                Log("ERROR loading TIA instances: " + ex.Message);
            }
        }
        private void btnListPlcs_Click(object sender, EventArgs e)
        {
            dgvPlcs.Rows.Clear();

            try
            {
                int selectedIndex = GetSelectedTiaProcessIndex();

                if (selectedIndex < 0)
                {
                    Log("ERROR: Please select one TIA Portal instance.");
                    return;
                }

                selectedTiaPortal = tiaProcesses[selectedIndex].Attach();
                selectedProject = selectedTiaPortal.Projects.FirstOrDefault();

                if (selectedProject == null)
                {
                    Log("ERROR: No project is open in selected TIA Portal instance.");
                    return;
                }

                Log("Connected to project: " + selectedProject.Name);

                foreach (Device device in selectedProject.Devices)
                {
                    FindPlcsInDeviceItems(device.DeviceItems, device.Name);
                }

                Log("PLC list completed. Found " + dgvPlcs.Rows.Count + " PLC(s).");
            }
            catch (Exception ex)
            {
                Log("ERROR listing PLCs: " + ex.Message);
            }
        }
        private int GetSelectedTiaProcessIndex()
        {
            foreach (DataGridViewRow row in dgvTiaInstances.Rows)
            {
                bool selected = Convert.ToBoolean(row.Cells[0].Value);

                if (selected)
                {
                    return Convert.ToInt32(row.Cells[1].Value);
                }
            }

            return -1;
        }

        private void FindPlcsInDeviceItems(DeviceItemComposition deviceItems, string deviceName)
        {
            foreach (DeviceItem deviceItem in deviceItems)
            {
                SoftwareContainer softwareContainer = deviceItem.GetService<SoftwareContainer>();

                if (softwareContainer != null && softwareContainer.Software is PlcSoftware)
                {
                    int rowIndex = dgvPlcs.Rows.Add(
                        false,
                        deviceName,
                        deviceItem.Name
                    );

                    dgvPlcs.Rows[rowIndex].Tag = softwareContainer.Software;

                    Log("PLC found: " + deviceName + " / " + deviceItem.Name);
                }

                if (deviceItem.DeviceItems != null)
                {
                    FindPlcsInDeviceItems(deviceItem.DeviceItems, deviceName);
                }
            }
        }
        private async void btnImportExternalSource_Click(object sender, EventArgs e)
        {
            try
            {
                dgvSourceFiles.EndEdit();
                dgvPlcs.EndEdit();

                List<string> selectedSourceUrls = GetSelectedSourceFileUrls();
                List<PlcSoftware> selectedPlcs = GetSelectedPlcSoftwareList();

                if (selectedSourceUrls.Count == 0)
                {
                    Log("ERROR: Please select at least one source file from Git Repository tab.");
                    return;
                }

                if (selectedPlcs.Count == 0)
                {
                    Log("ERROR: Please select at least one PLC from TIA Portal tab.");
                    return;
                }

                
                string tempFolder = Path.Combine(Application.StartupPath, "Common Handlers");

                if (!Directory.Exists(tempFolder))
                {
                    Directory.CreateDirectory(tempFolder);
                }

                Log("Downloading selected source files to: " + tempFolder);

                List<string> localFiles = await DownloadSelectedSourceFiles(selectedSourceUrls, tempFolder);

                foreach (PlcSoftware plcSoftware in selectedPlcs)
                {
                    Log("Importing external sources to PLC: " + plcSoftware.Name);

                    foreach (string localFile in localFiles)
                    {
                        string sourceName = Path.GetFileNameWithoutExtension(localFile);

                        PlcExternalSource existingSource =
                            plcSoftware.ExternalSourceGroup.ExternalSources.Find(sourceName);

                        if (existingSource != null)
                        {
                            existingSource.Delete();
                            Log("Deleted old external source: " + sourceName);
                        }

                        plcSoftware.ExternalSourceGroup.ExternalSources.CreateFromFile(sourceName, localFile);

                        Log("Imported external source: " + Path.GetFileName(localFile));
                    }
                }

                Log("Import external source completed.");
            }
            catch (Exception ex)
            {
                Log("ERROR importing external source: " + ex.Message);
            }
        }

        private List<string> GetSelectedSourceNames()
        {
            List<string> selectedSourceNames = new List<string>();

            foreach (DataGridViewRow row in dgvSourceFiles.Rows)
            {
                bool selected = Convert.ToBoolean(row.Cells[0].Value);

                if (selected)
                {
                    string fileName = row.Cells[1].Value.ToString();
                    string sourceName = Path.GetFileNameWithoutExtension(fileName);

                    selectedSourceNames.Add(sourceName);
                }
            }

            return selectedSourceNames;
        }

        private void btnGenerateBlocks_Click(object sender, EventArgs e)
        {
            try
            {
                dgvSourceFiles.EndEdit();
                dgvPlcs.EndEdit();

                List<string> selectedSourceNames = GetSelectedSourceNames();
                List<PlcSoftware> selectedPlcs = GetSelectedPlcSoftwareList();

                if (selectedSourceNames.Count == 0)
                {
                    Log("ERROR: Please select at least one source file from Git Repository tab.");
                    return;
                }

                if (selectedPlcs.Count == 0)
                {
                    Log("ERROR: Please select at least one PLC from TIA Portal tab.");
                    return;
                }

                foreach (PlcSoftware plcSoftware in selectedPlcs)
                {
                    Log("Generating blocks for PLC: " + plcSoftware.Name);

                    foreach (string sourceName in selectedSourceNames)
                    {
                        PlcExternalSource externalSource =
                            plcSoftware.ExternalSourceGroup.ExternalSources.Find(sourceName);

                        if (externalSource == null)
                        {
                            Log("WARNING: External source not found in PLC " + plcSoftware.Name + ": " + sourceName);
                            continue;
                        }

                        try
                        {
                            IList<IEngineeringObject> generatedObjects =
                                externalSource.GenerateBlocksFromSource(GenerateBlockOption.KeepOnError);

                            Log("OK: Generated blocks from source: " + sourceName);
                            Log("Generated object count: " + generatedObjects.Count);
                        }
                        catch (Exception ex)
                        {
                            Log("ERROR: Failed to generate from source " + sourceName + ": " + ex.Message);
                        }
                    }
                }

                Log("Generate blocks completed.");
            }
            catch (Exception ex)
            {
                Log("ERROR generating blocks: " + ex.Message);
            }
        }

        private void CompileSelectedPlcs(string compileMode)
        {
            try
            {
                dgvPlcs.EndEdit();

                List<PlcSoftware> selectedPlcs = GetSelectedPlcSoftwareList();

                if (selectedPlcs.Count == 0)
                {
                    Log("ERROR: Please select at least one PLC.");
                    return;
                }

                Log("Compile started. Mode: " + compileMode);

                foreach (PlcSoftware plcSoftware in selectedPlcs)
                {
                    try
                    {
                        Log("Compiling PLC: " + plcSoftware.Name);

                        ICompilable compilable = plcSoftware.GetService<ICompilable>();

                        if (compilable == null)
                        {
                            Log("ERROR: Compile service not available for PLC: " + plcSoftware.Name);
                            continue;
                        }

                        CompilerResult result = compilable.Compile();

                        Log("Compile result for PLC " + plcSoftware.Name + ": " + result.State);
                        Log("Warnings: " + result.WarningCount);
                        Log("Errors: " + result.ErrorCount);

                        LogCompilerMessages(result.Messages);
                    }
                    catch (Exception ex)
                    {
                        Log("ERROR compiling PLC " + plcSoftware.Name + ": " + ex.Message);
                    }
                }

                Log("Compile completed. Mode: " + compileMode);
            }
            catch (Exception ex)
            {
                Log("ERROR during compile: " + ex.Message);
            }
        }

        private void LogCompilerMessages(CompilerResultMessageComposition messages)
        {
            foreach (CompilerResultMessage message in messages)
            {
                Log(
                    message.State +
                    " | Errors: " + message.ErrorCount +
                    " | Warnings: " + message.WarningCount +
                    " | " + message.Description
                );

                if (!string.IsNullOrWhiteSpace(message.Path))
                {
                    Log("Path: " + message.Path);
                }

                if (message.Messages != null && message.Messages.Count > 0)
                {
                    LogCompilerMessages(message.Messages);
                }
            }
        }

        private void btnCompileChanges_Click(object sender, EventArgs e)
        {
            CompileSelectedPlcs("software only changes");
        }

        private void btnCompileAll_Click(object sender, EventArgs e)
        {
            CompileSelectedPlcs("software all");
        }

        private List<string> GetSelectedSourceFileUrls()
{
    List<string> selectedFiles = new List<string>();

    foreach (DataGridViewRow row in dgvSourceFiles.Rows)
    {
        bool selected = Convert.ToBoolean(row.Cells[0].Value);

        if (selected)
        {
            string fileUrl = row.Cells[3].Value.ToString();
            selectedFiles.Add(fileUrl);
        }
    }

    return selectedFiles;
}

private List<PlcSoftware> GetSelectedPlcSoftwareList()
{
    List<PlcSoftware> selectedPlcs = new List<PlcSoftware>();

    foreach (DataGridViewRow row in dgvPlcs.Rows)
    {
        bool selected = Convert.ToBoolean(row.Cells[0].Value);

        if (selected && row.Tag is PlcSoftware)
        {
            selectedPlcs.Add((PlcSoftware)row.Tag);
        }
    }

    return selectedPlcs;
}

private async Task<List<string>> DownloadSelectedSourceFiles(List<string> sourceUrls, string tempFolder)
{
    List<string> localFiles = new List<string>();

    using (HttpClient client = new HttpClient())
    {
        client.DefaultRequestHeaders.UserAgent.ParseAdd("TiaSourceUpdater");

        foreach (string sourceUrl in sourceUrls)
        {
            string fileName = Path.GetFileName(new Uri(sourceUrl).LocalPath);
            string localFile = Path.Combine(tempFolder, fileName);

            byte[] fileBytes = await client.GetByteArrayAsync(sourceUrl);

            File.WriteAllBytes(localFile, fileBytes);

            localFiles.Add(localFile);

            Log("Downloaded: " + fileName);
        }
    }

    return localFiles;
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
            string logLine = DateTime.Now.ToString("HH:mm:ss") + "  " + message + Environment.NewLine;

            if (txtLog != null)
            {
                txtLog.AppendText(logLine);
            }

            if (txtTiaLog != null)
            {
                txtTiaLog.AppendText(logLine);
            }
        }
    }

    public class GitHubRepositoryInfo
    {
        public string Owner { get; set; }
        public string Name { get; set; }
    }
}