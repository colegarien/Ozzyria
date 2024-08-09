using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Windows.Forms.AxHost;

namespace Ozzyria.Gryp.Models.Data
{

    internal class AutoTileRecord
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "";

        [JsonPropertyName("priority")]
        public int Priority { get; set; } = 0;

        [JsonPropertyName("path_pieces")]
        public Dictionary<string, string> PathPieces { get; set; }

        [JsonPropertyName("transition_pieces")]
        public Dictionary<string, string> TransitionPieces { get; set; }

        [JsonPropertyName("transition_from")]
        public List<string> TransitionsFrom { get; set; }
    }

    internal class AutoTileConfig
    {
        [JsonPropertyName("depth")]
        public int Depth { get; set; } = 1;

        [JsonPropertyName("configuration")]
        public Dictionary<string, AutoTileRecord> Configuration { get; set; }

        [JsonPropertyName("aliases")]
        public Dictionary<string, string> Aliases { get; set; }

        private static AutoTileConfig _instance = null;
        public static AutoTileConfig GetInstance()
        {
            if (_instance == null)
            {
                _instance = JsonSerializer.Deserialize<AutoTileConfig>(File.ReadAllText("auto_tile.json"), Content.Util.JsonOptionsFactory.GetOptions()) ?? new AutoTileConfig();
            }

            return _instance;
        }

        public void AutoTile(Tile? tile, Map map, int x, int y,  int depth = 1)
        {
            if(tile == null || tile.DrawableIds.Count < 1 || depth > Depth)
            {
                // not an auto-tilable tile or auto-tiling too deep
                return;
            }

            // resolve potential alias to find the "real" autoTileId
            var autoTileId = tile.DrawableIds[0];
            if (Aliases.ContainsKey(autoTileId))
            {
                autoTileId = Aliases[autoTileId];
            }

            if (!Configuration.ContainsKey(autoTileId))
            {
                // no configuration could be found, all done
                return;
            }

            var configRecord = Configuration[autoTileId];
            if(configRecord.Type == "path")
            {
                var northTile = map.GetTile(x, y - 1);
                var southTile = map.GetTile(x, y + 1);
                var eastTile = map.GetTile(x + 1, y);
                var westTile = map.GetTile(x - 1, y);

                var northIsConnectable = northTile != null && northTile.DrawableIds.Count > 0 && (northTile.DrawableIds[0] == autoTileId || (Aliases.ContainsKey(northTile.DrawableIds[0]) && Aliases[northTile.DrawableIds[0]] == autoTileId));
                var southIsConnectable = southTile != null && southTile.DrawableIds.Count > 0 && (southTile.DrawableIds[0] == autoTileId || Aliases.ContainsKey(southTile.DrawableIds[0]) && Aliases[southTile.DrawableIds[0]] == autoTileId);
                var eastIsConnectable = eastTile != null && eastTile.DrawableIds.Count > 0 && (eastTile.DrawableIds[0] == autoTileId || Aliases.ContainsKey(eastTile.DrawableIds[0]) && Aliases[eastTile.DrawableIds[0]] == autoTileId);
                var westIsConnectable = westTile != null && westTile.DrawableIds.Count > 0 && (westTile.DrawableIds[0] == autoTileId || Aliases.ContainsKey(westTile.DrawableIds[0]) && Aliases[westTile.DrawableIds[0]] == autoTileId);

                if(!northIsConnectable && !southIsConnectable && !eastIsConnectable && !westIsConnectable)
                {
                    tile.DrawableIds[0] = autoTileId;
                    return;
                }

                tile.DrawableIds.Clear();
                if (westIsConnectable && !eastIsConnectable && !northIsConnectable && !southIsConnectable)
                {
                    tile.DrawableIds.Add(configRecord.PathPieces["w"]);
                    AutoTile(westTile, map, x - 1, y, depth + 1);
                }
                else if (!westIsConnectable && eastIsConnectable && !northIsConnectable && !southIsConnectable)
                {
                    tile.DrawableIds.Add(configRecord.PathPieces["e"]);
                    AutoTile(eastTile, map, x + 1, y, depth + 1);
                }
                else if (!westIsConnectable && !eastIsConnectable && northIsConnectable && !southIsConnectable)
                {
                    tile.DrawableIds.Add(configRecord.PathPieces["n"]);
                    AutoTile(northTile, map, x, y - 1, depth + 1);
                }
                else if (!westIsConnectable && !eastIsConnectable && !northIsConnectable && southIsConnectable)
                {
                    tile.DrawableIds.Add(configRecord.PathPieces["s"]);
                    AutoTile(southTile, map, x, y + 1, depth + 1);
                }
                else if (westIsConnectable && eastIsConnectable && !northIsConnectable && !southIsConnectable)
                {
                    tile.DrawableIds.Add(configRecord.PathPieces["ew"]);
                    AutoTile(eastTile, map, x + 1, y, depth + 1);
                    AutoTile(westTile, map, x - 1, y, depth + 1);
                }
                else if (westIsConnectable && eastIsConnectable && northIsConnectable && !southIsConnectable)
                {
                    tile.DrawableIds.Add(configRecord.PathPieces["new"]);
                    AutoTile(northTile, map, x, y - 1, depth + 1);
                    AutoTile(eastTile, map, x + 1, y, depth + 1);
                    AutoTile(westTile, map, x - 1, y, depth + 1);
                }
                else if (westIsConnectable && eastIsConnectable && !northIsConnectable && southIsConnectable)
                {
                    tile.DrawableIds.Add(configRecord.PathPieces["sew"]);
                    AutoTile(southTile, map, x, y + 1, depth + 1);
                    AutoTile(eastTile, map, x + 1, y, depth + 1);
                    AutoTile(westTile, map, x - 1, y, depth + 1);
                }
                else if (!westIsConnectable && !eastIsConnectable && northIsConnectable && southIsConnectable)
                {
                    tile.DrawableIds.Add(configRecord.PathPieces["ns"]);
                    AutoTile(northTile, map, x, y - 1, depth + 1);
                    AutoTile(southTile, map, x, y + 1, depth + 1);
                }
                else if (westIsConnectable && !eastIsConnectable && northIsConnectable && southIsConnectable)
                {
                    tile.DrawableIds.Add(configRecord.PathPieces["nsw"]);
                    AutoTile(northTile, map, x, y - 1, depth + 1);
                    AutoTile(southTile, map, x, y + 1, depth + 1);
                    AutoTile(westTile, map, x - 1, y, depth + 1);
                }
                else if (!westIsConnectable && eastIsConnectable && northIsConnectable && southIsConnectable)
                {
                    tile.DrawableIds.Add(configRecord.PathPieces["nse"]);
                    AutoTile(northTile, map, x, y - 1, depth + 1);
                    AutoTile(southTile, map, x, y + 1, depth + 1);
                    AutoTile(eastTile, map, x + 1, y, depth + 1);
                }
                else if (westIsConnectable && !eastIsConnectable && northIsConnectable && !southIsConnectable)
                {
                    tile.DrawableIds.Add(configRecord.PathPieces["nw"]);
                    AutoTile(northTile, map, x, y - 1, depth + 1);
                    AutoTile(westTile, map, x - 1, y, depth + 1);
                }
                else if (!westIsConnectable && eastIsConnectable && northIsConnectable && !southIsConnectable)
                {
                    tile.DrawableIds.Add(configRecord.PathPieces["ne"]);
                    AutoTile(northTile, map, x, y - 1, depth + 1);
                    AutoTile(eastTile, map, x + 1, y, depth + 1);
                }
                else if (!westIsConnectable && eastIsConnectable && !northIsConnectable && southIsConnectable)
                {
                    tile.DrawableIds.Add(configRecord.PathPieces["se"]);
                    AutoTile(southTile, map, x, y + 1, depth + 1);
                    AutoTile(eastTile, map, x + 1, y, depth + 1);
                }
                else if (westIsConnectable && !eastIsConnectable && !northIsConnectable && southIsConnectable)
                {
                    tile.DrawableIds.Add(configRecord.PathPieces["sw"]);
                    AutoTile(southTile, map, x, y + 1, depth + 1);
                    AutoTile(westTile, map, x - 1, y, depth + 1);
                }
                else if (westIsConnectable && eastIsConnectable && northIsConnectable && southIsConnectable)
                {
                    tile.DrawableIds.Add(configRecord.PathPieces["nsew"]);
                    AutoTile(northTile, map, x, y - 1, depth + 1);
                    AutoTile(southTile, map, x, y + 1, depth + 1);
                    AutoTile(eastTile, map, x + 1, y, depth + 1);
                    AutoTile(westTile, map, x - 1, y, depth + 1);
                }
            } 
            else if(configRecord.Type == "transition")
            {
                var northTile = map.GetTile(x, y - 1);
                var southTile = map.GetTile(x, y + 1);
                var eastTile = map.GetTile(x + 1, y);
                var westTile = map.GetTile(x - 1, y);

                var northEastTile = map.GetTile(x + 1, y - 1);
                var northWestTile = map.GetTile(x - 1, y - 1);
                var southEastTile = map.GetTile(x + 1, y + 1);
                var southWestTile = map.GetTile(x - 1, y + 1);

                if (configRecord.TransitionsFrom.Count <= 0)
                {
                    // nothing to transition from, just adjust surrounding auto-tiles
                    AutoTile(northTile, map, x, y - 1, depth + 1);
                    AutoTile(southTile, map, x, y + 1, depth + 1);
                    AutoTile(eastTile, map, x + 1, y, depth + 1);
                    AutoTile(westTile, map, x - 1, y, depth + 1);
                    AutoTile(northEastTile, map, x + 1, y - 1, depth + 1);
                    AutoTile(northWestTile, map, x - 1, y - 1, depth + 1);
                    AutoTile(southEastTile, map, x + 1, y + 1, depth + 1);
                    AutoTile(southWestTile, map, x - 1, y + 1, depth + 1);
                    return;
                }

                // normalize drawables ids to "base" autotile
                tile.DrawableIds.Clear();
                tile.DrawableIds.Add(autoTileId);

                var northIsTransitionFrom = northTile != null && northTile.DrawableIds.Count > 0 && configRecord.TransitionsFrom.Contains(northTile.DrawableIds[0]) && Configuration.ContainsKey(northTile.DrawableIds[0]) && Configuration[northTile.DrawableIds[0]].Type == "transition";
                var southIsTransitionFrom = southTile != null && southTile.DrawableIds.Count > 0 && configRecord.TransitionsFrom.Contains(southTile.DrawableIds[0]) && Configuration.ContainsKey(southTile.DrawableIds[0]) && Configuration[southTile.DrawableIds[0]].Type == "transition";
                var eastIsTransitionFrom = eastTile != null && eastTile.DrawableIds.Count > 0 && configRecord.TransitionsFrom.Contains(eastTile.DrawableIds[0]) && Configuration.ContainsKey(eastTile.DrawableIds[0]) && Configuration[eastTile.DrawableIds[0]].Type == "transition";
                var westIsTransitionFrom = westTile != null && westTile.DrawableIds.Count > 0 && configRecord.TransitionsFrom.Contains(westTile.DrawableIds[0]) && Configuration.ContainsKey(westTile.DrawableIds[0]) && Configuration[westTile.DrawableIds[0]].Type == "transition";

                var northEastIsTransitionFrom = northEastTile != null && northEastTile.DrawableIds.Count > 0 && configRecord.TransitionsFrom.Contains(northEastTile.DrawableIds[0]) && Configuration.ContainsKey(northEastTile.DrawableIds[0]) && Configuration[northEastTile.DrawableIds[0]].Type == "transition";
                var northWestIsTransitionFrom = northWestTile != null && northWestTile.DrawableIds.Count > 0 && configRecord.TransitionsFrom.Contains(northWestTile.DrawableIds[0]) && Configuration.ContainsKey(northWestTile.DrawableIds[0]) && Configuration[northWestTile.DrawableIds[0]].Type == "transition";
                var southEastIsTransitionFrom = southEastTile != null && southEastTile.DrawableIds.Count > 0 && configRecord.TransitionsFrom.Contains(southEastTile.DrawableIds[0]) && Configuration.ContainsKey(southEastTile.DrawableIds[0]) && Configuration[southEastTile.DrawableIds[0]].Type == "transition";
                var southWestIsTransitionFrom = southWestTile != null && southWestTile.DrawableIds.Count > 0 && configRecord.TransitionsFrom.Contains(southWestTile.DrawableIds[0]) && Configuration.ContainsKey(southWestTile.DrawableIds[0]) && Configuration[southWestTile.DrawableIds[0]].Type == "transition";

                // Pack in variables
                var package = new TransitionPackage();
                package.MarkNorth(northTile?.DrawableIds?.FirstOrDefault() ?? "", northIsTransitionFrom);
                package.MarkSouth(southTile?.DrawableIds?.FirstOrDefault() ?? "", southIsTransitionFrom);
                package.MarkEast(eastTile?.DrawableIds?.FirstOrDefault() ?? "", eastIsTransitionFrom);
                package.MarkWest(westTile?.DrawableIds?.FirstOrDefault() ?? "", westIsTransitionFrom);

                package.MarkNorthEast(northEastTile?.DrawableIds?.FirstOrDefault() ?? "", northEastIsTransitionFrom);
                package.MarkNorthWest(northWestTile?.DrawableIds?.FirstOrDefault() ?? "", northWestIsTransitionFrom);
                package.MarkSouthEast(southEastTile?.DrawableIds?.FirstOrDefault() ?? "", southEastIsTransitionFrom);
                package.MarkSouthWest(southWestTile?.DrawableIds?.FirstOrDefault() ?? "", southWestIsTransitionFrom);

                foreach (var state in package.GetStates().Where(s => s.AutoTileId != "" && Configuration.ContainsKey(s.AutoTileId)).OrderBy(s => Configuration[s.AutoTileId].Priority))
                {
                    var stateConfiguration = Configuration[state.AutoTileId];

                    var borderKey = state.GetBorderKey();
                    if (borderKey != "" && stateConfiguration.TransitionPieces.ContainsKey(borderKey))
                    {
                        tile.DrawableIds.Add(stateConfiguration.TransitionPieces[borderKey]);
                    }

                    var cornerKey = state.GetCornerKey();
                    if (cornerKey != "" && stateConfiguration.TransitionPieces.ContainsKey(cornerKey))
                    {
                        tile.DrawableIds.Add(stateConfiguration.TransitionPieces[cornerKey]);
                    }
                }

                // for stupidity's sake, just run auto-tile on everything surrounding
                AutoTile(northTile, map, x, y - 1, depth + 1);
                AutoTile(southTile, map, x, y + 1, depth + 1);
                AutoTile(eastTile, map, x + 1, y, depth + 1);
                AutoTile(westTile, map, x - 1, y, depth + 1);
                AutoTile(northEastTile, map, x + 1, y - 1, depth + 1);
                AutoTile(northWestTile, map, x - 1, y - 1, depth + 1);
                AutoTile(southEastTile, map, x + 1, y + 1, depth + 1);
                AutoTile(southWestTile, map, x - 1, y + 1, depth + 1);
            }
        }
    }

    internal class TransitionPackage
    {
        private Dictionary<string, TransitionState> states = new Dictionary<string, TransitionState>();

        public IEnumerable<TransitionState> GetStates()
        {
            return states.Values;
        }

        private void CheckAutoTileId(string autoTileId)
        {
            if (!states.ContainsKey(autoTileId))
            {
                states[autoTileId] = new TransitionState
                {
                    AutoTileId = autoTileId
                };
            }
        }

        public void MarkNorth(string autoTileId, bool north)
        {
            CheckAutoTileId(autoTileId);
            states[autoTileId].MarkNorth(north);
        }

        public void MarkSouth(string autoTileId, bool south)
        {
            CheckAutoTileId(autoTileId);
            states[autoTileId].MarkSouth(south);
        }

        public void MarkEast(string autoTileId, bool east)
        {
            CheckAutoTileId(autoTileId);
            states[autoTileId].MarkEast(east);
        }

        public void MarkWest(string autoTileId, bool west)
        {
            CheckAutoTileId(autoTileId);
            states[autoTileId].MarkWest(west);
        }

        public void MarkNorthEast(string autoTileId, bool northEast)
        {
            CheckAutoTileId(autoTileId);
            states[autoTileId].MarkNorthEast(northEast);
        }

        public void MarkNorthWest(string autoTileId, bool northWest)
        {
            CheckAutoTileId(autoTileId);
            states[autoTileId].MarkNorthWest(northWest);
        }

        public void MarkSouthEast(string autoTileId, bool southEast)
        {
            CheckAutoTileId(autoTileId);
            states[autoTileId].MarkSouthEast(southEast);
        }

        public void MarkSouthWest(string autoTileId, bool southWest)
        {
            CheckAutoTileId(autoTileId);
            states[autoTileId].MarkSouthWest(southWest);
        }
    }

    internal class TransitionState
    {
        public string AutoTileId { get; set; } = "";

        private bool north = false;
        private bool south = false;
        private bool east = false;
        private bool west = false;

        private bool northEast = false;
        private bool northWest = false;
        private bool southEast = false;
        private bool southWest = false;

        public void MarkNorth(bool north)
        {
            this.north = north;
            if (north)
            {
                // borders override corners
                northEast = false;
                northWest = false;
            }
        }

        public void MarkSouth(bool south)
        {
            this.south = south;
            if (south)
            {
                // borders override corners
                southEast = false;
                southWest = false;
            }
        }

        public void MarkEast(bool east)
        {
            this.east = east;
            if(east)
            {
                // borders override corners
                northEast = false;
                southEast = false;
            }
        }

        public void MarkWest(bool west)
        {
            this.west = west;
            if(west)
            {
                // borders override corners
                northWest = false;
                southWest = false;
            }
        }

        public void MarkNorthEast(bool northEast)
        {
            if(!north)
            {
                // can't set corners if border set
                this.northEast = northEast;
            }
        }

        public void MarkNorthWest(bool northWest)
        {
            if (!north)
            {
                // can't set corners if border set
                this.northWest = northWest;
            }
        }

        public void MarkSouthEast(bool southEast)
        {
            if (!south)
            {
                // can't set corners if border set
                this.southEast = southEast;
            }
        }

        public void MarkSouthWest(bool southWest)
        {
            if (!south)
            {
                // can't set corners if border set
                this.southWest = southWest;
            }
        }

        public string GetBorderKey()
        {
            var westIsConnectable = west;
            var eastIsConnectable = east;
            var northIsConnectable = north;
            var southIsConnectable = south;

            if (westIsConnectable && !eastIsConnectable && !northIsConnectable && !southIsConnectable)
            {
                return "w";
            }
            else if (!westIsConnectable && eastIsConnectable && !northIsConnectable && !southIsConnectable)
            {
                return "e";
            }
            else if (!westIsConnectable && !eastIsConnectable && northIsConnectable && !southIsConnectable)
            {
                return "n";
            }
            else if (!westIsConnectable && !eastIsConnectable && !northIsConnectable && southIsConnectable)
            {
                return "s";
            }
            else if (westIsConnectable && eastIsConnectable && !northIsConnectable && !southIsConnectable)
            {
                return "ew";
            }
            else if (westIsConnectable && eastIsConnectable && northIsConnectable && !southIsConnectable)
            {
                return "new";
            }
            else if (westIsConnectable && eastIsConnectable && !northIsConnectable && southIsConnectable)
            {
                return "sew";
            }
            else if (!westIsConnectable && !eastIsConnectable && northIsConnectable && southIsConnectable)
            {
                return "ns";
            }
            else if (westIsConnectable && !eastIsConnectable && northIsConnectable && southIsConnectable)
            {
                return "nsw";
            }
            else if (!westIsConnectable && eastIsConnectable && northIsConnectable && southIsConnectable)
            {
                return "nse";
            }
            else if (westIsConnectable && !eastIsConnectable && northIsConnectable && !southIsConnectable)
            {
                return "nw";
            }
            else if (!westIsConnectable && eastIsConnectable && northIsConnectable && !southIsConnectable)
            {
                return "ne";
            }
            else if (!westIsConnectable && eastIsConnectable && !northIsConnectable && southIsConnectable)
            {
                return "se";
            }
            else if (westIsConnectable && !eastIsConnectable && !northIsConnectable && southIsConnectable)
            {
                return "sw";
            }
            else if (westIsConnectable && eastIsConnectable && northIsConnectable && southIsConnectable)
            {
                return "nsew";
            }

            return "";
        }

        public string GetCornerKey()
        {
            var westIsConnectable = southWest;
            var eastIsConnectable = southEast;
            var northIsConnectable = northWest;
            var southIsConnectable = northEast;

            if (westIsConnectable && !eastIsConnectable && !northIsConnectable && !southIsConnectable)
            {
                return "d";
            }
            else if (!westIsConnectable && eastIsConnectable && !northIsConnectable && !southIsConnectable)
            {
                return "c";
            }
            else if (!westIsConnectable && !eastIsConnectable && northIsConnectable && !southIsConnectable)
            {
                return "a";
            }
            else if (!westIsConnectable && !eastIsConnectable && !northIsConnectable && southIsConnectable)
            {
                return "b";
            }
            else if (westIsConnectable && eastIsConnectable && !northIsConnectable && !southIsConnectable)
            {
                return "cd";
            }
            else if (westIsConnectable && eastIsConnectable && northIsConnectable && !southIsConnectable)
            {
                return "acd";
            }
            else if (westIsConnectable && eastIsConnectable && !northIsConnectable && southIsConnectable)
            {
                return "bcd";
            }
            else if (!westIsConnectable && !eastIsConnectable && northIsConnectable && southIsConnectable)
            {
                return "ab";
            }
            else if (westIsConnectable && !eastIsConnectable && northIsConnectable && southIsConnectable)
            {
                return "abd";
            }
            else if (!westIsConnectable && eastIsConnectable && northIsConnectable && southIsConnectable)
            {
                return "abc";
            }
            else if (westIsConnectable && !eastIsConnectable && northIsConnectable && !southIsConnectable)
            {
                return "ad";
            }
            else if (!westIsConnectable && eastIsConnectable && northIsConnectable && !southIsConnectable)
            {
                return "ac";
            }
            else if (!westIsConnectable && eastIsConnectable && !northIsConnectable && southIsConnectable)
            {
                return "bc";
            }
            else if (westIsConnectable && !eastIsConnectable && !northIsConnectable && southIsConnectable)
            {
                return "bd";
            }
            else if (westIsConnectable && eastIsConnectable && northIsConnectable && southIsConnectable)
            {
                return "abcd";
            }

            return "";
        }
    }
}
