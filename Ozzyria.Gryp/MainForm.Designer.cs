using Ozzyria.Gryp.UI.Elements;

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
            exitToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            undoToolStripMenuItem = new ToolStripMenuItem();
            redoToolStripMenuItem = new ToolStripMenuItem();
            mainToolbelt = new ToolBeltStrip();
            toolSelect = new PixelToolStripButton();
            toolMove = new PixelToolStripButton();
            toolStripSeparator = new ToolStripSeparator();
            toolEntity = new PixelToolStripButton();
            toolPath = new PixelToolStripButton();
            toolWall = new PixelToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            toolDropper = new PixelToolStripButton();
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
            mapViewPort = new MapViewPort();
            logicTimer = new System.Windows.Forms.Timer(components);
            btnNewLayer = new Button();
            btnRemoveLayer = new Button();
            btnHideShowLayer = new Button();
            listCurrentBrush = new ListView();
            btnRemoveBrush = new Button();
            btnAddBrush = new Button();
            btnBrushPreset = new Button();
            toolTabs = new TabControl();
            tabBrush = new TabPage();
            tabEntity = new TabPage();
            tableEntityAttributes = new DataGridView();
            columnKey = new DataGridViewTextBoxColumn();
            columnValue = new DataGridViewTextBoxColumn();
            cmbPrefab = new ComboBox();
            lblPrefab = new Label();
            menuStrip.SuspendLayout();
            mainToolbelt.SuspendLayout();
            mainStatusStrip.SuspendLayout();
            toolTabs.SuspendLayout();
            tabBrush.SuspendLayout();
            tabEntity.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)tableEntityAttributes).BeginInit();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.ImageScalingSize = new Size(32, 32);
            menuStrip.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(875, 24);
            menuStrip.TabIndex = 0;
            menuStrip.Text = "Main Menu";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newToolStripMenuItem, openToolStripMenuItem, saveToolStripMenuItem, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.Size = new Size(138, 22);
            newToolStripMenuItem.Text = "New...";
            newToolStripMenuItem.Click += newToolStripMenuItem_Click;
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(138, 22);
            openToolStripMenuItem.Text = "Open...";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            saveToolStripMenuItem.Size = new Size(138, 22);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(138, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { undoToolStripMenuItem, redoToolStripMenuItem });
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(39, 20);
            editToolStripMenuItem.Text = "Edit";
            // 
            // undoToolStripMenuItem
            // 
            undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            undoToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Z;
            undoToolStripMenuItem.Size = new Size(144, 22);
            undoToolStripMenuItem.Text = "Undo";
            undoToolStripMenuItem.Click += undoToolStripMenuItem_Click;
            // 
            // redoToolStripMenuItem
            // 
            redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            redoToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Y;
            redoToolStripMenuItem.Size = new Size(144, 22);
            redoToolStripMenuItem.Text = "Redo";
            redoToolStripMenuItem.Click += redoToolStripMenuItem_Click;
            // 
            // mainToolbelt
            // 
            mainToolbelt.Dock = DockStyle.Left;
            mainToolbelt.GripStyle = ToolStripGripStyle.Hidden;
            mainToolbelt.ImageScalingSize = new Size(32, 32);
            mainToolbelt.Items.AddRange(new ToolStripItem[] { toolSelect, toolMove, toolStripSeparator, toolEntity, toolPath, toolWall, toolStripSeparator1, toolDropper, toolBrush, toolFill, toolRectangle, toolFilledRectangle, toolLine });
            mainToolbelt.Location = new Point(0, 24);
            mainToolbelt.Name = "mainToolbelt";
            mainToolbelt.Padding = new Padding(0, 0, 2, 0);
            mainToolbelt.Size = new Size(38, 469);
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
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(33, 6);
            // 
            // toolDropper
            // 
            toolDropper.CheckOnClick = true;
            toolDropper.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolDropper.Image = (Image)resources.GetObject("toolDropper.Image");
            toolDropper.ImageTransparentColor = Color.Magenta;
            toolDropper.Name = "toolDropper";
            toolDropper.Size = new Size(33, 36);
            toolDropper.Tag = "dropper";
            toolDropper.Text = "&Dropper";
            toolDropper.ToolTipText = "Dropper";
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
            // 
            // mainStatusStrip
            // 
            mainStatusStrip.ImageScalingSize = new Size(32, 32);
            mainStatusStrip.Items.AddRange(new ToolStripItem[] { mainStatusLabel });
            mainStatusStrip.Location = new Point(38, 471);
            mainStatusStrip.Name = "mainStatusStrip";
            mainStatusStrip.Size = new Size(837, 22);
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
            layerList.Size = new Size(199, 205);
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
            mapViewPort.Size = new Size(623, 444);
            mapViewPort.TabIndex = 8;
            mapViewPort.VSync = false;
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
            btnRemoveLayer.Location = new Point(729, 235);
            btnRemoveLayer.Name = "btnRemoveLayer";
            btnRemoveLayer.Size = new Size(69, 23);
            btnRemoveLayer.TabIndex = 11;
            btnRemoveLayer.Text = "Remove";
            btnRemoveLayer.UseVisualStyleBackColor = true;
            btnRemoveLayer.Click += btnRemoveLayer_Click;
            // 
            // btnHideShowLayer
            // 
            btnHideShowLayer.Location = new Point(809, 235);
            btnHideShowLayer.Name = "btnHideShowLayer";
            btnHideShowLayer.Size = new Size(54, 23);
            btnHideShowLayer.TabIndex = 12;
            btnHideShowLayer.Text = "Hide";
            btnHideShowLayer.UseVisualStyleBackColor = true;
            btnHideShowLayer.Click += btnHideShowLayer_Click;
            // 
            // listCurrentBrush
            // 
            listCurrentBrush.Location = new Point(6, 6);
            listCurrentBrush.MultiSelect = false;
            listCurrentBrush.Name = "listCurrentBrush";
            listCurrentBrush.Size = new Size(189, 136);
            listCurrentBrush.TabIndex = 14;
            listCurrentBrush.UseCompatibleStateImageBehavior = false;
            listCurrentBrush.DoubleClick += listCurrentBrush_DoubleClick;
            // 
            // btnRemoveBrush
            // 
            btnRemoveBrush.Location = new Point(66, 148);
            btnRemoveBrush.Name = "btnRemoveBrush";
            btnRemoveBrush.Size = new Size(69, 23);
            btnRemoveBrush.TabIndex = 16;
            btnRemoveBrush.Text = "Remove";
            btnRemoveBrush.UseVisualStyleBackColor = true;
            btnRemoveBrush.Click += btnRemoveBrush_Click;
            // 
            // btnAddBrush
            // 
            btnAddBrush.Location = new Point(6, 147);
            btnAddBrush.Name = "btnAddBrush";
            btnAddBrush.Size = new Size(54, 23);
            btnAddBrush.TabIndex = 15;
            btnAddBrush.Text = "Add";
            btnAddBrush.UseVisualStyleBackColor = true;
            btnAddBrush.Click += btnAddBrush_Click;
            // 
            // btnBrushPreset
            // 
            btnBrushPreset.Location = new Point(141, 147);
            btnBrushPreset.Name = "btnBrushPreset";
            btnBrushPreset.Size = new Size(54, 23);
            btnBrushPreset.TabIndex = 17;
            btnBrushPreset.Text = "Preset";
            btnBrushPreset.UseVisualStyleBackColor = true;
            btnBrushPreset.Click += btnBrushPreset_Click;
            // 
            // toolTabs
            // 
            toolTabs.Controls.Add(tabBrush);
            toolTabs.Controls.Add(tabEntity);
            toolTabs.Location = new Point(664, 264);
            toolTabs.Name = "toolTabs";
            toolTabs.SelectedIndex = 0;
            toolTabs.Size = new Size(208, 203);
            toolTabs.TabIndex = 18;
            // 
            // tabBrush
            // 
            tabBrush.Controls.Add(listCurrentBrush);
            tabBrush.Controls.Add(btnBrushPreset);
            tabBrush.Controls.Add(btnRemoveBrush);
            tabBrush.Controls.Add(btnAddBrush);
            tabBrush.Location = new Point(4, 24);
            tabBrush.Name = "tabBrush";
            tabBrush.Padding = new Padding(3);
            tabBrush.Size = new Size(200, 175);
            tabBrush.TabIndex = 0;
            tabBrush.Text = "Brush";
            tabBrush.UseVisualStyleBackColor = true;
            // 
            // tabEntity
            // 
            tabEntity.Controls.Add(tableEntityAttributes);
            tabEntity.Controls.Add(cmbPrefab);
            tabEntity.Controls.Add(lblPrefab);
            tabEntity.Location = new Point(4, 24);
            tabEntity.Name = "tabEntity";
            tabEntity.Padding = new Padding(3);
            tabEntity.Size = new Size(200, 175);
            tabEntity.TabIndex = 1;
            tabEntity.Text = "Entity";
            tabEntity.UseVisualStyleBackColor = true;
            // 
            // tableEntityAttributes
            // 
            tableEntityAttributes.AllowUserToAddRows = false;
            tableEntityAttributes.AllowUserToDeleteRows = false;
            tableEntityAttributes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tableEntityAttributes.ColumnHeadersVisible = false;
            tableEntityAttributes.Columns.AddRange(new DataGridViewColumn[] { columnKey, columnValue });
            tableEntityAttributes.EditMode = DataGridViewEditMode.EditOnEnter;
            tableEntityAttributes.Location = new Point(6, 38);
            tableEntityAttributes.Name = "tableEntityAttributes";
            tableEntityAttributes.RowHeadersVisible = false;
            tableEntityAttributes.Size = new Size(188, 131);
            tableEntityAttributes.TabIndex = 2;
            tableEntityAttributes.CellValueChanged += tableEntityAttributes_CellValueChanged;
            // 
            // columnKey
            // 
            columnKey.HeaderText = "Key";
            columnKey.Name = "columnKey";
            columnKey.ReadOnly = true;
            // 
            // columnValue
            // 
            columnValue.HeaderText = "Value";
            columnValue.Name = "columnValue";
            // 
            // cmbPrefab
            // 
            cmbPrefab.FormattingEnabled = true;
            cmbPrefab.Location = new Point(53, 9);
            cmbPrefab.Name = "cmbPrefab";
            cmbPrefab.Size = new Size(141, 23);
            cmbPrefab.TabIndex = 1;
            cmbPrefab.SelectedIndexChanged += cmbPrefab_SelectedIndexChanged;
            // 
            // lblPrefab
            // 
            lblPrefab.AutoSize = true;
            lblPrefab.Location = new Point(6, 12);
            lblPrefab.Name = "lblPrefab";
            lblPrefab.Size = new Size(41, 15);
            lblPrefab.TabIndex = 0;
            lblPrefab.Text = "Prefab";
            // 
            // MainGrypWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(875, 493);
            Controls.Add(toolTabs);
            Controls.Add(btnHideShowLayer);
            Controls.Add(btnRemoveLayer);
            Controls.Add(btnNewLayer);
            Controls.Add(mapViewPort);
            Controls.Add(layerList);
            Controls.Add(mainStatusStrip);
            Controls.Add(mainToolbelt);
            Controls.Add(menuStrip);
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            MainMenuStrip = menuStrip;
            Name = "MainGrypWindow";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Gryp";
            FormClosing += MainGrypWindow_FormClosing;
            KeyDown += MainGrypWindow_KeyDown;
            KeyUp += MainGrypWindow_KeyUp;
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            mainToolbelt.ResumeLayout(false);
            mainToolbelt.PerformLayout();
            mainStatusStrip.ResumeLayout(false);
            mainStatusStrip.PerformLayout();
            toolTabs.ResumeLayout(false);
            tabBrush.ResumeLayout(false);
            tabEntity.ResumeLayout(false);
            tabEntity.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)tableEntityAttributes).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolBeltStrip mainToolbelt;
        private ToolStripSeparator toolStripSeparator;
        private ToolStripSeparator toolStripSeparator1;
        private StatusStrip mainStatusStrip;
        private ToolStripStatusLabel mainStatusLabel;
        private ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.Timer reRenderTimer;
        private ListView layerList;
        private ImageList layerImageList;
        private MapViewPort mapViewPort;
        private System.Windows.Forms.Timer logicTimer;
        private Button btnNewLayer;
        private Button btnRemoveLayer;
        private Button btnHideShowLayer;
        private ListView listCurrentBrush;
        private Button btnRemoveBrush;
        private Button btnAddBrush;
        private ComboBox cmbPrefab;
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
        private TabControl toolTabs;
        private TabPage tabBrush;
        private TabPage tabEntity;
        private PixelToolStripButton toolDropper;
        private Label lblPrefab;
        private DataGridView tableEntityAttributes;
        private DataGridViewTextBoxColumn columnKey;
        private DataGridViewTextBoxColumn columnValue;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem undoToolStripMenuItem;
        private ToolStripMenuItem redoToolStripMenuItem;
    }
}
