
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupTileTypeSettings.SuspendLayout();
            this.grpIsWall.SuspendLayout();
            this.grpIsPathable.SuspendLayout();
            this.grpIsTransitionable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxTileSet
            // 
            this.comboBoxTileSet.FormattingEnabled = true;
            this.comboBoxTileSet.Location = new System.Drawing.Point(13, 13);
            this.comboBoxTileSet.Name = "comboBoxTileSet";
            this.comboBoxTileSet.Size = new System.Drawing.Size(195, 28);
            this.comboBoxTileSet.TabIndex = 0;
            this.comboBoxTileSet.SelectedIndexChanged += new System.EventHandler(this.comboBoxTileSet_SelectedIndexChanged);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(14, 308);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(94, 29);
            this.buttonSave.TabIndex = 1;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(114, 308);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(94, 29);
            this.buttonClose.TabIndex = 2;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonNew
            // 
            this.buttonNew.Location = new System.Drawing.Point(13, 48);
            this.buttonNew.Name = "buttonNew";
            this.buttonNew.Size = new System.Drawing.Size(94, 29);
            this.buttonNew.TabIndex = 3;
            this.buttonNew.Text = "New";
            this.buttonNew.UseVisualStyleBackColor = true;
            this.buttonNew.Click += new System.EventHandler(this.buttonNew_Click);
            // 
            // listTileTypes
            // 
            this.listTileTypes.FormattingEnabled = true;
            this.listTileTypes.ItemHeight = 20;
            this.listTileTypes.Location = new System.Drawing.Point(13, 83);
            this.listTileTypes.Name = "listTileTypes";
            this.listTileTypes.Size = new System.Drawing.Size(195, 184);
            this.listTileTypes.TabIndex = 4;
            // 
            // buttonNewTileType
            // 
            this.buttonNewTileType.Location = new System.Drawing.Point(13, 270);
            this.buttonNewTileType.Name = "buttonNewTileType";
            this.buttonNewTileType.Size = new System.Drawing.Size(94, 29);
            this.buttonNewTileType.TabIndex = 5;
            this.buttonNewTileType.Text = "New...";
            this.buttonNewTileType.UseVisualStyleBackColor = true;
            this.buttonNewTileType.Click += new System.EventHandler(this.buttonNewTileType_Click);
            // 
            // groupTileTypeSettings
            // 
            this.groupTileTypeSettings.Controls.Add(this.grpIsWall);
            this.groupTileTypeSettings.Controls.Add(this.grpIsPathable);
            this.groupTileTypeSettings.Controls.Add(this.grpIsTransitionable);
            this.groupTileTypeSettings.Controls.Add(this.labelZDepth);
            this.groupTileTypeSettings.Controls.Add(this.dropDownZDepth);
            this.groupTileTypeSettings.Location = new System.Drawing.Point(214, 13);
            this.groupTileTypeSettings.Name = "groupTileTypeSettings";
            this.groupTileTypeSettings.Size = new System.Drawing.Size(209, 324);
            this.groupTileTypeSettings.TabIndex = 6;
            this.groupTileTypeSettings.TabStop = false;
            this.groupTileTypeSettings.Text = "Tile Type";
            // 
            // grpIsWall
            // 
            this.grpIsWall.Controls.Add(this.radWallNo);
            this.grpIsWall.Controls.Add(this.radWallYes);
            this.grpIsWall.Location = new System.Drawing.Point(7, 191);
            this.grpIsWall.Name = "grpIsWall";
            this.grpIsWall.Size = new System.Drawing.Size(196, 49);
            this.grpIsWall.TabIndex = 4;
            this.grpIsWall.TabStop = false;
            this.grpIsWall.Text = "Is Wall?";
            // 
            // radWallNo
            // 
            this.radWallNo.AutoSize = true;
            this.radWallNo.Location = new System.Drawing.Point(64, 21);
            this.radWallNo.Name = "radWallNo";
            this.radWallNo.Size = new System.Drawing.Size(50, 24);
            this.radWallNo.TabIndex = 1;
            this.radWallNo.TabStop = true;
            this.radWallNo.Text = "No";
            this.radWallNo.UseVisualStyleBackColor = true;
            // 
            // radWallYes
            // 
            this.radWallYes.AutoSize = true;
            this.radWallYes.Location = new System.Drawing.Point(7, 21);
            this.radWallYes.Name = "radWallYes";
            this.radWallYes.Size = new System.Drawing.Size(51, 24);
            this.radWallYes.TabIndex = 0;
            this.radWallYes.TabStop = true;
            this.radWallYes.Text = "Yes";
            this.radWallYes.UseVisualStyleBackColor = true;
            // 
            // grpIsPathable
            // 
            this.grpIsPathable.Controls.Add(this.radPathableNo);
            this.grpIsPathable.Controls.Add(this.radPathableYes);
            this.grpIsPathable.Location = new System.Drawing.Point(7, 136);
            this.grpIsPathable.Name = "grpIsPathable";
            this.grpIsPathable.Size = new System.Drawing.Size(196, 49);
            this.grpIsPathable.TabIndex = 3;
            this.grpIsPathable.TabStop = false;
            this.grpIsPathable.Text = "Is Pathable?";
            // 
            // radPathableNo
            // 
            this.radPathableNo.AutoSize = true;
            this.radPathableNo.Location = new System.Drawing.Point(64, 21);
            this.radPathableNo.Name = "radPathableNo";
            this.radPathableNo.Size = new System.Drawing.Size(50, 24);
            this.radPathableNo.TabIndex = 1;
            this.radPathableNo.TabStop = true;
            this.radPathableNo.Text = "No";
            this.radPathableNo.UseVisualStyleBackColor = true;
            // 
            // radPathableYes
            // 
            this.radPathableYes.AutoSize = true;
            this.radPathableYes.Location = new System.Drawing.Point(7, 21);
            this.radPathableYes.Name = "radPathableYes";
            this.radPathableYes.Size = new System.Drawing.Size(51, 24);
            this.radPathableYes.TabIndex = 0;
            this.radPathableYes.TabStop = true;
            this.radPathableYes.Text = "Yes";
            this.radPathableYes.UseVisualStyleBackColor = true;
            // 
            // grpIsTransitionable
            // 
            this.grpIsTransitionable.Controls.Add(this.radTranistionableNo);
            this.grpIsTransitionable.Controls.Add(this.radTranistionableYes);
            this.grpIsTransitionable.Location = new System.Drawing.Point(7, 85);
            this.grpIsTransitionable.Name = "grpIsTransitionable";
            this.grpIsTransitionable.Size = new System.Drawing.Size(196, 49);
            this.grpIsTransitionable.TabIndex = 2;
            this.grpIsTransitionable.TabStop = false;
            this.grpIsTransitionable.Text = "Is Transitionable?";
            // 
            // radTranistionableNo
            // 
            this.radTranistionableNo.AutoSize = true;
            this.radTranistionableNo.Location = new System.Drawing.Point(64, 21);
            this.radTranistionableNo.Name = "radTranistionableNo";
            this.radTranistionableNo.Size = new System.Drawing.Size(50, 24);
            this.radTranistionableNo.TabIndex = 1;
            this.radTranistionableNo.TabStop = true;
            this.radTranistionableNo.Text = "No";
            this.radTranistionableNo.UseVisualStyleBackColor = true;
            // 
            // radTranistionableYes
            // 
            this.radTranistionableYes.AutoSize = true;
            this.radTranistionableYes.Location = new System.Drawing.Point(7, 21);
            this.radTranistionableYes.Name = "radTranistionableYes";
            this.radTranistionableYes.Size = new System.Drawing.Size(51, 24);
            this.radTranistionableYes.TabIndex = 0;
            this.radTranistionableYes.TabStop = true;
            this.radTranistionableYes.Text = "Yes";
            this.radTranistionableYes.UseVisualStyleBackColor = true;
            // 
            // labelZDepth
            // 
            this.labelZDepth.AutoSize = true;
            this.labelZDepth.Location = new System.Drawing.Point(7, 27);
            this.labelZDepth.Name = "labelZDepth";
            this.labelZDepth.Size = new System.Drawing.Size(63, 20);
            this.labelZDepth.TabIndex = 1;
            this.labelZDepth.Text = "Z Depth";
            // 
            // dropDownZDepth
            // 
            this.dropDownZDepth.FormattingEnabled = true;
            this.dropDownZDepth.Location = new System.Drawing.Point(7, 50);
            this.dropDownZDepth.Name = "dropDownZDepth";
            this.dropDownZDepth.Size = new System.Drawing.Size(196, 28);
            this.dropDownZDepth.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(435, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(318, 324);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // TileSetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(761, 342);
            this.ControlBox = false;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupTileTypeSettings);
            this.Controls.Add(this.buttonNewTileType);
            this.Controls.Add(this.listTileTypes);
            this.Controls.Add(this.buttonNew);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.comboBoxTileSet);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TileSetForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Tile Set";
            this.groupTileTypeSettings.ResumeLayout(false);
            this.groupTileTypeSettings.PerformLayout();
            this.grpIsWall.ResumeLayout(false);
            this.grpIsWall.PerformLayout();
            this.grpIsPathable.ResumeLayout(false);
            this.grpIsPathable.PerformLayout();
            this.grpIsTransitionable.ResumeLayout(false);
            this.grpIsTransitionable.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox grpIsTransitionable;
        private System.Windows.Forms.RadioButton radTranistionableNo;
        private System.Windows.Forms.RadioButton radTranistionableYes;
        private System.Windows.Forms.GroupBox grpIsPathable;
        private System.Windows.Forms.RadioButton radPathableNo;
        private System.Windows.Forms.RadioButton radPathableYes;
        private System.Windows.Forms.GroupBox grpIsWall;
        private System.Windows.Forms.RadioButton radWallNo;
        private System.Windows.Forms.RadioButton radWallYes;
    }
}