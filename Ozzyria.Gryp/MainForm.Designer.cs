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
            openToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            mainToolbelt = new ToolStrip();
            toolSelect = new PixelToolStripButton();
            toolMove = new PixelToolStripButton();
            toolStripSeparator = new ToolStripSeparator();
            toolEntity = new PixelToolStripButton();
            toolPath = new PixelToolStripButton();
            toolWall = new PixelToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            toolBrush = new PixelToolStripButton();
            toolFill = new PixelToolStripButton();
            toolRectangle = new PixelToolStripButton();
            toolFilledRectangle = new PixelToolStripButton();
            toolLine = new PixelToolStripButton();
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
            lblBrush = new Label();
            listCurrentBrush = new ListView();
            btnRemoveBrush = new Button();
            btnAddBrush = new Button();
            btnBrushPreset = new Button();
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
            menuStrip.Size = new Size(860, 24);
            menuStrip.TabIndex = 0;
            menuStrip.Text = "Main Menu";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newToolStripMenuItem, openToolStripMenuItem, saveToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.Size = new Size(180, 22);
            newToolStripMenuItem.Text = "New...";
            newToolStripMenuItem.Click += newToolStripMenuItem_Click;
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(180, 22);
            openToolStripMenuItem.Text = "Open...";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            saveToolStripMenuItem.Size = new Size(180, 22);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
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
            mainToolbelt.Size = new Size(38, 426);
            mainToolbelt.TabIndex = 1;
            mainToolbelt.Tag = "select";
            mainToolbelt.Text = "Toolbelt";
            // 
            // toolSelect
            // 
            toolSelect.CheckOnClick = true;
            toolSelect.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolSelect.Image = (Image)resources.GetObject("toolSelect.Image");
            toolSelect.ImageTransparentColor = Color.Magenta;
            toolSelect.Name = "toolSelect";
            toolSelect.Size = new Size(33, 36);
            toolSelect.Tag = "select";
            toolSelect.Text = "&Select";
            toolSelect.CheckedChanged += onToolChecked_CheckedChanged;
            // 
            // toolMove
            // 
            toolMove.CheckOnClick = true;
            toolMove.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolMove.Image = (Image)resources.GetObject("toolMove.Image");
            toolMove.ImageTransparentColor = Color.Magenta;
            toolMove.Name = "toolMove";
            toolMove.Size = new Size(33, 36);
            toolMove.Tag = "move";
            toolMove.Text = "&Move";
            toolMove.CheckedChanged += onToolChecked_CheckedChanged;
            // 
            // toolStripSeparator
            // 
            toolStripSeparator.Name = "toolStripSeparator";
            toolStripSeparator.Size = new Size(33, 6);
            // 
            // toolEntity
            // 
            toolEntity.CheckOnClick = true;
            toolEntity.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolEntity.Image = (Image)resources.GetObject("toolEntity.Image");
            toolEntity.ImageTransparentColor = Color.Magenta;
            toolEntity.Name = "toolEntity";
            toolEntity.Size = new Size(33, 36);
            toolEntity.Tag = "entity";
            toolEntity.Text = "&Entity";
            toolEntity.CheckedChanged += onToolChecked_CheckedChanged;
            // 
            // toolPath
            // 
            toolPath.CheckOnClick = true;
            toolPath.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolPath.Image = (Image)resources.GetObject("toolPath.Image");
            toolPath.ImageTransparentColor = Color.Magenta;
            toolPath.Name = "toolPath";
            toolPath.Size = new Size(33, 36);
            toolPath.Tag = "path";
            toolPath.Text = "&Path";
            toolPath.CheckedChanged += onToolChecked_CheckedChanged;
            // 
            // toolWall
            // 
            toolWall.CheckOnClick = true;
            toolWall.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolWall.Image = (Image)resources.GetObject("toolWall.Image");
            toolWall.ImageTransparentColor = Color.Magenta;
            toolWall.Name = "toolWall";
            toolWall.Size = new Size(33, 36);
            toolWall.Tag = "wall";
            toolWall.Text = "&Wall";
            toolWall.CheckedChanged += onToolChecked_CheckedChanged;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(33, 6);
            // 
            // toolBrush
            // 
            toolBrush.CheckOnClick = true;
            toolBrush.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolBrush.Image = (Image)resources.GetObject("toolBrush.Image");
            toolBrush.ImageTransparentColor = Color.Magenta;
            toolBrush.Name = "toolBrush";
            toolBrush.Size = new Size(33, 36);
            toolBrush.Tag = "brush";
            toolBrush.Text = "&Brush";
            toolBrush.CheckedChanged += onToolChecked_CheckedChanged;
            // 
            // toolFill
            // 
            toolFill.CheckOnClick = true;
            toolFill.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolFill.Image = (Image)resources.GetObject("toolFill.Image");
            toolFill.ImageTransparentColor = Color.Magenta;
            toolFill.Name = "toolFill";
            toolFill.Size = new Size(33, 36);
            toolFill.Tag = "fill";
            toolFill.Text = "&Fill";
            toolFill.CheckedChanged += onToolChecked_CheckedChanged;
            // 
            // toolRectangle
            // 
            toolRectangle.CheckOnClick = true;
            toolRectangle.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolRectangle.Image = (Image)resources.GetObject("toolRectangle.Image");
            toolRectangle.ImageTransparentColor = Color.Magenta;
            toolRectangle.Name = "toolRectangle";
            toolRectangle.Size = new Size(33, 36);
            toolRectangle.Tag = "rectangle";
            toolRectangle.Text = "&Rectangle";
            toolRectangle.CheckedChanged += onToolChecked_CheckedChanged;
            // 
            // toolFilledRectangle
            // 
            toolFilledRectangle.CheckOnClick = true;
            toolFilledRectangle.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolFilledRectangle.Image = (Image)resources.GetObject("toolFilledRectangle.Image");
            toolFilledRectangle.ImageTransparentColor = Color.Magenta;
            toolFilledRectangle.Name = "toolFilledRectangle";
            toolFilledRectangle.Size = new Size(33, 36);
            toolFilledRectangle.Tag = "filled_rectangle";
            toolFilledRectangle.Text = "&Filled Rectangle";
            toolFilledRectangle.CheckedChanged += onToolChecked_CheckedChanged;
            // 
            // toolLine
            // 
            toolLine.CheckOnClick = true;
            toolLine.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolLine.Image = (Image)resources.GetObject("toolLine.Image");
            toolLine.ImageTransparentColor = Color.Magenta;
            toolLine.Name = "toolLine";
            toolLine.Size = new Size(33, 36);
            toolLine.Tag = "line";
            toolLine.Text = "&Line";
            toolLine.CheckedChanged += onToolChecked_CheckedChanged;
            // 
            // mainStatusStrip
            // 
            mainStatusStrip.ImageScalingSize = new Size(32, 32);
            mainStatusStrip.Items.AddRange(new ToolStripItem[] { mainStatusLabel });
            mainStatusStrip.Location = new Point(38, 428);
            mainStatusStrip.Name = "mainStatusStrip";
            mainStatusStrip.Size = new Size(822, 22);
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
            layerList.Location = new Point(664, 24);
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
            layerImageList.ImageSize = new Size(32, 32);
            layerImageList.TransparentColor = Color.Transparent;
            // 
            // mapViewPort
            // 
            mapViewPort.BackColor = Color.Black;
            mapViewPort.Location = new Point(34, 24);
            mapViewPort.Margin = new Padding(4, 3, 4, 3);
            mapViewPort.Name = "mapViewPort";
            mapViewPort.Size = new Size(623, 401);
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
            btnNewLayer.Location = new Point(664, 235);
            btnNewLayer.Name = "btnNewLayer";
            btnNewLayer.Size = new Size(54, 23);
            btnNewLayer.TabIndex = 10;
            btnNewLayer.Text = "Add";
            btnNewLayer.UseVisualStyleBackColor = true;
            btnNewLayer.Click += btnNewLayer_Click;
            // 
            // btnRemoveLayer
            // 
            btnRemoveLayer.Location = new Point(724, 235);
            btnRemoveLayer.Name = "btnRemoveLayer";
            btnRemoveLayer.Size = new Size(69, 23);
            btnRemoveLayer.TabIndex = 11;
            btnRemoveLayer.Text = "Remove";
            btnRemoveLayer.UseVisualStyleBackColor = true;
            btnRemoveLayer.Click += btnRemoveLayer_Click;
            // 
            // btnHideShowLayer
            // 
            btnHideShowLayer.Location = new Point(799, 235);
            btnHideShowLayer.Name = "btnHideShowLayer";
            btnHideShowLayer.Size = new Size(54, 23);
            btnHideShowLayer.TabIndex = 12;
            btnHideShowLayer.Text = "Hide";
            btnHideShowLayer.UseVisualStyleBackColor = true;
            btnHideShowLayer.Click += btnHideShowLayer_Click;
            // 
            // lblBrush
            // 
            lblBrush.AutoSize = true;
            lblBrush.Location = new Point(664, 277);
            lblBrush.Name = "lblBrush";
            lblBrush.Size = new Size(37, 15);
            lblBrush.TabIndex = 13;
            lblBrush.Text = "Brush";
            // 
            // listCurrentBrush
            // 
            listCurrentBrush.Location = new Point(664, 295);
            listCurrentBrush.MultiSelect = false;
            listCurrentBrush.Name = "listCurrentBrush";
            listCurrentBrush.Size = new Size(189, 100);
            listCurrentBrush.TabIndex = 14;
            listCurrentBrush.UseCompatibleStateImageBehavior = false;
            listCurrentBrush.DoubleClick += listCurrentBrush_DoubleClick;
            // 
            // btnRemoveBrush
            // 
            btnRemoveBrush.Location = new Point(724, 402);
            btnRemoveBrush.Name = "btnRemoveBrush";
            btnRemoveBrush.Size = new Size(69, 23);
            btnRemoveBrush.TabIndex = 16;
            btnRemoveBrush.Text = "Remove";
            btnRemoveBrush.UseVisualStyleBackColor = true;
            btnRemoveBrush.Click += btnRemoveBrush_Click;
            // 
            // btnAddBrush
            // 
            btnAddBrush.Location = new Point(664, 401);
            btnAddBrush.Name = "btnAddBrush";
            btnAddBrush.Size = new Size(54, 23);
            btnAddBrush.TabIndex = 15;
            btnAddBrush.Text = "Add";
            btnAddBrush.UseVisualStyleBackColor = true;
            btnAddBrush.Click += btnAddBrush_Click;
            // 
            // btnBrushPreset
            // 
            btnBrushPreset.Location = new Point(799, 401);
            btnBrushPreset.Name = "btnBrushPreset";
            btnBrushPreset.Size = new Size(54, 23);
            btnBrushPreset.TabIndex = 17;
            btnBrushPreset.Text = "Preset";
            btnBrushPreset.UseVisualStyleBackColor = true;
            btnBrushPreset.Click += btnBrushPreset_Click;
            // 
            // MainGrypWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(860, 450);
            Controls.Add(btnBrushPreset);
            Controls.Add(btnRemoveBrush);
            Controls.Add(btnAddBrush);
            Controls.Add(listCurrentBrush);
            Controls.Add(lblBrush);
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
        private ToolStripSeparator toolStripSeparator;
        private ToolStripSeparator toolStripSeparator1;
        private StatusStrip mainStatusStrip;
        private ToolStripStatusLabel mainStatusLabel;
        private ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.Timer reRenderTimer;
        private ListView layerList;
        private ImageList layerImageList;
        private SkiaSharp.Views.Desktop.SKGLControl mapViewPort;
        private System.Windows.Forms.Timer logicTimer;
        private Button btnNewLayer;
        private Button btnRemoveLayer;
        private Button btnHideShowLayer;
        private Label lblBrush;
        private ListView listCurrentBrush;
        private Button btnRemoveBrush;
        private Button btnAddBrush;
        private ComboBox comboBox1;
        private Button btnBrushPreset;
        private PixelToolStripButton toolSelect;
        private PixelToolStripButton toolMove;
        private PixelToolStripButton toolBrush;
        private PixelToolStripButton toolFill;
        private PixelToolStripButton toolEntity;
        private PixelToolStripButton toolPath;
        private PixelToolStripButton toolWall;
        private PixelToolStripButton toolRectangle;
        private PixelToolStripButton toolFilledRectangle;
        private PixelToolStripButton toolLine;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
    }
}
