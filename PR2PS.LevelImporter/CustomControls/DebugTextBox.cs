using System;
using System.Drawing;
using System.Windows.Forms;

namespace PR2PS.LevelImporter.CustomControls
{
    public class DebugTextbox : RichTextBox
    {
        public DebugTextbox()
            : base()
        {
            this.BackColor = Color.Black;
            this.ForeColor = Color.LightGray;
            this.Font = new Font("Consolas", 9.75f);
            this.ReadOnly = true;
            this.ScrollBars = RichTextBoxScrollBars.Vertical;
        }

        private void WriteAction(String str, Color color)
        {
            this.SelectionStart = this.TextLength;
            this.SelectionLength = 0;

            this.SelectionColor = color;
            String line = String.Format("[{0}]: {1}{2}", DateTime.Now.ToLongTimeString(), str, Environment.NewLine);
            this.AppendText(line);
            this.SelectionColor = this.ForeColor;

            this.SelectionStart = this.TextLength;
            this.ScrollToCaret();
        }

        public void Write(String str, Color color)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => this.WriteAction(str, color)));
            }
            else
            {
                this.WriteAction(str, color);
            }
        }

        public void Write(String str)
        {
            this.Write(str, this.ForeColor);
        }
    }
}
