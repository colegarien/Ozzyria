using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ozzyria.Gryp.Models.Data
{

    internal class AutoTileRecord
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "";

        [JsonPropertyName("path_pieces")]
        public Dictionary<string, string> PathPieces { get; set; } = new Dictionary<string, string>();
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

        public void AutoTile(Tile tile, Map map, int x, int y,  int depth = 1)
        {
            if(tile.DrawableIds.Count < 1 || depth > Depth)
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
        }
    }
}
