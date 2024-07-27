using Ozzyria.Gryp.Models.Data;

namespace Ozzyria.Gryp
{
    public partial class BrushPresetDialog : Form
    {
        internal string SelectedPreset { get; set; } = "";
        internal List<string> PresetResult { get; set; } = new List<string>();

        internal Dictionary<string, List<string>> Presets { get; set; } = new Dictionary<string, List<string>>()
        {
            { "water", new List<string>() {         "water" } },
            { "grass", new List<string>() {         "grass"} },
            { "stone", new List<string>() {         "stone" } },
            { "water_g_w", new List<string>() {     "water", "grass_0" } },
            { "water_g_n", new List<string>() {     "water", "grass_1" } },
            { "water_g_nw", new List<string>() {    "water", "grass_2" } },
            { "water_g_e", new List<string>() {     "water", "grass_3" } },
            { "water_g_ew", new List<string>() {    "water", "grass_4" } },
            { "water_g_ne", new List<string>() {    "water", "grass_5" } },
            { "water_g_new", new List<string>() {   "water", "grass_6" } },
            { "water_g_s", new List<string>() {     "water", "grass_7" } },
            { "water_g_sw", new List<string>() {    "water", "grass_8" } },
            { "water_g_ns", new List<string>() {    "water", "grass_9" } },
            { "water_g_nsw", new List<string>() {   "water", "grass_10" } },
            { "water_g_se", new List<string>() {    "water", "grass_11" } },
            { "water_g_sew", new List<string>() {   "water", "grass_12" } },
            { "water_g_nse", new List<string>() {   "water", "grass_13" } },
            { "water_g_nsew", new List<string>() {  "water", "grass_14" } },
            { "water_g_a", new List<string>() {     "water", "grass_15" } },
            { "water_g_b", new List<string>() {     "water", "grass_16" } },
            { "water_g_ab", new List<string>() {    "water", "grass_17" } },
            { "water_g_c", new List<string>() {     "water", "grass_18" } },
            { "water_g_ac", new List<string>() {    "water", "grass_19" } },
            { "water_g_bc", new List<string>() {    "water", "grass_20" } },
            { "water_g_abc", new List<string>() {   "water", "grass_21" } },
            { "water_g_d", new List<string>() {     "water", "grass_22" } },
            { "water_g_ad", new List<string>() {    "water", "grass_23" } },
            { "water_g_bd", new List<string>() {    "water", "grass_24" } },
            { "water_g_abd", new List<string>() {   "water", "grass_25" } },
            { "water_g_cd", new List<string>() {    "water", "grass_26" } },
            { "water_g_acd", new List<string>() {   "water", "grass_27" } },
            { "water_g_bcd", new List<string>() {   "water", "grass_28" } },
            { "water_g_abcd", new List<string>() {  "water", "grass_29" } },
            { "water_s_w", new List<string>() {     "water", "stone_0" } },
            { "water_s_n", new List<string>() {     "water", "stone_1" } },
            { "water_s_nw", new List<string>() {    "water", "stone_2" } },
            { "water_s_e", new List<string>() {     "water", "stone_3" } },
            { "water_s_ew", new List<string>() {    "water", "stone_4" } },
            { "water_s_ne", new List<string>() {    "water", "stone_5" } },
            { "water_s_new", new List<string>() {   "water", "stone_6" } },
            { "water_s_s", new List<string>() {     "water", "stone_7" } },
            { "water_s_sw", new List<string>() {    "water", "stone_8" } },
            { "water_s_ns", new List<string>() {    "water", "stone_9" } },
            { "water_s_nsw", new List<string>() {   "water", "stone_10" } },
            { "water_s_se", new List<string>() {    "water", "stone_11" } },
            { "water_s_sew", new List<string>() {   "water", "stone_12" } },
            { "water_s_nse", new List<string>() {   "water", "stone_13" } },
            { "water_s_nsew", new List<string>() {  "water", "stone_14" } },
            { "water_s_a", new List<string>() {     "water", "stone_15" } },
            { "water_s_b", new List<string>() {     "water", "stone_16" } },
            { "water_s_ab", new List<string>() {    "water", "stone_17" } },
            { "water_s_c", new List<string>() {     "water", "stone_18" } },
            { "water_s_ac", new List<string>() {    "water", "stone_19" } },
            { "water_s_bc", new List<string>() {    "water", "stone_20" } },
            { "water_s_abc", new List<string>() {   "water", "stone_21" } },
            { "water_s_d", new List<string>() {     "water", "stone_22" } },
            { "water_s_ad", new List<string>() {    "water", "stone_23" } },
            { "water_s_bd", new List<string>() {    "water", "stone_24" } },
            { "water_s_abd", new List<string>() {   "water", "stone_25" } },
            { "water_s_cd", new List<string>() {    "water", "stone_26" } },
            { "water_s_acd", new List<string>() {   "water", "stone_27" } },
            { "water_s_bcd", new List<string>() {   "water", "stone_28" } },
            { "water_s_abcd", new List<string>() {  "water", "stone_29" } },
            { "grass_s_w", new List<string>() {     "grass", "stone_0" } },
            { "grass_s_n", new List<string>() {     "grass", "stone_1" } },
            { "grass_s_nw", new List<string>() {    "grass", "stone_2" } },
            { "grass_s_e", new List<string>() {     "grass", "stone_3" } },
            { "grass_s_ew", new List<string>() {    "grass", "stone_4" } },
            { "grass_s_ne", new List<string>() {    "grass", "stone_5" } },
            { "grass_s_new", new List<string>() {   "grass", "stone_6" } },
            { "grass_s_s", new List<string>() {     "grass", "stone_7" } },
            { "grass_s_sw", new List<string>() {    "grass", "stone_8" } },
            { "grass_s_ns", new List<string>() {    "grass", "stone_9" } },
            { "grass_s_nsw", new List<string>() {   "grass", "stone_10" } },
            { "grass_s_se", new List<string>() {    "grass", "stone_11" } },
            { "grass_s_sew", new List<string>() {   "grass", "stone_12" } },
            { "grass_s_nse", new List<string>() {   "grass", "stone_13" } },
            { "grass_s_nsew", new List<string>() {  "grass", "stone_14" } },
            { "grass_s_a", new List<string>() {     "grass", "stone_15" } },
            { "grass_s_b", new List<string>() {     "grass", "stone_16" } },
            { "grass_s_ab", new List<string>() {    "grass", "stone_17" } },
            { "grass_s_c", new List<string>() {     "grass", "stone_18" } },
            { "grass_s_ac", new List<string>() {    "grass", "stone_19" } },
            { "grass_s_bc", new List<string>() {    "grass", "stone_20" } },
            { "grass_s_abc", new List<string>() {   "grass", "stone_21" } },
            { "grass_s_d", new List<string>() {     "grass", "stone_22" } },
            { "grass_s_ad", new List<string>() {    "grass", "stone_23" } },
            { "grass_s_bd", new List<string>() {    "grass", "stone_24" } },
            { "grass_s_abd", new List<string>() {   "grass", "stone_25" } },
            { "grass_s_cd", new List<string>() {    "grass", "stone_26" } },
            { "grass_s_acd", new List<string>() {   "grass", "stone_27" } },
            { "grass_s_bcd", new List<string>() {   "grass", "stone_28" } },
            { "grass_s_abcd", new List<string>() {  "grass", "stone_29" } },
        };

        public BrushPresetDialog(string defaultPresetSelection = "")
        {
            InitializeComponent();

            comboPreset.Items.AddRange(Presets.Keys.ToArray());
            if (Presets.ContainsKey(defaultPresetSelection))
            {
                comboPreset.SelectedItem = defaultPresetSelection;
            }
        }

        /// <summary>
        /// Extract values from the form elements and pack them into Result Object
        /// </summary>
        private void PopulateResult()
        {
            var key = comboPreset.SelectedItem?.ToString() ?? "";
            if(Presets.ContainsKey(key))
            {
                SelectedPreset = key;
                PresetResult = Presets[key];
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
