namespace Ozzyria.Gryp.Models.Form
{
    internal class MouseState
    {
        public bool IsLeftDown { get; set; }
        public bool IsRightDown { get; set; }
        public bool IsMiddleDown { get; set; }

        public float PreviousMouseX { get; set; } = 0f;
        public float PreviousMouseY { get; set; } = 0f;
        public float MouseX { get; set; } = 0f;
        public float MouseY { get; set; } = 0f;

        public float LeftDownStartX { get; set; } = 0f;
        public float LeftDownStartY { get; set; } = 0f;

        public float RightDownStartX { get; set; } = 0f;
        public float RightDownStartY { get; set; } = 0f;

        public float MiddleDownStartX { get; set; } = 0f;
        public float MiddleDownStartY { get; set; } = 0f;
    }
}
