using Ozzyria.Content.Models.Area;
namespace Ozzyria.Gryp.UI.Dialogs
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var selectedId = (string)(cmbMap?.SelectedItem ?? "");
            if (selectedId != "")
            {
                var result = MessageBox.Show("Are you sure you want to remove map "+selectedId+"?", "Destroy Map", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if(result == DialogResult.Yes)
                {
                    AreaData.Delete(selectedId);
                    cmbMap?.Items?.Remove(selectedId);
                }
            }
        }
    }
}
