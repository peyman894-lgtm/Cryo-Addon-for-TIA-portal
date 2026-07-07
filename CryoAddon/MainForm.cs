using Siemens.Engineering;
using Siemens.Engineering.Compiler;
using Siemens.Engineering.HW;
using Siemens.Engineering.HW.Features;
using Siemens.Engineering.SW;
using Siemens.Engineering.SW.ExternalSources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly List<TiaPortalProcess> tiaProcesses = new List<TiaPortalProcess>();

        private TiaPortal selectedTiaPortal;
        private Project selectedProject;

        public MainForm()
        {
            InitializeComponent();
            txtIniFile.Text = Path.Combine(Application.StartupPath, "settings.ini");
        }

        // ============================================================
        // Home / Navigation
        // ============================================================

        private void btnOpenUpdateHandlers_Click(object sender, EventArgs e)
        {
            tabMain.SelectedTab = tabGit;
            Log("Update Handlers workflow selected.");
        }

        private void btnOpenUpdateEpics_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Update EPICS Devices is planned for Phase 2 and is not fully implemented yet.",
                "Phase 2",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            Log("Update EPICS Devices selected.");
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            tabMain.SelectedTab = tabTia;
            Log("Please open the TIA Portal project.");
        }

        // ============================================================
        // Common Git Repository - Update Handlers
        // ============================================================

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

            string repositoryUrl = ReadCommonRepositoryFromIni(iniPath);

            if (string.IsNullOrWhiteSpace(repositoryUrl))
            {
                Log("ERROR: RepositoryPath or RepositoryUrl not found in [Common].");
                return;
            }

            lblRepositoryPath.Text = repositoryUrl;
            Log("Common repository: " + repositoryUrl);

            try
            {
                await LoadFilesFromGitHub(repositoryUrl);
            }
            catch (Exception ex)
            {
                Log("ERROR loading common repository files: " + ex.Message);
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

                LogValueIfAvailable("Latest commit SHA", GetJsonValue(commitJson, "sha"));
                LogValueIfAvailable("Latest commit message", GetNestedCommitMessage(commitJson));
                LogValueIfAvailable("Latest commit author", GetNestedCommitAuthor(commitJson));

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

                    if (extension != ".db" && extension != ".scl" && extension != ".udt")
                    {
                        continue;
                    }

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

                Log("Number of files found: " + totalCount);
                Log("Number of .scl files: " + sclCount);
                Log("Number of .db files: " + dbCount);
                Log("Number of .udt files: " + udtCount);
            }
        }

        // ============================================================
        // TIA Portal Connection / PLC Discovery
        // ============================================================

        private void btnLoadTiaInstances_Click(object sender, EventArgs e)
        {
            dgvTiaInstances.Rows.Clear();
            tiaProcesses.Clear();

            try
            {
                tiaProcesses.AddRange(TiaPortal.GetProcesses());

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
                    SearchDeviceForPlcs(device, "Project root");
                }

                foreach (DeviceUserGroup group in selectedProject.DeviceGroups)
                {
                    SearchDeviceGroupForPlcs(group, group.Name);
                }

                Log("PLC list completed. Found " + dgvPlcs.Rows.Count + " PLC(s).");
            }
            catch (Exception ex)
            {
                Log("ERROR listing PLCs: " + ex.Message);
            }
        }

        private void SearchDeviceGroupForPlcs(DeviceUserGroup group, string groupPath)
        {
            Log("Searching group: " + groupPath);

            foreach (Device device in group.Devices)
            {
                SearchDeviceForPlcs(device, groupPath);
            }

            foreach (DeviceUserGroup childGroup in group.Groups)
            {
                SearchDeviceGroupForPlcs(childGroup, groupPath + "\\" + childGroup.Name);
            }
        }

        private void SearchDeviceForPlcs(Device device, string groupPath)
        {
            Log("Searching device: " + groupPath + " / " + device.Name);
            FindPlcsInDeviceItems(device.DeviceItems, device.Name, groupPath);
        }

        private void FindPlcsInDeviceItems(DeviceItemComposition deviceItems, string deviceName, string groupPath)
        {
            foreach (DeviceItem deviceItem in deviceItems)
            {
                SoftwareContainer softwareContainer = deviceItem.GetService<SoftwareContainer>();

                if (softwareContainer != null && softwareContainer.Software is PlcSoftware)
                {
                    PlcSoftware plcSoftware = (PlcSoftware)softwareContainer.Software;

                    int rowIndex = dgvPlcs.Rows.Add(
                        false,
                        groupPath + " / " + deviceName,
                        deviceItem.Name
                    );

                    dgvPlcs.Rows[rowIndex].Tag = plcSoftware;

                    Log("PLC found: " + groupPath + " / " + deviceName + " / " + deviceItem.Name);
                }

                if (deviceItem.DeviceItems != null)
                {
                    FindPlcsInDeviceItems(deviceItem.DeviceItems, deviceName, groupPath);
                }
            }
        }

        // ============================================================
        // Import Common Handler Sources
        // ============================================================

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
                    ImportLocalFilesToPlcExternalSources(plcSoftware, localFiles, "handler");
                }

                Log("Import external source completed.");
            }
            catch (Exception ex)
            {
                Log("ERROR importing external source: " + ex.Message);
            }
        }

        // ============================================================
        // Import EPICS Sources - Git Clone / Pull Approach
        // ============================================================

        private List<string> CloneOrUpdateOnlyEpicsFiles(string repositoryUrl, string localRepositoryFolder)
        {
            string parentFolder = Path.GetDirectoryName(localRepositoryFolder);

            if (!Directory.Exists(parentFolder))
            {
                Directory.CreateDirectory(parentFolder);
            }

            string file1 = "PLC/PLCIntegrator_external_source_standard_TIAv17.scl";
            string file2 = "PLC/PLCIntegrator_external_source_TIAv17.scl";

            if (!Directory.Exists(localRepositoryFolder))
            {
                Log("Cloning only needed EPICS files to: " + localRepositoryFolder);

                RunGitCommand(
                    "clone --filter=blob:none --depth 1 --sparse \"" + repositoryUrl + "\" \"" + localRepositoryFolder + "\"",
                    parentFolder
                );

                RunGitCommand(
                    "sparse-checkout set \"" + file1 + "\" \"" + file2 + "\"",
                    localRepositoryFolder
                );

                Log("Sparse clone completed.");
            }
            else
            {
                Log("Updating only needed EPICS files in: " + localRepositoryFolder);

                RunGitCommand(
                    "sparse-checkout set \"" + file1 + "\" \"" + file2 + "\"",
                    localRepositoryFolder
                );

                RunGitCommand(
                    "pull --depth 1",
                    localRepositoryFolder
                );

                Log("Sparse pull completed.");
            }

            List<string> files = new List<string>();

            string localFile1 = Path.Combine(localRepositoryFolder, "PLC", "PLCIntegrator_external_source_standard_TIAv17.scl");
            string localFile2 = Path.Combine(localRepositoryFolder, "PLC", "PLCIntegrator_external_source_TIAv17.scl");

            if (File.Exists(localFile1))
            {
                files.Add(localFile1);
                Log("Found EPICS file: " + localFile1);
            }
            else
            {
                Log("WARNING: Missing EPICS file: " + localFile1);
            }

            if (File.Exists(localFile2))
            {
                files.Add(localFile2);
                Log("Found EPICS file: " + localFile2);
            }
            else
            {
                Log("WARNING: Missing EPICS file: " + localFile2);
            }

            return files;
        }


        private void btnImportEpicsSource_Click(object sender, EventArgs e)
        {
            try
            {
                dgvPlcs.EndEdit();

                string iniPath = txtIniFile.Text.Trim();

                if (!File.Exists(iniPath))
                {
                    Log("ERROR: settings.ini file not found.");
                    return;
                }

                Dictionary<string, string> plcRepositories = ReadPlcRepositoriesFromIni(iniPath);
                List<DataGridViewRow> selectedPlcRows = GetSelectedPlcRows();

                if (selectedPlcRows.Count == 0)
                {
                    Log("ERROR: Please select at least one PLC.");
                    return;
                }

                foreach (DataGridViewRow row in selectedPlcRows)
                {
                    string plcName = Convert.ToString(row.Cells[2].Value);
                    PlcSoftware plcSoftware = row.Tag as PlcSoftware;

                    if (plcSoftware == null)
                    {
                        Log("ERROR: PLC software object not found for: " + plcName);
                        continue;
                    }

                    if (!plcRepositories.ContainsKey(plcName))
                    {
                        Log("ERROR: No dedicated repository found in [PLC_REPOSITORIES] for PLC: " + plcName);
                        continue;
                    }

                    string repositoryUrl = plcRepositories[plcName];

                    Log("EPICS import started for PLC: " + plcName);
                    Log("Dedicated repository: " + repositoryUrl);

                    string safePlcName = MakeSafeFolderName(plcName);

                    string plcTempFolder = Path.Combine(
                        Application.StartupPath,
                        "TempSources",
                        safePlcName
                    );

                    string localRepositoryFolder = Path.Combine(plcTempFolder, "Repository");

                    List<string> sclFiles = CloneOrUpdateOnlyEpicsFiles(repositoryUrl, localRepositoryFolder);

                    if (sclFiles.Count == 0)
                    {
                        Log("WARNING: No .scl files found in: " + plcName);
                        continue;
                    }

                    Log("Found " + sclFiles.Count + " EPICS .scl file(s) for PLC: " + plcName);

                    foreach (string sclFile in sclFiles)
                    {
                        Log("EPICS source file: " + sclFile);
                    }

                    ImportLocalFilesToPlcExternalSources(plcSoftware, sclFiles, "EPICS");

                    Log("EPICS import completed for PLC: " + plcName);
                }
            }
            catch (Exception ex)
            {
                Log("ERROR importing EPICS source: " + ex.Message);
            }
        }

        private void CloneOrPullRepository(string repositoryUrl, string localRepositoryFolder)
        {
            string parentFolder = Path.GetDirectoryName(localRepositoryFolder);

            if (!Directory.Exists(parentFolder))
            {
                Directory.CreateDirectory(parentFolder);
            }

            if (!Directory.Exists(localRepositoryFolder))
            {
                Log("Cloning repository to: " + localRepositoryFolder);

                RunGitCommand(
                    "clone \"" + repositoryUrl + "\" \"" + localRepositoryFolder + "\"",
                    parentFolder
                );

                Log("Git clone completed.");
                return;
            }

            string gitFolder = Path.Combine(localRepositoryFolder, ".git");

            if (!Directory.Exists(gitFolder))
            {
                throw new Exception("Local repository folder exists but is not a Git repository: " + localRepositoryFolder);
            }

            Log("Repository already exists. Running git pull: " + localRepositoryFolder);

            RunGitCommand("pull", localRepositoryFolder);

            Log("Git pull completed.");
        }

        private void RunGitCommand(string arguments, string workingDirectory)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "git";
            startInfo.Arguments = arguments;
            startInfo.WorkingDirectory = workingDirectory;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;

            using (Process process = new Process())
            {
                process.StartInfo = startInfo;

                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                if (!string.IsNullOrWhiteSpace(output))
                {
                    Log(output.Trim());
                }

                if (!string.IsNullOrWhiteSpace(error))
                {
                    Log(error.Trim());
                }

                if (process.ExitCode != 0)
                {
                    throw new Exception("Git command failed. Exit code: " + process.ExitCode);
                }
            }
        }

        // ============================================================
        // Generate Blocks
        // ============================================================

        private void btnGenerateBlocks_Click(object sender, EventArgs e)
        {
            try
            {
                dgvPlcs.EndEdit();

                List<PlcSoftware> selectedPlcs = GetSelectedPlcSoftwareList();

                if (selectedPlcs.Count == 0)
                {
                    Log("ERROR: Please select at least one PLC from TIA Portal tab.");
                    return;
                }

                foreach (PlcSoftware plcSoftware in selectedPlcs)
                {
                    Log("Generating blocks from all external sources for PLC: " + plcSoftware.Name);

                    int sourceCount = 0;
                    int successCount = 0;
                    int errorCount = 0;

                    foreach (PlcExternalSource externalSource in plcSoftware.ExternalSourceGroup.ExternalSources)
                    {
                        sourceCount++;

                        try
                        {
                            Log("Generating from external source: " + externalSource.Name);

                            IList<IEngineeringObject> generatedObjects =
                                externalSource.GenerateBlocksFromSource(GenerateBlockOption.KeepOnError);

                            successCount++;

                            Log("OK: Generated from source: " + externalSource.Name);
                            Log("Generated object count: " + generatedObjects.Count);
                        }
                        catch (Exception ex)
                        {
                            errorCount++;
                            Log("ERROR: Failed to generate from source " + externalSource.Name + ": " + ex.Message);
                        }
                    }

                    if (sourceCount == 0)
                    {
                        Log("WARNING: No external sources found in PLC: " + plcSoftware.Name);
                    }

                    Log(
                        "Generate summary for PLC " + plcSoftware.Name +
                        " | Sources: " + sourceCount +
                        " | OK: " + successCount +
                        " | Errors: " + errorCount
                    );
                }

                Log("Generate blocks completed.");
            }
            catch (Exception ex)
            {
                Log("ERROR generating blocks: " + ex.Message);
            }
        }

        // ============================================================
        // Compile
        // ============================================================

        private void btnCompileChanges_Click(object sender, EventArgs e)
        {
            CompileSelectedPlcs("software only changes");
        }

        private void btnCompileAll_Click(object sender, EventArgs e)
        {
            CompileSelectedPlcs("software all");
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

        // ============================================================
        // Import Helpers
        // ============================================================

        private void ImportLocalFilesToPlcExternalSources(
            PlcSoftware plcSoftware,
            List<string> localFiles,
            string sourceType
        )
        {
            Log("Importing " + sourceType + " external sources to PLC: " + plcSoftware.Name);

            foreach (string localFile in localFiles)
            {
                string sourceName = Path.GetFileNameWithoutExtension(localFile);

                PlcExternalSource existingSource =
                    plcSoftware.ExternalSourceGroup.ExternalSources.Find(sourceName);

                if (existingSource != null)
                {
                    existingSource.Delete();
                    Log("Deleted old " + sourceType + " external source: " + sourceName);
                }

                plcSoftware.ExternalSourceGroup.ExternalSources.CreateFromFile(sourceName, localFile);

                Log("Imported " + sourceType + " source: " + Path.GetFileName(localFile));
            }
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

        // ============================================================
        // Selection Helpers
        // ============================================================

        private int GetSelectedTiaProcessIndex()
        {
            foreach (DataGridViewRow row in dgvTiaInstances.Rows)
            {
                if (IsRowChecked(row))
                {
                    return Convert.ToInt32(row.Cells[1].Value);
                }
            }

            return -1;
        }

        private List<string> GetSelectedSourceFileUrls()
        {
            List<string> selectedFiles = new List<string>();

            foreach (DataGridViewRow row in dgvSourceFiles.Rows)
            {
                if (IsRowChecked(row))
                {
                    selectedFiles.Add(Convert.ToString(row.Cells[3].Value));
                }
            }

            return selectedFiles;
        }

        private List<string> GetSelectedSourceNames()
        {
            List<string> selectedSourceNames = new List<string>();

            foreach (DataGridViewRow row in dgvSourceFiles.Rows)
            {
                if (IsRowChecked(row))
                {
                    string fileName = Convert.ToString(row.Cells[1].Value);
                    selectedSourceNames.Add(Path.GetFileNameWithoutExtension(fileName));
                }
            }

            return selectedSourceNames;
        }

        private List<PlcSoftware> GetSelectedPlcSoftwareList()
        {
            List<PlcSoftware> selectedPlcs = new List<PlcSoftware>();

            foreach (DataGridViewRow row in dgvPlcs.Rows)
            {
                if (IsRowChecked(row) && row.Tag is PlcSoftware)
                {
                    selectedPlcs.Add((PlcSoftware)row.Tag);
                }
            }

            return selectedPlcs;
        }

        private List<DataGridViewRow> GetSelectedPlcRows()
        {
            List<DataGridViewRow> selectedRows = new List<DataGridViewRow>();

            foreach (DataGridViewRow row in dgvPlcs.Rows)
            {
                if (IsRowChecked(row))
                {
                    selectedRows.Add(row);
                }
            }

            return selectedRows;
        }

        private bool IsRowChecked(DataGridViewRow row)
        {
            if (row == null || row.IsNewRow || row.Cells.Count == 0)
            {
                return false;
            }

            object value = row.Cells[0].Value;

            return value != null && Convert.ToBoolean(value);
        }

        // ============================================================
        // INI Helpers
        // ============================================================

        private string ReadCommonRepositoryFromIni(string iniPath)
        {
            string repositoryPath = ReadValueFromIni(iniPath, "Common", "RepositoryPath");

            if (!string.IsNullOrWhiteSpace(repositoryPath))
            {
                return repositoryPath;
            }

            return ReadValueFromIni(iniPath, "Common", "RepositoryUrl");
        }

        private Dictionary<string, string> ReadPlcRepositoriesFromIni(string iniPath)
        {
            Dictionary<string, string> repositories = new Dictionary<string, string>();
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
                    insideSection = line.Equals("[PLC_REPOSITORIES]", StringComparison.OrdinalIgnoreCase);
                    continue;
                }

                if (insideSection && line.Contains("="))
                {
                    string[] parts = line.Split(new char[] { '=' }, 2);

                    string plcName = parts[0].Trim();
                    string repositoryUrl = parts[1].Trim();

                    repositories[plcName] = repositoryUrl;
                }
            }

            return repositories;
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

        // ============================================================
        // Small Utility Helpers
        // ============================================================

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

        private string MakeSafeFolderName(string name)
        {
            foreach (char invalidChar in Path.GetInvalidFileNameChars())
            {
                name = name.Replace(invalidChar, '_');
            }

            return name;
        }

        private void LogValueIfAvailable(string title, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                Log(title + ": " + value);
            }
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