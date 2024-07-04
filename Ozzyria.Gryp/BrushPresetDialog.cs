using Ozzyria.Gryp.Models.Data;

namespace Ozzyria.Gryp
{
    public partial class BrushPresetDialog : Form
    {
        internal List<string> PresetResult { get; set; } = new List<string>();

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
                    PresetResult.Add("grass");
                    break;
                case "water":
                    PresetResult.Add("water");
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
