using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        #endregion

        #region Constructor.

        public MainForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Form event handlers.

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Log("Initializing...");

            this.comboBoxSearchUserMode.SelectedIndex = 0;
            this.comboBoxSearchBy.SelectedIndex = 0;
            this.comboBoxSortBy.SelectedIndex = 0;
            this.comboBoxSortOrder.SelectedIndex = 0;

            this.MainForm_Resize(sender, e);

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

        #endregion

        private void Log(String message)
        {
            this.logTextBox.Write(message);
        }

        private void connDisconnBtn_Click(object sender, EventArgs e)
        {

        }

        private void infoBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, INFO, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
