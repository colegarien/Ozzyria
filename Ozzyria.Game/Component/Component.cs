namespace Ozzyria.Game.Component
{

    public enum ComponentType
    {
        None = -1,
        Movement = 2,
        Stats = 3,
        Combat = 4,
        Thought = 5,
        ExperienceBoost = 6,
        Input = 7,
        Collision = 8,
        Renderable = 9,
    }

    public abstract class Component
    {
        public Entity Owner { get; set; }

        public virtual ComponentType Type() => ComponentType.None;
    }
}
