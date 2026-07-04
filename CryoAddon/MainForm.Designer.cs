namespace CryoAddon
{
    
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

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

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
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
            ((System.ComponentModel.ISupportInitialize)(this.dgvSourceFiles)).BeginInit();
            this.SuspendLayout();
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
            this.txtIniFile.Size = new System.Drawing.Size(560, 22);
            this.txtIniFile.TabIndex = 1;
            // 
            // btnBrowseIni
            // 
            this.btnBrowseIni.Location = new System.Drawing.Point(685, 17);
            this.btnBrowseIni.Name = "btnBrowseIni";
            this.btnBrowseIni.Size = new System.Drawing.Size(90, 27);
            this.btnBrowseIni.TabIndex = 2;
            this.btnBrowseIni.Text = "Browse";
            this.btnBrowseIni.UseVisualStyleBackColor = true;
            this.btnBrowseIni.Click += new System.EventHandler(this.btnBrowseIni_Click);
            // 
            // btnLoadFiles
            // 
            this.btnLoadFiles.Location = new System.Drawing.Point(790, 17);
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
            this.lblRepositoryPath.Size = new System.Drawing.Size(770, 27);
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
            this.dgvSourceFiles.RowTemplate.Height = 24;
            this.dgvSourceFiles.Size = new System.Drawing.Size(890, 360);
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
            this.colFullPath.HeaderText = "Full Path";
            this.colFullPath.MinimumWidth = 6;
            this.colFullPath.Name = "colFullPath";
            this.colFullPath.ReadOnly = true;
            this.colFullPath.Width = 480;
            // 
            // lblLog
            // 
            this.lblLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblLog.AutoSize = true;
            this.lblLog.Location = new System.Drawing.Point(20, 475);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(33, 16);
            this.lblLog.TabIndex = 7;
            this.lblLog.Text = "Log:";
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Location = new System.Drawing.Point(20, 500);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(890, 120);
            this.txtLog.TabIndex = 8;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(935, 640);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.lblLog);
            this.Controls.Add(this.dgvSourceFiles);
            this.Controls.Add(this.lblRepositoryPath);
            this.Controls.Add(this.lblRepositoryTitle);
            this.Controls.Add(this.btnLoadFiles);
            this.Controls.Add(this.btnBrowseIni);
            this.Controls.Add(this.txtIniFile);
            this.Controls.Add(this.lblIniFile);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TIA Source Updater";
            ((System.ComponentModel.ISupportInitialize)(this.dgvSourceFiles)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}


