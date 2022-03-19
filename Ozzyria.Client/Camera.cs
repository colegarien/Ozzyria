using SFML.System;

namespace Ozzyria.Client
{
    class Camera
    {
        public const uint RENDER_RESOLUTION_W = 640;
        public const uint RENDER_RESOLUTION_H = 360;

        private Vector2f Position { get; set; } = new Vector2f(0, 0);
        private Vector2u ViewSize { get; set; } = new Vector2u(0, 0);
        public uint ViewPadding = 0;

        public float hScale = 0.5f;
        public float vScale = 0.5f;

        private Vector2f inversePosition = new Vector2f(0, 0);
        private float halfViewWidth = 0f;
        private float halfViewHeight = 0f;
        private float minRenderX = 0f;
        private float maxRenderX = 0f;
        private float minRenderY = 0f;
        private float maxRenderY = 0f;

        public Camera(uint width, uint height)
        {
            ResizeView(width, height);
        }

        private void RecalculateInternals()
        {
            // So that the MATH is only done once, and only when it really needs to be done
            inversePosition = -(new Vector2f(Position.X * hScale, Position.Y * vScale));
            halfViewWidth = ViewSize.X * 0.5f / hScale;
            halfViewHeight = ViewSize.Y * 0.5f / vScale;
            minRenderX = Position.X - ViewPadding;
            maxRenderX = Position.X + ViewSize.X + ViewPadding;
            minRenderY = Position.Y - ViewPadding;
            maxRenderY = Position.Y + ViewSize.Y + ViewPadding;
        }

        public void ResizeView(uint width, uint height)
        {
            if (width != ViewSize.X || height != ViewSize.Y) // this 'if' is not here for any good reason, just paranoid trying to avoid newing
            {
                hScale = width / RENDER_RESOLUTION_W;
                vScale = height / RENDER_RESOLUTION_H;
                ViewSize = new Vector2u(width, height);
                RecalculateInternals();
            }
        }

        public void CenterView(float x, float y)
        {
            if (x != Position.X || y != Position.Y) // this 'if' is not here for any good reason, just paranoid trying to avoid newing
            {
                Position = new Vector2f
                {
                    X = x - halfViewWidth,
                    Y = y - halfViewHeight
                };
                RecalculateInternals();
            }
        }

        public bool IsInView(float x, float y, float w, float h)
        {
            return (
                x < maxRenderX &&
                x + w > minRenderX &&
                y < maxRenderY &&
                y + h > minRenderY
            );
        }

        public Vector2f GetTranslationVector()
        {
            return inversePosition;
        }
    }
}
