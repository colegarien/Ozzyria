using Microsoft.Xna.Framework;
using System;

namespace Ozzyria.MonoGameClient
{
    public class Camera
    {
        public const int RENDER_RESOLUTION_W = 640;
        public const int RENDER_RESOLUTION_H = 360;

        private Vector2 Position { get; set; } = new Vector2(0, 0);
        private Vector2 ViewSize { get; set; } = new Vector2(0, 0);
        public int ViewPadding = 16;

        public float hScale = 0.5f;
        public float vScale = 0.5f;

        private Vector3 scaleVector = new Vector3(0.5f, 0.5f, 0);
        private Vector3 inversePosition = new Vector3(0, 0, 0);
        private Matrix scaleMatrix;
        private Matrix viewMatrix;
        private float fullViewWidth = 0f;
        private float fullViewHeight = 0f;
        private float halfViewWidth = 0f;
        private float halfViewHeight = 0f;
        private float minRenderX = 0f;
        private float maxRenderX = 0f;
        private float minRenderY = 0f;
        private float maxRenderY = 0f;

        public Camera(int width, int height)
        {
            ResizeView(width, height);
        }

        private void RecalculateInternals()
        {
            // So that the MATH is only done once, and only when it really needs to be done
            inversePosition = -(new Vector3((float)Math.Round(Position.X * hScale, MidpointRounding.AwayFromZero) / hScale, (float)Math.Round(Position.Y * vScale, MidpointRounding.AwayFromZero) / vScale, 0)); // round to v and h scale to avoid float issues & tearing
            fullViewWidth = ViewSize.X / hScale;
            fullViewHeight = ViewSize.Y / vScale;
            halfViewWidth = ViewSize.X * 0.5f / hScale;
            halfViewHeight = ViewSize.Y * 0.5f / vScale;
            minRenderX = Position.X - ViewPadding;
            maxRenderX = Position.X + ViewSize.X + ViewPadding;
            minRenderY = Position.Y - ViewPadding;
            maxRenderY = Position.Y + ViewSize.Y + ViewPadding;
            viewMatrix = Matrix.Identity * Matrix.CreateTranslation(inversePosition) * scaleMatrix;
        }

        public void ResizeView(int width, int height)
        {
            if (width != ViewSize.X || height != ViewSize.Y) // this 'if' is not here for any good reason, just paranoid trying to avoid newing
            {
                hScale = width / RENDER_RESOLUTION_W;
                vScale = height / RENDER_RESOLUTION_H;
                scaleVector = new Vector3(hScale, vScale, 1f);
                scaleMatrix = Matrix.Identity * Matrix.CreateScale(scaleVector);
                ViewSize = new Vector2(width, height);
                RecalculateInternals();
            }
        }

        public void CenterView(float x, float y)
        {
            if (x - halfViewWidth != Position.X || y - halfViewHeight != Position.Y) // this 'if' is not here for any good reason, just paranoid trying to avoid newing
            {
                Position = new Vector2
                {
                    X = x - halfViewWidth,
                    Y = y - halfViewHeight
                };
                RecalculateInternals();
            }
        }

        public void ApplyBounds(float left, float top, float right, float bottom)
        {
            var x = Position.X;
            var y = Position.Y;

            if (x < left && x + fullViewWidth > right)
            {
                // small bounds so center camera
                x = right - left - halfViewWidth;
            }
            else if (x < left)
            {
                x = left;
            }
            else if(x + fullViewWidth > right)
            {
                x = right - fullViewWidth;
            }

            if(y < top && y + fullViewHeight > bottom)
            {
                // small bounds so center camera
                y = bottom - top - halfViewHeight;
            }
            else if(y < top)
            {
                y = top;
            }
            else if(y + fullViewHeight > bottom)
            {
                y = bottom - fullViewHeight;
            }


            if(x != minRenderX || y != minRenderY)
            {
                Position = new Vector2
                {
                    X = x,
                    Y = y
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

        public Matrix GetScaleMatrix()
        {
            return scaleMatrix;
        }

        public Matrix GetViewMatrix()
        {
            return viewMatrix;
        }
    }
}
