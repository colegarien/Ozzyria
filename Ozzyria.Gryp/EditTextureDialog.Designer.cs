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
            cmbDrawables = new ComboBox();
            SuspendLayout();
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(120, 41);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 7;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(39, 41);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 23);
            btnSave.TabIndex = 6;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // lblResource
            // 
            lblResource.AutoSize = true;
            lblResource.Location = new Point(12, 9);
            lblResource.Name = "lblResource";
            lblResource.Size = new Size(56, 15);
            lblResource.TabIndex = 9;
            lblResource.Text = "Drawable";
            // 
            // cmbDrawables
            // 
            cmbDrawables.FormattingEnabled = true;
            cmbDrawables.Location = new Point(75, 6);
            cmbDrawables.Name = "cmbDrawables";
            cmbDrawables.Size = new Size(121, 23);
            cmbDrawables.TabIndex = 10;
            // 
            // EditTextureDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(204, 72);
            ControlBox = false;
            Controls.Add(cmbDrawables);
            Controls.Add(lblResource);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "EditTextureDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Edit Brush";
            TopMost = true;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnCancel;
        private Button btnSave;
        private Label lblResource;
        private ComboBox cmbDrawables;
    }
}