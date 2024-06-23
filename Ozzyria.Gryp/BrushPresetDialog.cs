using Ozzyria.Gryp.Models.Data;

namespace Ozzyria.Gryp
{
    public partial class BrushPresetDialog : Form
    {
        internal List<TextureCoords> PresetResult { get; set; } = new List<TextureCoords>();

        public BrushPresetDialog()
        {
            InitializeComponent();

            comboPreset.Items.Add("grass");
            comboPreset.Items.Add("water");
        }

        /// <summary>
        /// Extract values from the form elements and pack them into Result Object
        /// </summary>
        private void PopulateResult()
        {
            switch (comboPreset.SelectedItem?.ToString()?.ToLower() ?? "") {
                case "grass":
                    PresetResult.Add(new TextureCoords
                    {
                        Resource = 1,
                        TextureX = 0,
                        TextureY = 128
                    });
                    break;
                case "water":
                    PresetResult.Add(new TextureCoords
                    {
                        Resource = 1,
                        TextureX = 0,
                        TextureY = 160
                    });
                    break;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
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
