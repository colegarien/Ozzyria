using Ozzyria.Gryp.Models.Data;

namespace Ozzyria.Gryp
{
    public partial class EditTextureDialog : Form
    {
        internal TextureCoords TextureResult { get; set; }

        public EditTextureDialog(uint resource, int texX, int texY)
        {
            InitializeComponent();
            numResource.Value = resource;
            numTextureX.Value = texX;
            numTextureY.Value = texY;
        }

        /// <summary>
        /// Extract values from the form elements and pack them into Result Object
        /// </summary>
        private void PopulateResult()
        {
            TextureResult = new TextureCoords
            {
                Resource = (uint)numResource.Value,
                TextureX = (int)numTextureX.Value,
                TextureY = (int)numTextureY.Value
            };
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
