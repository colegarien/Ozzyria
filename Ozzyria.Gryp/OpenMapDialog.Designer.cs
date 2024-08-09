namespace Ozzyria.Gryp
{
    partial class OpenMapDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpenMapDialog));
            btnCancel = new Button();
            btnOpen = new Button();
            lblMap = new Label();
            cmbMap = new ComboBox();
            btnDelete = new Button();
            SuspendLayout();
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(131, 52);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 6;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnOpen
            // 
            btnOpen.Location = new Point(12, 52);
            btnOpen.Name = "btnOpen";
            btnOpen.Size = new Size(75, 23);
            btnOpen.TabIndex = 5;
            btnOpen.Text = "Open";
            btnOpen.UseVisualStyleBackColor = true;
            btnOpen.Click += btnOpen_Click;
            // 
            // lblMap
            // 
            lblMap.AutoSize = true;
            lblMap.Location = new Point(12, 9);
            lblMap.Name = "lblMap";
            lblMap.Size = new Size(31, 15);
            lblMap.TabIndex = 7;
            lblMap.Text = "Map";
            // 
            // cmbMap
            // 
            cmbMap.FormattingEnabled = true;
            cmbMap.Location = new Point(49, 6);
            cmbMap.Name = "cmbMap";
            cmbMap.Size = new Size(121, 23);
            cmbMap.TabIndex = 8;
            // 
            // btnDelete
            // 
            btnDelete.Image = (Image)resources.GetObject("btnDelete.Image");
            btnDelete.Location = new Point(179, 6);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(27, 23);
            btnDelete.TabIndex = 9;
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // OpenMapDialog
            // 
            AcceptButton = btnOpen;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            CancelButton = btnCancel;
            ClientSize = new Size(218, 82);
            ControlBox = false;
            Controls.Add(btnDelete);
            Controls.Add(cmbMap);
            Controls.Add(lblMap);
            Controls.Add(btnCancel);
            Controls.Add(btnOpen);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "OpenMapDialog";
            ShowIcon = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Open Map";
            TopMost = true;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnCancel;
        private Button btnOpen;
        private Label lblMap;
        private ComboBox cmbMap;
        private Button btnDelete;
    }
}