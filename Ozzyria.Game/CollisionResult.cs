namespace Ozzyria.Game
{
    public class CollisionResult
    {
        public bool Collided { get; set; } = false;
        public float Depth { get; set; } = 0f;
        public float NormalX { get; set; } = 0f;
        public float NormalY { get; set; } = 0f;
    }
}
