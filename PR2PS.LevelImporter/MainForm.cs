﻿using System;
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
        public MainForm()
        {
            InitializeComponent();
        }

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

        private void Log(String message)
        {
            this.logTextBox.Write(message);
        }
    }
}
