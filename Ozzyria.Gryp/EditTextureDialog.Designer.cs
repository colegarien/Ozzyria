namespace Ozzyria.Gryp
{
    partial class EditTextureDialog
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
            btnCancel = new Button();
            btnSave = new Button();
            lblResource = new Label();
            numResource = new NumericUpDown();
            lblTextureX = new Label();
            numTextureX = new NumericUpDown();
            lblTextureY = new Label();
            numTextureY = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)numResource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numTextureX).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numTextureY).BeginInit();
            SuspendLayout();
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(173, 198);
            btnCancel.Margin = new Padding(6, 6, 6, 6);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(139, 49);
            btnCancel.TabIndex = 7;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(22, 198);
            btnSave.Margin = new Padding(6, 6, 6, 6);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(139, 49);
            btnSave.TabIndex = 6;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // lblResource
            // 
            lblResource.AutoSize = true;
            lblResource.Location = new Point(22, 19);
            lblResource.Margin = new Padding(6, 0, 6, 0);
            lblResource.Name = "lblResource";
            lblResource.Size = new Size(110, 32);
            lblResource.TabIndex = 9;
            lblResource.Text = "Resource";
            // 
            // numResource
            // 
            numResource.Location = new Point(136, 15);
            numResource.Margin = new Padding(6, 6, 6, 6);
            numResource.Maximum = new decimal(new int[] { 1024, 0, 0, 0 });
            numResource.Name = "numResource";
            numResource.Size = new Size(106, 39);
            numResource.TabIndex = 8;
            numResource.Value = new decimal(new int[] { 32, 0, 0, 0 });
            // 
            // lblTextureX
            // 
            lblTextureX.AutoSize = true;
            lblTextureX.Location = new Point(22, 81);
            lblTextureX.Margin = new Padding(6, 0, 6, 0);
            lblTextureX.Name = "lblTextureX";
            lblTextureX.Size = new Size(113, 32);
            lblTextureX.TabIndex = 11;
            lblTextureX.Text = "Texture X";
            // 
            // numTextureX
            // 
            numTextureX.Location = new Point(136, 77);
            numTextureX.Margin = new Padding(6, 6, 6, 6);
            numTextureX.Maximum = new decimal(new int[] { 4096, 0, 0, 0 });
            numTextureX.Name = "numTextureX";
            numTextureX.Size = new Size(106, 39);
            numTextureX.TabIndex = 10;
            numTextureX.Value = new decimal(new int[] { 32, 0, 0, 0 });
            // 
            // lblTextureY
            // 
            lblTextureY.AutoSize = true;
            lblTextureY.Location = new Point(22, 141);
            lblTextureY.Margin = new Padding(6, 0, 6, 0);
            lblTextureY.Name = "lblTextureY";
            lblTextureY.Size = new Size(112, 32);
            lblTextureY.TabIndex = 13;
            lblTextureY.Text = "Texture Y";
            // 
            // numTextureY
            // 
            numTextureY.Location = new Point(136, 137);
            numTextureY.Margin = new Padding(6, 6, 6, 6);
            numTextureY.Maximum = new decimal(new int[] { 4096, 0, 0, 0 });
            numTextureY.Name = "numTextureY";
            numTextureY.Size = new Size(106, 39);
            numTextureY.TabIndex = 12;
            numTextureY.Value = new decimal(new int[] { 32, 0, 0, 0 });
            // 
            // EditTextureDialog
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(327, 269);
            ControlBox = false;
            Controls.Add(lblTextureY);
            Controls.Add(numTextureY);
            Controls.Add(lblTextureX);
            Controls.Add(numTextureX);
            Controls.Add(lblResource);
            Controls.Add(numResource);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(6, 6, 6, 6);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "EditTextureDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Edit Brush";
            TopMost = true;
            ((System.ComponentModel.ISupportInitialize)numResource).EndInit();
            ((System.ComponentModel.ISupportInitialize)numTextureX).EndInit();
            ((System.ComponentModel.ISupportInitialize)numTextureY).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnCancel;
        private Button btnSave;
        private Label lblResource;
        private NumericUpDown numResource;
        private Label lblTextureX;
        private NumericUpDown numTextureX;
        private Label lblTextureY;
        private NumericUpDown numTextureY;
    }
}