namespace Ozzyria.Gryp.Models.Data
{
    internal class Map
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public List<Layer> Layers { get; set; } = new List<Layer>();

        public int ActiveLayer { get; set; } = -1;
        public Dictionary<int, bool> IsLayerHidden { get; set; } = new Dictionary<int, bool>();

        public TileBoundary? SelectedRegion { get; set; } = null;

        public List<string> CurrentBrush { get; set; } = new List<string>();

        public bool IsLayerVisible(int layer)
        {
            return !IsLayerHidden.ContainsKey(layer) || !IsLayerHidden[layer];
        }

        public void PushLayer()
        {
            Layers.Add(new Layer(new TileBoundary
            {
                TileWidth = Width,
                TileHeight = Height
            }));
        }

        public void PushTile(TileData tileData, int x, int y)
        {
            bool isInSelection = SelectedRegion == null
                || SelectedRegion.TileWidth <= 0
                || SelectedRegion.TileHeight <= 0
                || SelectedRegion.Contains(x, y);

            if (isInSelection && ActiveLayer >= 0 && ActiveLayer < Layers.Count)
            {
                Layers[ActiveLayer].PushTile(tileData, x, y);
            }
        }

        public void PaintArea(TileData tileData, int originX, int originY)
        {
            if (ActiveLayer >= 0 && ActiveLayer < Layers.Count)
            {
                Layers[ActiveLayer].PaintArea(SelectedRegion, tileData, originX, originY);
            }
        }

        public void AddWall(WorldBoundary wall)
        {
            if (ActiveLayer >= 0 && ActiveLayer < Layers.Count)
            {
                Layers[ActiveLayer].AddWall(wall);
            }
        }

        public void RemoveWalls(float worldX, float worldY)
        {
            if (ActiveLayer >= 0 && ActiveLayer < Layers.Count)
            {
                Layers[ActiveLayer].RemoveWalls(worldX, worldY);
            }
        }
    }

}
