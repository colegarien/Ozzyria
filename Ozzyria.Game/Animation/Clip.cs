namespace Ozzyria.Game.Animation
{
    public struct Clip
    {
        public Frame[] Frames { get; set; }

        public int Tick(int index)
        {
            var lastFrame = GetLastFrame();
            if (index == lastFrame)
                return 0;

            return NextFrameIndex(index);
        }

        public Frame GetFrame(int index)
        {
            return Frames[index % Frames.Length];
        }

        public int GetLastFrame()
        {
            return Frames.Length - 1;
        }

        public int NextFrameIndex(int index)
        {
            return (index + 1) % Frames.Length;
        }
    }
}
