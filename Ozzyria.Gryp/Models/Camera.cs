namespace Ozzyria.Gryp.Models
{
    internal class Camera
    {
        public float ViewX { get; set; } = 0f;
        public float ViewY { get; set; } = 0f;
        public float WorldX { get; set; } = 0f;
        public float WorldY { get; set; } = 0f;

        public float Scale { get; set; } = 1f;

        public void MoveToWorldCoordinates(float worldX, float worldY)
        {
            WorldX = worldX;
            WorldY = worldY;
            ViewX = WorldToView(WorldX);
            ViewY = WorldToView(WorldY);
        }

        public void MoveToViewCoordinates(float viewX, float viewY)
        {
            ViewX = viewX;
            ViewY = viewY;
            WorldX = ViewToWorld(viewX);
            WorldY = ViewToWorld(viewY);
        }

        public void ScaleTo(int viewX, int viewY, float targetScale)
        {
            var previousWorldXOrigin = ViewToWorld(viewX);
            var previousWorldYOrigin = ViewToWorld(viewY);

            Scale = targetScale;
            if (Scale < 0.05f)
            {
                Scale = 0.05f;
            }
            else if (Scale > 10f)
            {
                Scale = 10f;
            }

            var deltaWorldX = ViewToWorld(viewX) - previousWorldXOrigin;
            var deltaWorldY = ViewToWorld(viewY) - previousWorldYOrigin;

            // shift camera by delta
            MoveToWorldCoordinates(WorldX + deltaWorldX, WorldY + deltaWorldY);
        }

        public float WorldToView(float world)
        {
            return world * Scale;
        }

        public float ViewToWorld(float view)
        {
            return view / Scale;
        }
    }
}
