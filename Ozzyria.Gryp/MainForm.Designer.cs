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
            toolPath = new ToolStripButton();
            toolWall = new ToolStripButton();
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
            menuStrip.ImageScalingSize = new Size(32, 32);
            menuStrip.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Padding = new Padding(11, 4, 0, 4);
            menuStrip.Size = new Size(1486, 44);
            menuStrip.TabIndex = 0;
            menuStrip.Text = "Main Menu";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(71, 36);
            fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.Size = new Size(210, 44);
            newToolStripMenuItem.Text = "New...";
            newToolStripMenuItem.Click += newToolStripMenuItem_Click;
            // 
            // mainToolbelt
            // 
            mainToolbelt.Dock = DockStyle.Left;
            mainToolbelt.GripStyle = ToolStripGripStyle.Hidden;
            mainToolbelt.ImageScalingSize = new Size(32, 32);
            mainToolbelt.Items.AddRange(new ToolStripItem[] { toolSelect, toolMove, toolStripSeparator, toolEntity, toolPath, toolWall, toolStripSeparator1, toolBrush, toolFill, toolRectangle, toolFilledRectangle, toolLine });
            mainToolbelt.Location = new Point(0, 44);
            mainToolbelt.Name = "mainToolbelt";
            mainToolbelt.Padding = new Padding(0, 0, 4, 0);
            mainToolbelt.Size = new Size(66, 916);
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
            toolSelect.Size = new Size(57, 36);
            toolSelect.Text = "&Select";
            // 
            // toolMove
            // 
            toolMove.CheckOnClick = true;
            toolMove.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolMove.Image = (Image)resources.GetObject("toolMove.Image");
            toolMove.ImageTransparentColor = Color.Magenta;
            toolMove.Name = "toolMove";
            toolMove.Size = new Size(57, 36);
            toolMove.Text = "&Move";
            // 
            // toolStripSeparator
            // 
            toolStripSeparator.Name = "toolStripSeparator";
            toolStripSeparator.Size = new Size(57, 6);
            // 
            // toolEntity
            // 
            toolEntity.CheckOnClick = true;
            toolEntity.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolEntity.Image = (Image)resources.GetObject("toolEntity.Image");
            toolEntity.ImageTransparentColor = Color.Magenta;
            toolEntity.Name = "toolEntity";
            toolEntity.Size = new Size(57, 36);
            toolEntity.Text = "&Entity";
            // 
            // toolPath
            // 
            toolPath.CheckOnClick = true;
            toolPath.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolPath.Image = (Image)resources.GetObject("toolPath.Image");
            toolPath.ImageTransparentColor = Color.Magenta;
            toolPath.Name = "toolPath";
            toolPath.Size = new Size(57, 36);
            toolPath.Text = "&Path";
            // 
            // toolWall
            // 
            toolWall.CheckOnClick = true;
            toolWall.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolWall.Image = (Image)resources.GetObject("toolWall.Image");
            toolWall.ImageTransparentColor = Color.Magenta;
            toolWall.Name = "toolWall";
            toolWall.Size = new Size(57, 36);
            toolWall.Text = "&Wall";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(57, 6);
            // 
            // toolBrush
            // 
            toolBrush.CheckOnClick = true;
            toolBrush.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolBrush.Image = (Image)resources.GetObject("toolBrush.Image");
            toolBrush.ImageTransparentColor = Color.Magenta;
            toolBrush.Name = "toolBrush";
            toolBrush.Size = new Size(57, 36);
            toolBrush.Text = "&Brush";
            // 
            // toolFill
            // 
            toolFill.CheckOnClick = true;
            toolFill.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolFill.Image = (Image)resources.GetObject("toolFill.Image");
            toolFill.ImageTransparentColor = Color.Magenta;
            toolFill.Name = "toolFill";
            toolFill.Size = new Size(57, 36);
            toolFill.Text = "&Fill";
            // 
            // toolRectangle
            // 
            toolRectangle.CheckOnClick = true;
            toolRectangle.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolRectangle.Image = (Image)resources.GetObject("toolRectangle.Image");
            toolRectangle.ImageTransparentColor = Color.Magenta;
            toolRectangle.Name = "toolRectangle";
            toolRectangle.Size = new Size(57, 36);
            toolRectangle.Text = "&Rectangle";
            // 
            // toolFilledRectangle
            // 
            toolFilledRectangle.CheckOnClick = true;
            toolFilledRectangle.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolFilledRectangle.Image = (Image)resources.GetObject("toolFilledRectangle.Image");
            toolFilledRectangle.ImageTransparentColor = Color.Magenta;
            toolFilledRectangle.Name = "toolFilledRectangle";
            toolFilledRectangle.Size = new Size(57, 36);
            toolFilledRectangle.Text = "&Filled Rectangle";
            // 
            // toolLine
            // 
            toolLine.CheckOnClick = true;
            toolLine.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolLine.Image = (Image)resources.GetObject("toolLine.Image");
            toolLine.ImageTransparentColor = Color.Magenta;
            toolLine.Name = "toolLine";
            toolLine.Size = new Size(57, 36);
            toolLine.Text = "&Line";
            // 
            // mainStatusStrip
            // 
            mainStatusStrip.ImageScalingSize = new Size(32, 32);
            mainStatusStrip.Items.AddRange(new ToolStripItem[] { mainStatusLabel });
            mainStatusStrip.Location = new Point(66, 918);
            mainStatusStrip.Name = "mainStatusStrip";
            mainStatusStrip.Padding = new Padding(2, 0, 26, 0);
            mainStatusStrip.Size = new Size(1420, 42);
            mainStatusStrip.SizingGrip = false;
            mainStatusStrip.TabIndex = 4;
            mainStatusStrip.Text = "Main Status Strip";
            // 
            // mainStatusLabel
            // 
            mainStatusLabel.Name = "mainStatusLabel";
            mainStatusLabel.Size = new Size(92, 32);
            mainStatusLabel.Text = "Loaded";
            // 
            // viewPortPanel
            // 
            viewPortPanel.BackColor = Color.FromArgb(64, 64, 64);
            viewPortPanel.Location = new Point(50, 51);
            viewPortPanel.Margin = new Padding(6, 6, 6, 6);
            viewPortPanel.Name = "viewPortPanel";
            viewPortPanel.Size = new Size(1051, 855);
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
            layerList.Location = new Point(1112, 51);
            layerList.Margin = new Padding(6, 6, 6, 6);
            layerList.MultiSelect = false;
            layerList.Name = "layerList";
            layerList.Size = new Size(348, 433);
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
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1486, 960);
            Controls.Add(layerList);
            Controls.Add(viewPortPanel);
            Controls.Add(mainStatusStrip);
            Controls.Add(mainToolbelt);
            Controls.Add(menuStrip);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip;
            Margin = new Padding(6, 6, 6, 6);
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
        private ToolStripButton toolPath;
        private ToolStripButton toolWall;
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
