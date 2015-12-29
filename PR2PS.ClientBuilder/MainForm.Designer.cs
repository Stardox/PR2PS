namespace PR2PS.ClientBuilder
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.stripStatus = new System.Windows.Forms.StatusStrip();
            this.labelStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuMain = new System.Windows.Forms.ToolStrip();
            this.btnInfo = new System.Windows.Forms.ToolStripButton();
            this.groupBoxFlashProjector = new System.Windows.Forms.GroupBox();
            this.textBoxFlashProjector = new System.Windows.Forms.TextBox();
            this.btnHelpFlashProjector = new System.Windows.Forms.Button();
            this.btnBrowseFlashProjector = new System.Windows.Forms.Button();
            this.groupBoxClientFile = new System.Windows.Forms.GroupBox();
            this.btnHelpClientFile = new System.Windows.Forms.Button();
            this.btnBrowseClientFile = new System.Windows.Forms.Button();
            this.textBoxClientFile = new System.Windows.Forms.TextBox();
            this.groupBoxURLs = new System.Windows.Forms.GroupBox();
            this.btnHelpLevelsServerURL = new System.Windows.Forms.Button();
            this.textBoxLevelsServerURL = new System.Windows.Forms.TextBox();
            this.labelLevelsServerURL = new System.Windows.Forms.Label();
            this.btnHelpWebServerURL = new System.Windows.Forms.Button();
            this.textBoxWebServerURL = new System.Windows.Forms.TextBox();
            this.labelWebServerURL = new System.Windows.Forms.Label();
            this.groupBoxOutput = new System.Windows.Forms.GroupBox();
            this.checkBoxOutputBAT = new System.Windows.Forms.CheckBox();
            this.checkBoxOutputLNK = new System.Windows.Forms.CheckBox();
            this.groupBoxDestination = new System.Windows.Forms.GroupBox();
            this.btnHelpDestinationFolder = new System.Windows.Forms.Button();
            this.btnBrowseDestinationFolder = new System.Windows.Forms.Button();
            this.textBoxDestinationFolder = new System.Windows.Forms.TextBox();
            this.btnBuild = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.stripStatus.SuspendLayout();
            this.menuMain.SuspendLayout();
            this.groupBoxFlashProjector.SuspendLayout();
            this.groupBoxClientFile.SuspendLayout();
            this.groupBoxURLs.SuspendLayout();
            this.groupBoxOutput.SuspendLayout();
            this.groupBoxDestination.SuspendLayout();
            this.SuspendLayout();
            // 
            // stripStatus
            // 
            this.stripStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labelStatus});
            this.stripStatus.Location = new System.Drawing.Point(0, 439);
            this.stripStatus.Name = "stripStatus";
            this.stripStatus.Size = new System.Drawing.Size(394, 22);
            this.stripStatus.TabIndex = 1;
            this.stripStatus.Text = "statusStrip1";
            // 
            // labelStatus
            // 
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(39, 17);
            this.labelStatus.Text = "Ready";
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnInfo});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(394, 25);
            this.menuMain.TabIndex = 2;
            this.menuMain.Text = "toolStrip1";
            // 
            // btnInfo
            // 
            this.btnInfo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnInfo.Image = ((System.Drawing.Image)(resources.GetObject("btnInfo.Image")));
            this.btnInfo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnInfo.Name = "btnInfo";
            this.btnInfo.Size = new System.Drawing.Size(74, 22);
            this.btnInfo.Text = "Information";
            this.btnInfo.Click += new System.EventHandler(this.btnInfo_Click);
            // 
            // groupBoxFlashProjector
            // 
            this.groupBoxFlashProjector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxFlashProjector.Controls.Add(this.textBoxFlashProjector);
            this.groupBoxFlashProjector.Controls.Add(this.btnHelpFlashProjector);
            this.groupBoxFlashProjector.Controls.Add(this.btnBrowseFlashProjector);
            this.groupBoxFlashProjector.Location = new System.Drawing.Point(13, 29);
            this.groupBoxFlashProjector.Name = "groupBoxFlashProjector";
            this.groupBoxFlashProjector.Size = new System.Drawing.Size(369, 55);
            this.groupBoxFlashProjector.TabIndex = 3;
            this.groupBoxFlashProjector.TabStop = false;
            this.groupBoxFlashProjector.Text = "Standalone Flash projector executable";
            // 
            // textBoxFlashProjector
            // 
            this.textBoxFlashProjector.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxFlashProjector.Location = new System.Drawing.Point(6, 22);
            this.textBoxFlashProjector.Name = "textBoxFlashProjector";
            this.textBoxFlashProjector.Size = new System.Drawing.Size(235, 20);
            this.textBoxFlashProjector.TabIndex = 11;
            // 
            // btnHelpFlashProjector
            // 
            this.btnHelpFlashProjector.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnHelpFlashProjector.Location = new System.Drawing.Point(333, 20);
            this.btnHelpFlashProjector.Name = "btnHelpFlashProjector";
            this.btnHelpFlashProjector.Size = new System.Drawing.Size(30, 23);
            this.btnHelpFlashProjector.TabIndex = 10;
            this.btnHelpFlashProjector.Text = "?";
            this.btnHelpFlashProjector.UseVisualStyleBackColor = true;
            this.btnHelpFlashProjector.Click += new System.EventHandler(this.btnHelpFlashProjector_Click);
            // 
            // btnBrowseFlashProjector
            // 
            this.btnBrowseFlashProjector.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnBrowseFlashProjector.Location = new System.Drawing.Point(252, 20);
            this.btnBrowseFlashProjector.Name = "btnBrowseFlashProjector";
            this.btnBrowseFlashProjector.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseFlashProjector.TabIndex = 0;
            this.btnBrowseFlashProjector.Text = "Browse";
            this.btnBrowseFlashProjector.UseVisualStyleBackColor = true;
            this.btnBrowseFlashProjector.Click += new System.EventHandler(this.btnBrowseFlashProjector_Click);
            // 
            // groupBoxClientFile
            // 
            this.groupBoxClientFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxClientFile.Controls.Add(this.btnHelpClientFile);
            this.groupBoxClientFile.Controls.Add(this.btnBrowseClientFile);
            this.groupBoxClientFile.Controls.Add(this.textBoxClientFile);
            this.groupBoxClientFile.Location = new System.Drawing.Point(13, 90);
            this.groupBoxClientFile.Name = "groupBoxClientFile";
            this.groupBoxClientFile.Size = new System.Drawing.Size(369, 55);
            this.groupBoxClientFile.TabIndex = 4;
            this.groupBoxClientFile.TabStop = false;
            this.groupBoxClientFile.Text = "Modded client SWF file";
            // 
            // btnHelpClientFile
            // 
            this.btnHelpClientFile.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnHelpClientFile.Location = new System.Drawing.Point(333, 19);
            this.btnHelpClientFile.Name = "btnHelpClientFile";
            this.btnHelpClientFile.Size = new System.Drawing.Size(30, 23);
            this.btnHelpClientFile.TabIndex = 13;
            this.btnHelpClientFile.Text = "?";
            this.btnHelpClientFile.UseVisualStyleBackColor = true;
            this.btnHelpClientFile.Click += new System.EventHandler(this.btnHelpClientFile_Click);
            // 
            // btnBrowseClientFile
            // 
            this.btnBrowseClientFile.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnBrowseClientFile.Location = new System.Drawing.Point(252, 19);
            this.btnBrowseClientFile.Name = "btnBrowseClientFile";
            this.btnBrowseClientFile.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseClientFile.TabIndex = 12;
            this.btnBrowseClientFile.Text = "Browse";
            this.btnBrowseClientFile.UseVisualStyleBackColor = true;
            this.btnBrowseClientFile.Click += new System.EventHandler(this.btnBrowseClientFile_Click);
            // 
            // textBoxClientFile
            // 
            this.textBoxClientFile.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxClientFile.Location = new System.Drawing.Point(6, 21);
            this.textBoxClientFile.Name = "textBoxClientFile";
            this.textBoxClientFile.Size = new System.Drawing.Size(235, 20);
            this.textBoxClientFile.TabIndex = 12;
            // 
            // groupBoxURLs
            // 
            this.groupBoxURLs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxURLs.Controls.Add(this.btnHelpLevelsServerURL);
            this.groupBoxURLs.Controls.Add(this.textBoxLevelsServerURL);
            this.groupBoxURLs.Controls.Add(this.labelLevelsServerURL);
            this.groupBoxURLs.Controls.Add(this.btnHelpWebServerURL);
            this.groupBoxURLs.Controls.Add(this.textBoxWebServerURL);
            this.groupBoxURLs.Controls.Add(this.labelWebServerURL);
            this.groupBoxURLs.Location = new System.Drawing.Point(13, 151);
            this.groupBoxURLs.Name = "groupBoxURLs";
            this.groupBoxURLs.Size = new System.Drawing.Size(369, 110);
            this.groupBoxURLs.TabIndex = 5;
            this.groupBoxURLs.TabStop = false;
            this.groupBoxURLs.Text = "URLs";
            // 
            // btnHelpLevelsServerURL
            // 
            this.btnHelpLevelsServerURL.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnHelpLevelsServerURL.Location = new System.Drawing.Point(333, 73);
            this.btnHelpLevelsServerURL.Name = "btnHelpLevelsServerURL";
            this.btnHelpLevelsServerURL.Size = new System.Drawing.Size(30, 23);
            this.btnHelpLevelsServerURL.TabIndex = 17;
            this.btnHelpLevelsServerURL.Text = "?";
            this.btnHelpLevelsServerURL.UseVisualStyleBackColor = true;
            this.btnHelpLevelsServerURL.Click += new System.EventHandler(this.btnHelpLevelsServerURL_Click);
            // 
            // textBoxLevelsServerURL
            // 
            this.textBoxLevelsServerURL.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxLevelsServerURL.Location = new System.Drawing.Point(6, 75);
            this.textBoxLevelsServerURL.Name = "textBoxLevelsServerURL";
            this.textBoxLevelsServerURL.Size = new System.Drawing.Size(235, 20);
            this.textBoxLevelsServerURL.TabIndex = 16;
            this.textBoxLevelsServerURL.Text = "http://pr2hub.com/levels";
            // 
            // labelLevelsServerURL
            // 
            this.labelLevelsServerURL.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelLevelsServerURL.AutoSize = true;
            this.labelLevelsServerURL.Location = new System.Drawing.Point(7, 59);
            this.labelLevelsServerURL.Name = "labelLevelsServerURL";
            this.labelLevelsServerURL.Size = new System.Drawing.Size(98, 13);
            this.labelLevelsServerURL.TabIndex = 15;
            this.labelLevelsServerURL.Text = "Levels server URL:";
            // 
            // btnHelpWebServerURL
            // 
            this.btnHelpWebServerURL.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnHelpWebServerURL.Location = new System.Drawing.Point(333, 34);
            this.btnHelpWebServerURL.Name = "btnHelpWebServerURL";
            this.btnHelpWebServerURL.Size = new System.Drawing.Size(30, 23);
            this.btnHelpWebServerURL.TabIndex = 14;
            this.btnHelpWebServerURL.Text = "?";
            this.btnHelpWebServerURL.UseVisualStyleBackColor = true;
            this.btnHelpWebServerURL.Click += new System.EventHandler(this.btnHelpWebServerURL_Click);
            // 
            // textBoxWebServerURL
            // 
            this.textBoxWebServerURL.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxWebServerURL.Location = new System.Drawing.Point(6, 36);
            this.textBoxWebServerURL.Name = "textBoxWebServerURL";
            this.textBoxWebServerURL.Size = new System.Drawing.Size(235, 20);
            this.textBoxWebServerURL.TabIndex = 14;
            // 
            // labelWebServerURL
            // 
            this.labelWebServerURL.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelWebServerURL.AutoSize = true;
            this.labelWebServerURL.Location = new System.Drawing.Point(7, 20);
            this.labelWebServerURL.Name = "labelWebServerURL";
            this.labelWebServerURL.Size = new System.Drawing.Size(90, 13);
            this.labelWebServerURL.TabIndex = 0;
            this.labelWebServerURL.Text = "Web server URL:";
            // 
            // groupBoxOutput
            // 
            this.groupBoxOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxOutput.Controls.Add(this.checkBoxOutputBAT);
            this.groupBoxOutput.Controls.Add(this.checkBoxOutputLNK);
            this.groupBoxOutput.Location = new System.Drawing.Point(13, 267);
            this.groupBoxOutput.Name = "groupBoxOutput";
            this.groupBoxOutput.Size = new System.Drawing.Size(369, 70);
            this.groupBoxOutput.TabIndex = 6;
            this.groupBoxOutput.TabStop = false;
            this.groupBoxOutput.Text = "Output type";
            // 
            // checkBoxOutputBAT
            // 
            this.checkBoxOutputBAT.AutoSize = true;
            this.checkBoxOutputBAT.Checked = true;
            this.checkBoxOutputBAT.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxOutputBAT.Location = new System.Drawing.Point(6, 19);
            this.checkBoxOutputBAT.Name = "checkBoxOutputBAT";
            this.checkBoxOutputBAT.Size = new System.Drawing.Size(47, 17);
            this.checkBoxOutputBAT.TabIndex = 1;
            this.checkBoxOutputBAT.Text = "BAT";
            this.checkBoxOutputBAT.UseVisualStyleBackColor = true;
            // 
            // checkBoxOutputLNK
            // 
            this.checkBoxOutputLNK.AutoSize = true;
            this.checkBoxOutputLNK.Enabled = false;
            this.checkBoxOutputLNK.Location = new System.Drawing.Point(6, 42);
            this.checkBoxOutputLNK.Name = "checkBoxOutputLNK";
            this.checkBoxOutputLNK.Size = new System.Drawing.Size(115, 17);
            this.checkBoxOutputLNK.TabIndex = 0;
            this.checkBoxOutputLNK.Text = "LNK (unsupported)";
            this.checkBoxOutputLNK.UseVisualStyleBackColor = true;
            // 
            // groupBoxDestination
            // 
            this.groupBoxDestination.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxDestination.Controls.Add(this.btnHelpDestinationFolder);
            this.groupBoxDestination.Controls.Add(this.btnBrowseDestinationFolder);
            this.groupBoxDestination.Controls.Add(this.textBoxDestinationFolder);
            this.groupBoxDestination.Location = new System.Drawing.Point(13, 343);
            this.groupBoxDestination.Name = "groupBoxDestination";
            this.groupBoxDestination.Size = new System.Drawing.Size(369, 55);
            this.groupBoxDestination.TabIndex = 7;
            this.groupBoxDestination.TabStop = false;
            this.groupBoxDestination.Text = "Destination folder";
            // 
            // btnHelpDestinationFolder
            // 
            this.btnHelpDestinationFolder.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnHelpDestinationFolder.Location = new System.Drawing.Point(333, 17);
            this.btnHelpDestinationFolder.Name = "btnHelpDestinationFolder";
            this.btnHelpDestinationFolder.Size = new System.Drawing.Size(30, 23);
            this.btnHelpDestinationFolder.TabIndex = 14;
            this.btnHelpDestinationFolder.Text = "?";
            this.btnHelpDestinationFolder.UseVisualStyleBackColor = true;
            this.btnHelpDestinationFolder.Click += new System.EventHandler(this.btnHelpDestinationFolder_Click);
            // 
            // btnBrowseDestinationFolder
            // 
            this.btnBrowseDestinationFolder.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnBrowseDestinationFolder.Location = new System.Drawing.Point(252, 17);
            this.btnBrowseDestinationFolder.Name = "btnBrowseDestinationFolder";
            this.btnBrowseDestinationFolder.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseDestinationFolder.TabIndex = 14;
            this.btnBrowseDestinationFolder.Text = "Browse";
            this.btnBrowseDestinationFolder.UseVisualStyleBackColor = true;
            this.btnBrowseDestinationFolder.Click += new System.EventHandler(this.btnBrowseDestinationFolder_Click);
            // 
            // textBoxDestinationFolder
            // 
            this.textBoxDestinationFolder.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxDestinationFolder.Location = new System.Drawing.Point(6, 19);
            this.textBoxDestinationFolder.Name = "textBoxDestinationFolder";
            this.textBoxDestinationFolder.Size = new System.Drawing.Size(235, 20);
            this.textBoxDestinationFolder.TabIndex = 12;
            // 
            // btnBuild
            // 
            this.btnBuild.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBuild.Location = new System.Drawing.Point(12, 413);
            this.btnBuild.Name = "btnBuild";
            this.btnBuild.Size = new System.Drawing.Size(75, 23);
            this.btnBuild.TabIndex = 8;
            this.btnBuild.Text = "Build";
            this.btnBuild.UseVisualStyleBackColor = true;
            this.btnBuild.Click += new System.EventHandler(this.btnBuild_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Location = new System.Drawing.Point(307, 413);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 9;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 461);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnBuild);
            this.Controls.Add(this.groupBoxDestination);
            this.Controls.Add(this.groupBoxOutput);
            this.Controls.Add(this.groupBoxURLs);
            this.Controls.Add(this.groupBoxClientFile);
            this.Controls.Add(this.groupBoxFlashProjector);
            this.Controls.Add(this.menuMain);
            this.Controls.Add(this.stripStatus);
            this.MinimumSize = new System.Drawing.Size(410, 500);
            this.Name = "MainForm";
            this.Text = "PR2PS Client builder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.stripStatus.ResumeLayout(false);
            this.stripStatus.PerformLayout();
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.groupBoxFlashProjector.ResumeLayout(false);
            this.groupBoxFlashProjector.PerformLayout();
            this.groupBoxClientFile.ResumeLayout(false);
            this.groupBoxClientFile.PerformLayout();
            this.groupBoxURLs.ResumeLayout(false);
            this.groupBoxURLs.PerformLayout();
            this.groupBoxOutput.ResumeLayout(false);
            this.groupBoxOutput.PerformLayout();
            this.groupBoxDestination.ResumeLayout(false);
            this.groupBoxDestination.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip stripStatus;
        private System.Windows.Forms.ToolStripStatusLabel labelStatus;
        private System.Windows.Forms.ToolStrip menuMain;
        private System.Windows.Forms.ToolStripButton btnInfo;
        private System.Windows.Forms.GroupBox groupBoxFlashProjector;
        private System.Windows.Forms.GroupBox groupBoxClientFile;
        private System.Windows.Forms.GroupBox groupBoxURLs;
        private System.Windows.Forms.GroupBox groupBoxOutput;
        private System.Windows.Forms.GroupBox groupBoxDestination;
        private System.Windows.Forms.Button btnBuild;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnBrowseFlashProjector;
        private System.Windows.Forms.TextBox textBoxFlashProjector;
        private System.Windows.Forms.Button btnHelpFlashProjector;
        private System.Windows.Forms.Button btnHelpClientFile;
        private System.Windows.Forms.Button btnBrowseClientFile;
        private System.Windows.Forms.TextBox textBoxClientFile;
        private System.Windows.Forms.TextBox textBoxWebServerURL;
        private System.Windows.Forms.Label labelWebServerURL;
        private System.Windows.Forms.Button btnHelpLevelsServerURL;
        private System.Windows.Forms.TextBox textBoxLevelsServerURL;
        private System.Windows.Forms.Label labelLevelsServerURL;
        private System.Windows.Forms.Button btnHelpWebServerURL;
        private System.Windows.Forms.TextBox textBoxDestinationFolder;
        private System.Windows.Forms.Button btnHelpDestinationFolder;
        private System.Windows.Forms.Button btnBrowseDestinationFolder;
        private System.Windows.Forms.CheckBox checkBoxOutputBAT;
        private System.Windows.Forms.CheckBox checkBoxOutputLNK;
    }
}

