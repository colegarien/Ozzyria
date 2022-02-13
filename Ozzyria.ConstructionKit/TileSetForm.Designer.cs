
namespace Ozzyria.ConstructionKit
{
    partial class TileSetForm
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
            this.comboBoxTileSet = new System.Windows.Forms.ComboBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonNew = new System.Windows.Forms.Button();
            this.listTileTypes = new System.Windows.Forms.ListBox();
            this.buttonNewTileType = new System.Windows.Forms.Button();
            this.groupTileTypeSettings = new System.Windows.Forms.GroupBox();
            this.numWallThickness = new System.Windows.Forms.NumericUpDown();
            this.lblWallThickness = new System.Windows.Forms.Label();
            this.numWallOffsetY = new System.Windows.Forms.NumericUpDown();
            this.numWallOffsetX = new System.Windows.Forms.NumericUpDown();
            this.lblWallOffset = new System.Windows.Forms.Label();
            this.grpIsWall = new System.Windows.Forms.GroupBox();
            this.radWallNo = new System.Windows.Forms.RadioButton();
            this.radWallYes = new System.Windows.Forms.RadioButton();
            this.grpIsPathable = new System.Windows.Forms.GroupBox();
            this.radPathableNo = new System.Windows.Forms.RadioButton();
            this.radPathableYes = new System.Windows.Forms.RadioButton();
            this.grpIsTransitionable = new System.Windows.Forms.GroupBox();
            this.radTranistionableNo = new System.Windows.Forms.RadioButton();
            this.radTranistionableYes = new System.Windows.Forms.RadioButton();
            this.labelZDepth = new System.Windows.Forms.Label();
            this.dropDownZDepth = new System.Windows.Forms.ComboBox();
            this.picTileSet = new System.Windows.Forms.PictureBox();
            this.listTransitionPrecedence = new System.Windows.Forms.ListBox();
            this.lblTransitionPrecedence = new System.Windows.Forms.Label();
            this.groupTileTypeSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numWallThickness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWallOffsetY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWallOffsetX)).BeginInit();
            this.grpIsWall.SuspendLayout();
            this.grpIsPathable.SuspendLayout();
            this.grpIsTransitionable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picTileSet)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxTileSet
            // 
            this.comboBoxTileSet.FormattingEnabled = true;
            this.comboBoxTileSet.Location = new System.Drawing.Point(11, 10);
            this.comboBoxTileSet.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBoxTileSet.Name = "comboBoxTileSet";
            this.comboBoxTileSet.Size = new System.Drawing.Size(171, 23);
            this.comboBoxTileSet.TabIndex = 0;
            this.comboBoxTileSet.SelectedIndexChanged += new System.EventHandler(this.comboBoxTileSet_SelectedIndexChanged);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(12, 492);
            this.buttonSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(82, 22);
            this.buttonSave.TabIndex = 1;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(100, 492);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(82, 22);
            this.buttonClose.TabIndex = 2;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonNew
            // 
            this.buttonNew.Location = new System.Drawing.Point(11, 36);
            this.buttonNew.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonNew.Name = "buttonNew";
            this.buttonNew.Size = new System.Drawing.Size(82, 22);
            this.buttonNew.TabIndex = 3;
            this.buttonNew.Text = "New";
            this.buttonNew.UseVisualStyleBackColor = true;
            this.buttonNew.Click += new System.EventHandler(this.buttonNew_Click);
            // 
            // listTileTypes
            // 
            this.listTileTypes.FormattingEnabled = true;
            this.listTileTypes.ItemHeight = 15;
            this.listTileTypes.Location = new System.Drawing.Point(11, 62);
            this.listTileTypes.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listTileTypes.Name = "listTileTypes";
            this.listTileTypes.Size = new System.Drawing.Size(171, 139);
            this.listTileTypes.TabIndex = 4;
            this.listTileTypes.SelectedIndexChanged += new System.EventHandler(this.listTileTypes_SelectedIndexChanged);
            // 
            // buttonNewTileType
            // 
            this.buttonNewTileType.Location = new System.Drawing.Point(11, 202);
            this.buttonNewTileType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonNewTileType.Name = "buttonNewTileType";
            this.buttonNewTileType.Size = new System.Drawing.Size(82, 22);
            this.buttonNewTileType.TabIndex = 5;
            this.buttonNewTileType.Text = "New...";
            this.buttonNewTileType.UseVisualStyleBackColor = true;
            this.buttonNewTileType.Click += new System.EventHandler(this.buttonNewTileType_Click);
            // 
            // groupTileTypeSettings
            // 
            this.groupTileTypeSettings.Controls.Add(this.numWallThickness);
            this.groupTileTypeSettings.Controls.Add(this.lblWallThickness);
            this.groupTileTypeSettings.Controls.Add(this.numWallOffsetY);
            this.groupTileTypeSettings.Controls.Add(this.numWallOffsetX);
            this.groupTileTypeSettings.Controls.Add(this.lblWallOffset);
            this.groupTileTypeSettings.Controls.Add(this.grpIsWall);
            this.groupTileTypeSettings.Controls.Add(this.grpIsPathable);
            this.groupTileTypeSettings.Controls.Add(this.grpIsTransitionable);
            this.groupTileTypeSettings.Controls.Add(this.labelZDepth);
            this.groupTileTypeSettings.Controls.Add(this.dropDownZDepth);
            this.groupTileTypeSettings.Location = new System.Drawing.Point(187, 10);
            this.groupTileTypeSettings.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupTileTypeSettings.Name = "groupTileTypeSettings";
            this.groupTileTypeSettings.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupTileTypeSettings.Size = new System.Drawing.Size(183, 288);
            this.groupTileTypeSettings.TabIndex = 6;
            this.groupTileTypeSettings.TabStop = false;
            this.groupTileTypeSettings.Text = "Tile Type";
            // 
            // numWallThickness
            // 
            this.numWallThickness.Location = new System.Drawing.Point(6, 254);
            this.numWallThickness.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.numWallThickness.Name = "numWallThickness";
            this.numWallThickness.Size = new System.Drawing.Size(48, 23);
            this.numWallThickness.TabIndex = 10;
            this.numWallThickness.ValueChanged += new System.EventHandler(this.numWallThickness_ValueChanged);
            // 
            // lblWallThickness
            // 
            this.lblWallThickness.AutoSize = true;
            this.lblWallThickness.Location = new System.Drawing.Point(6, 236);
            this.lblWallThickness.Name = "lblWallThickness";
            this.lblWallThickness.Size = new System.Drawing.Size(84, 15);
            this.lblWallThickness.TabIndex = 9;
            this.lblWallThickness.Text = "Wall Thickness";
            // 
            // numWallOffsetY
            // 
            this.numWallOffsetY.Location = new System.Drawing.Point(61, 210);
            this.numWallOffsetY.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numWallOffsetY.Minimum = new decimal(new int[] {
            16,
            0,
            0,
            -2147483648});
            this.numWallOffsetY.Name = "numWallOffsetY";
            this.numWallOffsetY.Size = new System.Drawing.Size(49, 23);
            this.numWallOffsetY.TabIndex = 8;
            this.numWallOffsetY.ValueChanged += new System.EventHandler(this.numWallOffsetY_ValueChanged);
            // 
            // numWallOffsetX
            // 
            this.numWallOffsetX.Location = new System.Drawing.Point(6, 210);
            this.numWallOffsetX.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numWallOffsetX.Minimum = new decimal(new int[] {
            16,
            0,
            0,
            -2147483648});
            this.numWallOffsetX.Name = "numWallOffsetX";
            this.numWallOffsetX.Size = new System.Drawing.Size(49, 23);
            this.numWallOffsetX.TabIndex = 6;
            this.numWallOffsetX.ValueChanged += new System.EventHandler(this.numWallOffsetX_ValueChanged);
            // 
            // lblWallOffset
            // 
            this.lblWallOffset.AutoSize = true;
            this.lblWallOffset.Location = new System.Drawing.Point(6, 192);
            this.lblWallOffset.Name = "lblWallOffset";
            this.lblWallOffset.Size = new System.Drawing.Size(65, 15);
            this.lblWallOffset.TabIndex = 5;
            this.lblWallOffset.Text = "Wall Offset";
            // 
            // grpIsWall
            // 
            this.grpIsWall.Controls.Add(this.radWallNo);
            this.grpIsWall.Controls.Add(this.radWallYes);
            this.grpIsWall.Location = new System.Drawing.Point(6, 143);
            this.grpIsWall.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpIsWall.Name = "grpIsWall";
            this.grpIsWall.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpIsWall.Size = new System.Drawing.Size(172, 37);
            this.grpIsWall.TabIndex = 4;
            this.grpIsWall.TabStop = false;
            this.grpIsWall.Text = "Is Wall?";
            // 
            // radWallNo
            // 
            this.radWallNo.AutoSize = true;
            this.radWallNo.Location = new System.Drawing.Point(56, 16);
            this.radWallNo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radWallNo.Name = "radWallNo";
            this.radWallNo.Size = new System.Drawing.Size(41, 19);
            this.radWallNo.TabIndex = 1;
            this.radWallNo.TabStop = true;
            this.radWallNo.Text = "No";
            this.radWallNo.UseVisualStyleBackColor = true;
            // 
            // radWallYes
            // 
            this.radWallYes.AutoSize = true;
            this.radWallYes.Location = new System.Drawing.Point(6, 16);
            this.radWallYes.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radWallYes.Name = "radWallYes";
            this.radWallYes.Size = new System.Drawing.Size(42, 19);
            this.radWallYes.TabIndex = 0;
            this.radWallYes.TabStop = true;
            this.radWallYes.Text = "Yes";
            this.radWallYes.UseVisualStyleBackColor = true;
            this.radWallYes.CheckedChanged += new System.EventHandler(this.radWallYes_CheckedChanged);
            // 
            // grpIsPathable
            // 
            this.grpIsPathable.Controls.Add(this.radPathableNo);
            this.grpIsPathable.Controls.Add(this.radPathableYes);
            this.grpIsPathable.Location = new System.Drawing.Point(6, 102);
            this.grpIsPathable.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpIsPathable.Name = "grpIsPathable";
            this.grpIsPathable.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpIsPathable.Size = new System.Drawing.Size(172, 37);
            this.grpIsPathable.TabIndex = 3;
            this.grpIsPathable.TabStop = false;
            this.grpIsPathable.Text = "Is Pathable?";
            // 
            // radPathableNo
            // 
            this.radPathableNo.AutoSize = true;
            this.radPathableNo.Location = new System.Drawing.Point(56, 16);
            this.radPathableNo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radPathableNo.Name = "radPathableNo";
            this.radPathableNo.Size = new System.Drawing.Size(41, 19);
            this.radPathableNo.TabIndex = 1;
            this.radPathableNo.TabStop = true;
            this.radPathableNo.Text = "No";
            this.radPathableNo.UseVisualStyleBackColor = true;
            // 
            // radPathableYes
            // 
            this.radPathableYes.AutoSize = true;
            this.radPathableYes.Location = new System.Drawing.Point(6, 16);
            this.radPathableYes.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radPathableYes.Name = "radPathableYes";
            this.radPathableYes.Size = new System.Drawing.Size(42, 19);
            this.radPathableYes.TabIndex = 0;
            this.radPathableYes.TabStop = true;
            this.radPathableYes.Text = "Yes";
            this.radPathableYes.UseVisualStyleBackColor = true;
            this.radPathableYes.CheckedChanged += new System.EventHandler(this.radPathableYes_CheckedChanged);
            // 
            // grpIsTransitionable
            // 
            this.grpIsTransitionable.Controls.Add(this.radTranistionableNo);
            this.grpIsTransitionable.Controls.Add(this.radTranistionableYes);
            this.grpIsTransitionable.Location = new System.Drawing.Point(6, 64);
            this.grpIsTransitionable.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpIsTransitionable.Name = "grpIsTransitionable";
            this.grpIsTransitionable.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpIsTransitionable.Size = new System.Drawing.Size(172, 37);
            this.grpIsTransitionable.TabIndex = 2;
            this.grpIsTransitionable.TabStop = false;
            this.grpIsTransitionable.Text = "Is Transitionable?";
            // 
            // radTranistionableNo
            // 
            this.radTranistionableNo.AutoSize = true;
            this.radTranistionableNo.Location = new System.Drawing.Point(56, 16);
            this.radTranistionableNo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radTranistionableNo.Name = "radTranistionableNo";
            this.radTranistionableNo.Size = new System.Drawing.Size(41, 19);
            this.radTranistionableNo.TabIndex = 1;
            this.radTranistionableNo.TabStop = true;
            this.radTranistionableNo.Text = "No";
            this.radTranistionableNo.UseVisualStyleBackColor = true;
            // 
            // radTranistionableYes
            // 
            this.radTranistionableYes.AutoSize = true;
            this.radTranistionableYes.Location = new System.Drawing.Point(6, 16);
            this.radTranistionableYes.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radTranistionableYes.Name = "radTranistionableYes";
            this.radTranistionableYes.Size = new System.Drawing.Size(42, 19);
            this.radTranistionableYes.TabIndex = 0;
            this.radTranistionableYes.TabStop = true;
            this.radTranistionableYes.Text = "Yes";
            this.radTranistionableYes.UseVisualStyleBackColor = true;
            this.radTranistionableYes.CheckedChanged += new System.EventHandler(this.radTranistionableYes_CheckedChanged);
            // 
            // labelZDepth
            // 
            this.labelZDepth.AutoSize = true;
            this.labelZDepth.Location = new System.Drawing.Point(6, 20);
            this.labelZDepth.Name = "labelZDepth";
            this.labelZDepth.Size = new System.Drawing.Size(49, 15);
            this.labelZDepth.TabIndex = 1;
            this.labelZDepth.Text = "Z Depth";
            // 
            // dropDownZDepth
            // 
            this.dropDownZDepth.FormattingEnabled = true;
            this.dropDownZDepth.Location = new System.Drawing.Point(6, 38);
            this.dropDownZDepth.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dropDownZDepth.Name = "dropDownZDepth";
            this.dropDownZDepth.Size = new System.Drawing.Size(172, 23);
            this.dropDownZDepth.TabIndex = 0;
            this.dropDownZDepth.SelectedIndexChanged += new System.EventHandler(this.dropDownZDepth_SelectedIndexChanged);
            // 
            // picTileSet
            // 
            this.picTileSet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picTileSet.Location = new System.Drawing.Point(381, 10);
            this.picTileSet.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.picTileSet.Name = "picTileSet";
            this.picTileSet.Size = new System.Drawing.Size(512, 512);
            this.picTileSet.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picTileSet.TabIndex = 7;
            this.picTileSet.TabStop = false;
            this.picTileSet.Paint += new System.Windows.Forms.PaintEventHandler(this.picTileSet_Paint);
            this.picTileSet.DoubleClick += new System.EventHandler(this.picTileSet_DoubleClick);
            // 
            // listTransitionPrecedence
            // 
            this.listTransitionPrecedence.AllowDrop = true;
            this.listTransitionPrecedence.FormattingEnabled = true;
            this.listTransitionPrecedence.ItemHeight = 15;
            this.listTransitionPrecedence.Location = new System.Drawing.Point(11, 298);
            this.listTransitionPrecedence.Name = "listTransitionPrecedence";
            this.listTransitionPrecedence.Size = new System.Drawing.Size(171, 94);
            this.listTransitionPrecedence.TabIndex = 8;
            this.listTransitionPrecedence.DragDrop += new System.Windows.Forms.DragEventHandler(this.listTransitionPrecedence_DragDrop);
            this.listTransitionPrecedence.DragOver += new System.Windows.Forms.DragEventHandler(this.listTransitionPrecedence_DragOver);
            this.listTransitionPrecedence.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listTransitionPrecedence_MouseDown);
            // 
            // lblTransitionPrecedence
            // 
            this.lblTransitionPrecedence.AutoSize = true;
            this.lblTransitionPrecedence.Location = new System.Drawing.Point(12, 280);
            this.lblTransitionPrecedence.Name = "lblTransitionPrecedence";
            this.lblTransitionPrecedence.Size = new System.Drawing.Size(139, 15);
            this.lblTransitionPrecedence.TabIndex = 9;
            this.lblTransitionPrecedence.Text = "Precedence (low to high)";
            // 
            // TileSetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(905, 529);
            this.ControlBox = false;
            this.Controls.Add(this.lblTransitionPrecedence);
            this.Controls.Add(this.listTransitionPrecedence);
            this.Controls.Add(this.picTileSet);
            this.Controls.Add(this.groupTileTypeSettings);
            this.Controls.Add(this.buttonNewTileType);
            this.Controls.Add(this.listTileTypes);
            this.Controls.Add(this.buttonNew);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.comboBoxTileSet);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TileSetForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Tile Set";
            this.groupTileTypeSettings.ResumeLayout(false);
            this.groupTileTypeSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numWallThickness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWallOffsetY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWallOffsetX)).EndInit();
            this.grpIsWall.ResumeLayout(false);
            this.grpIsWall.PerformLayout();
            this.grpIsPathable.ResumeLayout(false);
            this.grpIsPathable.PerformLayout();
            this.grpIsTransitionable.ResumeLayout(false);
            this.grpIsTransitionable.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picTileSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxTileSet;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonNew;
        private System.Windows.Forms.ListBox listTileTypes;
        private System.Windows.Forms.Button buttonNewTileType;
        private System.Windows.Forms.GroupBox groupTileTypeSettings;
        private System.Windows.Forms.Label labelZDepth;
        private System.Windows.Forms.ComboBox dropDownZDepth;
        private System.Windows.Forms.PictureBox picTileSet;
        private System.Windows.Forms.GroupBox grpIsTransitionable;
        private System.Windows.Forms.RadioButton radTranistionableNo;
        private System.Windows.Forms.RadioButton radTranistionableYes;
        private System.Windows.Forms.GroupBox grpIsPathable;
        private System.Windows.Forms.RadioButton radPathableNo;
        private System.Windows.Forms.RadioButton radPathableYes;
        private System.Windows.Forms.GroupBox grpIsWall;
        private System.Windows.Forms.RadioButton radWallNo;
        private System.Windows.Forms.RadioButton radWallYes;
        private System.Windows.Forms.NumericUpDown numWallThickness;
        private System.Windows.Forms.Label lblWallThickness;
        private System.Windows.Forms.NumericUpDown numWallOffsetY;
        private System.Windows.Forms.NumericUpDown numWallOffsetX;
        private System.Windows.Forms.Label lblWallOffset;
        private System.Windows.Forms.ListBox listTransitionPrecedence;
        private System.Windows.Forms.Label lblTransitionPrecedence;
    }
}