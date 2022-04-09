namespace Ozzyria.Game.ECS
{
    public abstract class TriggerSystem
    {
        // TODO OZ-14 components should be immutable to make this work better?
        public abstract EntityQuery GetQuery(EntityContext context);
        public abstract bool Filter(Entity entity);
        public abstract void Execute(EntityContext context);
    }
}
