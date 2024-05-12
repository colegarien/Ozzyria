namespace Ozzyria.Gryp
{
    partial class MainGrypWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainGrypWindow));
            menuStrip = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            newToolStripMenuItem = new ToolStripMenuItem();
            mainToolbelt = new ToolStrip();
            toolSelect = new ToolStripButton();
            toolMove = new ToolStripButton();
            toolStripSeparator = new ToolStripSeparator();
            toolEntity = new ToolStripButton();
            copyToolStripButton = new ToolStripButton();
            pasteToolStripButton = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            toolBrush = new ToolStripButton();
            toolFill = new ToolStripButton();
            toolRectangle = new ToolStripButton();
            toolFilledRectangle = new ToolStripButton();
            toolLine = new ToolStripButton();
            mainStatusStrip = new StatusStrip();
            mainStatusLabel = new ToolStripStatusLabel();
            viewPortPanel = new Panel();
            reRenderTimer = new System.Windows.Forms.Timer(components);
            layerList = new ListView();
            layerImageList = new ImageList(components);
            menuStrip.SuspendLayout();
            mainToolbelt.SuspendLayout();
            mainStatusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(800, 24);
            menuStrip.TabIndex = 0;
            menuStrip.Text = "Main Menu";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.Size = new Size(107, 22);
            newToolStripMenuItem.Text = "New...";
            newToolStripMenuItem.Click += newToolStripMenuItem_Click;
            // 
            // mainToolbelt
            // 
            mainToolbelt.Dock = DockStyle.Left;
            mainToolbelt.GripStyle = ToolStripGripStyle.Hidden;
            mainToolbelt.Items.AddRange(new ToolStripItem[] { toolSelect, toolMove, toolStripSeparator, toolEntity, copyToolStripButton, pasteToolStripButton, toolStripSeparator1, toolBrush, toolFill, toolRectangle, toolFilledRectangle, toolLine });
            mainToolbelt.Location = new Point(0, 24);
            mainToolbelt.Name = "mainToolbelt";
            mainToolbelt.Size = new Size(24, 426);
            mainToolbelt.TabIndex = 1;
            mainToolbelt.Text = "Toolbelt";
            // 
            // toolSelect
            // 
            toolSelect.CheckOnClick = true;
            toolSelect.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolSelect.Image = (Image)resources.GetObject("toolSelect.Image");
            toolSelect.ImageTransparentColor = Color.Magenta;
            toolSelect.Name = "toolSelect";
            toolSelect.Size = new Size(21, 20);
            toolSelect.Text = "&Select";
            // 
            // toolMove
            // 
            toolMove.CheckOnClick = true;
            toolMove.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolMove.Image = (Image)resources.GetObject("toolMove.Image");
            toolMove.ImageTransparentColor = Color.Magenta;
            toolMove.Name = "toolMove";
            toolMove.Size = new Size(21, 20);
            toolMove.Text = "&Move";
            // 
            // toolStripSeparator
            // 
            toolStripSeparator.Name = "toolStripSeparator";
            toolStripSeparator.Size = new Size(21, 6);
            // 
            // toolEntity
            // 
            toolEntity.CheckOnClick = true;
            toolEntity.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolEntity.Image = (Image)resources.GetObject("toolEntity.Image");
            toolEntity.ImageTransparentColor = Color.Magenta;
            toolEntity.Name = "toolEntity";
            toolEntity.Size = new Size(21, 20);
            toolEntity.Text = "&Entity";
            // 
            // copyToolStripButton
            // 
            copyToolStripButton.CheckOnClick = true;
            copyToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            copyToolStripButton.Image = (Image)resources.GetObject("copyToolStripButton.Image");
            copyToolStripButton.ImageTransparentColor = Color.Magenta;
            copyToolStripButton.Name = "copyToolStripButton";
            copyToolStripButton.Size = new Size(21, 20);
            copyToolStripButton.Text = "&Path";
            // 
            // pasteToolStripButton
            // 
            pasteToolStripButton.CheckOnClick = true;
            pasteToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            pasteToolStripButton.Image = (Image)resources.GetObject("pasteToolStripButton.Image");
            pasteToolStripButton.ImageTransparentColor = Color.Magenta;
            pasteToolStripButton.Name = "pasteToolStripButton";
            pasteToolStripButton.Size = new Size(21, 20);
            pasteToolStripButton.Text = "&Wall";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(21, 6);
            // 
            // toolBrush
            // 
            toolBrush.CheckOnClick = true;
            toolBrush.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolBrush.Image = (Image)resources.GetObject("toolBrush.Image");
            toolBrush.ImageTransparentColor = Color.Magenta;
            toolBrush.Name = "toolBrush";
            toolBrush.Size = new Size(21, 20);
            toolBrush.Text = "&Brush";
            // 
            // toolFill
            // 
            toolFill.CheckOnClick = true;
            toolFill.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolFill.Image = (Image)resources.GetObject("toolFill.Image");
            toolFill.ImageTransparentColor = Color.Magenta;
            toolFill.Name = "toolFill";
            toolFill.Size = new Size(21, 20);
            toolFill.Text = "&Fill";
            // 
            // toolRectangle
            // 
            toolRectangle.CheckOnClick = true;
            toolRectangle.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolRectangle.Image = (Image)resources.GetObject("toolRectangle.Image");
            toolRectangle.ImageTransparentColor = Color.Magenta;
            toolRectangle.Name = "toolRectangle";
            toolRectangle.Size = new Size(21, 20);
            toolRectangle.Text = "&Rectangle";
            // 
            // toolFilledRectangle
            // 
            toolFilledRectangle.CheckOnClick = true;
            toolFilledRectangle.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolFilledRectangle.Image = (Image)resources.GetObject("toolFilledRectangle.Image");
            toolFilledRectangle.ImageTransparentColor = Color.Magenta;
            toolFilledRectangle.Name = "toolFilledRectangle";
            toolFilledRectangle.Size = new Size(21, 20);
            toolFilledRectangle.Text = "&Filled Rectangle";
            // 
            // toolLine
            // 
            toolLine.CheckOnClick = true;
            toolLine.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolLine.Image = (Image)resources.GetObject("toolLine.Image");
            toolLine.ImageTransparentColor = Color.Magenta;
            toolLine.Name = "toolLine";
            toolLine.Size = new Size(21, 20);
            toolLine.Text = "&Line";
            // 
            // mainStatusStrip
            // 
            mainStatusStrip.Items.AddRange(new ToolStripItem[] { mainStatusLabel });
            mainStatusStrip.Location = new Point(24, 428);
            mainStatusStrip.Name = "mainStatusStrip";
            mainStatusStrip.Size = new Size(776, 22);
            mainStatusStrip.SizingGrip = false;
            mainStatusStrip.TabIndex = 4;
            mainStatusStrip.Text = "Main Status Strip";
            // 
            // mainStatusLabel
            // 
            mainStatusLabel.Name = "mainStatusLabel";
            mainStatusLabel.Size = new Size(46, 17);
            mainStatusLabel.Text = "Loaded";
            // 
            // viewPortPanel
            // 
            viewPortPanel.BackColor = Color.FromArgb(64, 64, 64);
            viewPortPanel.Location = new Point(27, 24);
            viewPortPanel.Name = "viewPortPanel";
            viewPortPanel.Size = new Size(566, 401);
            viewPortPanel.TabIndex = 6;
            viewPortPanel.Scroll += viewPortPanel_Scroll;
            viewPortPanel.Paint += viewPortPanel_Paint;
            viewPortPanel.MouseMove += viewPortPanel_MouseMove;
            // 
            // reRenderTimer
            // 
            reRenderTimer.Enabled = true;
            reRenderTimer.Interval = 16;
            reRenderTimer.Tick += reRenderTimer_Tick;
            // 
            // layerList
            // 
            layerList.LargeImageList = layerImageList;
            layerList.Location = new Point(599, 24);
            layerList.MultiSelect = false;
            layerList.Name = "layerList";
            layerList.Size = new Size(189, 205);
            layerList.SmallImageList = layerImageList;
            layerList.TabIndex = 7;
            layerList.UseCompatibleStateImageBehavior = false;
            layerList.KeyUp += layerList_KeyUp;
            // 
            // layerImageList
            // 
            layerImageList.ColorDepth = ColorDepth.Depth32Bit;
            layerImageList.ImageSize = new Size(16, 16);
            layerImageList.TransparentColor = Color.Transparent;
            // 
            // MainGrypWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(layerList);
            Controls.Add(viewPortPanel);
            Controls.Add(mainStatusStrip);
            Controls.Add(mainToolbelt);
            Controls.Add(menuStrip);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip;
            Name = "MainGrypWindow";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Gryp";
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            mainToolbelt.ResumeLayout(false);
            mainToolbelt.PerformLayout();
            mainStatusStrip.ResumeLayout(false);
            mainStatusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStrip mainToolbelt;
        private ToolStripButton toolSelect;
        private ToolStripButton toolMove;
        private ToolStripButton toolBrush;
        private ToolStripButton toolFill;
        private ToolStripSeparator toolStripSeparator;
        private ToolStripButton toolEntity;
        private ToolStripButton copyToolStripButton;
        private ToolStripButton pasteToolStripButton;
        private ToolStripSeparator toolStripSeparator1;
        private StatusStrip mainStatusStrip;
        private ToolStripStatusLabel mainStatusLabel;
        private ToolStripMenuItem newToolStripMenuItem;
        private Panel viewPortPanel;
        private ToolStripButton toolRectangle;
        private ToolStripButton toolFilledRectangle;
        private ToolStripButton toolLine;
        private System.Windows.Forms.Timer reRenderTimer;
        private ListView layerList;
        private ImageList layerImageList;
    }
}
