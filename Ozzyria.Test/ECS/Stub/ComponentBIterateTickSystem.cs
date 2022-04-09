using Ozzyria.Game.ECS;

namespace Ozzyria.Test.ECS.Stub
{
    internal class ComponentBIterateTickSystem : TickSystem
    {
        public override void Execute(EntityContext context)
        {
            var query = new EntityQuery();
            query.And(typeof(ComponentB));

            var bEntities = context.GetEntities(query);
            foreach(var entity in bEntities)
            {
                // TODO OZ-14 : is this the way to update componets or should do a "replace" situation?
                var c = (ComponentB)entity.GetComponent(typeof(ComponentB));
                c.SomeNumber++;
            }
        }
    }
}
