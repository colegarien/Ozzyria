
namespace Ozzyria.ConstructionKit
{
    partial class ConstructionKitForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConstructionKitForm));
            menuTopBar = new System.Windows.Forms.MenuStrip();
            menuItemTileSet = new System.Windows.Forms.ToolStripMenuItem();
            menuItemMap = new System.Windows.Forms.ToolStripMenuItem();
            spltTopLevel = new System.Windows.Forms.SplitContainer();
            spltRight = new System.Windows.Forms.SplitContainer();
            panelMapEditor = new System.Windows.Forms.Panel();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            toolPaint = new System.Windows.Forms.ToolStripButton();
            toolFill = new System.Windows.Forms.ToolStripButton();
            toolErase = new System.Windows.Forms.ToolStripButton();
            btnMapSave = new System.Windows.Forms.ToolStripButton();
            toolTileType = new System.Windows.Forms.ToolStripComboBox();
            btnAddLayer = new System.Windows.Forms.Button();
            dataLayers = new System.Windows.Forms.DataGridView();
            showLayer = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            activeLayer = new System.Windows.Forms.DataGridViewImageColumn();
            layerNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            deleteLayer = new System.Windows.Forms.DataGridViewButtonColumn();
            listMap = new System.Windows.Forms.ListBox();
            toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            menuTopBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)spltTopLevel).BeginInit();
            spltTopLevel.Panel2.SuspendLayout();
            spltTopLevel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)spltRight).BeginInit();
            spltRight.Panel1.SuspendLayout();
            spltRight.Panel2.SuspendLayout();
            spltRight.SuspendLayout();
            toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataLayers).BeginInit();
            SuspendLayout();
            // 
            // menuTopBar
            // 
            menuTopBar.ImageScalingSize = new System.Drawing.Size(20, 20);
            menuTopBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { menuItemTileSet, menuItemMap });
            menuTopBar.Location = new System.Drawing.Point(0, 0);
            menuTopBar.Name = "menuTopBar";
            menuTopBar.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            menuTopBar.Size = new System.Drawing.Size(1232, 24);
            menuTopBar.TabIndex = 0;
            menuTopBar.Text = "menuStrip";
            menuTopBar.ItemClicked += menuStrip_ItemClicked;
            // 
            // menuItemTileSet
            // 
            menuItemTileSet.Name = "menuItemTileSet";
            menuItemTileSet.Size = new System.Drawing.Size(65, 20);
            menuItemTileSet.Text = "Tile Set...";
            // 
            // menuItemMap
            // 
            menuItemMap.Name = "menuItemMap";
            menuItemMap.Size = new System.Drawing.Size(52, 20);
            menuItemMap.Text = "Map...";
            // 
            // spltTopLevel
            // 
            spltTopLevel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            spltTopLevel.Cursor = System.Windows.Forms.Cursors.VSplit;
            spltTopLevel.Dock = System.Windows.Forms.DockStyle.Fill;
            spltTopLevel.Location = new System.Drawing.Point(0, 24);
            spltTopLevel.Name = "spltTopLevel";
            // 
            // spltTopLevel.Panel1
            // 
            spltTopLevel.Panel1.Cursor = System.Windows.Forms.Cursors.Default;
            // 
            // spltTopLevel.Panel2
            // 
            spltTopLevel.Panel2.Controls.Add(spltRight);
            spltTopLevel.Panel2.Cursor = System.Windows.Forms.Cursors.Default;
            spltTopLevel.Size = new System.Drawing.Size(1232, 793);
            spltTopLevel.SplitterDistance = 409;
            spltTopLevel.TabIndex = 1;
            // 
            // spltRight
            // 
            spltRight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            spltRight.Cursor = System.Windows.Forms.Cursors.HSplit;
            spltRight.Dock = System.Windows.Forms.DockStyle.Fill;
            spltRight.Location = new System.Drawing.Point(0, 0);
            spltRight.Name = "spltRight";
            spltRight.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spltRight.Panel1
            // 
            spltRight.Panel1.Controls.Add(panelMapEditor);
            spltRight.Panel1.Cursor = System.Windows.Forms.Cursors.Default;
            // 
            // spltRight.Panel2
            // 
            spltRight.Panel2.Controls.Add(toolStrip1);
            spltRight.Panel2.Controls.Add(btnAddLayer);
            spltRight.Panel2.Controls.Add(dataLayers);
            spltRight.Panel2.Controls.Add(listMap);
            spltRight.Panel2.Cursor = System.Windows.Forms.Cursors.Default;
            spltRight.Size = new System.Drawing.Size(819, 793);
            spltRight.SplitterDistance = 529;
            spltRight.TabIndex = 0;
            // 
            // panelMapEditor
            // 
            panelMapEditor.Location = new System.Drawing.Point(3, 3);
            panelMapEditor.Name = "panelMapEditor";
            panelMapEditor.Size = new System.Drawing.Size(811, 521);
            panelMapEditor.TabIndex = 0;
            panelMapEditor.Paint += panelMapEditor_Paint;
            panelMapEditor.MouseDown += panelMapEditor_MouseDown;
            panelMapEditor.MouseMove += panelMapEditor_MouseMove;
            panelMapEditor.MouseUp += panelMapEditor_MouseUp;
            panelMapEditor.MouseWheel += panelMapEditor_MouseWheel;
            // 
            // toolStrip1
            // 
            toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { btnMapSave, toolStripSeparator2, toolPaint, toolFill, toolErase, toolStripSeparator1, toolStripLabel1, toolTileType });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            toolStrip1.Size = new System.Drawing.Size(817, 25);
            toolStrip1.TabIndex = 3;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolPaint
            // 
            toolPaint.CheckOnClick = true;
            toolPaint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolPaint.Image = (System.Drawing.Image)resources.GetObject("toolPaint.Image");
            toolPaint.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolPaint.Name = "toolPaint";
            toolPaint.Size = new System.Drawing.Size(23, 22);
            toolPaint.Text = "&Paint";
            toolPaint.CheckedChanged += toolPaint_CheckedChanged;
            // 
            // toolFill
            // 
            toolFill.CheckOnClick = true;
            toolFill.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolFill.Image = (System.Drawing.Image)resources.GetObject("toolFill.Image");
            toolFill.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolFill.Name = "toolFill";
            toolFill.Size = new System.Drawing.Size(23, 22);
            toolFill.Text = "Fill";
            toolFill.CheckedChanged += toolFill_CheckedChanged;
            // 
            // toolErase
            // 
            toolErase.CheckOnClick = true;
            toolErase.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolErase.Image = (System.Drawing.Image)resources.GetObject("toolErase.Image");
            toolErase.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolErase.Name = "toolErase";
            toolErase.Size = new System.Drawing.Size(23, 22);
            toolErase.Text = "&Erase";
            toolErase.CheckedChanged += toolErase_CheckedChanged;
            // 
            // btnMapSave
            // 
            btnMapSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnMapSave.Image = (System.Drawing.Image)resources.GetObject("btnMapSave.Image");
            btnMapSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnMapSave.Name = "btnMapSave";
            btnMapSave.Size = new System.Drawing.Size(23, 22);
            btnMapSave.Text = "&Save";
            btnMapSave.Click += btnMapSave_Click;
            // 
            // toolTileType
            // 
            toolTileType.MaxDropDownItems = 100;
            toolTileType.Name = "toolTileType";
            toolTileType.Size = new System.Drawing.Size(121, 25);
            // 
            // btnAddLayer
            // 
            btnAddLayer.Location = new System.Drawing.Point(233, 209);
            btnAddLayer.Name = "btnAddLayer";
            btnAddLayer.Size = new System.Drawing.Size(75, 23);
            btnAddLayer.TabIndex = 2;
            btnAddLayer.Text = "Add Layer";
            btnAddLayer.UseVisualStyleBackColor = true;
            btnAddLayer.Click += btnAddLayer_Click;
            // 
            // dataLayers
            // 
            dataLayers.AllowDrop = true;
            dataLayers.AllowUserToAddRows = false;
            dataLayers.AllowUserToDeleteRows = false;
            dataLayers.AllowUserToResizeColumns = false;
            dataLayers.AllowUserToResizeRows = false;
            dataLayers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataLayers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { showLayer, activeLayer, layerNumber, deleteLayer });
            dataLayers.Location = new System.Drawing.Point(233, 33);
            dataLayers.Name = "dataLayers";
            dataLayers.RowHeadersVisible = false;
            dataLayers.RowTemplate.Height = 25;
            dataLayers.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            dataLayers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dataLayers.ShowCellToolTips = false;
            dataLayers.ShowEditingIcon = false;
            dataLayers.Size = new System.Drawing.Size(256, 173);
            dataLayers.TabIndex = 1;
            dataLayers.CellContentClick += dataLayers_CellContentClick;
            dataLayers.CellFormatting += dataLayers_CellFormatting;
            dataLayers.CellValueChanged += dataLayers_CellValueChanged;
            dataLayers.DragDrop += dataLayers_DragDrop;
            dataLayers.DragOver += dataLayers_DragOver;
            dataLayers.MouseDown += dataLayers_MouseDown;
            dataLayers.MouseMove += dataLayers_MouseMove;
            // 
            // showLayer
            // 
            showLayer.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            showLayer.HeaderText = "Show";
            showLayer.Name = "showLayer";
            showLayer.Width = 42;
            // 
            // activeLayer
            // 
            activeLayer.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            activeLayer.HeaderText = "Active";
            activeLayer.Name = "activeLayer";
            activeLayer.ReadOnly = true;
            activeLayer.Width = 46;
            // 
            // layerNumber
            // 
            layerNumber.HeaderText = "Layer";
            layerNumber.MaxInputLength = 255;
            layerNumber.Name = "layerNumber";
            layerNumber.ReadOnly = true;
            layerNumber.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            layerNumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // deleteLayer
            // 
            deleteLayer.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            deleteLayer.HeaderText = "Delete";
            deleteLayer.Name = "deleteLayer";
            deleteLayer.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            deleteLayer.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            deleteLayer.Width = 65;
            // 
            // listMap
            // 
            listMap.FormattingEnabled = true;
            listMap.ItemHeight = 15;
            listMap.Location = new System.Drawing.Point(3, 33);
            listMap.Name = "listMap";
            listMap.Size = new System.Drawing.Size(224, 199);
            listMap.TabIndex = 0;
            listMap.SelectedIndexChanged += listMap_SelectedIndexChanged;
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new System.Drawing.Size(79, 22);
            toolStripLabel1.Text = "Tile Selection:";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // ConstructionKitForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1232, 817);
            Controls.Add(spltTopLevel);
            Controls.Add(menuTopBar);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuTopBar;
            Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            Name = "ConstructionKitForm";
            Text = "Ozzyria Construction Kit";
            menuTopBar.ResumeLayout(false);
            menuTopBar.PerformLayout();
            spltTopLevel.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)spltTopLevel).EndInit();
            spltTopLevel.ResumeLayout(false);
            spltRight.Panel1.ResumeLayout(false);
            spltRight.Panel2.ResumeLayout(false);
            spltRight.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)spltRight).EndInit();
            spltRight.ResumeLayout(false);
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataLayers).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuTopBar;
        private System.Windows.Forms.ToolStripMenuItem menuItemTileSet;
        private System.Windows.Forms.SplitContainer spltTopLevel;
        private System.Windows.Forms.SplitContainer spltRight;
        private System.Windows.Forms.ToolStripMenuItem menuItemMap;
        private System.Windows.Forms.ListBox listMap;
        private System.Windows.Forms.Panel panelMapEditor;
        private System.Windows.Forms.DataGridView dataLayers;
        private System.Windows.Forms.DataGridViewCheckBoxColumn showLayer;
        private System.Windows.Forms.DataGridViewImageColumn activeLayer;
        private System.Windows.Forms.DataGridViewTextBoxColumn layerNumber;
        private System.Windows.Forms.DataGridViewButtonColumn deleteLayer;
        private System.Windows.Forms.Button btnAddLayer;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolPaint;
        private System.Windows.Forms.ToolStripButton toolErase;
        private System.Windows.Forms.ToolStripButton btnMapSave;
        private System.Windows.Forms.ToolStripButton toolFill;
        private System.Windows.Forms.ToolStripComboBox toolTileType;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}

