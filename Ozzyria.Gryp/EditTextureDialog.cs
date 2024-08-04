using Ozzyria.Content;

namespace Ozzyria.Gryp
{
    public partial class EditTextureDialog : Form
    {
        internal string TextureResult { get; set; }

        public EditTextureDialog(string currentDrawableId)
        {
            InitializeComponent();

            var registry = Registry.GetInstance();
            cmbDrawables.Items.AddRange(registry.Drawables.Keys.ToArray());
            if(registry.Drawables.ContainsKey(currentDrawableId))
            {
                cmbDrawables.SelectedItem = currentDrawableId;
            }
        }

        /// <summary>
        /// Extract values from the form elements and pack them into Result Object
        /// </summary>
        private void PopulateResult()
        {
            TextureResult = cmbDrawables.SelectedItem?.ToString() ?? "";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            PopulateResult();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
