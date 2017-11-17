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
            this.dataGridViewUserResuts = new System.Windows.Forms.DataGridView();
            this.btnAssignUser = new System.Windows.Forms.Button();
            this.btnSearchUser = new System.Windows.Forms.Button();
            this.comboBoxSearchUserMode = new System.Windows.Forms.ComboBox();
            this.textBoxSearchUserTerm = new System.Windows.Forms.TextBox();
            this.labelSearchUserMode = new System.Windows.Forms.Label();
            this.labelSearchUserTerm = new System.Windows.Forms.Label();
            this.tabImportFromFile = new System.Windows.Forms.TabPage();
            this.listBoxLocalLevels = new System.Windows.Forms.ListBox();
            this.btnAddLocalToPipeline = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.tabImportById = new System.Windows.Forms.TabPage();
            this.btnAddExactToPipeLine = new System.Windows.Forms.Button();
            this.textBoxLevelVersion = new System.Windows.Forms.TextBox();
            this.textBoxLevelId = new System.Windows.Forms.TextBox();
            this.labelLevelVersion = new System.Windows.Forms.Label();
            this.labelLevelId = new System.Windows.Forms.Label();
            this.tabImportBySearch = new System.Windows.Forms.TabPage();
            this.numericPage = new System.Windows.Forms.NumericUpDown();
            this.comboBoxSortOrder = new System.Windows.Forms.ComboBox();
            this.comboBoxSortBy = new System.Windows.Forms.ComboBox();
            this.comboBoxSearchBy = new System.Windows.Forms.ComboBox();
            this.textBoxLevelsSearchTerm = new System.Windows.Forms.TextBox();
            this.runBtn = new System.Windows.Forms.Button();
            this.delFromPipelineBtn = new System.Windows.Forms.Button();
            this.pipelineListBox = new System.Windows.Forms.ListBox();
            this.pipelineLabel = new System.Windows.Forms.Label();
            this.labelSearchLevelsTerm = new System.Windows.Forms.Label();
            this.labelSearchLevelsBy = new System.Windows.Forms.Label();
            this.labelSearchLevelsSortBy = new System.Windows.Forms.Label();
            this.labelSearchLevelsSortOrder = new System.Windows.Forms.Label();
            this.labelSearchLevelsPageNumber = new System.Windows.Forms.Label();
            this.btnSearchLevels = new System.Windows.Forms.Button();
            this.btnAddSearchResultsToPipeline = new System.Windows.Forms.Button();
            this.dataGridViewLevelResults = new System.Windows.Forms.DataGridView();
            this.logTextBox = new PR2PS.LevelImporter.CustomControls.DebugTextbox();
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
            this.tabAssignUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUserResuts)).BeginInit();
            this.tabImportFromFile.SuspendLayout();
            this.tabImportById.SuspendLayout();
            this.tabImportBySearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericPage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLevelResults)).BeginInit();
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
            this.toolStrip.Size = new System.Drawing.Size(984, 25);
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
            this.connDisconnBtn.Click += new System.EventHandler(this.connDisconnBtn_Click);
            // 
            // infoBtn
            // 
            this.infoBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.infoBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.infoBtn.Name = "infoBtn";
            this.infoBtn.Size = new System.Drawing.Size(74, 22);
            this.infoBtn.Text = "Information";
            this.infoBtn.Click += new System.EventHandler(this.infoBtn_Click);
            // 
            // exitBtn
            // 
            this.exitBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.exitBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.exitBtn.Name = "exitBtn";
            this.exitBtn.Size = new System.Drawing.Size(29, 22);
            this.exitBtn.Text = "Exit";
            this.exitBtn.Click += new System.EventHandler(this.exitBtn_Click);
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
            this.splitContainerMain.Size = new System.Drawing.Size(984, 656);
            this.splitContainerMain.SplitterDistance = 758;
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
            this.splitContainerSub.Size = new System.Drawing.Size(758, 656);
            this.splitContainerSub.SplitterDistance = 485;
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
            this.tabControl.Size = new System.Drawing.Size(756, 483);
            this.tabControl.TabIndex = 0;
            // 
            // tabAssignUser
            // 
            this.tabAssignUser.BackColor = System.Drawing.SystemColors.Control;
            this.tabAssignUser.Controls.Add(this.dataGridViewUserResuts);
            this.tabAssignUser.Controls.Add(this.btnAssignUser);
            this.tabAssignUser.Controls.Add(this.btnSearchUser);
            this.tabAssignUser.Controls.Add(this.comboBoxSearchUserMode);
            this.tabAssignUser.Controls.Add(this.textBoxSearchUserTerm);
            this.tabAssignUser.Controls.Add(this.labelSearchUserMode);
            this.tabAssignUser.Controls.Add(this.labelSearchUserTerm);
            this.tabAssignUser.Location = new System.Drawing.Point(4, 22);
            this.tabAssignUser.Name = "tabAssignUser";
            this.tabAssignUser.Padding = new System.Windows.Forms.Padding(3);
            this.tabAssignUser.Size = new System.Drawing.Size(748, 457);
            this.tabAssignUser.TabIndex = 0;
            this.tabAssignUser.Text = "Assign User";
            // 
            // dataGridViewUserResuts
            // 
            this.dataGridViewUserResuts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewUserResuts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewUserResuts.Location = new System.Drawing.Point(10, 60);
            this.dataGridViewUserResuts.Name = "dataGridViewUserResuts";
            this.dataGridViewUserResuts.Size = new System.Drawing.Size(732, 391);
            this.dataGridViewUserResuts.TabIndex = 6;
            // 
            // btnAssignUser
            // 
            this.btnAssignUser.Location = new System.Drawing.Point(318, 31);
            this.btnAssignUser.Name = "btnAssignUser";
            this.btnAssignUser.Size = new System.Drawing.Size(75, 23);
            this.btnAssignUser.TabIndex = 5;
            this.btnAssignUser.Text = "Assign";
            this.btnAssignUser.UseVisualStyleBackColor = true;
            // 
            // btnSearchUser
            // 
            this.btnSearchUser.Location = new System.Drawing.Point(237, 31);
            this.btnSearchUser.Name = "btnSearchUser";
            this.btnSearchUser.Size = new System.Drawing.Size(75, 23);
            this.btnSearchUser.TabIndex = 4;
            this.btnSearchUser.Text = "Search";
            this.btnSearchUser.UseVisualStyleBackColor = true;
            // 
            // comboBoxSearchUserMode
            // 
            this.comboBoxSearchUserMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSearchUserMode.FormattingEnabled = true;
            this.comboBoxSearchUserMode.Items.AddRange(new object[] {
            "User Name",
            "Id"});
            this.comboBoxSearchUserMode.Location = new System.Drawing.Point(81, 33);
            this.comboBoxSearchUserMode.Name = "comboBoxSearchUserMode";
            this.comboBoxSearchUserMode.Size = new System.Drawing.Size(150, 21);
            this.comboBoxSearchUserMode.TabIndex = 3;
            // 
            // textBoxSearchUserTerm
            // 
            this.textBoxSearchUserTerm.Location = new System.Drawing.Point(81, 6);
            this.textBoxSearchUserTerm.Name = "textBoxSearchUserTerm";
            this.textBoxSearchUserTerm.Size = new System.Drawing.Size(150, 20);
            this.textBoxSearchUserTerm.TabIndex = 2;
            // 
            // labelSearchUserMode
            // 
            this.labelSearchUserMode.AutoSize = true;
            this.labelSearchUserMode.Location = new System.Drawing.Point(7, 36);
            this.labelSearchUserMode.Name = "labelSearchUserMode";
            this.labelSearchUserMode.Size = new System.Drawing.Size(56, 13);
            this.labelSearchUserMode.TabIndex = 1;
            this.labelSearchUserMode.Text = "Search By";
            // 
            // labelSearchUserTerm
            // 
            this.labelSearchUserTerm.AutoSize = true;
            this.labelSearchUserTerm.Location = new System.Drawing.Point(7, 9);
            this.labelSearchUserTerm.Name = "labelSearchUserTerm";
            this.labelSearchUserTerm.Size = new System.Drawing.Size(68, 13);
            this.labelSearchUserTerm.TabIndex = 0;
            this.labelSearchUserTerm.Text = "Search Term";
            // 
            // tabImportFromFile
            // 
            this.tabImportFromFile.BackColor = System.Drawing.SystemColors.Control;
            this.tabImportFromFile.Controls.Add(this.listBoxLocalLevels);
            this.tabImportFromFile.Controls.Add(this.btnAddLocalToPipeline);
            this.tabImportFromFile.Controls.Add(this.btnBrowse);
            this.tabImportFromFile.Location = new System.Drawing.Point(4, 22);
            this.tabImportFromFile.Name = "tabImportFromFile";
            this.tabImportFromFile.Padding = new System.Windows.Forms.Padding(3);
            this.tabImportFromFile.Size = new System.Drawing.Size(748, 457);
            this.tabImportFromFile.TabIndex = 1;
            this.tabImportFromFile.Text = "Import From File";
            // 
            // listBoxLocalLevels
            // 
            this.listBoxLocalLevels.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxLocalLevels.FormattingEnabled = true;
            this.listBoxLocalLevels.Location = new System.Drawing.Point(8, 37);
            this.listBoxLocalLevels.Name = "listBoxLocalLevels";
            this.listBoxLocalLevels.Size = new System.Drawing.Size(734, 394);
            this.listBoxLocalLevels.TabIndex = 2;
            // 
            // btnAddLocalToPipeline
            // 
            this.btnAddLocalToPipeline.Location = new System.Drawing.Point(113, 7);
            this.btnAddLocalToPipeline.Name = "btnAddLocalToPipeline";
            this.btnAddLocalToPipeline.Size = new System.Drawing.Size(100, 23);
            this.btnAddLocalToPipeline.TabIndex = 1;
            this.btnAddLocalToPipeline.Text = "Add To Pipeline";
            this.btnAddLocalToPipeline.UseVisualStyleBackColor = true;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(7, 7);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(100, 23);
            this.btnBrowse.TabIndex = 0;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            // 
            // tabImportById
            // 
            this.tabImportById.BackColor = System.Drawing.SystemColors.Control;
            this.tabImportById.Controls.Add(this.btnAddExactToPipeLine);
            this.tabImportById.Controls.Add(this.textBoxLevelVersion);
            this.tabImportById.Controls.Add(this.textBoxLevelId);
            this.tabImportById.Controls.Add(this.labelLevelVersion);
            this.tabImportById.Controls.Add(this.labelLevelId);
            this.tabImportById.Location = new System.Drawing.Point(4, 22);
            this.tabImportById.Name = "tabImportById";
            this.tabImportById.Size = new System.Drawing.Size(748, 457);
            this.tabImportById.TabIndex = 2;
            this.tabImportById.Text = "Import By Id";
            // 
            // btnAddExactToPipeLine
            // 
            this.btnAddExactToPipeLine.Location = new System.Drawing.Point(191, 27);
            this.btnAddExactToPipeLine.Name = "btnAddExactToPipeLine";
            this.btnAddExactToPipeLine.Size = new System.Drawing.Size(100, 23);
            this.btnAddExactToPipeLine.TabIndex = 4;
            this.btnAddExactToPipeLine.Text = "Add To Pipeline";
            this.btnAddExactToPipeLine.UseVisualStyleBackColor = true;
            // 
            // textBoxLevelVersion
            // 
            this.textBoxLevelVersion.Location = new System.Drawing.Point(85, 29);
            this.textBoxLevelVersion.Name = "textBoxLevelVersion";
            this.textBoxLevelVersion.Size = new System.Drawing.Size(100, 20);
            this.textBoxLevelVersion.TabIndex = 3;
            // 
            // textBoxLevelId
            // 
            this.textBoxLevelId.Location = new System.Drawing.Point(85, 3);
            this.textBoxLevelId.Name = "textBoxLevelId";
            this.textBoxLevelId.Size = new System.Drawing.Size(100, 20);
            this.textBoxLevelId.TabIndex = 2;
            // 
            // labelLevelVersion
            // 
            this.labelLevelVersion.AutoSize = true;
            this.labelLevelVersion.Location = new System.Drawing.Point(7, 32);
            this.labelLevelVersion.Name = "labelLevelVersion";
            this.labelLevelVersion.Size = new System.Drawing.Size(42, 13);
            this.labelLevelVersion.TabIndex = 1;
            this.labelLevelVersion.Text = "Version";
            // 
            // labelLevelId
            // 
            this.labelLevelId.AutoSize = true;
            this.labelLevelId.Location = new System.Drawing.Point(7, 6);
            this.labelLevelId.Name = "labelLevelId";
            this.labelLevelId.Size = new System.Drawing.Size(45, 13);
            this.labelLevelId.TabIndex = 0;
            this.labelLevelId.Text = "Level Id";
            // 
            // tabImportBySearch
            // 
            this.tabImportBySearch.BackColor = System.Drawing.SystemColors.Control;
            this.tabImportBySearch.Controls.Add(this.dataGridViewLevelResults);
            this.tabImportBySearch.Controls.Add(this.btnAddSearchResultsToPipeline);
            this.tabImportBySearch.Controls.Add(this.btnSearchLevels);
            this.tabImportBySearch.Controls.Add(this.labelSearchLevelsPageNumber);
            this.tabImportBySearch.Controls.Add(this.labelSearchLevelsSortOrder);
            this.tabImportBySearch.Controls.Add(this.labelSearchLevelsSortBy);
            this.tabImportBySearch.Controls.Add(this.labelSearchLevelsBy);
            this.tabImportBySearch.Controls.Add(this.labelSearchLevelsTerm);
            this.tabImportBySearch.Controls.Add(this.numericPage);
            this.tabImportBySearch.Controls.Add(this.comboBoxSortOrder);
            this.tabImportBySearch.Controls.Add(this.comboBoxSortBy);
            this.tabImportBySearch.Controls.Add(this.comboBoxSearchBy);
            this.tabImportBySearch.Controls.Add(this.textBoxLevelsSearchTerm);
            this.tabImportBySearch.Location = new System.Drawing.Point(4, 22);
            this.tabImportBySearch.Name = "tabImportBySearch";
            this.tabImportBySearch.Size = new System.Drawing.Size(748, 457);
            this.tabImportBySearch.TabIndex = 3;
            this.tabImportBySearch.Text = "Import By Search";
            // 
            // numericPage
            // 
            this.numericPage.Location = new System.Drawing.Point(85, 56);
            this.numericPage.Name = "numericPage";
            this.numericPage.Size = new System.Drawing.Size(100, 20);
            this.numericPage.TabIndex = 4;
            // 
            // comboBoxSortOrder
            // 
            this.comboBoxSortOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSortOrder.FormattingEnabled = true;
            this.comboBoxSortOrder.Items.AddRange(new object[] {
            "Descending",
            "Ascending"});
            this.comboBoxSortOrder.Location = new System.Drawing.Point(265, 30);
            this.comboBoxSortOrder.Name = "comboBoxSortOrder";
            this.comboBoxSortOrder.Size = new System.Drawing.Size(100, 21);
            this.comboBoxSortOrder.TabIndex = 3;
            // 
            // comboBoxSortBy
            // 
            this.comboBoxSortBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSortBy.FormattingEnabled = true;
            this.comboBoxSortBy.Items.AddRange(new object[] {
            "Date",
            "Alphabetical",
            "Rating",
            "Popularity"});
            this.comboBoxSortBy.Location = new System.Drawing.Point(85, 29);
            this.comboBoxSortBy.Name = "comboBoxSortBy";
            this.comboBoxSortBy.Size = new System.Drawing.Size(100, 21);
            this.comboBoxSortBy.TabIndex = 2;
            // 
            // comboBoxSearchBy
            // 
            this.comboBoxSearchBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSearchBy.FormattingEnabled = true;
            this.comboBoxSearchBy.Items.AddRange(new object[] {
            "User Name",
            "Course Line"});
            this.comboBoxSearchBy.Location = new System.Drawing.Point(265, 3);
            this.comboBoxSearchBy.Name = "comboBoxSearchBy";
            this.comboBoxSearchBy.Size = new System.Drawing.Size(100, 21);
            this.comboBoxSearchBy.TabIndex = 1;
            // 
            // textBoxLevelsSearchTerm
            // 
            this.textBoxLevelsSearchTerm.Location = new System.Drawing.Point(85, 3);
            this.textBoxLevelsSearchTerm.Name = "textBoxLevelsSearchTerm";
            this.textBoxLevelsSearchTerm.Size = new System.Drawing.Size(100, 20);
            this.textBoxLevelsSearchTerm.TabIndex = 0;
            // 
            // runBtn
            // 
            this.runBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.runBtn.Location = new System.Drawing.Point(5, 613);
            this.runBtn.Name = "runBtn";
            this.runBtn.Size = new System.Drawing.Size(204, 30);
            this.runBtn.TabIndex = 3;
            this.runBtn.Text = "Run Import Procedure";
            this.runBtn.UseVisualStyleBackColor = true;
            // 
            // delFromPipelineBtn
            // 
            this.delFromPipelineBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.delFromPipelineBtn.Location = new System.Drawing.Point(5, 577);
            this.delFromPipelineBtn.Name = "delFromPipelineBtn";
            this.delFromPipelineBtn.Size = new System.Drawing.Size(204, 30);
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
            this.pipelineListBox.Size = new System.Drawing.Size(204, 537);
            this.pipelineListBox.TabIndex = 1;
            // 
            // pipelineLabel
            // 
            this.pipelineLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pipelineLabel.AutoSize = true;
            this.pipelineLabel.Location = new System.Drawing.Point(2, 4);
            this.pipelineLabel.Name = "pipelineLabel";
            this.pipelineLabel.Size = new System.Drawing.Size(171, 13);
            this.pipelineLabel.TabIndex = 0;
            this.pipelineLabel.Text = "Pipeline Of Levels To Be Imported:";
            // 
            // labelSearchLevelsTerm
            // 
            this.labelSearchLevelsTerm.AutoSize = true;
            this.labelSearchLevelsTerm.Location = new System.Drawing.Point(8, 6);
            this.labelSearchLevelsTerm.Name = "labelSearchLevelsTerm";
            this.labelSearchLevelsTerm.Size = new System.Drawing.Size(68, 13);
            this.labelSearchLevelsTerm.TabIndex = 5;
            this.labelSearchLevelsTerm.Text = "Search Term";
            // 
            // labelSearchLevelsBy
            // 
            this.labelSearchLevelsBy.AutoSize = true;
            this.labelSearchLevelsBy.Location = new System.Drawing.Point(203, 6);
            this.labelSearchLevelsBy.Margin = new System.Windows.Forms.Padding(15, 0, 3, 0);
            this.labelSearchLevelsBy.Name = "labelSearchLevelsBy";
            this.labelSearchLevelsBy.Size = new System.Drawing.Size(56, 13);
            this.labelSearchLevelsBy.TabIndex = 6;
            this.labelSearchLevelsBy.Text = "Search By";
            // 
            // labelSearchLevelsSortBy
            // 
            this.labelSearchLevelsSortBy.AutoSize = true;
            this.labelSearchLevelsSortBy.Location = new System.Drawing.Point(8, 32);
            this.labelSearchLevelsSortBy.Name = "labelSearchLevelsSortBy";
            this.labelSearchLevelsSortBy.Size = new System.Drawing.Size(41, 13);
            this.labelSearchLevelsSortBy.TabIndex = 7;
            this.labelSearchLevelsSortBy.Text = "Sort By";
            // 
            // labelSearchLevelsSortOrder
            // 
            this.labelSearchLevelsSortOrder.AutoSize = true;
            this.labelSearchLevelsSortOrder.Location = new System.Drawing.Point(203, 33);
            this.labelSearchLevelsSortOrder.Margin = new System.Windows.Forms.Padding(15, 0, 3, 0);
            this.labelSearchLevelsSortOrder.Name = "labelSearchLevelsSortOrder";
            this.labelSearchLevelsSortOrder.Size = new System.Drawing.Size(55, 13);
            this.labelSearchLevelsSortOrder.TabIndex = 8;
            this.labelSearchLevelsSortOrder.Text = "Sort Order";
            // 
            // labelSearchLevelsPageNumber
            // 
            this.labelSearchLevelsPageNumber.AutoSize = true;
            this.labelSearchLevelsPageNumber.Location = new System.Drawing.Point(8, 58);
            this.labelSearchLevelsPageNumber.Name = "labelSearchLevelsPageNumber";
            this.labelSearchLevelsPageNumber.Size = new System.Drawing.Size(32, 13);
            this.labelSearchLevelsPageNumber.TabIndex = 9;
            this.labelSearchLevelsPageNumber.Text = "Page";
            // 
            // btnSearchLevels
            // 
            this.btnSearchLevels.Location = new System.Drawing.Point(11, 82);
            this.btnSearchLevels.Name = "btnSearchLevels";
            this.btnSearchLevels.Size = new System.Drawing.Size(100, 23);
            this.btnSearchLevels.TabIndex = 10;
            this.btnSearchLevels.Text = "Search";
            this.btnSearchLevels.UseVisualStyleBackColor = true;
            // 
            // btnAddSearchResultsToPipeline
            // 
            this.btnAddSearchResultsToPipeline.Location = new System.Drawing.Point(117, 82);
            this.btnAddSearchResultsToPipeline.Name = "btnAddSearchResultsToPipeline";
            this.btnAddSearchResultsToPipeline.Size = new System.Drawing.Size(100, 23);
            this.btnAddSearchResultsToPipeline.TabIndex = 11;
            this.btnAddSearchResultsToPipeline.Text = "Add To Pipeline";
            this.btnAddSearchResultsToPipeline.UseVisualStyleBackColor = true;
            // 
            // dataGridViewLevelResults
            // 
            this.dataGridViewLevelResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewLevelResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewLevelResults.Location = new System.Drawing.Point(11, 112);
            this.dataGridViewLevelResults.Name = "dataGridViewLevelResults";
            this.dataGridViewLevelResults.Size = new System.Drawing.Size(734, 342);
            this.dataGridViewLevelResults.TabIndex = 12;
            // 
            // logTextBox
            // 
            this.logTextBox.BackColor = System.Drawing.Color.Black;
            this.logTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logTextBox.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.logTextBox.ForeColor = System.Drawing.Color.LightGray;
            this.logTextBox.Location = new System.Drawing.Point(0, 0);
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ReadOnly = true;
            this.logTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.logTextBox.Size = new System.Drawing.Size(756, 165);
            this.logTextBox.TabIndex = 0;
            this.logTextBox.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 681);
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
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerSub)).EndInit();
            this.splitContainerSub.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabAssignUser.ResumeLayout(false);
            this.tabAssignUser.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUserResuts)).EndInit();
            this.tabImportFromFile.ResumeLayout(false);
            this.tabImportById.ResumeLayout(false);
            this.tabImportById.PerformLayout();
            this.tabImportBySearch.ResumeLayout(false);
            this.tabImportBySearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericPage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLevelResults)).EndInit();
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
        private System.Windows.Forms.ListBox pipelineListBox;
        private System.Windows.Forms.Label pipelineLabel;
        private System.Windows.Forms.Button runBtn;
        private System.Windows.Forms.Button delFromPipelineBtn;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabAssignUser;
        private System.Windows.Forms.TabPage tabImportFromFile;
        private System.Windows.Forms.TabPage tabImportById;
        private System.Windows.Forms.TabPage tabImportBySearch;
        private System.Windows.Forms.ComboBox comboBoxSearchUserMode;
        private System.Windows.Forms.TextBox textBoxSearchUserTerm;
        private System.Windows.Forms.Label labelSearchUserMode;
        private System.Windows.Forms.Label labelSearchUserTerm;
        private System.Windows.Forms.Button btnAssignUser;
        private System.Windows.Forms.Button btnSearchUser;
        private System.Windows.Forms.DataGridView dataGridViewUserResuts;
        private System.Windows.Forms.Button btnAddLocalToPipeline;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.ListBox listBoxLocalLevels;
        private System.Windows.Forms.Label labelLevelVersion;
        private System.Windows.Forms.Label labelLevelId;
        private System.Windows.Forms.Button btnAddExactToPipeLine;
        private System.Windows.Forms.TextBox textBoxLevelVersion;
        private System.Windows.Forms.TextBox textBoxLevelId;
        private System.Windows.Forms.ComboBox comboBoxSortOrder;
        private System.Windows.Forms.ComboBox comboBoxSortBy;
        private System.Windows.Forms.ComboBox comboBoxSearchBy;
        private System.Windows.Forms.TextBox textBoxLevelsSearchTerm;
        private System.Windows.Forms.NumericUpDown numericPage;
        private System.Windows.Forms.Label labelSearchLevelsBy;
        private System.Windows.Forms.Label labelSearchLevelsTerm;
        private System.Windows.Forms.Label labelSearchLevelsPageNumber;
        private System.Windows.Forms.Label labelSearchLevelsSortOrder;
        private System.Windows.Forms.Label labelSearchLevelsSortBy;
        private System.Windows.Forms.Button btnAddSearchResultsToPipeline;
        private System.Windows.Forms.Button btnSearchLevels;
        private System.Windows.Forms.DataGridView dataGridViewLevelResults;
        private CustomControls.DebugTextbox logTextBox;
    }
}

