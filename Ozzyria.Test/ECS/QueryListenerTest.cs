using Ozzyria.Game.ECS;
using Ozzyria.Test.ECS.Stub;
using Xunit;

namespace Ozzyria.Test.ECS
{
    public class QueryListenerTest
    {
        private readonly EntityContext _context;

        public QueryListenerTest()
        {
            _context = new EntityContext();
        }

        [Fact]
        public void NoChangesReturnEmpty()
        {
            var query = new EntityQuery().And(typeof(ComponentA));
            var listener = _context.CreateListener(query);

            var actual = listener.Gather();

            Assert.Empty(actual);
        }

        [Fact]
        public void EntityChangedBeforeListenerCreated()
        {
            var e = _context.CreateEntity();
            e.AddComponent(e.CreateComponent(typeof(ComponentA)));

            var query = new EntityQuery().And(typeof(ComponentA));
            var listener = _context.CreateListener(query);

            var actual = listener.Gather();

            Assert.Empty(actual);
        }

        [Fact]
        public void EntityChangedIsNotInQuery()
        {
            var e = _context.CreateEntity();
            var query = new EntityQuery().And(typeof(ComponentA));
            var listener = _context.CreateListener(query);

            e.AddComponent(e.CreateComponent(typeof(ComponentB)));

            var actual = listener.Gather();

            Assert.Empty(actual);
        }

        [Fact]
        public void EntityChangedAddThenRemoveComponentBeforeGather()
        {
            var e = _context.CreateEntity();
            var query = new EntityQuery().And(typeof(ComponentA));
            var listener = _context.CreateListener(query);

            var componentA = e.CreateComponent(typeof(ComponentA));
            e.AddComponent(componentA);
            e.RemoveComponent(componentA);

            var actual = listener.Gather();

            Assert.Empty(actual);
        }

        [Fact]
        public void EntityMeetsQuery()
        {
            var e = _context.CreateEntity();
            var query = new EntityQuery().And(typeof(ComponentA));
            var listener = _context.CreateListener(query);

            var componentA = e.CreateComponent(typeof(ComponentA));
            e.AddComponent(componentA);

            var actualFirst = listener.Gather();
            var actualSecond = listener.Gather();

            Assert.Same(e, actualFirst[0]);
            Assert.Empty(actualSecond);
        }
    }
}
