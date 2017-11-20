using PR2PS.LevelImporter.Core;
using PR2PS.LevelImporter.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static PR2PS.LevelImporter.Core.Enums;

namespace PR2PS.LevelImporter
{
    public partial class MainForm : Form
    {
        #region Fields.

        private const String INFO =
            "Use this application to import existing PR2 levels into PR2PS database. Instructions to use:\n\n"
            + "1.) Attach PR2PS database using Connect button.\n\n"
            + "2.) Use the Assign User tab to search the databse for existing user:\n"
            + "- Levels added to the pipeline will have that user assigned to them\n"
            + "- User can be changed before adding another level to the pipeline\n\n"
            + "3.) Use relevant tabs to search and add levels to the pipeline:\n"
            + "- Import From File - Used to import downloaded levels\n"
            + "- Import By Id - Used to import live levels specified by level id\n"
            + "- Import By Search - Used to search and import live levels according to given criteria\n\n"
            + "4.) Click on Run Import Procedure to initiate the import process.";

        private DatabaseConnector database;
        private UserModel selectedUser;

        #endregion

        #region Constructor.

        public MainForm()
        {

            InitializeComponent();
        }

        #endregion

        #region Form event handlers.

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.Log("Initializing...");

            this.database = new DatabaseConnector();

            this.comboBoxSearchUserMode.SelectedIndex = 0;
            this.comboBoxSearchBy.SelectedIndex = 0;
            this.comboBoxSortBy.SelectedIndex = 0;
            this.comboBoxSortOrder.SelectedIndex = 0;

            this.btnConnectMainDb.Tag = AttachType.Main;
            this.btnConnectLevelsDb.Tag = AttachType.Levels;

            this.listBoxLocalLevels.DataSource = new List<String>();
            this.pipelineListBox.DataSource = new List<LevelModel>();

            this.MainForm_Resize(null, null);

            this.Log("Ready.");
        }

        private void MainForm_Resize(Object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                this.splitContainerMain.SplitterDistance = this.splitContainerMain.Width - 250;
                this.splitContainerSub.SplitterDistance = this.splitContainerSub.Height - 150;
            }
        }

        private void MainForm_FormClosing(Object sender, FormClosingEventArgs e)
        {
            this.Log("Closing and cleaning up...");

            this.database.Dispose();
        }

        #endregion

        #region Menu buttons handlers.

        private void btnAttachDb_Click(Object sender, EventArgs e)
        {
            ToolStripButton btn = sender as ToolStripButton;
            if (btn == null)
            {
                return;
            }

            AttachType? attachType = btn.Tag as AttachType?;
            if (!attachType.HasValue)
            {
                return;
            }

            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                fileDialog.Filter = "SQLite Database File |*.sqlite";

                if (fileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    Log(String.Format("Attempting to attach database file '{0}'.", fileDialog.FileName));

                    try
                    {
                        this.database.Attach(fileDialog.FileName, attachType.Value);
                        btn.Enabled = false;

                        Log("Database attached successfully!", Color.LimeGreen);
                    }
                    catch (DbValidationException ex)
                    {
                        Log(String.Concat("Attached database is invalid: ", ex.Message), Color.Red);
                    }
                    catch (Exception ex)
                    {
                        Log(String.Concat("Error occured while attaching database:\n", ex), Color.Red);
                    }
                }
            }
        }

        private void infoBtn_Click(Object sender, EventArgs e)
        {
            MessageBox.Show(this, INFO, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void exitBtn_Click(Object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion

        #region Search user tab handlers.

        private void btnSearchUser_Click(Object sender, EventArgs e)
        {
            try
            {
                String term = this.textBoxSearchUserTerm.Text;
                if (String.IsNullOrEmpty(term))
                {
                    Log("Please enter search term into the text box.", Color.Orange);
                    return;
                }

                if (!this.database.IsMainDbAttached)
                {
                    Log("You need to attach the main database firstly.", Color.Orange);
                    return;
                }

                Log("Searching...");

                IEnumerable<UserModel> results = this.database.FindUsers(term, (UserSearchMode)this.comboBoxSearchUserMode.SelectedIndex);

                Log("Search completed.");

                this.dataGridViewUserResuts.DataSource = results;
            }
            catch (Exception ex)
            {
                Log(String.Concat("Error occured while searching database for users:\n", ex), Color.Red);
            }
        }

        private void btnAssignUser_Click(Object sender, EventArgs e)
        {
            try
            {
                UserModel selected = (UserModel)this.dataGridViewUserResuts.CurrentRow?.DataBoundItem;
                if (selected == null)
                {
                    Log("You have to select user from the grid below.", Color.Orange);
                    return;
                }

                this.selectedUser = selected;

                Log(String.Format("User {0} selected.", selected.ToString()));
            }
            catch (Exception ex)
            {
                Log(String.Concat("Error occured while selecting user:\n", ex), Color.Red);
            }
        }

        #endregion

        #region Import from file tab handlers.

        private void btnBrowse_Click(Object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog fileDialog = new OpenFileDialog { Multiselect = true })
                {
                    fileDialog.Filter = "All Files |*.*";

                    if (fileDialog.ShowDialog(this) == DialogResult.OK)
                    {
                        List<String> dataSource = (List<String>)this.listBoxLocalLevels.DataSource;
                        dataSource.AddRange(fileDialog.FileNames);

                        this.RebindListBoxDataSource(this.listBoxLocalLevels, dataSource, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Log(String.Concat("Error occured while loading levels:\n", ex), Color.Red);
            }
        }

        private void btnAddLocalToPipeline_Click(Object sender, EventArgs e)
        {
            try
            {
                if (this.selectedUser == null)
                {
                    Log("You have to select the user who will become the owner of selected levels.", Color.Orange);
                    return;
                }

                String[] selected = this.listBoxLocalLevels.SelectedItems.Cast<String>().ToArray();
                if (!selected.Any())
                {
                    Log("You have to select level(s) from the list below.", Color.Orange);
                    return;
                }

                List<String> dataSource = (List<String>)this.listBoxLocalLevels.DataSource;
                dataSource = dataSource.Except(selected).ToList();
                this.RebindListBoxDataSource(this.listBoxLocalLevels, dataSource, null);

                this.AddToPipeline(selected.Select(f => new LevelModel(this.selectedUser, f)));

                Log(String.Format("Successfully added {0} item(s) to the pipeline.", selected.Length));
            }
            catch (Exception ex)
            {
                Log(String.Concat("Error occured during while adding levels to the pipeline:\n", ex), Color.Red);
            }
        }

        #endregion

        private void AddToPipeline(IEnumerable<LevelModel> levels)
        {
            if (levels == null || !levels.Any())
            {
                return;
            }

            List<LevelModel> dataSource = (List<LevelModel>)this.pipelineListBox.DataSource;
            dataSource.AddRange(levels);

            this.RebindListBoxDataSource(this.pipelineListBox, dataSource, "Render");
        }

        private void RebindListBoxDataSource(ListBox listBox, Object dataSource, String displayMember)
        {
            listBox.DataSource = null; // We have to do this for some strange reason...
            listBox.DataSource = dataSource;
            listBox.DisplayMember = displayMember;
        }

        #region Logging methods.

        private void Log(String message)
        {
            this.logTextBox.Write(message);
        }

        private void Log(String message, Color color)
        {
            this.logTextBox.Write(message, color);
        }

        #endregion
    }
}
