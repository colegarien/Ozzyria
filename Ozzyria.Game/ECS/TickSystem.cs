namespace Ozzyria.Game.ECS
{
    public abstract class TickSystem
    {
        public abstract void Execute(EntityContext context);
    }
}
