using Ozzyria.Gryp.Models.Form;

namespace Ozzyria.Gryp
{
    public partial class NewMapDialog : Form
    {
        internal NewMapSettings NewMapResult;

        public NewMapDialog()
        {
            InitializeComponent();
            NewMapResult = new NewMapSettings();
        }

        /// <summary>
        /// Extract values from the form elements and pack them into Result Object
        /// </summary>
        private void PopulateResult()
        {
            NewMapResult.Width = (int)numWidth.Value;
            NewMapResult.Height = (int)numHeight.Value;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            this.PopulateResult();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
