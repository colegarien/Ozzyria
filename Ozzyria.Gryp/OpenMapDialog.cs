using Ozzyria.Content.Models.Area;
namespace Ozzyria.Gryp
{
    public partial class OpenMapDialog : Form
    {
        public string AreaId { get; set; } = "";

        public OpenMapDialog()
        {
            InitializeComponent();

            cmbMap.Items.Clear();
            cmbMap.Items.AddRange(AreaData.RetrieveAreaIds());
        }

        /// <summary>
        /// Extract values from the form elements and pack them into Result Object
        /// </summary>
        private void PopulateResult()
        {
            AreaId = (string)(cmbMap?.SelectedItem ?? "");
        }

        private void btnOpen_Click(object sender, EventArgs e)
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
