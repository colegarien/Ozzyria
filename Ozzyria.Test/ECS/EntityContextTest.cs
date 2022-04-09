using Ozzyria.Game.ECS;
using Xunit;

namespace Ozzyria.Test.ECS
{
    public class EntityContextTest
    {
        private readonly EntityContext _context;


        public EntityContextTest()
        {
            _context = new EntityContext();
        }

        [Fact]
        public void CreateEntitiesWithUniqueIds()
        {
            var a = _context.CreateEntity();
            var b = _context.CreateEntity();
            var c = _context.CreateEntity();

            Assert.NotSame(a, b);
            Assert.NotSame(b, c);

            Assert.NotEqual(a.id, b.id);
            Assert.NotEqual(b.id, c.id);
        }

        [Fact]
        public void RemoveEntitiesTest()
        {
            var a = _context.CreateEntity();
            var b = _context.CreateEntity();
            var c = _context.CreateEntity();

            _context.DestroyEntity(b);

            var entities = _context.GetEntities();
            Assert.Contains(entities, e => e == a);
            Assert.DoesNotContain(entities, e => e == b);
            Assert.Contains(entities, e => e == c);
        }
    }
}
