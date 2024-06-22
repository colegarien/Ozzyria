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
            reRenderTimer = new System.Windows.Forms.Timer(components);
            layerList = new ListView();
            layerImageList = new ImageList(components);
            mapViewPort = new SkiaSharp.Views.Desktop.SKGLControl();
            logicTimer = new System.Windows.Forms.Timer(components);
            btnNewLayer = new Button();
            btnRemoveLayer = new Button();
            btnHideShowLayer = new Button();
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
            mainToolbelt.ImageScalingSize = new Size(32, 32);
            mainToolbelt.Items.AddRange(new ToolStripItem[] { toolSelect, toolMove, toolStripSeparator, toolEntity, toolPath, toolWall, toolStripSeparator1, toolBrush, toolFill, toolRectangle, toolFilledRectangle, toolLine });
            mainToolbelt.Location = new Point(0, 24);
            mainToolbelt.Name = "mainToolbelt";
            mainToolbelt.Padding = new Padding(0, 0, 2, 0);
            mainToolbelt.Size = new Size(33, 426);
            mainToolbelt.TabIndex = 1;
            mainToolbelt.Tag = "select";
            mainToolbelt.Text = "Toolbelt";
            // 
            // toolSelect
            // 
            toolSelect.CheckOnClick = true;
            toolSelect.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolSelect.Image = (Image)resources.GetObject("toolSelect.Image");
            toolSelect.ImageScaling = ToolStripItemImageScaling.None;
            toolSelect.ImageTransparentColor = Color.Magenta;
            toolSelect.Name = "toolSelect";
            toolSelect.Size = new Size(28, 20);
            toolSelect.Tag = "select";
            toolSelect.Text = "&Select";
            toolSelect.CheckedChanged += onToolChecked_CheckedChanged;
            // 
            // toolMove
            // 
            toolMove.CheckOnClick = true;
            toolMove.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolMove.Image = (Image)resources.GetObject("toolMove.Image");
            toolMove.ImageScaling = ToolStripItemImageScaling.None;
            toolMove.ImageTransparentColor = Color.Magenta;
            toolMove.Name = "toolMove";
            toolMove.Size = new Size(28, 20);
            toolMove.Tag = "move";
            toolMove.Text = "&Move";
            toolMove.CheckedChanged += onToolChecked_CheckedChanged;
            // 
            // toolStripSeparator
            // 
            toolStripSeparator.Name = "toolStripSeparator";
            toolStripSeparator.Size = new Size(28, 6);
            // 
            // toolEntity
            // 
            toolEntity.CheckOnClick = true;
            toolEntity.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolEntity.Image = (Image)resources.GetObject("toolEntity.Image");
            toolEntity.ImageScaling = ToolStripItemImageScaling.None;
            toolEntity.ImageTransparentColor = Color.Magenta;
            toolEntity.Name = "toolEntity";
            toolEntity.Size = new Size(28, 20);
            toolEntity.Tag = "entity";
            toolEntity.Text = "&Entity";
            toolEntity.CheckedChanged += onToolChecked_CheckedChanged;
            // 
            // toolPath
            // 
            toolPath.CheckOnClick = true;
            toolPath.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolPath.Image = (Image)resources.GetObject("toolPath.Image");
            toolPath.ImageScaling = ToolStripItemImageScaling.None;
            toolPath.ImageTransparentColor = Color.Magenta;
            toolPath.Name = "toolPath";
            toolPath.Size = new Size(28, 20);
            toolPath.Tag = "path";
            toolPath.Text = "&Path";
            toolPath.CheckedChanged += onToolChecked_CheckedChanged;
            // 
            // toolWall
            // 
            toolWall.CheckOnClick = true;
            toolWall.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolWall.Image = (Image)resources.GetObject("toolWall.Image");
            toolWall.ImageScaling = ToolStripItemImageScaling.None;
            toolWall.ImageTransparentColor = Color.Magenta;
            toolWall.Name = "toolWall";
            toolWall.Size = new Size(28, 20);
            toolWall.Tag = "wall";
            toolWall.Text = "&Wall";
            toolWall.CheckedChanged += onToolChecked_CheckedChanged;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(28, 6);
            // 
            // toolBrush
            // 
            toolBrush.CheckOnClick = true;
            toolBrush.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolBrush.Image = (Image)resources.GetObject("toolBrush.Image");
            toolBrush.ImageScaling = ToolStripItemImageScaling.None;
            toolBrush.ImageTransparentColor = Color.Magenta;
            toolBrush.Name = "toolBrush";
            toolBrush.Size = new Size(28, 20);
            toolBrush.Tag = "brush";
            toolBrush.Text = "&Brush";
            toolBrush.CheckedChanged += onToolChecked_CheckedChanged;
            // 
            // toolFill
            // 
            toolFill.CheckOnClick = true;
            toolFill.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolFill.Image = (Image)resources.GetObject("toolFill.Image");
            toolFill.ImageScaling = ToolStripItemImageScaling.None;
            toolFill.ImageTransparentColor = Color.Magenta;
            toolFill.Name = "toolFill";
            toolFill.Size = new Size(28, 20);
            toolFill.Tag = "fill";
            toolFill.Text = "&Fill";
            toolFill.CheckedChanged += onToolChecked_CheckedChanged;
            // 
            // toolRectangle
            // 
            toolRectangle.CheckOnClick = true;
            toolRectangle.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolRectangle.Image = (Image)resources.GetObject("toolRectangle.Image");
            toolRectangle.ImageScaling = ToolStripItemImageScaling.None;
            toolRectangle.ImageTransparentColor = Color.Magenta;
            toolRectangle.Name = "toolRectangle";
            toolRectangle.Size = new Size(28, 20);
            toolRectangle.Tag = "rectangle";
            toolRectangle.Text = "&Rectangle";
            toolRectangle.CheckedChanged += onToolChecked_CheckedChanged;
            // 
            // toolFilledRectangle
            // 
            toolFilledRectangle.CheckOnClick = true;
            toolFilledRectangle.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolFilledRectangle.Image = (Image)resources.GetObject("toolFilledRectangle.Image");
            toolFilledRectangle.ImageScaling = ToolStripItemImageScaling.None;
            toolFilledRectangle.ImageTransparentColor = Color.Magenta;
            toolFilledRectangle.Name = "toolFilledRectangle";
            toolFilledRectangle.Size = new Size(28, 20);
            toolFilledRectangle.Tag = "filled_rectangle";
            toolFilledRectangle.Text = "&Filled Rectangle";
            toolFilledRectangle.CheckedChanged += onToolChecked_CheckedChanged;
            // 
            // toolLine
            // 
            toolLine.CheckOnClick = true;
            toolLine.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolLine.Image = (Image)resources.GetObject("toolLine.Image");
            toolLine.ImageScaling = ToolStripItemImageScaling.None;
            toolLine.ImageTransparentColor = Color.Magenta;
            toolLine.Name = "toolLine";
            toolLine.Size = new Size(28, 20);
            toolLine.Tag = "line";
            toolLine.Text = "&Line";
            toolLine.CheckedChanged += onToolChecked_CheckedChanged;
            // 
            // mainStatusStrip
            // 
            mainStatusStrip.ImageScalingSize = new Size(32, 32);
            mainStatusStrip.Items.AddRange(new ToolStripItem[] { mainStatusLabel });
            mainStatusStrip.Location = new Point(33, 428);
            mainStatusStrip.Name = "mainStatusStrip";
            mainStatusStrip.Size = new Size(767, 22);
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
            // reRenderTimer
            // 
            reRenderTimer.Enabled = true;
            reRenderTimer.Interval = 30;
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
            layerList.SelectedIndexChanged += layerList_SelectedIndexChanged;
            // 
            // layerImageList
            // 
            layerImageList.ColorDepth = ColorDepth.Depth32Bit;
            layerImageList.ImageSize = new Size(16, 16);
            layerImageList.TransparentColor = Color.Transparent;
            // 
            // mapViewPort
            // 
            mapViewPort.BackColor = Color.Black;
            mapViewPort.Location = new Point(29, 24);
            mapViewPort.Margin = new Padding(4, 3, 4, 3);
            mapViewPort.Name = "mapViewPort";
            mapViewPort.Size = new Size(563, 401);
            mapViewPort.TabIndex = 8;
            mapViewPort.VSync = false;
            mapViewPort.PaintSurface += mapViewPort_PaintSurface;
            mapViewPort.MouseDown += mapViewPort_MouseDown;
            mapViewPort.MouseMove += mapViewPort_MouseMove;
            mapViewPort.MouseUp += mapViewPort_MouseUp;
            mapViewPort.MouseWheel += mapViewPort_MouseWheel;
            // 
            // logicTimer
            // 
            logicTimer.Enabled = true;
            logicTimer.Interval = 20;
            logicTimer.Tick += logicTimer_Tick;
            // 
            // btnNewLayer
            // 
            btnNewLayer.Location = new Point(599, 235);
            btnNewLayer.Name = "btnNewLayer";
            btnNewLayer.Size = new Size(54, 23);
            btnNewLayer.TabIndex = 10;
            btnNewLayer.Text = "Add";
            btnNewLayer.UseVisualStyleBackColor = true;
            btnNewLayer.Click += btnNewLayer_Click;
            // 
            // btnRemoveLayer
            // 
            btnRemoveLayer.Location = new Point(659, 235);
            btnRemoveLayer.Name = "btnRemoveLayer";
            btnRemoveLayer.Size = new Size(69, 23);
            btnRemoveLayer.TabIndex = 11;
            btnRemoveLayer.Text = "Remove";
            btnRemoveLayer.UseVisualStyleBackColor = true;
            btnRemoveLayer.Click += btnRemoveLayer_Click;
            // 
            // btnHideShowLayer
            // 
            btnHideShowLayer.Location = new Point(734, 235);
            btnHideShowLayer.Name = "btnHideShowLayer";
            btnHideShowLayer.Size = new Size(54, 23);
            btnHideShowLayer.TabIndex = 12;
            btnHideShowLayer.Text = "Hide";
            btnHideShowLayer.UseVisualStyleBackColor = true;
            btnHideShowLayer.Click += btnHideShowLayer_Click;
            // 
            // MainGrypWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnHideShowLayer);
            Controls.Add(btnRemoveLayer);
            Controls.Add(btnNewLayer);
            Controls.Add(mapViewPort);
            Controls.Add(layerList);
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
        private ToolStripButton toolPath;
        private ToolStripButton toolWall;
        private ToolStripSeparator toolStripSeparator1;
        private StatusStrip mainStatusStrip;
        private ToolStripStatusLabel mainStatusLabel;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripButton toolRectangle;
        private ToolStripButton toolFilledRectangle;
        private ToolStripButton toolLine;
        private System.Windows.Forms.Timer reRenderTimer;
        private ListView layerList;
        private ImageList layerImageList;
        private SkiaSharp.Views.Desktop.SKGLControl mapViewPort;
        private System.Windows.Forms.Timer logicTimer;
        private Button btnNewLayer;
        private Button btnRemoveLayer;
        private Button btnHideShowLayer;
    }
}
