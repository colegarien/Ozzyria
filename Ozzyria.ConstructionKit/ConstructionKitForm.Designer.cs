﻿
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
            this.listMap = new System.Windows.Forms.ListBox();
            this.menuTopBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spltTopLevel)).BeginInit();
            this.spltTopLevel.Panel2.SuspendLayout();
            this.spltTopLevel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spltRight)).BeginInit();
            this.spltRight.Panel1.SuspendLayout();
            this.spltRight.Panel2.SuspendLayout();
            this.spltRight.SuspendLayout();
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
    }
}

