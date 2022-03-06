
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
            this.menuTopBar = new System.Windows.Forms.MenuStrip();
            this.menuItemTileSet = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemMap = new System.Windows.Forms.ToolStripMenuItem();
            this.spltTopLevel = new System.Windows.Forms.SplitContainer();
            this.spltRight = new System.Windows.Forms.SplitContainer();
            this.panelMapEditor = new System.Windows.Forms.Panel();
            this.dataLayers = new System.Windows.Forms.DataGridView();
            this.listMap = new System.Windows.Forms.ListBox();
            this.showLayer = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.activeLayer = new System.Windows.Forms.DataGridViewImageColumn();
            this.layerNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuTopBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spltTopLevel)).BeginInit();
            this.spltTopLevel.Panel2.SuspendLayout();
            this.spltTopLevel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spltRight)).BeginInit();
            this.spltRight.Panel1.SuspendLayout();
            this.spltRight.Panel2.SuspendLayout();
            this.spltRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayers)).BeginInit();
            this.SuspendLayout();
            // 
            // menuTopBar
            // 
            this.menuTopBar.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuTopBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemTileSet,
            this.menuItemMap});
            this.menuTopBar.Location = new System.Drawing.Point(0, 0);
            this.menuTopBar.Name = "menuTopBar";
            this.menuTopBar.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuTopBar.Size = new System.Drawing.Size(1232, 24);
            this.menuTopBar.TabIndex = 0;
            this.menuTopBar.Text = "menuStrip";
            this.menuTopBar.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip_ItemClicked);
            // 
            // menuItemTileSet
            // 
            this.menuItemTileSet.Name = "menuItemTileSet";
            this.menuItemTileSet.Size = new System.Drawing.Size(65, 20);
            this.menuItemTileSet.Text = "Tile Set...";
            // 
            // menuItemMap
            // 
            this.menuItemMap.Name = "menuItemMap";
            this.menuItemMap.Size = new System.Drawing.Size(52, 20);
            this.menuItemMap.Text = "Map...";
            // 
            // spltTopLevel
            // 
            this.spltTopLevel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.spltTopLevel.Cursor = System.Windows.Forms.Cursors.VSplit;
            this.spltTopLevel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spltTopLevel.Location = new System.Drawing.Point(0, 24);
            this.spltTopLevel.Name = "spltTopLevel";
            // 
            // spltTopLevel.Panel1
            // 
            this.spltTopLevel.Panel1.Cursor = System.Windows.Forms.Cursors.Default;
            // 
            // spltTopLevel.Panel2
            // 
            this.spltTopLevel.Panel2.Controls.Add(this.spltRight);
            this.spltTopLevel.Panel2.Cursor = System.Windows.Forms.Cursors.Default;
            this.spltTopLevel.Size = new System.Drawing.Size(1232, 793);
            this.spltTopLevel.SplitterDistance = 409;
            this.spltTopLevel.TabIndex = 1;
            // 
            // spltRight
            // 
            this.spltRight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.spltRight.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.spltRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spltRight.Location = new System.Drawing.Point(0, 0);
            this.spltRight.Name = "spltRight";
            this.spltRight.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spltRight.Panel1
            // 
            this.spltRight.Panel1.Controls.Add(this.panelMapEditor);
            this.spltRight.Panel1.Cursor = System.Windows.Forms.Cursors.Default;
            // 
            // spltRight.Panel2
            // 
            this.spltRight.Panel2.Controls.Add(this.dataLayers);
            this.spltRight.Panel2.Controls.Add(this.listMap);
            this.spltRight.Panel2.Cursor = System.Windows.Forms.Cursors.Default;
            this.spltRight.Size = new System.Drawing.Size(819, 793);
            this.spltRight.SplitterDistance = 529;
            this.spltRight.TabIndex = 0;
            // 
            // panelMapEditor
            // 
            this.panelMapEditor.Location = new System.Drawing.Point(3, 3);
            this.panelMapEditor.Name = "panelMapEditor";
            this.panelMapEditor.Size = new System.Drawing.Size(811, 521);
            this.panelMapEditor.TabIndex = 0;
            this.panelMapEditor.Paint += new System.Windows.Forms.PaintEventHandler(this.panelMapEditor_Paint);
            this.panelMapEditor.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelMapEditor_MouseDown);
            this.panelMapEditor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelMapEditor_MouseMove);
            this.panelMapEditor.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelMapEditor_MouseUp);
            this.panelMapEditor.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.panelMapEditor_MouseWheel);
            // 
            // dataLayers
            // 
            this.dataLayers.AllowDrop = true;
            this.dataLayers.AllowUserToAddRows = false;
            this.dataLayers.AllowUserToDeleteRows = false;
            this.dataLayers.AllowUserToResizeColumns = false;
            this.dataLayers.AllowUserToResizeRows = false;
            this.dataLayers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataLayers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.showLayer,
            this.activeLayer,
            this.layerNumber});
            this.dataLayers.Location = new System.Drawing.Point(233, 3);
            this.dataLayers.Name = "dataLayers";
            this.dataLayers.RowHeadersVisible = false;
            this.dataLayers.RowTemplate.Height = 25;
            this.dataLayers.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataLayers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataLayers.ShowCellToolTips = false;
            this.dataLayers.ShowEditingIcon = false;
            this.dataLayers.Size = new System.Drawing.Size(192, 229);
            this.dataLayers.TabIndex = 1;
            this.dataLayers.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataLayers_CellContentClick);
            this.dataLayers.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataLayers_CellFormatting);
            this.dataLayers.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataLayers_CellValueChanged);
            this.dataLayers.DragDrop += new System.Windows.Forms.DragEventHandler(this.dataLayers_DragDrop);
            this.dataLayers.DragOver += new System.Windows.Forms.DragEventHandler(this.dataLayers_DragOver);
            this.dataLayers.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataLayers_MouseDown);
            this.dataLayers.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dataLayers_MouseMove);
            // 
            // listMap
            // 
            this.listMap.FormattingEnabled = true;
            this.listMap.ItemHeight = 15;
            this.listMap.Location = new System.Drawing.Point(3, 3);
            this.listMap.Name = "listMap";
            this.listMap.Size = new System.Drawing.Size(224, 229);
            this.listMap.TabIndex = 0;
            this.listMap.SelectedIndexChanged += new System.EventHandler(this.listMap_SelectedIndexChanged);
            // 
            // showLayer
            // 
            this.showLayer.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.showLayer.HeaderText = "Show";
            this.showLayer.Name = "showLayer";
            this.showLayer.Width = 42;
            // 
            // activeLayer
            // 
            this.activeLayer.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.activeLayer.HeaderText = "Active";
            this.activeLayer.Name = "activeLayer";
            this.activeLayer.Width = 46;
            // 
            // layerNumber
            // 
            this.layerNumber.HeaderText = "Layer";
            this.layerNumber.MaxInputLength = 255;
            this.layerNumber.Name = "layerNumber";
            this.layerNumber.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.layerNumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ConstructionKitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1232, 817);
            this.Controls.Add(this.spltTopLevel);
            this.Controls.Add(this.menuTopBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuTopBar;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ConstructionKitForm";
            this.Text = "Ozzyria Construction Kit";
            this.menuTopBar.ResumeLayout(false);
            this.menuTopBar.PerformLayout();
            this.spltTopLevel.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spltTopLevel)).EndInit();
            this.spltTopLevel.ResumeLayout(false);
            this.spltRight.Panel1.ResumeLayout(false);
            this.spltRight.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spltRight)).EndInit();
            this.spltRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataLayers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}

