namespace Ozzyria.Gryp
{
    partial class NewMapDialog
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
            numWidth = new NumericUpDown();
            lblWidth = new Label();
            lblHeight = new Label();
            numHeight = new NumericUpDown();
            btnNew = new Button();
            btnCancel = new Button();
            lblDisplayName = new Label();
            txtDisplayName = new TextBox();
            ((System.ComponentModel.ISupportInitialize)numWidth).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numHeight).BeginInit();
            SuspendLayout();
            // 
            // numWidth
            // 
            numWidth.Location = new Point(61, 39);
            numWidth.Maximum = new decimal(new int[] { 1024, 0, 0, 0 });
            numWidth.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numWidth.Name = "numWidth";
            numWidth.Size = new Size(57, 23);
            numWidth.TabIndex = 1;
            numWidth.Value = new decimal(new int[] { 32, 0, 0, 0 });
            // 
            // lblWidth
            // 
            lblWidth.AutoSize = true;
            lblWidth.Location = new Point(16, 41);
            lblWidth.Name = "lblWidth";
            lblWidth.Size = new Size(39, 15);
            lblWidth.TabIndex = 6;
            lblWidth.Text = "Width";
            // 
            // lblHeight
            // 
            lblHeight.AutoSize = true;
            lblHeight.Location = new Point(12, 70);
            lblHeight.Name = "lblHeight";
            lblHeight.Size = new Size(43, 15);
            lblHeight.TabIndex = 7;
            lblHeight.Text = "Height";
            // 
            // numHeight
            // 
            numHeight.Location = new Point(61, 68);
            numHeight.Maximum = new decimal(new int[] { 1024, 0, 0, 0 });
            numHeight.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numHeight.Name = "numHeight";
            numHeight.Size = new Size(57, 23);
            numHeight.TabIndex = 2;
            numHeight.Value = new decimal(new int[] { 32, 0, 0, 0 });
            // 
            // btnNew
            // 
            btnNew.Location = new Point(5, 105);
            btnNew.Name = "btnNew";
            btnNew.Size = new Size(75, 23);
            btnNew.TabIndex = 3;
            btnNew.Text = "New";
            btnNew.UseVisualStyleBackColor = true;
            btnNew.Click += btnNew_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(86, 105);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // lblDisplayName
            // 
            lblDisplayName.AutoSize = true;
            lblDisplayName.Location = new Point(16, 13);
            lblDisplayName.Name = "lblDisplayName";
            lblDisplayName.Size = new Size(39, 15);
            lblDisplayName.TabIndex = 5;
            lblDisplayName.Text = "Name";
            // 
            // txtDisplayName
            // 
            txtDisplayName.Location = new Point(61, 10);
            txtDisplayName.MaxLength = 256;
            txtDisplayName.Name = "txtDisplayName";
            txtDisplayName.Size = new Size(100, 23);
            txtDisplayName.TabIndex = 0;
            // 
            // NewMapDialog
            // 
            AcceptButton = btnNew;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            CancelButton = btnCancel;
            ClientSize = new Size(166, 128);
            ControlBox = false;
            Controls.Add(txtDisplayName);
            Controls.Add(lblDisplayName);
            Controls.Add(btnCancel);
            Controls.Add(btnNew);
            Controls.Add(lblHeight);
            Controls.Add(numHeight);
            Controls.Add(lblWidth);
            Controls.Add(numWidth);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "NewMapDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "New Map";
            TopMost = true;
            ((System.ComponentModel.ISupportInitialize)numWidth).EndInit();
            ((System.ComponentModel.ISupportInitialize)numHeight).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private NumericUpDown numWidth;
        private Label lblWidth;
        private Label lblHeight;
        private NumericUpDown numHeight;
        private Button btnNew;
        private Button btnCancel;
        private Label lblDisplayName;
        private TextBox txtDisplayName;
    }
}