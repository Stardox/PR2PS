namespace PR2PS.LevelImporter
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
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.connDisconnBtn = new System.Windows.Forms.ToolStripButton();
            this.infoBtn = new System.Windows.Forms.ToolStripButton();
            this.exitBtn = new System.Windows.Forms.ToolStripButton();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.splitContainerSub = new System.Windows.Forms.SplitContainer();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabAssignUser = new System.Windows.Forms.TabPage();
            this.tabImportFromFile = new System.Windows.Forms.TabPage();
            this.tabImportById = new System.Windows.Forms.TabPage();
            this.tabImportBySearch = new System.Windows.Forms.TabPage();
            this.logTextBox = new System.Windows.Forms.TextBox();
            this.runBtn = new System.Windows.Forms.Button();
            this.delFromPipelineBtn = new System.Windows.Forms.Button();
            this.pipelineListBox = new System.Windows.Forms.ListBox();
            this.pipelineLabel = new System.Windows.Forms.Label();
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerSub)).BeginInit();
            this.splitContainerSub.Panel1.SuspendLayout();
            this.splitContainerSub.Panel2.SuspendLayout();
            this.splitContainerSub.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connDisconnBtn,
            this.infoBtn,
            this.exitBtn});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(1167, 25);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip";
            // 
            // connDisconnBtn
            // 
            this.connDisconnBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.connDisconnBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.connDisconnBtn.Name = "connDisconnBtn";
            this.connDisconnBtn.Size = new System.Drawing.Size(56, 22);
            this.connDisconnBtn.Text = "Connect";
            // 
            // infoBtn
            // 
            this.infoBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.infoBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.infoBtn.Name = "infoBtn";
            this.infoBtn.Size = new System.Drawing.Size(74, 22);
            this.infoBtn.Text = "Information";
            // 
            // exitBtn
            // 
            this.exitBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.exitBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.exitBtn.Name = "exitBtn";
            this.exitBtn.Size = new System.Drawing.Size(29, 22);
            this.exitBtn.Text = "Exit";
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.IsSplitterFixed = true;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 25);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.splitContainerSub);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.runBtn);
            this.splitContainerMain.Panel2.Controls.Add(this.delFromPipelineBtn);
            this.splitContainerMain.Panel2.Controls.Add(this.pipelineListBox);
            this.splitContainerMain.Panel2.Controls.Add(this.pipelineLabel);
            this.splitContainerMain.Size = new System.Drawing.Size(1167, 674);
            this.splitContainerMain.SplitterDistance = 900;
            this.splitContainerMain.TabIndex = 1;
            // 
            // splitContainerSub
            // 
            this.splitContainerSub.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainerSub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerSub.IsSplitterFixed = true;
            this.splitContainerSub.Location = new System.Drawing.Point(0, 0);
            this.splitContainerSub.Name = "splitContainerSub";
            this.splitContainerSub.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerSub.Panel1
            // 
            this.splitContainerSub.Panel1.Controls.Add(this.tabControl);
            // 
            // splitContainerSub.Panel2
            // 
            this.splitContainerSub.Panel2.Controls.Add(this.logTextBox);
            this.splitContainerSub.Size = new System.Drawing.Size(900, 674);
            this.splitContainerSub.SplitterDistance = 499;
            this.splitContainerSub.TabIndex = 0;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabAssignUser);
            this.tabControl.Controls.Add(this.tabImportFromFile);
            this.tabControl.Controls.Add(this.tabImportById);
            this.tabControl.Controls.Add(this.tabImportBySearch);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(898, 497);
            this.tabControl.TabIndex = 0;
            // 
            // tabAssignUser
            // 
            this.tabAssignUser.BackColor = System.Drawing.SystemColors.Control;
            this.tabAssignUser.Location = new System.Drawing.Point(4, 22);
            this.tabAssignUser.Name = "tabAssignUser";
            this.tabAssignUser.Padding = new System.Windows.Forms.Padding(3);
            this.tabAssignUser.Size = new System.Drawing.Size(890, 471);
            this.tabAssignUser.TabIndex = 0;
            this.tabAssignUser.Text = "Assign User";
            // 
            // tabImportFromFile
            // 
            this.tabImportFromFile.BackColor = System.Drawing.SystemColors.Control;
            this.tabImportFromFile.Location = new System.Drawing.Point(4, 22);
            this.tabImportFromFile.Name = "tabImportFromFile";
            this.tabImportFromFile.Padding = new System.Windows.Forms.Padding(3);
            this.tabImportFromFile.Size = new System.Drawing.Size(890, 471);
            this.tabImportFromFile.TabIndex = 1;
            this.tabImportFromFile.Text = "Import From File";
            // 
            // tabImportById
            // 
            this.tabImportById.BackColor = System.Drawing.SystemColors.Control;
            this.tabImportById.Location = new System.Drawing.Point(4, 22);
            this.tabImportById.Name = "tabImportById";
            this.tabImportById.Size = new System.Drawing.Size(890, 471);
            this.tabImportById.TabIndex = 2;
            this.tabImportById.Text = "Import By Id";
            // 
            // tabImportBySearch
            // 
            this.tabImportBySearch.BackColor = System.Drawing.SystemColors.Control;
            this.tabImportBySearch.Location = new System.Drawing.Point(4, 22);
            this.tabImportBySearch.Name = "tabImportBySearch";
            this.tabImportBySearch.Size = new System.Drawing.Size(890, 471);
            this.tabImportBySearch.TabIndex = 3;
            this.tabImportBySearch.Text = "Import By Searching";
            // 
            // logTextBox
            // 
            this.logTextBox.BackColor = System.Drawing.Color.Black;
            this.logTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logTextBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.logTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.logTextBox.Location = new System.Drawing.Point(0, 0);
            this.logTextBox.Multiline = true;
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ReadOnly = true;
            this.logTextBox.Size = new System.Drawing.Size(898, 169);
            this.logTextBox.TabIndex = 0;
            // 
            // runBtn
            // 
            this.runBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.runBtn.Location = new System.Drawing.Point(5, 631);
            this.runBtn.Name = "runBtn";
            this.runBtn.Size = new System.Drawing.Size(245, 30);
            this.runBtn.TabIndex = 3;
            this.runBtn.Text = "Run Import Procedure";
            this.runBtn.UseVisualStyleBackColor = true;
            // 
            // delFromPipelineBtn
            // 
            this.delFromPipelineBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.delFromPipelineBtn.Location = new System.Drawing.Point(5, 595);
            this.delFromPipelineBtn.Name = "delFromPipelineBtn";
            this.delFromPipelineBtn.Size = new System.Drawing.Size(245, 30);
            this.delFromPipelineBtn.TabIndex = 2;
            this.delFromPipelineBtn.Text = "Delete Selected Items";
            this.delFromPipelineBtn.UseVisualStyleBackColor = true;
            // 
            // pipelineListBox
            // 
            this.pipelineListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pipelineListBox.FormattingEnabled = true;
            this.pipelineListBox.Location = new System.Drawing.Point(5, 21);
            this.pipelineListBox.Name = "pipelineListBox";
            this.pipelineListBox.Size = new System.Drawing.Size(245, 563);
            this.pipelineListBox.TabIndex = 1;
            // 
            // pipelineLabel
            // 
            this.pipelineLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pipelineLabel.AutoSize = true;
            this.pipelineLabel.Location = new System.Drawing.Point(2, 4);
            this.pipelineLabel.Name = "pipelineLabel";
            this.pipelineLabel.Size = new System.Drawing.Size(87, 13);
            this.pipelineLabel.TabIndex = 0;
            this.pipelineLabel.Text = "Pipeline of tasks:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1167, 699);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.toolStrip);
            this.Name = "MainForm";
            this.Text = "PR2PS Level Importer";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.splitContainerSub.Panel1.ResumeLayout(false);
            this.splitContainerSub.Panel2.ResumeLayout(false);
            this.splitContainerSub.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerSub)).EndInit();
            this.splitContainerSub.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton connDisconnBtn;
        private System.Windows.Forms.ToolStripButton infoBtn;
        private System.Windows.Forms.ToolStripButton exitBtn;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.SplitContainer splitContainerSub;
        private System.Windows.Forms.TextBox logTextBox;
        private System.Windows.Forms.ListBox pipelineListBox;
        private System.Windows.Forms.Label pipelineLabel;
        private System.Windows.Forms.Button runBtn;
        private System.Windows.Forms.Button delFromPipelineBtn;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabAssignUser;
        private System.Windows.Forms.TabPage tabImportFromFile;
        private System.Windows.Forms.TabPage tabImportById;
        private System.Windows.Forms.TabPage tabImportBySearch;
    }
}

