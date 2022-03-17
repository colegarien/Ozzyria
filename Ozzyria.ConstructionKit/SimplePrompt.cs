using System;
using System.Windows.Forms;

namespace Ozzyria.ConstructionKit
{
    public partial class SimplePrompt : Form
    {
        public SimplePrompt(string title="Prompt")
        {
            InitializeComponent();
            this.Text = title;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public string GetPromptInput()
        {
            return this.textBoxPromptInput?.Text?.Trim()?.ToLower() ?? "";
        }
    }
}
