using Ozzyria.Content.Models.Area;
using Ozzyria.Gryp.Models.Form;
using System.Text.RegularExpressions;

namespace Ozzyria.Gryp.UI.Dialogs
{
    public partial class NewMapDialog : Form
    {
        internal const string MAP_ID_REGEX = "^[a-z0-9_-]*$";
        internal const string MAP_NAME_REGEX = "^[a-zA-Z0-9()\', _-]*$";
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
            NewMapResult.Id = txtId.Text.ToLower().Trim();
            NewMapResult.DisplayName = txtDisplayName.Text.Trim();
            NewMapResult.Width = (int)numWidth.Value;
            NewMapResult.Height = (int)numHeight.Value;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            PopulateResult();

            List<string> errors = new List<string>();
            if(NewMapResult.Id == "")
            {
                errors.Add("ID is required.");
            }
            else if (Regex.Matches(NewMapResult.Id, "^[a-z0-9_-]*$").Count <= 0)
            {
                errors.Add("ID must be alpha-numeric (plus _ and -).");
            }
            else if (AreaData.Exists(NewMapResult.Id))
            {
                errors.Add("ID `"+ NewMapResult.Id + "` is already taken.");
            }

            if (NewMapResult.DisplayName == "")
            {
                errors.Add("Display Name required.");
            }
            else if (Regex.Matches(NewMapResult.DisplayName, MAP_NAME_REGEX).Count <= 0)
            {
                errors.Add("Display Name must match "+ MAP_NAME_REGEX + ".");
            }

            if(NewMapResult.Width <= 0 || NewMapResult.Height <= 0)
            {
                errors.Add("Dimensions must be at-least 1x1.");
            }

            if (errors.Count <= 0)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show(
                    "You have invalid input:\r\n  - " + String.Join("\r\n  - ", errors),
                    "Invalid Input!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
