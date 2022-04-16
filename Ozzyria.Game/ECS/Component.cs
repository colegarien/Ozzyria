namespace Ozzyria.Game.ECS
{
    public class Component : IComponent
    {
        public Entity Owner { get; set; }
        public ComponentEvent OnComponentChanged { get; set; }
    }
}
