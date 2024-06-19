namespace Ozzyria.Gryp.Models.Data
{
    internal class Boundary
    {
        public float WorldX { get; set; }
        public float WorldY { get; set; }
        public float WorldWidth { get; set; }
        public float WorldHeight { get; set; }

        public bool Contains(float worldX, float worldY)
        {
            return worldX < WorldX + WorldWidth
                && worldY < WorldY + WorldHeight
                && WorldX <= worldX
                && WorldY <= worldY;
        }

        public bool Intersects(Boundary other)
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
}
