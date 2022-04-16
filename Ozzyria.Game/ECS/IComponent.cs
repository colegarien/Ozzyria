namespace Ozzyria.Game.ECS
{
    public interface IComponent
    {
        public Entity Owner { get; set; }
        public ComponentEvent OnComponentChanged { get; set; } // TODO OZ-14 think on this one.. maybe a sneakier way...
    }
}
