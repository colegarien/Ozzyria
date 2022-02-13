using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Ozzyria.ConstructionKit
{

    public partial class TileSetForm : Form
    {

        private Image _tileSetImage;

        public TileSetForm()
        {
            InitializeComponent();
            var metaData = TileSetMetaDataFactory.tileSetMetaDatas;

            var tileSetNames = metaData.Keys;
            comboBoxTileSet.Items.AddRange(tileSetNames.ToArray());

            // TODO be better about this, maybe specify somewhere (see Renderable.cs constants)
            dropDownZDepth.Items.AddRange(new ComboBoxItem[]
            {
                new ComboBoxItem{ Id = 0, Name = "Background"},
                new ComboBoxItem{ Id = 10, Name = "Items"},
                new ComboBoxItem{ Id = 25, Name = "Middleground"},
                new ComboBoxItem{ Id = 50, Name = "Foreground"},
                new ComboBoxItem{ Id = 255, Name = "In-Game UI"},
                new ComboBoxItem{ Id = 99999, Name = "Debug"},
            });

            // TODO OZ-17 add a title to the window to signify "Unsaved Changes"
            // TODO OZ-17 draw transition piece overlay (lowest precedence items shouldn't have transition pieces!!
            // TODO OZ-17 add visibility toggle for pathing, walling, and transitioning overlays
            // TODO OZ-17 add a "precedence preview" box that shows how the tiles will overlay
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            this.Close(); // TODO save stuff and don't close;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close(); // TODO save stuff and close
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            var prompt = new SimplePrompt("New Tile Set");
            var result = prompt.ShowDialog();

            if (result == DialogResult.OK)
            {
                var newTileSetId = prompt.GetPromptInput();

                var errorCaption = "";
                var errorMessage = "";

                if (newTileSetId.Equals(""))
                {
                    errorCaption = "Missing ID";
                    errorMessage = "New Tile Set ID should not be empty!";
                }
                else if (isTileSetIdInUse(newTileSetId))
                {
                    errorCaption = "ID In Use";
                    errorMessage = "Supplied ID \"" + newTileSetId + "\" is already in use!";
                }

                if (!errorMessage.Equals(""))
                {
                    MessageBox.Show(errorMessage, errorCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    TileSetMetaDataFactory.AddNewTileSet(newTileSetId);
                    // TODO save meta to file
                    comboBoxTileSet.Items.Add(newTileSetId);
                    comboBoxTileSet.SelectedItem = newTileSetId;
                }
            }
        }

        private bool isTileSetIdInUse(string id)
        {
            return comboBoxTileSet.Items.IndexOf(id) != -1;
        }

        private void buttonNewTileType_Click(object sender, EventArgs e)
        {
            var tileSetId = (string)comboBoxTileSet.SelectedItem ?? "";
            if (!TileSetMetaDataFactory.tileSetMetaDatas.ContainsKey(tileSetId))
            {
                MessageBox.Show("Must First Select a Tile Set!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var prompt = new SimplePrompt("New Tile Type");
            var result = prompt.ShowDialog();

            if (result == DialogResult.OK)
            {
                var newTileTypeName = prompt.GetPromptInput();

                var errorCaption = "";
                var errorMessage = "";

                if (newTileTypeName.Equals(""))
                {
                    errorCaption = "Missing Name";
                    errorMessage = "New Tile Type Name should not be empty!";
                }
                else if (isTileTypeNameInUse(newTileTypeName))
                {
                    errorCaption = "Name In Use";
                    errorMessage = "Supplied Name \"" + newTileTypeName + "\" is already in use!";
                }

                if (!errorMessage.Equals(""))
                {
                    MessageBox.Show(errorMessage, errorCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    var existingTileTypeIds = TileSetMetaDataFactory.tileSetMetaDatas[tileSetId].TileTypes;
                    var newTileTypeId = existingTileTypeIds.Count > 0 ? (existingTileTypeIds.Max() + 1) : 0;
                    var newTileTypeItem = new ComboBoxItem
                    {
                        Id = newTileTypeId,
                        Name = newTileTypeName
                    };
                    listTileTypes.Items.Add(newTileTypeItem);
                    listTileTypes.SelectedItem = newTileTypeItem;
                    TileSetMetaDataFactory.AddNewTileType(tileSetId, newTileTypeItem.Id, newTileTypeItem.Name);
                }
            }
        }

        private bool isTileTypeNameInUse(string name)
        {
            var tileSetId = (string)comboBoxTileSet.SelectedItem ?? "";
            return TileSetMetaDataFactory.tileSetMetaDatas[tileSetId].TileNames.Values.Any(n => n == name);
        }

        private void comboBoxTileSet_SelectedIndexChanged(object sender, EventArgs e)
        {
            var tileSetName = (string)comboBoxTileSet.SelectedItem ?? "";
            if (TileSetMetaDataFactory.tileSetMetaDatas.ContainsKey(tileSetName))
            {
                var metaData = TileSetMetaDataFactory.tileSetMetaDatas[tileSetName];

                // TODO avoid double clearing and loading if tileset is already been selected
                listTileTypes.Items.Clear();
                foreach(var tileTypeIdNamePair in metaData.TileNames)
                {
                    listTileTypes.Items.Add(new ComboBoxItem
                    {
                        Id = tileTypeIdNamePair.Key,
                        Name = tileTypeIdNamePair.Value,
                    });
                }

                listTransitionPrecedence.Items.Clear();
                foreach(var tileTypeId in metaData.TilesThatSupportTransitions)
                {
                    if (metaData.TileNames.ContainsKey(tileTypeId)) {
                        listTransitionPrecedence.Items.Add(new ComboBoxItem
                        {
                            Id = tileTypeId,
                            Name = metaData.TileNames[tileTypeId]
                        });
                    }
                }


                _tileSetImage = Image.FromFile("TileSets/Sprites/" + tileSetName + ".png");
                if (picTileSet.Image != null) picTileSet.Image.Dispose();
                picTileSet.Image = _tileSetImage;
            }
        }

        private void listTileTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            // TODO move into re-usabler function! (RefreshTileTypeForm or something?)
            if (TileSetMetaDataFactory.tileSetMetaDatas.ContainsKey((string)comboBoxTileSet.SelectedItem ?? ""))
            {
                var metaData = TileSetMetaDataFactory.tileSetMetaDatas[(string)comboBoxTileSet.SelectedItem];
                var tileTypeId = (listTileTypes.SelectedItem as ComboBoxItem)?.Id ?? 0;

                dropDownZDepth.SelectedItem = dropDownZDepth.Items.Cast<ComboBoxItem>()
                    .FirstOrDefault(i =>
                        (metaData.BaseTileZ.ContainsKey(tileTypeId) && i.Id == metaData.BaseTileZ[tileTypeId])
                        || (!metaData.BaseTileZ.ContainsKey(tileTypeId) && i.Id == 0)
                    );


                if (metaData.TilesThatSupportTransitions.Any(i => i == tileTypeId))
                    radTranistionableYes.Checked = true;
                else
                    radTranistionableNo.Checked = true;
                if (metaData.TilesThatSupportPathing.Any(i => i == tileTypeId))
                    radPathableYes.Checked = true;
                else
                    radPathableNo.Checked = true;
                if (metaData.TilesThatSupportWalling.Any(i => i == tileTypeId))
                    radWallYes.Checked = true;
                else
                    radWallNo.Checked = true;

                numWallOffsetX.Value = metaData.WallingCenterXOffset.ContainsKey(tileTypeId) ? metaData.WallingCenterXOffset[tileTypeId] : 0;
                numWallOffsetY.Value = metaData.WallingCenterYOffset.ContainsKey(tileTypeId) ? metaData.WallingCenterYOffset[tileTypeId] : 0;
                numWallThickness.Value = metaData.WallingThickness.ContainsKey(tileTypeId) ? metaData.WallingThickness[tileTypeId] : 0;

                picTileSet.Refresh();
            }
        }

        private void radTranistionableYes_CheckedChanged(object sender, EventArgs e)
        {
            if (TileSetMetaDataFactory.tileSetMetaDatas.ContainsKey((string)comboBoxTileSet.SelectedItem ?? ""))
            {
                var metaData = TileSetMetaDataFactory.tileSetMetaDatas[(string)comboBoxTileSet.SelectedItem];
                var tileTypeId = (listTileTypes.SelectedItem as ComboBoxItem)?.Id ?? 0;

                if (radTranistionableYes.Checked && !metaData.TilesThatSupportTransitions.Contains(tileTypeId))
                {
                    metaData.TilesThatSupportTransitions.Add(tileTypeId);
                    listTransitionPrecedence.Items.Add(new ComboBoxItem
                    {
                        Id = tileTypeId,
                        Name = metaData.TileNames[tileTypeId]
                    });
                }
                else if (!radTranistionableYes.Checked && metaData.TilesThatSupportTransitions.Contains(tileTypeId))
                {
                    metaData.TilesThatSupportTransitions.Remove(tileTypeId);
                    listTransitionPrecedence.Items.Remove(listTransitionPrecedence.Items.OfType<ComboBoxItem>().FirstOrDefault(i => i.Id == tileTypeId));
                }
            }

            picTileSet.Refresh();
        }

        private void radPathableYes_CheckedChanged(object sender, EventArgs e)
        {
            if (TileSetMetaDataFactory.tileSetMetaDatas.ContainsKey((string)comboBoxTileSet.SelectedItem ?? ""))
            {
                var metaData = TileSetMetaDataFactory.tileSetMetaDatas[(string)comboBoxTileSet.SelectedItem];
                var tileTypeId = (listTileTypes.SelectedItem as ComboBoxItem)?.Id ?? 0;

                if (radPathableYes.Checked && !metaData.TilesThatSupportPathing.Contains(tileTypeId))
                {
                    metaData.TilesThatSupportPathing.Add(tileTypeId);
                }
                else if (!radPathableYes.Checked && metaData.TilesThatSupportPathing.Contains(tileTypeId))
                {
                    metaData.TilesThatSupportPathing.Remove(tileTypeId);
                    radWallNo.Checked = true;
                }
            }

            picTileSet.Refresh();
        }

        private void radWallYes_CheckedChanged(object sender, EventArgs e)
        {
            if (TileSetMetaDataFactory.tileSetMetaDatas.ContainsKey((string)comboBoxTileSet.SelectedItem ?? ""))
            {
                var metaData = TileSetMetaDataFactory.tileSetMetaDatas[(string)comboBoxTileSet.SelectedItem];
                var tileTypeId = (listTileTypes.SelectedItem as ComboBoxItem)?.Id ?? 0;

                if (!radPathableYes.Checked && radWallYes.Checked)
                {
                    radWallNo.Checked = true; // walling tile needs to be pathable
                }

                if (radWallYes.Checked && !metaData.TilesThatSupportWalling.Contains(tileTypeId))
                {
                    metaData.TilesThatSupportWalling.Add(tileTypeId);
                }
                else if (!radWallYes.Checked && metaData.TilesThatSupportWalling.Contains(tileTypeId))
                {
                    metaData.TilesThatSupportWalling.Remove(tileTypeId);
                }

                if (radWallNo.Checked)
                {
                    numWallOffsetX.Value = 0;
                    numWallOffsetY.Value = 0;
                    numWallThickness.Value = 0;
                }
            }

            picTileSet.Refresh();
        }

        private void numWallOffsetX_ValueChanged(object sender, EventArgs e)
        {
            if (radWallNo.Checked)
            {
                numWallOffsetX.Value = 0;
            }

            if (TileSetMetaDataFactory.tileSetMetaDatas.ContainsKey((string)comboBoxTileSet.SelectedItem ?? ""))
            {
                var metaData = TileSetMetaDataFactory.tileSetMetaDatas[(string)comboBoxTileSet.SelectedItem];
                var tileTypeId = (listTileTypes.SelectedItem as ComboBoxItem)?.Id ?? 0;

                if (numWallOffsetX.Value == 0 && metaData.WallingCenterXOffset.ContainsKey(tileTypeId))
                {
                    metaData.WallingCenterXOffset.Remove(tileTypeId);
                }
                else if (numWallOffsetX.Value != 0)
                {
                    metaData.WallingCenterXOffset[tileTypeId] = (int)numWallOffsetX.Value;
                }
            }

            picTileSet.Refresh();
        }

        private void numWallOffsetY_ValueChanged(object sender, EventArgs e)
        {
            if (radWallNo.Checked)
            {
                numWallOffsetY.Value = 0;
            }

            if (TileSetMetaDataFactory.tileSetMetaDatas.ContainsKey((string)comboBoxTileSet.SelectedItem ?? ""))
            {
                var metaData = TileSetMetaDataFactory.tileSetMetaDatas[(string)comboBoxTileSet.SelectedItem];
                var tileTypeId = (listTileTypes.SelectedItem as ComboBoxItem)?.Id ?? 0;

                if (numWallOffsetY.Value == 0 && metaData.WallingCenterYOffset.ContainsKey(tileTypeId))
                {
                    metaData.WallingCenterYOffset.Remove(tileTypeId);
                }
                else if (numWallOffsetY.Value != 0)
                {
                    metaData.WallingCenterYOffset[tileTypeId] = (int)numWallOffsetY.Value;
                }
            }

            picTileSet.Refresh();
        }

        private void numWallThickness_ValueChanged(object sender, EventArgs e)
        {
            if (radWallNo.Checked)
            {
                numWallThickness.Value = 0;
            }

            if (TileSetMetaDataFactory.tileSetMetaDatas.ContainsKey((string)comboBoxTileSet.SelectedItem ?? ""))
            {
                var metaData = TileSetMetaDataFactory.tileSetMetaDatas[(string)comboBoxTileSet.SelectedItem];
                var tileTypeId = (listTileTypes.SelectedItem as ComboBoxItem)?.Id ?? 0;

                if (numWallThickness.Value == 0 && metaData.WallingThickness.ContainsKey(tileTypeId))
                {
                    metaData.WallingThickness.Remove(tileTypeId);
                }
                else if (numWallThickness.Value != 0)
                {
                    metaData.WallingThickness[tileTypeId] = (int)numWallThickness.Value;
                }
            }

            picTileSet.Refresh();
        }

        private void dropDownZDepth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TileSetMetaDataFactory.tileSetMetaDatas.ContainsKey((string)comboBoxTileSet.SelectedItem ?? ""))
            {
                var metaData = TileSetMetaDataFactory.tileSetMetaDatas[(string)comboBoxTileSet.SelectedItem];
                var tileTypeId = (listTileTypes.SelectedItem as ComboBoxItem)?.Id ?? 0;

                var item = dropDownZDepth.SelectedItem as ComboBoxItem;

                if (item.Id == 0 && metaData.BaseTileZ.ContainsKey(tileTypeId))
                {
                    metaData.BaseTileZ.Remove(tileTypeId);
                }
                else if(item.Id != 0)
                {
                    metaData.BaseTileZ[tileTypeId] = item.Id;
                }
            }
        }

        private void listTransitionPrecedence_MouseDown(object sender, MouseEventArgs e)
        {
            if (listTransitionPrecedence.SelectedItem == null) return;
            listTransitionPrecedence.DoDragDrop(listTransitionPrecedence.SelectedItem, DragDropEffects.Move);
        }

        private void listTransitionPrecedence_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void listTransitionPrecedence_DragDrop(object sender, DragEventArgs e)
        {
            Point point = listTransitionPrecedence.PointToClient(new Point(e.X, e.Y));
            int index = listTransitionPrecedence.IndexFromPoint(point);
            if (index < 0) index = listTransitionPrecedence.Items.Count - 1;
            object data = listTransitionPrecedence.SelectedItem;
            listTransitionPrecedence.Items.Remove(data);
            listTransitionPrecedence.Items.Insert(index, data);


            if (TileSetMetaDataFactory.tileSetMetaDatas.ContainsKey((string)comboBoxTileSet.SelectedItem ?? ""))
            {
                var metaData = TileSetMetaDataFactory.tileSetMetaDatas[(string)comboBoxTileSet.SelectedItem];
                metaData.TilesThatSupportTransitions = listTransitionPrecedence.Items.OfType<ComboBoxItem>().Select(i => i.Id).ToList();
            }
        }

        // TODO make a custom thing that extends picturebox to hide all this
        private void picTileSet_Paint(object sender, PaintEventArgs e)
        {
            if (_tileSetImage != null && TileSetMetaDataFactory.tileSetMetaDatas.ContainsKey((string)comboBoxTileSet.SelectedItem ?? ""))
            {
                var metaData = TileSetMetaDataFactory.tileSetMetaDatas[(string)comboBoxTileSet.SelectedItem];
                var tileTypeId = (listTileTypes.SelectedItem as ComboBoxItem)?.Id ?? -1;
                if (tileTypeId == -1)
                    return;

                var highlightPen = new Pen(Color.Red, 2);
                var pathingPen = new Pen(Color.Red, 2);
                var wallingPen = new Pen(Color.Magenta, 2);

                var imageWidth = _tileSetImage.Width;
                var imageHeight = _tileSetImage.Height;
                var tileWidth = Game.Tile.DIMENSION * ((float)picTileSet.ClientSize.Width / (float)imageWidth);
                var tileHeight = Game.Tile.DIMENSION * ((float)picTileSet.ClientSize.Height / (float)imageHeight);

                var textureCoordX = metaData.BaseTileX[tileTypeId];
                var textureCoordY = metaData.BaseTileY[tileTypeId];

                var x = textureCoordX * tileWidth;
                var y = textureCoordY * tileHeight;

                e.Graphics.DrawRectangle(highlightPen, x, y, tileWidth, tileHeight);

                // TODO allow selecting baseX and baseY by clicking on picture box
                if (radTranistionableYes.Checked)
                {
                    // TODO OZ-17 draw transitions
                }

                if (radPathableYes.Checked)
                {
                    DrawPathLayout(pathingPen, e.Graphics, x, y, tileWidth, tileHeight);
                }

                if (radWallYes.Checked)
                {
                    var xOffset = metaData.WallingCenterXOffset.ContainsKey(tileTypeId)
                        ? metaData.WallingCenterXOffset[tileTypeId] : 0;
                    var yOffset = metaData.WallingCenterYOffset.ContainsKey(tileTypeId)
                        ? metaData.WallingCenterYOffset[tileTypeId] : 0;
                    var thickness = metaData.WallingThickness.ContainsKey(tileTypeId)
                        ? metaData.WallingThickness[tileTypeId] : 0;

                    DrawPathLayout(wallingPen, e.Graphics, x, y, tileWidth, tileHeight, xOffset, yOffset, thickness);
                }
            }
        }

        private void DrawPathLayout(Pen pen, Graphics g, float baseX, float baseY, float tileWidth, float tileHeight, float cxOffset = 0, float cyOffset = 0, float thickness = 0)
        {
            // OZ-17 : move these text coordinates (and cooridnates for transtions) into the meta data!!!!!! (see metadatafactory)

            //case PathDirection.Left:
            DrawLeft(pen, g, baseX + (2 * tileWidth), baseY + (3 * tileHeight), tileWidth, tileHeight, cxOffset, cyOffset, thickness);

            //case PathDirection.Right:
            DrawRight(pen, g, baseX + (3 * tileWidth), baseY + (3 * tileHeight), tileWidth, tileHeight, cxOffset, cyOffset, thickness);

            //case PathDirection.Up:
            DrawUp(pen, g, baseX + (0 * tileWidth), baseY + (3 * tileHeight), tileWidth, tileHeight, cxOffset, cyOffset, thickness);

            //case PathDirection.Down:
            DrawDown(pen, g, baseX + (1 * tileWidth), baseY + (3 * tileHeight), tileWidth, tileHeight, cxOffset, cyOffset, thickness);

            //case PathDirection.LeftRight:
            DrawHorizontal(pen, g, baseX + (1 * tileWidth), baseY + (0 * tileHeight), tileWidth, tileHeight, cxOffset, cyOffset, thickness);

            //case PathDirection.UpT:
            DrawUp(pen, g, baseX + (3 * tileWidth), baseY + (0 * tileHeight), tileWidth, tileHeight, cxOffset, cyOffset, thickness);
            DrawHorizontal(pen, g, baseX + (3 * tileWidth), baseY + (0 * tileHeight), tileWidth, tileHeight, cxOffset, cyOffset, thickness);

            //case PathDirection.DownT:
            DrawDown(pen, g, baseX + (2 * tileWidth), baseY + (0 * tileHeight), tileWidth, tileHeight, cxOffset, cyOffset, thickness);
            DrawHorizontal(pen, g, baseX + (2 * tileWidth), baseY + (0 * tileHeight), tileWidth, tileHeight, cxOffset, cyOffset, thickness);

            //case PathDirection.UpDown:
            DrawVertical(pen, g, baseX + (2 * tileWidth), baseY + (2 * tileHeight), tileWidth, tileHeight, cxOffset, cyOffset, thickness);

            //case PathDirection.LeftT:
            DrawLeft(pen, g, baseX + (3 * tileWidth), baseY + (2 * tileHeight), tileWidth, tileHeight, cxOffset, cyOffset, thickness);
            DrawVertical(pen, g, baseX + (3 * tileWidth), baseY + (2 * tileHeight), tileWidth, tileHeight, cxOffset, cyOffset, thickness);

            //case PathDirection.RightT:
            DrawRight(pen, g, baseX + (3 * tileWidth), baseY + (1 * tileHeight), tileWidth, tileHeight, cxOffset, cyOffset, thickness);
            DrawVertical(pen, g, baseX + (3 * tileWidth), baseY + (1 * tileHeight), tileWidth, tileHeight, cxOffset, cyOffset, thickness);

            //case PathDirection.UpLeft:
            DrawUp(pen, g, baseX + (1 * tileWidth), baseY + (2 * tileHeight), tileWidth, tileHeight, cxOffset, cyOffset, thickness);
            DrawLeft(pen, g, baseX + (1 * tileWidth), baseY + (2 * tileHeight), tileWidth, tileHeight, cxOffset, cyOffset, thickness);

            //case PathDirection.UpRight:
            DrawUp(pen, g, baseX + (0 * tileWidth), baseY + (2 * tileHeight), tileWidth, tileHeight, cxOffset, cyOffset, thickness);
            DrawRight(pen, g, baseX + (0 * tileWidth), baseY + (2 * tileHeight), tileWidth, tileHeight, cxOffset, cyOffset, thickness);

            //case PathDirection.DownRight:
            DrawDown(pen, g, baseX + (0 * tileWidth), baseY + (1 * tileHeight), tileWidth, tileHeight, cxOffset, cyOffset, thickness);
            DrawRight(pen, g, baseX + (0 * tileWidth), baseY + (1 * tileHeight), tileWidth, tileHeight, cxOffset, cyOffset, thickness);

            //case PathDirection.DownLeft:
            DrawDown(pen, g, baseX + (1 * tileWidth), baseY + (1 * tileHeight), tileWidth, tileHeight, cxOffset, cyOffset, thickness);
            DrawLeft(pen, g, baseX + (1 * tileWidth), baseY + (1 * tileHeight), tileWidth, tileHeight, cxOffset, cyOffset, thickness);

            //case PathDirection.All:
            DrawVertical(pen, g, baseX + (0 * tileWidth), baseY + (0 * tileHeight), tileWidth, tileHeight, cxOffset, cyOffset, thickness);
            DrawHorizontal(pen, g, baseX + (0 * tileWidth), baseY + (0 * tileHeight), tileWidth, tileHeight, cxOffset, cyOffset, thickness);

            // center:
            DrawCenter(pen, g, baseX + (2 * tileWidth), baseY + (1 * tileHeight), tileWidth, tileHeight, cxOffset, cyOffset, thickness);

        }

        private void DrawHorizontal(Pen pen, Graphics g, float left, float top, float width, float height, float cxOffset=0, float cyOffset=0, float thickness=0)
        {
            var centerX = left + (width * 0.5f) + cxOffset;
            var centerY = top + (height * 0.5f) + cyOffset;
            g.DrawRectangle(pen, left, centerY - (thickness * 0.5f), width, Math.Max(thickness, 1));
        }

        private void DrawVertical(Pen pen, Graphics g, float left, float top, float width, float height, float cxOffset = 0, float cyOffset = 0, float thickness = 0)
        {
            var centerX = left + (width * 0.5f) + cxOffset;
            var centerY = top + (height * 0.5f) + cyOffset;
            g.DrawRectangle(pen, centerX - (thickness * 0.5f), top, Math.Max(thickness, 1), height);
        }

        private void DrawLeft(Pen pen, Graphics g, float left, float top, float width, float height, float cxOffset = 0, float cyOffset = 0, float thickness = 0)
        {
            var centerX = left + (width * 0.5f) + cxOffset;
            var centerY = top + (height * 0.5f) + cyOffset;
            g.DrawRectangle(pen, left, centerY - (thickness * 0.5f), centerX-left + (thickness*0.5f), Math.Max(thickness, 1));
        }

        private void DrawRight(Pen pen, Graphics g, float left, float top, float width, float height, float cxOffset = 0, float cyOffset = 0, float thickness = 0)
        {
            var centerX = left + (width * 0.5f) + cxOffset;
            var centerY = top + (height * 0.5f) + cyOffset;
            g.DrawRectangle(pen, centerX - (thickness * 0.5f), centerY - (thickness * 0.5f), width - (centerX - left) + (thickness * 0.5f), Math.Max(thickness, 1));
        }

        private void DrawUp(Pen pen, Graphics g, float left, float top, float width, float height, float cxOffset = 0, float cyOffset = 0, float thickness = 0)
        {
            var centerX = left + (width * 0.5f) + cxOffset;
            var centerY = top + (height * 0.5f) + cyOffset;
            g.DrawRectangle(pen, centerX - (thickness * 0.5f), top, Math.Max(thickness, 1), centerY- top + (thickness * 0.5f));
        }

        private void DrawDown(Pen pen, Graphics g, float left, float top, float width, float height, float cxOffset = 0, float cyOffset = 0, float thickness = 0)
        {
            var centerX = left + (width * 0.5f) + cxOffset;
            var centerY = top + (height * 0.5f) + cyOffset;
            g.DrawRectangle(pen, centerX - (thickness * 0.5f), centerY - (thickness * 0.5f), Math.Max(thickness, 1), height - (centerY - top) + (thickness * 0.5f));
        }

        private void DrawCenter(Pen pen, Graphics g, float left, float top, float width, float height, float cxOffset = 0, float cyOffset = 0, float thickness = 0)
        {
            var centerX = left + (width * 0.5f) + cxOffset;
            var centerY = top + (height * 0.5f) + cyOffset;
            g.DrawRectangle(pen, centerX - (thickness * 0.5f), centerY - (thickness * 0.5f), Math.Max(thickness, 1), Math.Max(thickness, 1));
        }
    }

    class ComboBoxItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
