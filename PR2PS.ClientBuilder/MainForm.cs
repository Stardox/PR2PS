using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PR2PS.ClientBuilder
{
    public partial class MainForm : Form
    {
        #region Constants

        private const String INFO =
            "Use this application to 'build' PR2 client which will make requests to the specified URLs "
            + "instead of the default ones (pr2hub.com).\n\n"
            + "User is required to specify following parameters:\n"
            + "- Path to Standalone Flash Projector executable (not included)\n"
            + "- Path to modified PR2 client SWF file (included)\n"
            + "- URL of game web server\n"
            + "- URL of levels web server\n"
            + "- Output type (only *.BAT is supported at the moment)\n"
            + "- Destination folder (which can be distributed)\n\n"
            + "After build is finished, the destination folder should contain copy of the projector executable, "
            + "the modified client and *.BAT file which can be used to launch the client. The content of that "
            + "folder can be then sent to your friends so they can join your server.\n\n"
            + "If you need further help, click on '?' button.\n";

        private const String HELP_PROJECTOR =
            "You need to provide path to Standalone Flash Projector executable file. The file is not included "
            + "in this project and you will have to download it yourself from the official Adobe website.\n\n"
            + "Link to the downloads section:\n"
            + "https://www.adobe.com/support/flashplayer/debug_downloads.html\n\n"
            + "In the 'Windows' section look for link 'Download the Flash Player projector', thats the file.";

        private const String HELP_CLIENT =
            "You need to provide path to the modified PR2 client SWF file. The file is included in this project "
            + "and is called 'client.swf'. You can find it either in 'bin' folders or in root folder of this project.";

        private const String HELP_WEB_URL =
           "You need to provide URL of game web server. Put here the same address as the address which you specified "
           + "on 'PR2PS.Web' startup. Valid examples:\n"
           + "- http://localhost:8080\n"
           + "- https://mydomain.com";

        private const String HELP_LEVELS_URL =
           "Since the level editor functionality is not yet implemented, you can leave the "
           + "'http://pr2hub.com/levels' URL as it is, so that the client can download the PR2 maps from there. "
           + "Once the level editor and the level host functionalities will be implemented, you can change this.";

        private const String HELP_DESTINATION =
            "You need to specify the destination folder path. Once the build finishes, this folder will contain "
            + "all the files necessary to launch the modified PR2 client and connect to the private server.\n\n"
            + "You can compress the folder and send it to your friends so that they can connect to your server.";

        #endregion

        private BackgroundWorker buildWorker;

        public MainForm()
        {
            InitializeComponent();
        }

        #region Event handlers

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.buildWorker = new BackgroundWorker();
            this.buildWorker.WorkerReportsProgress = true;
            this.buildWorker.DoWork += buildWorker_DoWork;
            this.buildWorker.ProgressChanged += buildWorker_ProgressChanged;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.buildWorker != null && this.buildWorker.IsBusy)
            {
                e.Cancel = true;
            }
        }

        private void btnBrowseFlashProjector_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                fileDialog.Filter = "Executables |*.exe";
                if (fileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    this.textBoxFlashProjector.Text = fileDialog.FileName;
                }
            }
        }

        private void btnBrowseClientFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                fileDialog.Filter = "Shockwave Flash |*.swf";
                if (fileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    this.textBoxClientFile.Text = fileDialog.FileName;
                }
            }
        }

        private void btnBrowseDestinationFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog(this) == DialogResult.OK)
                {
                    this.textBoxDestinationFolder.Text = folderDialog.SelectedPath;
                }
            }
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, INFO, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnHelpFlashProjector_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, HELP_PROJECTOR, "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnHelpClientFile_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, HELP_CLIENT, "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnHelpWebServerURL_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, HELP_WEB_URL, "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnHelpLevelsServerURL_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, HELP_LEVELS_URL, "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnHelpDestinationFolder_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, HELP_DESTINATION, "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnBuild_Click(object sender, EventArgs e)
        {
            if (this.buildWorker.IsBusy)
            {
                MessageBox.Show(this, "Build process is already in progress.", "Busy", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Display errors if any.
            List<String> buildErrors = this.getBuildErrors();
            if (buildErrors.Any())
            {
                String errorMessage = String.Concat(
                    "Can not proceed with the build due to following errors:\n\n",
                    buildErrors.Aggregate((e1, e2) => String.Concat(e1, '\n', e2)));
                MessageBox.Show(this, errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.setComponentsStatus(false);
            this.buildWorker.RunWorkerAsync();
        }

        void buildWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                this.buildWorker.ReportProgress(1);
                File.Copy(
                    this.textBoxFlashProjector.Text,
                    Path.Combine(this.textBoxDestinationFolder.Text, Path.GetFileName(this.textBoxFlashProjector.Text)),
                    true);

                this.buildWorker.ReportProgress(2);
                File.Copy(
                    this.textBoxClientFile.Text,
                    Path.Combine(this.textBoxDestinationFolder.Text, Path.GetFileName(this.textBoxClientFile.Text)),
                    true);

                this.buildWorker.ReportProgress(3);
                File.WriteAllText(Path.Combine(this.textBoxDestinationFolder.Text, "Run.bat"), this.getBATContent());

                this.buildWorker.ReportProgress(100);
            }
            catch (Exception ex)
            {
                this.buildWorker.ReportProgress(0, ex);
            }
        }

        void buildWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e == null)
            {
                return;
            }

            switch (e.ProgressPercentage)
            { 
                case 0:
                    this.labelStatus.Text = "Finished with error";
                    Exception ex = e.UserState as Exception;
                    if (ex == null)
                    {
                        MessageBox.Show(this, "Unknown error has occured.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show(
                            this,
                            String.Concat("Following error occured:\n\n", ex.Message, "\n\n", ex.StackTrace),
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                    this.setComponentsStatus(true);
                    break;
                case 1:
                    this.labelStatus.Text = "Copying Flash projector executable...";
                    break;
                case 2:
                    this.labelStatus.Text = "Copying client SWF file...";
                    break;
                case 3:
                    this.labelStatus.Text = "Generating BAT file...";
                    break;
                case 100:
                    this.labelStatus.Text = "Build finished";
                    MessageBox.Show(this, "Build process finished successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.setComponentsStatus(true);
                    break;
                default:
                    this.labelStatus.Text = "Ready";
                    this.setComponentsStatus(true);
                    break;
            }
        }

        #endregion

        #region Helper methods

        private void setComponentsStatus(Boolean enabled)
        {
            this.textBoxFlashProjector.Enabled = enabled;
            this.btnBrowseFlashProjector.Enabled = enabled;

            this.textBoxClientFile.Enabled = enabled;
            this.btnBrowseClientFile.Enabled = enabled;

            this.textBoxWebServerURL.Enabled = enabled;
            this.textBoxLevelsServerURL.Enabled = enabled;

            this.checkBoxOutputBAT.Enabled = enabled;
            //this.checkBoxOutputLNK.Enabled = enabled;

            this.textBoxDestinationFolder.Enabled = enabled;
            this.btnBrowseDestinationFolder.Enabled = enabled;

            this.btnBuild.Enabled = enabled;
            this.btnExit.Enabled = enabled;
        }

        private List<String> getBuildErrors()
        {
            List<String> errors = new List<String>();

            // 1.) Check if Flash projector executable has been specified.
            if (String.IsNullOrWhiteSpace(this.textBoxFlashProjector.Text))
            {
                errors.Add("- You need to specify the path to the Flash Projector executable.");
            }
            else if (!File.Exists(this.textBoxFlashProjector.Text))
            {
                errors.Add("- Could not find the Flash Projector executable. Make sure that you entered a valid path.");
            }

            // 2.) Check if client SWF file has been specified.
            if (String.IsNullOrWhiteSpace(this.textBoxClientFile.Text))
            {
                errors.Add("- You need to specify the path to the modded client SWF file.");
            }
            else if (!File.Exists(this.textBoxClientFile.Text))
            {
                errors.Add("- Could not find the modded client SWF file. Make sure that you entered a valid path.");
            }
            else if (Path.GetExtension(this.textBoxClientFile.Text).ToUpper() != ".SWF")
            {
                errors.Add("- Modded client has to be a SWF file.");
            }

            // 3.) Check if URLs have been specified.
            if (String.IsNullOrWhiteSpace(this.textBoxWebServerURL.Text))
            {
                errors.Add("- You need to specify URL of the web server.");
            }
            else if (!Uri.IsWellFormedUriString(this.textBoxWebServerURL.Text, UriKind.Absolute))
            {
                errors.Add("- Specified URL of the web server is not in proper format.");
            }

            if (String.IsNullOrWhiteSpace(this.textBoxLevelsServerURL.Text))
            {
                errors.Add("- You need to specify URL of the levels web server.");
            }
            else if (!Uri.IsWellFormedUriString(this.textBoxLevelsServerURL.Text, UriKind.Absolute))
            {
                errors.Add("- Specified URL of the levels web server is not in proper format.");
            }

            // 4.) Check if output type has been specified.
            if (!this.checkBoxOutputBAT.Checked && !this.checkBoxOutputLNK.Checked)
            {
                errors.Add("- You need to choose output type.");
            }

            // 5.) Check if output directory has been specified.
            if (String.IsNullOrWhiteSpace(this.textBoxDestinationFolder.Text))
            {
                errors.Add("- You need to specify the path to the output directory.");
            }
            else if (!Directory.Exists(this.textBoxDestinationFolder.Text))
            {
                errors.Add("- Path to the output directory is not valid.");
            }

            return errors;
        }

        private String getBATContent()
        {
            StringBuilder strBuilder = new StringBuilder();

            strBuilder.Append('\"');
            strBuilder.Append(Path.GetFileName(this.textBoxFlashProjector.Text));
            strBuilder.Append('\"');

            strBuilder.Append(' ');

            strBuilder.Append('\"');
            strBuilder.Append(Path.GetFileName(this.textBoxClientFile.Text));
            strBuilder.Append("?mainURL=");
            strBuilder.Append(Uri.EscapeDataString(this.textBoxWebServerURL.Text).Replace("%", "%%"));
            strBuilder.Append("&levelsURL=");
            strBuilder.Append(Uri.EscapeDataString(this.textBoxLevelsServerURL.Text).Replace("%", "%%"));
            strBuilder.Append('\"');

            return strBuilder.ToString();
        }

        #endregion
    }
}
