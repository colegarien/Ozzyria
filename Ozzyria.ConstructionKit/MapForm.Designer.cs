namespace Ozzyria.ConstructionKit
{
    partial class MapForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dropDownMap = new System.Windows.Forms.ComboBox();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblSize = new System.Windows.Forms.Label();
            this.numWidth = new System.Windows.Forms.NumericUpDown();
            this.numHeight = new System.Windows.Forms.NumericUpDown();
            this.numLayers = new System.Windows.Forms.NumericUpDown();
            this.lblLayers = new System.Windows.Forms.Label();
            this.dropDownTileSet = new System.Windows.Forms.ComboBox();
            this.lblTileSet = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLayers)).BeginInit();
            this.SuspendLayout();
            // 
            // dropDownMap
            // 
            this.dropDownMap.FormattingEnabled = true;
            this.dropDownMap.Location = new System.Drawing.Point(12, 12);
            this.dropDownMap.Name = "dropDownMap";
            this.dropDownMap.Size = new System.Drawing.Size(173, 23);
            this.dropDownMap.TabIndex = 0;
            this.dropDownMap.SelectedIndexChanged += new System.EventHandler(this.dropDownMap_SelectedIndexChanged);
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(12, 41);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 23);
            this.btnNew.TabIndex = 1;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(12, 219);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(93, 219);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(174, 219);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblSize
            // 
            this.lblSize.AutoSize = true;
            this.lblSize.Location = new System.Drawing.Point(12, 111);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(27, 15);
            this.lblSize.TabIndex = 5;
            this.lblSize.Text = "Size";
            // 
            // numWidth
            // 
            this.numWidth.Location = new System.Drawing.Point(12, 129);
            this.numWidth.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.numWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numWidth.Name = "numWidth";
            this.numWidth.Size = new System.Drawing.Size(54, 23);
            this.numWidth.TabIndex = 6;
            this.numWidth.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.numWidth.ValueChanged += new System.EventHandler(this.numWidth_ValueChanged);
            // 
            // numHeight
            // 
            this.numHeight.Location = new System.Drawing.Point(72, 129);
            this.numHeight.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.numHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numHeight.Name = "numHeight";
            this.numHeight.Size = new System.Drawing.Size(54, 23);
            this.numHeight.TabIndex = 8;
            this.numHeight.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.numHeight.ValueChanged += new System.EventHandler(this.numHeight_ValueChanged);
            // 
            // numLayers
            // 
            this.numLayers.Location = new System.Drawing.Point(12, 173);
            this.numLayers.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.numLayers.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numLayers.Name = "numLayers";
            this.numLayers.Size = new System.Drawing.Size(54, 23);
            this.numLayers.TabIndex = 9;
            this.numLayers.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numLayers.ValueChanged += new System.EventHandler(this.numLayers_ValueChanged);
            // 
            // lblLayers
            // 
            this.lblLayers.AutoSize = true;
            this.lblLayers.Location = new System.Drawing.Point(12, 155);
            this.lblLayers.Name = "lblLayers";
            this.lblLayers.Size = new System.Drawing.Size(40, 15);
            this.lblLayers.TabIndex = 10;
            this.lblLayers.Text = "Layers";
            // 
            // dropDownTileSet
            // 
            this.dropDownTileSet.FormattingEnabled = true;
            this.dropDownTileSet.Location = new System.Drawing.Point(12, 85);
            this.dropDownTileSet.Name = "dropDownTileSet";
            this.dropDownTileSet.Size = new System.Drawing.Size(173, 23);
            this.dropDownTileSet.TabIndex = 11;
            this.dropDownTileSet.SelectedIndexChanged += new System.EventHandler(this.dropDownTileSet_SelectedIndexChanged);
            // 
            // lblTileSet
            // 
            this.lblTileSet.AutoSize = true;
            this.lblTileSet.Location = new System.Drawing.Point(12, 67);
            this.lblTileSet.Name = "lblTileSet";
            this.lblTileSet.Size = new System.Drawing.Size(44, 15);
            this.lblTileSet.TabIndex = 12;
            this.lblTileSet.Text = "Tile Set";
            // 
            // MapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(270, 246);
            this.ControlBox = false;
            this.Controls.Add(this.lblTileSet);
            this.Controls.Add(this.dropDownTileSet);
            this.Controls.Add(this.lblLayers);
            this.Controls.Add(this.numLayers);
            this.Controls.Add(this.numHeight);
            this.Controls.Add(this.numWidth);
            this.Controls.Add(this.lblSize);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.dropDownMap);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MapForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Map";
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLayers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox dropDownMap;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblSize;
        private System.Windows.Forms.NumericUpDown numWidth;
        private System.Windows.Forms.NumericUpDown numHeight;
        private System.Windows.Forms.NumericUpDown numLayers;
        private System.Windows.Forms.Label lblLayers;
        private System.Windows.Forms.ComboBox dropDownTileSet;
        private System.Windows.Forms.Label lblTileSet;
    }
}