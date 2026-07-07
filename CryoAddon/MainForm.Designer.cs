namespace CryoAddon
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabGit;
        private System.Windows.Forms.TabPage tabTia;

        private System.Windows.Forms.TabPage tabHome;
        private System.Windows.Forms.Button btnOpenUpdateHandlers;
        private System.Windows.Forms.Button btnOpenUpdateEpics;
        private System.Windows.Forms.Label lblHomeTitle;

        private System.Windows.Forms.Label lblIniFile;
        private System.Windows.Forms.TextBox txtIniFile;
        private System.Windows.Forms.Button btnBrowseIni;
        private System.Windows.Forms.Button btnLoadFiles;
        private System.Windows.Forms.Label lblRepositoryTitle;
        private System.Windows.Forms.Label lblRepositoryPath;
        private System.Windows.Forms.DataGridView dgvSourceFiles;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colExtension;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFullPath;
        private System.Windows.Forms.Label lblLog;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnNext;

        private System.Windows.Forms.Label lblOpenTia;
        private System.Windows.Forms.Button btnLoadTiaInstances;
        private System.Windows.Forms.DataGridView dgvTiaInstances;
        private System.Windows.Forms.Button btnListPlcs;
        private System.Windows.Forms.DataGridView dgvPlcs;

        private System.Windows.Forms.Button btnImportExternalSource;
        private System.Windows.Forms.Button btnGenerateBlocks;
        private System.Windows.Forms.Button btnCompileChanges;
        private System.Windows.Forms.Button btnCompileAll;
        private System.Windows.Forms.Button btnImportEpicsSource;

        private System.Windows.Forms.Label lblTiaLog;
        private System.Windows.Forms.TextBox txtTiaLog;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabHome = new System.Windows.Forms.TabPage();
            this.lblHomeTitle = new System.Windows.Forms.Label();
            this.btnOpenUpdateHandlers = new System.Windows.Forms.Button();
            this.btnOpenUpdateEpics = new System.Windows.Forms.Button();
            this.tabGit = new System.Windows.Forms.TabPage();
            this.lblIniFile = new System.Windows.Forms.Label();
            this.txtIniFile = new System.Windows.Forms.TextBox();
            this.btnBrowseIni = new System.Windows.Forms.Button();
            this.btnLoadFiles = new System.Windows.Forms.Button();
            this.lblRepositoryTitle = new System.Windows.Forms.Label();
            this.lblRepositoryPath = new System.Windows.Forms.Label();
            this.dgvSourceFiles = new System.Windows.Forms.DataGridView();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colExtension = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFullPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblLog = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnNext = new System.Windows.Forms.Button();
            this.tabTia = new System.Windows.Forms.TabPage();
            this.lblOpenTia = new System.Windows.Forms.Label();
            this.btnLoadTiaInstances = new System.Windows.Forms.Button();
            this.dgvTiaInstances = new System.Windows.Forms.DataGridView();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnListPlcs = new System.Windows.Forms.Button();
            this.btnImportExternalSource = new System.Windows.Forms.Button();
            this.btnGenerateBlocks = new System.Windows.Forms.Button();
            this.btnCompileChanges = new System.Windows.Forms.Button();
            this.btnCompileAll = new System.Windows.Forms.Button();
            this.btnImportEpicsSource = new System.Windows.Forms.Button();
            this.dgvPlcs = new System.Windows.Forms.DataGridView();
            this.dataGridViewCheckBoxColumn2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblTiaLog = new System.Windows.Forms.Label();
            this.txtTiaLog = new System.Windows.Forms.TextBox();
            this.tabMain.SuspendLayout();
            this.tabHome.SuspendLayout();
            this.tabGit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSourceFiles)).BeginInit();
            this.tabTia.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTiaInstances)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlcs)).BeginInit();
            this.SuspendLayout();
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tabHome);
            this.tabMain.Controls.Add(this.tabGit);
            this.tabMain.Controls.Add(this.tabTia);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Location = new System.Drawing.Point(0, 0);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(980, 820);
            this.tabMain.TabIndex = 0;
            // 
            // tabHome
            // 
            this.tabHome.Controls.Add(this.lblHomeTitle);
            this.tabHome.Controls.Add(this.btnOpenUpdateHandlers);
            this.tabHome.Controls.Add(this.btnOpenUpdateEpics);
            this.tabHome.Location = new System.Drawing.Point(4, 25);
            this.tabHome.Name = "tabHome";
            this.tabHome.Size = new System.Drawing.Size(972, 791);
            this.tabHome.TabIndex = 0;
            this.tabHome.Text = "Home";
            this.tabHome.UseVisualStyleBackColor = true;
            // 
            // lblHomeTitle
            // 
            this.lblHomeTitle.AutoSize = true;
            this.lblHomeTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblHomeTitle.Location = new System.Drawing.Point(40, 40);
            this.lblHomeTitle.Name = "lblHomeTitle";
            this.lblHomeTitle.Size = new System.Drawing.Size(243, 29);
            this.lblHomeTitle.TabIndex = 0;
            this.lblHomeTitle.Text = "TIA Source Updater";
            // 
            // btnOpenUpdateHandlers
            // 
            this.btnOpenUpdateHandlers.Location = new System.Drawing.Point(45, 110);
            this.btnOpenUpdateHandlers.Name = "btnOpenUpdateHandlers";
            this.btnOpenUpdateHandlers.Size = new System.Drawing.Size(260, 55);
            this.btnOpenUpdateHandlers.TabIndex = 1;
            this.btnOpenUpdateHandlers.Text = "Update Handlers";
            this.btnOpenUpdateHandlers.UseVisualStyleBackColor = true;
            this.btnOpenUpdateHandlers.Click += new System.EventHandler(this.btnOpenUpdateHandlers_Click);
            // 
            // btnOpenUpdateEpics
            // 
            this.btnOpenUpdateEpics.Location = new System.Drawing.Point(45, 185);
            this.btnOpenUpdateEpics.Name = "btnOpenUpdateEpics";
            this.btnOpenUpdateEpics.Size = new System.Drawing.Size(260, 55);
            this.btnOpenUpdateEpics.TabIndex = 2;
            this.btnOpenUpdateEpics.Text = "Update EPICS Devices";
            this.btnOpenUpdateEpics.UseVisualStyleBackColor = true;
            this.btnOpenUpdateEpics.Click += new System.EventHandler(this.btnOpenUpdateEpics_Click);
            // 
            // tabGit
            // 
            this.tabGit.Controls.Add(this.lblIniFile);
            this.tabGit.Controls.Add(this.txtIniFile);
            this.tabGit.Controls.Add(this.btnBrowseIni);
            this.tabGit.Controls.Add(this.btnLoadFiles);
            this.tabGit.Controls.Add(this.lblRepositoryTitle);
            this.tabGit.Controls.Add(this.lblRepositoryPath);
            this.tabGit.Controls.Add(this.dgvSourceFiles);
            this.tabGit.Controls.Add(this.lblLog);
            this.tabGit.Controls.Add(this.txtLog);
            this.tabGit.Controls.Add(this.btnNext);
            this.tabGit.Location = new System.Drawing.Point(4, 25);
            this.tabGit.Name = "tabGit";
            this.tabGit.Padding = new System.Windows.Forms.Padding(3);
            this.tabGit.Size = new System.Drawing.Size(972, 791);
            this.tabGit.TabIndex = 0;
            this.tabGit.Text = "Update Handlers - Git";
            this.tabGit.UseVisualStyleBackColor = true;
            // 
            // lblIniFile
            // 
            this.lblIniFile.AutoSize = true;
            this.lblIniFile.Location = new System.Drawing.Point(20, 22);
            this.lblIniFile.Name = "lblIniFile";
            this.lblIniFile.Size = new System.Drawing.Size(75, 16);
            this.lblIniFile.TabIndex = 0;
            this.lblIniFile.Text = "INI file path:";
            // 
            // txtIniFile
            // 
            this.txtIniFile.Location = new System.Drawing.Point(110, 19);
            this.txtIniFile.Name = "txtIniFile";
            this.txtIniFile.Size = new System.Drawing.Size(570, 22);
            this.txtIniFile.TabIndex = 1;
            // 
            // btnBrowseIni
            // 
            this.btnBrowseIni.Location = new System.Drawing.Point(695, 17);
            this.btnBrowseIni.Name = "btnBrowseIni";
            this.btnBrowseIni.Size = new System.Drawing.Size(90, 27);
            this.btnBrowseIni.TabIndex = 2;
            this.btnBrowseIni.Text = "Browse";
            this.btnBrowseIni.UseVisualStyleBackColor = true;
            this.btnBrowseIni.Click += new System.EventHandler(this.btnBrowseIni_Click);
            // 
            // btnLoadFiles
            // 
            this.btnLoadFiles.Location = new System.Drawing.Point(800, 17);
            this.btnLoadFiles.Name = "btnLoadFiles";
            this.btnLoadFiles.Size = new System.Drawing.Size(120, 27);
            this.btnLoadFiles.TabIndex = 3;
            this.btnLoadFiles.Text = "Load Files";
            this.btnLoadFiles.UseVisualStyleBackColor = true;
            this.btnLoadFiles.Click += new System.EventHandler(this.btnLoadFiles_Click);
            // 
            // lblRepositoryTitle
            // 
            this.lblRepositoryTitle.AutoSize = true;
            this.lblRepositoryTitle.Location = new System.Drawing.Point(20, 62);
            this.lblRepositoryTitle.Name = "lblRepositoryTitle";
            this.lblRepositoryTitle.Size = new System.Drawing.Size(105, 16);
            this.lblRepositoryTitle.TabIndex = 4;
            this.lblRepositoryTitle.Text = "Repository path:";
            // 
            // lblRepositoryPath
            // 
            this.lblRepositoryPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRepositoryPath.Location = new System.Drawing.Point(140, 57);
            this.lblRepositoryPath.Name = "lblRepositoryPath";
            this.lblRepositoryPath.Size = new System.Drawing.Size(780, 27);
            this.lblRepositoryPath.TabIndex = 5;
            this.lblRepositoryPath.Text = "-";
            this.lblRepositoryPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvSourceFiles
            // 
            this.dgvSourceFiles.AllowUserToAddRows = false;
            this.dgvSourceFiles.AllowUserToDeleteRows = false;
            this.dgvSourceFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSourceFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSourceFiles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSelect,
            this.colFileName,
            this.colExtension,
            this.colFullPath});
            this.dgvSourceFiles.Location = new System.Drawing.Point(20, 100);
            this.dgvSourceFiles.Name = "dgvSourceFiles";
            this.dgvSourceFiles.RowHeadersWidth = 51;
            this.dgvSourceFiles.Size = new System.Drawing.Size(900, 330);
            this.dgvSourceFiles.TabIndex = 6;
            // 
            // colSelect
            // 
            this.colSelect.HeaderText = "Select";
            this.colSelect.MinimumWidth = 6;
            this.colSelect.Name = "colSelect";
            this.colSelect.Width = 70;
            // 
            // colFileName
            // 
            this.colFileName.HeaderText = "File Name";
            this.colFileName.MinimumWidth = 6;
            this.colFileName.Name = "colFileName";
            this.colFileName.ReadOnly = true;
            this.colFileName.Width = 220;
            // 
            // colExtension
            // 
            this.colExtension.HeaderText = "Type";
            this.colExtension.MinimumWidth = 6;
            this.colExtension.Name = "colExtension";
            this.colExtension.ReadOnly = true;
            this.colExtension.Width = 80;
            // 
            // colFullPath
            // 
            this.colFullPath.HeaderText = "Full Path / Raw URL";
            this.colFullPath.MinimumWidth = 6;
            this.colFullPath.Name = "colFullPath";
            this.colFullPath.ReadOnly = true;
            this.colFullPath.Width = 520;
            // 
            // lblLog
            // 
            this.lblLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblLog.AutoSize = true;
            this.lblLog.Location = new System.Drawing.Point(20, 445);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(33, 16);
            this.lblLog.TabIndex = 7;
            this.lblLog.Text = "Log:";
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Location = new System.Drawing.Point(20, 470);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(900, 150);
            this.txtLog.TabIndex = 8;
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.Location = new System.Drawing.Point(800, 635);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(120, 35);
            this.btnNext.TabIndex = 9;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // tabTia
            // 
            this.tabTia.Controls.Add(this.lblOpenTia);
            this.tabTia.Controls.Add(this.btnLoadTiaInstances);
            this.tabTia.Controls.Add(this.dgvTiaInstances);
            this.tabTia.Controls.Add(this.btnListPlcs);
            this.tabTia.Controls.Add(this.btnImportExternalSource);
            this.tabTia.Controls.Add(this.btnGenerateBlocks);
            this.tabTia.Controls.Add(this.btnCompileChanges);
            this.tabTia.Controls.Add(this.btnCompileAll);
            this.tabTia.Controls.Add(this.btnImportEpicsSource);
            this.tabTia.Controls.Add(this.dgvPlcs);
            this.tabTia.Controls.Add(this.lblTiaLog);
            this.tabTia.Controls.Add(this.txtTiaLog);
            this.tabTia.Location = new System.Drawing.Point(4, 25);
            this.tabTia.Name = "tabTia";
            this.tabTia.Padding = new System.Windows.Forms.Padding(3);
            this.tabTia.Size = new System.Drawing.Size(972, 791);
            this.tabTia.TabIndex = 1;
            this.tabTia.Text = "Update Handlers - TIA";
            this.tabTia.UseVisualStyleBackColor = true;
            // 
            // lblOpenTia
            // 
            this.lblOpenTia.AutoSize = true;
            this.lblOpenTia.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblOpenTia.Location = new System.Drawing.Point(20, 25);
            this.lblOpenTia.Name = "lblOpenTia";
            this.lblOpenTia.Size = new System.Drawing.Size(300, 20);
            this.lblOpenTia.TabIndex = 0;
            this.lblOpenTia.Text = "Please open the TIA Portal Project";
            // 
            // btnLoadTiaInstances
            // 
            this.btnLoadTiaInstances.Location = new System.Drawing.Point(20, 65);
            this.btnLoadTiaInstances.Name = "btnLoadTiaInstances";
            this.btnLoadTiaInstances.Size = new System.Drawing.Size(205, 32);
            this.btnLoadTiaInstances.TabIndex = 1;
            this.btnLoadTiaInstances.Text = "Connect To TIA";
            this.btnLoadTiaInstances.UseVisualStyleBackColor = true;
            this.btnLoadTiaInstances.Click += new System.EventHandler(this.btnLoadTiaInstances_Click);
            // 
            // dgvTiaInstances
            // 
            this.dgvTiaInstances.AllowUserToAddRows = false;
            this.dgvTiaInstances.AllowUserToDeleteRows = false;
            this.dgvTiaInstances.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTiaInstances.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewCheckBoxColumn1,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            this.dgvTiaInstances.Location = new System.Drawing.Point(20, 115);
            this.dgvTiaInstances.Name = "dgvTiaInstances";
            this.dgvTiaInstances.RowHeadersWidth = 51;
            this.dgvTiaInstances.Size = new System.Drawing.Size(916, 90);
            this.dgvTiaInstances.TabIndex = 2;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.MinimumWidth = 6;
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Width = 125;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Index";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 125;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Process ID";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 125;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Project Path";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 125;
            // 
            // btnListPlcs
            // 
            this.btnListPlcs.Location = new System.Drawing.Point(20, 220);
            this.btnListPlcs.Name = "btnListPlcs";
            this.btnListPlcs.Size = new System.Drawing.Size(120, 32);
            this.btnListPlcs.TabIndex = 3;
            this.btnListPlcs.Text = "List All PLCs";
            this.btnListPlcs.UseVisualStyleBackColor = true;
            this.btnListPlcs.Click += new System.EventHandler(this.btnListPlcs_Click);
            // 
            // btnImportExternalSource
            // 
            this.btnImportExternalSource.Location = new System.Drawing.Point(146, 220);
            this.btnImportExternalSource.Name = "btnImportExternalSource";
            this.btnImportExternalSource.Size = new System.Drawing.Size(180, 32);
            this.btnImportExternalSource.TabIndex = 4;
            this.btnImportExternalSource.Text = "Import Handlers Source";
            this.btnImportExternalSource.UseVisualStyleBackColor = true;
            this.btnImportExternalSource.Click += new System.EventHandler(this.btnImportExternalSource_Click);
            // 
            // btnGenerateBlocks
            // 
            this.btnGenerateBlocks.Location = new System.Drawing.Point(514, 220);
            this.btnGenerateBlocks.Name = "btnGenerateBlocks";
            this.btnGenerateBlocks.Size = new System.Drawing.Size(150, 32);
            this.btnGenerateBlocks.TabIndex = 5;
            this.btnGenerateBlocks.Text = "Generate Blocks";
            this.btnGenerateBlocks.UseVisualStyleBackColor = true;
            this.btnGenerateBlocks.Click += new System.EventHandler(this.btnGenerateBlocks_Click);
            // 
            // btnCompileChanges
            // 
            this.btnCompileChanges.Location = new System.Drawing.Point(670, 220);
            this.btnCompileChanges.Name = "btnCompileChanges";
            this.btnCompileChanges.Size = new System.Drawing.Size(140, 32);
            this.btnCompileChanges.TabIndex = 6;
            this.btnCompileChanges.Text = "Compile Changes";
            this.btnCompileChanges.UseVisualStyleBackColor = true;
            this.btnCompileChanges.Click += new System.EventHandler(this.btnCompileChanges_Click);
            // 
            // btnCompileAll
            // 
            this.btnCompileAll.Location = new System.Drawing.Point(816, 220);
            this.btnCompileAll.Name = "btnCompileAll";
            this.btnCompileAll.Size = new System.Drawing.Size(120, 32);
            this.btnCompileAll.TabIndex = 7;
            this.btnCompileAll.Text = "Compile All";
            this.btnCompileAll.UseVisualStyleBackColor = true;
            this.btnCompileAll.Click += new System.EventHandler(this.btnCompileAll_Click);
            // 
            // btnImportEpicsSource
            // 
            this.btnImportEpicsSource.Location = new System.Drawing.Point(332, 220);
            this.btnImportEpicsSource.Name = "btnImportEpicsSource";
            this.btnImportEpicsSource.Size = new System.Drawing.Size(160, 32);
            this.btnImportEpicsSource.TabIndex = 8;
            this.btnImportEpicsSource.Text = "Import EPICS Source";
            this.btnImportEpicsSource.UseVisualStyleBackColor = true;
            this.btnImportEpicsSource.Click += new System.EventHandler(this.btnImportEpicsSource_Click);
            // 
            // dgvPlcs
            // 
            this.dgvPlcs.AllowUserToAddRows = false;
            this.dgvPlcs.AllowUserToDeleteRows = false;
            this.dgvPlcs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPlcs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewCheckBoxColumn2,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5});
            this.dgvPlcs.Location = new System.Drawing.Point(20, 269);
            this.dgvPlcs.Name = "dgvPlcs";
            this.dgvPlcs.RowHeadersWidth = 51;
            this.dgvPlcs.Size = new System.Drawing.Size(916, 250);
            this.dgvPlcs.TabIndex = 9;
            // 
            // dataGridViewCheckBoxColumn2
            // 
            this.dataGridViewCheckBoxColumn2.MinimumWidth = 6;
            this.dataGridViewCheckBoxColumn2.Name = "dataGridViewCheckBoxColumn2";
            this.dataGridViewCheckBoxColumn2.Width = 125;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Device Name";
            this.dataGridViewTextBoxColumn4.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 125;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "PLC Name";
            this.dataGridViewTextBoxColumn5.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Width = 125;
            // 
            // lblTiaLog
            // 
            this.lblTiaLog.AutoSize = true;
            this.lblTiaLog.Location = new System.Drawing.Point(21, 531);
            this.lblTiaLog.Name = "lblTiaLog";
            this.lblTiaLog.Size = new System.Drawing.Size(33, 16);
            this.lblTiaLog.TabIndex = 10;
            this.lblTiaLog.Text = "Log:";
            // 
            // txtTiaLog
            // 
            this.txtTiaLog.Location = new System.Drawing.Point(20, 550);
            this.txtTiaLog.Multiline = true;
            this.txtTiaLog.Name = "txtTiaLog";
            this.txtTiaLog.ReadOnly = true;
            this.txtTiaLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTiaLog.Size = new System.Drawing.Size(916, 230);
            this.txtTiaLog.TabIndex = 11;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(980, 820);
            this.Controls.Add(this.tabMain);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TIA Source Updater";
            this.tabMain.ResumeLayout(false);
            this.tabHome.ResumeLayout(false);
            this.tabHome.PerformLayout();
            this.tabGit.ResumeLayout(false);
            this.tabGit.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSourceFiles)).EndInit();
            this.tabTia.ResumeLayout(false);
            this.tabTia.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTiaInstances)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlcs)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
    }
}