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

                picTileSet.Refresh();
            }
        }

        // TODO make a custom thing that extends picturebox to hide all this
        private void picTileSet_DoubleClick(object sender, EventArgs e)
        {
            MouseEventArgs me = e as MouseEventArgs;

            if (_tileSetImage != null && TileSetMetaDataFactory.tileSetMetaDatas.ContainsKey((string)comboBoxTileSet.SelectedItem ?? ""))
            {
                var metaData = TileSetMetaDataFactory.tileSetMetaDatas[(string)comboBoxTileSet.SelectedItem];
                var tileTypeId = (listTileTypes.SelectedItem as ComboBoxItem)?.Id ?? -1;
                if (tileTypeId == -1)
                    return;

                var imageWidth = _tileSetImage.Width;
                var imageHeight = _tileSetImage.Height;
                var tileWidth = Game.Tile.DIMENSION * ((float)picTileSet.ClientSize.Width / (float)imageWidth);
                var tileHeight = Game.Tile.DIMENSION * ((float)picTileSet.ClientSize.Height / (float)imageHeight);

                var textureCoordX = (int)Math.Floor(me.X / tileWidth);
                var textureCoordY = (int)Math.Floor(me.Y / tileHeight);

                metaData.BaseTileX[tileTypeId] = textureCoordX;
                metaData.BaseTileY[tileTypeId] = textureCoordY;

                picTileSet.Refresh();
            }
        }

        private void chkShowTransitions_CheckedChanged(object sender, EventArgs e)
        {
            picTileSet.Refresh();
        }

        private void chkShowPaths_CheckedChanged(object sender, EventArgs e)
        {
            picTileSet.Refresh();
        }

        private void chkShowWalls_CheckedChanged(object sender, EventArgs e)
        {
            picTileSet.Refresh();
        }

        private void picTileSet_Paint(object sender, PaintEventArgs e)
        {
            if (_tileSetImage != null && TileSetMetaDataFactory.tileSetMetaDatas.ContainsKey((string)comboBoxTileSet.SelectedItem ?? ""))
            {
                var metaData = TileSetMetaDataFactory.tileSetMetaDatas[(string)comboBoxTileSet.SelectedItem];
                var tileTypeId = (listTileTypes.SelectedItem as ComboBoxItem)?.Id ?? -1;
                if (tileTypeId == -1)
                    return;

                var highlightPen = new Pen(Color.Red, 2);
                var tranistionPen = new Pen(Color.Blue, 2);
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

                if (radTranistionableYes.Checked && chkShowTransitions.Checked && tileTypeId != metaData.TilesThatSupportTransitions.FirstOrDefault())
                {
                    DrawTransitionLayout(tranistionPen, e.Graphics, x, y, tileWidth, tileHeight, 4);
                }

                if (radPathableYes.Checked && chkShowPaths.Checked)
                {
                    DrawPathLayout(pathingPen, e.Graphics, x, y, tileWidth, tileHeight);
                }

                if (radWallYes.Checked && chkShowWalls.Checked)
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
        private void DrawTransitionLayout(Pen pen, Graphics g, float baseX, float baseY, float tileWidth, float tileHeight, float size)
        {
            var centerX = baseX + (tileWidth * 0.5f);
            var centerY = baseY + (tileHeight * 0.5f);

            // left - 1,0
            DrawLeft(pen, g, baseX + (1 * tileWidth), baseY + (0 * tileHeight), tileWidth, tileHeight);

            // up - 2,0
            DrawUp(pen, g, baseX + (2 * tileWidth), baseY + (0 * tileHeight), tileWidth, tileHeight);

            // leftup - 3,0
            DrawLeft(pen, g, baseX + (3 * tileWidth), baseY + (0 * tileHeight), tileWidth, tileHeight);
            DrawUp(pen, g, baseX + (3 * tileWidth), baseY + (0 * tileHeight), tileWidth, tileHeight);

            // right - 4,0
            DrawRight(pen, g, baseX + (4 * tileWidth), baseY + (0 * tileHeight), tileWidth, tileHeight);

            // rightleft - 5,0
            DrawLeft(pen, g, baseX + (5 * tileWidth), baseY + (0 * tileHeight), tileWidth, tileHeight);
            DrawRight(pen, g, baseX + (5 * tileWidth), baseY + (0 * tileHeight), tileWidth, tileHeight);

            // rightup - 6,0
            DrawUp(pen, g, baseX + (6 * tileWidth), baseY + (0 * tileHeight), tileWidth, tileHeight);
            DrawRight(pen, g, baseX + (6 * tileWidth), baseY + (0 * tileHeight), tileWidth, tileHeight);

            // leftrightup - 7,0
            DrawLeft(pen, g, baseX + (7 * tileWidth), baseY + (0 * tileHeight), tileWidth, tileHeight);
            DrawUp(pen, g, baseX + (7 * tileWidth), baseY + (0 * tileHeight), tileWidth, tileHeight);
            DrawRight(pen, g, baseX + (7 * tileWidth), baseY + (0 * tileHeight), tileWidth, tileHeight);

            // down - 8,0
            DrawDown(pen, g, baseX + (8 * tileWidth), baseY + (0 * tileHeight), tileWidth, tileHeight);

            // downleft - 9,0
            DrawLeft(pen, g, baseX + (9 * tileWidth), baseY + (0 * tileHeight), tileWidth, tileHeight);
            DrawDown(pen, g, baseX + (9 * tileWidth), baseY + (0 * tileHeight), tileWidth, tileHeight);

            // downup - 10,0
            DrawUp(pen, g, baseX + (10 * tileWidth), baseY + (0 * tileHeight), tileWidth, tileHeight);
            DrawDown(pen, g, baseX + (10 * tileWidth), baseY + (0 * tileHeight), tileWidth, tileHeight);

            // downleftup - 11,0
            DrawLeft(pen, g, baseX + (11 * tileWidth), baseY + (0 * tileHeight), tileWidth, tileHeight);
            DrawUp(pen, g, baseX + (11 * tileWidth), baseY + (0 * tileHeight), tileWidth, tileHeight);
            DrawDown(pen, g, baseX + (11 * tileWidth), baseY + (0 * tileHeight), tileWidth, tileHeight);

            // downright - 12,0
            DrawRight(pen, g, baseX + (12 * tileWidth), baseY + (0 * tileHeight), tileWidth, tileHeight);
            DrawDown(pen, g, baseX + (12 * tileWidth), baseY + (0 * tileHeight), tileWidth, tileHeight);

            // downrightleft - 13,0
            DrawLeft(pen, g, baseX + (13 * tileWidth), baseY + (0 * tileHeight), tileWidth, tileHeight);
            DrawRight(pen, g, baseX + (13 * tileWidth), baseY + (0 * tileHeight), tileWidth, tileHeight);
            DrawDown(pen, g, baseX + (13 * tileWidth), baseY + (0 * tileHeight), tileWidth, tileHeight);

            // downrightup - 14,0
            DrawUp(pen, g, baseX + (14 * tileWidth), baseY + (0 * tileHeight), tileWidth, tileHeight);
            DrawRight(pen, g, baseX + (14 * tileWidth), baseY + (0 * tileHeight), tileWidth, tileHeight);
            DrawDown(pen, g, baseX + (14 * tileWidth), baseY + (0 * tileHeight), tileWidth, tileHeight);

            // downleftrightup - 15,0
            DrawLeft(pen, g, baseX + (15 * tileWidth), baseY + (0 * tileHeight), tileWidth, tileHeight);
            DrawUp(pen, g, baseX + (15 * tileWidth), baseY + (0 * tileHeight), tileWidth, tileHeight);
            DrawRight(pen, g, baseX + (15 * tileWidth), baseY + (0 * tileHeight), tileWidth, tileHeight);
            DrawDown(pen, g, baseX + (15 * tileWidth), baseY + (0 * tileHeight), tileWidth, tileHeight);


            var originalWidth = pen.Width;
            pen.Width = 3;
            // upleft - 1,1
            g.DrawLine(pen, baseX + (1 * tileWidth), baseY + (1 * tileHeight), centerX + (1 * tileWidth), centerY + (1 * tileHeight));

            // upright - 2,1
            g.DrawLine(pen, baseX + tileWidth - 1 + (2 * tileWidth), baseY + (1 * tileHeight), centerX + (2 * tileWidth), centerY + (1 * tileHeight));

            // upleft,upright - 3,1
            g.DrawLine(pen, baseX + (3 * tileWidth), baseY + (1 * tileHeight), centerX + (3 * tileWidth), centerY + (1 * tileHeight));
            g.DrawLine(pen, baseX + tileWidth - 1 + (3 * tileWidth), baseY + (1 * tileHeight), centerX + (3 * tileWidth), centerY + (1 * tileHeight));

            // downright - 4,1
            g.DrawLine(pen, baseX + tileWidth - 1 + (4 * tileWidth), baseY + tileHeight - 1 + (1 * tileHeight), centerX + (4 * tileWidth), centerY + (1 * tileHeight));

            // downright,upleft - 5,1
            g.DrawLine(pen, baseX + (5 * tileWidth), baseY + (1 * tileHeight), centerX + (5 * tileWidth), centerY + (1 * tileHeight));
            g.DrawLine(pen, baseX + tileWidth - 1 + (5 * tileWidth), baseY + tileHeight - 1 + (1 * tileHeight), centerX + (5 * tileWidth), centerY + (1 * tileHeight));

            // downright,upright - 6,1
            g.DrawLine(pen, baseX + tileWidth - 1 + (6 * tileWidth), baseY + (1 * tileHeight), centerX + (6 * tileWidth), centerY + (1 * tileHeight));
            g.DrawLine(pen, baseX + tileWidth - 1 + (6 * tileWidth), baseY + tileHeight - 1 + (1 * tileHeight), centerX + (6 * tileWidth), centerY + (1 * tileHeight));

            // downright,upleft,upright - 7,1
            g.DrawLine(pen, baseX + (7 * tileWidth), baseY + (1 * tileHeight), centerX + (7 * tileWidth), centerY + (1 * tileHeight));
            g.DrawLine(pen, baseX + tileWidth - 1 + (7 * tileWidth), baseY + (1 * tileHeight), centerX + (7 * tileWidth), centerY + (1 * tileHeight));
            g.DrawLine(pen, baseX + tileWidth - 1 + (7 * tileWidth), baseY + tileHeight - 1 + (1 * tileHeight), centerX + (7 * tileWidth), centerY + (1 * tileHeight));

            // downleft - 8,1
            g.DrawLine(pen, baseX + (8 * tileWidth), baseY + tileHeight - 1 + (1 * tileHeight), centerX + (8 * tileWidth), centerY + (1 * tileHeight));

            // downleft,upleft - 9,1
            g.DrawLine(pen, baseX + (9 * tileWidth), baseY + (1 * tileHeight), centerX + (9 * tileWidth), centerY + (1 * tileHeight));
            g.DrawLine(pen, baseX + (9 * tileWidth), baseY + tileHeight - 1 + (1 * tileHeight), centerX + (9 * tileWidth), centerY + (1 * tileHeight));

            // downleft,upright - 10,1
            g.DrawLine(pen, baseX + tileWidth - 1 + (10 * tileWidth), baseY + (1 * tileHeight), centerX + (10 * tileWidth), centerY + (1 * tileHeight));
            g.DrawLine(pen, baseX + (10 * tileWidth), baseY + tileHeight - 1 + (1 * tileHeight), centerX + (10 * tileWidth), centerY + (1 * tileHeight));

            // downleft,upleft,upright - 11,1
            g.DrawLine(pen, baseX + (11 * tileWidth), baseY + (1 * tileHeight), centerX + (11 * tileWidth), centerY + (1 * tileHeight));
            g.DrawLine(pen, baseX + tileWidth - 1 + (11 * tileWidth), baseY + (1 * tileHeight), centerX + (11 * tileWidth), centerY + (1 * tileHeight));
            g.DrawLine(pen, baseX + (11 * tileWidth), baseY + tileHeight - 1 + (1 * tileHeight), centerX + (11 * tileWidth), centerY + (1 * tileHeight));

            // downleft,downright - 12,1
            g.DrawLine(pen, baseX + tileWidth - 1 + (12 * tileWidth), baseY + tileHeight - 1 + (1 * tileHeight), centerX + (12 * tileWidth), centerY + (1 * tileHeight));
            g.DrawLine(pen, baseX + (12 * tileWidth), baseY + tileHeight - 1 + (1 * tileHeight), centerX + (12 * tileWidth), centerY + (1 * tileHeight));

            // downleft,downright,upleft - 13,1
            g.DrawLine(pen, baseX + (13 * tileWidth), baseY + (1 * tileHeight), centerX + (13 * tileWidth), centerY + (1 * tileHeight));
            g.DrawLine(pen, baseX + tileWidth - 1 + (13 * tileWidth), baseY + tileHeight - 1 + (1 * tileHeight), centerX + (13 * tileWidth), centerY + (1 * tileHeight));
            g.DrawLine(pen, baseX + (13 * tileWidth), baseY + tileHeight - 1 + (1 * tileHeight), centerX + (13 * tileWidth), centerY + (1 * tileHeight));

            // downleft,downright,upright - 14,1
            g.DrawLine(pen, baseX + tileWidth - 1 + (14 * tileWidth), baseY + (1 * tileHeight), centerX + (14 * tileWidth), centerY + (1 * tileHeight));
            g.DrawLine(pen, baseX + tileWidth - 1 + (14 * tileWidth), baseY + tileHeight - 1 + (1 * tileHeight), centerX + (14 * tileWidth), centerY + (1 * tileHeight));
            g.DrawLine(pen, baseX + (14 * tileWidth), baseY + tileHeight - 1 + (1 * tileHeight), centerX + (14 * tileWidth), centerY + (1 * tileHeight));

            // downleft,downright,upleft,upright - 15,1
            g.DrawLine(pen, baseX + (15 * tileWidth), baseY + (1 * tileHeight), centerX + (15 * tileWidth), centerY + (1 * tileHeight));
            g.DrawLine(pen, baseX + tileWidth - 1 + (15 * tileWidth), baseY + (1 * tileHeight), centerX + (15 * tileWidth), centerY + (1 * tileHeight));
            g.DrawLine(pen, baseX + tileWidth - 1 + (15 * tileWidth), baseY + tileHeight - 1 + (1 * tileHeight), centerX + (15 * tileWidth), centerY + (1 * tileHeight));
            g.DrawLine(pen, baseX + (15 * tileWidth), baseY + tileHeight - 1 + (1 * tileHeight), centerX + (15 * tileWidth), centerY + (1 * tileHeight));


            pen.Width = originalWidth;
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
