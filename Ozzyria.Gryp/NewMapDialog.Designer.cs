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
            ((System.ComponentModel.ISupportInitialize)numWidth).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numHeight).BeginInit();
            SuspendLayout();
            // 
            // numWidth
            // 
            numWidth.Location = new Point(113, 17);
            numWidth.Margin = new Padding(6);
            numWidth.Maximum = new decimal(new int[] { 1024, 0, 0, 0 });
            numWidth.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numWidth.Name = "numWidth";
            numWidth.Size = new Size(106, 39);
            numWidth.TabIndex = 0;
            numWidth.Value = new decimal(new int[] { 32, 0, 0, 0 });
            // 
            // lblWidth
            // 
            lblWidth.AutoSize = true;
            lblWidth.Location = new Point(22, 21);
            lblWidth.Margin = new Padding(6, 0, 6, 0);
            lblWidth.Name = "lblWidth";
            lblWidth.Size = new Size(78, 32);
            lblWidth.TabIndex = 1;
            lblWidth.Text = "Width";
            // 
            // lblHeight
            // 
            lblHeight.AutoSize = true;
            lblHeight.Location = new Point(22, 79);
            lblHeight.Margin = new Padding(6, 0, 6, 0);
            lblHeight.Name = "lblHeight";
            lblHeight.Size = new Size(86, 32);
            lblHeight.TabIndex = 3;
            lblHeight.Text = "Height";
            // 
            // numHeight
            // 
            numHeight.Location = new Point(113, 79);
            numHeight.Margin = new Padding(6);
            numHeight.Maximum = new decimal(new int[] { 1024, 0, 0, 0 });
            numHeight.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numHeight.Name = "numHeight";
            numHeight.Size = new Size(106, 39);
            numHeight.TabIndex = 2;
            numHeight.Value = new decimal(new int[] { 32, 0, 0, 0 });
            // 
            // btnNew
            // 
            btnNew.Location = new Point(13, 141);
            btnNew.Margin = new Padding(6);
            btnNew.Name = "btnNew";
            btnNew.Size = new Size(139, 49);
            btnNew.TabIndex = 4;
            btnNew.Text = "New";
            btnNew.UseVisualStyleBackColor = true;
            btnNew.Click += btnNew_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(163, 141);
            btnCancel.Margin = new Padding(6);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(139, 49);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // NewMapDialog
            // 
            AcceptButton = btnNew;
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(325, 211);
            ControlBox = false;
            Controls.Add(btnCancel);
            Controls.Add(btnNew);
            Controls.Add(lblHeight);
            Controls.Add(numHeight);
            Controls.Add(lblWidth);
            Controls.Add(numWidth);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(6);
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
    }
}