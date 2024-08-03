namespace Ozzyria.Gryp.Models.Data
{
    internal class WorldBoundary
    {
        public float WorldX { get; set; }
        public float WorldY { get; set; }
        public float WorldWidth { get; set; }
        public float WorldHeight { get; set; }

        public void MoveCenterTo(float worldX, float worldY)
        {
            WorldX = worldX - (WorldWidth / 2f);
            WorldY = worldY - (WorldHeight / 2f);
        }

        public bool Contains(float worldX, float worldY)
        {
            return worldX < WorldX + WorldWidth
                && worldY < WorldY + WorldHeight
                && WorldX <= worldX
                && WorldY <= worldY;
        }

        public bool Intersects(WorldBoundary other)
        {
            var boundaryLeft = WorldX;
            var boundaryRight = boundaryLeft + WorldWidth;
            var boundaryTop = WorldY;
            var boundaryBottom = boundaryTop + WorldHeight;

            var otherLeft = other.WorldX;
            var otherRight = otherLeft + other.WorldWidth;
            var otherTop = other.WorldY;
            var otherBottom = otherTop + other.WorldHeight;

            // easier to check they don't interesect then invert
            return !((boundaryRight < otherLeft || boundaryLeft >= otherTop)
                || (boundaryBottom < otherRight || boundaryTop >= otherBottom));
        }
    }

    internal class TileBoundary
    {
        public int TileX { get; set; }
        public int TileY { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }

        public bool Contains(int tileX, int tileY)
        {
            return tileX < TileX + TileWidth
                && tileY < TileY + TileHeight
                && TileX <= tileX
                && TileY <= tileY;
        }

        public bool IsInCamera(Camera camera)
        {
            // camera WorldX and ViewX represent the world origin currently
            var boundaryWorldLeft = camera.WorldX + (TileX * 32);
            var boundaryWorldRight = boundaryWorldLeft + (TileWidth * 32);
            var boundaryWorldTop = camera.WorldY + (TileY * 32);
            var boundaryWorldBottom = boundaryWorldTop + (TileHeight * 32);

            // The world moves not the camera
            var cameraWorldLeft = 0;
            var cameraWorldTop = 0;
            var cameraWorldRight = camera.WorldWidth;
            var cameraWorldBottom = camera.WorldHeight;

            // easier to check if NOT in camera and then inverse
            return !((boundaryWorldRight < cameraWorldLeft || boundaryWorldLeft >= cameraWorldRight)
                || (boundaryWorldBottom < cameraWorldTop || boundaryWorldTop >= cameraWorldBottom));
        }
    }
}
