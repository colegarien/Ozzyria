namespace Ozzyria.Game.ECS
{
    public interface IComponent
    {
        public Entity Owner { get; set; }
        public ComponentEvent OnComponentChanged { get; set; }
    }
}
