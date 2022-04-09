using Ozzyria.Game.ECS;
using Ozzyria.Test.ECS.Stub;
using Xunit;

namespace Ozzyria.Test.ECS
{
    public class SystemCoordinatorTest
    {
        private readonly EntityContext _context;
        private readonly SystemCoordinator _coordinator;

        public SystemCoordinatorTest()
        {
            _context = new EntityContext();
            _coordinator = new SystemCoordinator();
        }

        // TODO OZ-14 test trigger systems
        [Fact]
        public void TickSystemOrdering()
        {
            var systemA = new NumberIncrementTickSystem();
            var systemB = new NumberIncrementTickSystem();
            var systemC = new NumberIncrementTickSystem();

            _coordinator.Add(systemA)
                .Add(systemB)
                .Add(systemC);

            _coordinator.Execute(_context);

            Assert.True(systemA.InstanceTick < systemB.InstanceTick);
            Assert.True(systemB.InstanceTick < systemC.InstanceTick);
        }

        [Fact]
        public void TickSystemUpdatingComponents()
        {
            var entity = _context.CreateEntity();
            var component = (ComponentB)entity.CreateComponent<ComponentB>();
            component.SomeNumber = 1;
            entity.AddComponent(component);

            _coordinator.Add(new ComponentBIterateTickSystem());

            _coordinator.Execute(_context);

            Assert.Equal(2, ((ComponentB)entity.GetComponent(typeof(ComponentB))).SomeNumber);
        }

        [Fact]
        public void TickSystemUpdatingComponents_MultipleSystems()
        {
            var entity = _context.CreateEntity();
            var component = (ComponentB)entity.CreateComponent<ComponentB>();
            component.SomeNumber = 1;
            entity.AddComponent(component);

            _coordinator.Add(new ComponentBIterateTickSystem())
                .Add(new ComponentBIterateTickSystem());

            _coordinator.Execute(_context);

            Assert.Equal(3, ((ComponentB)entity.GetComponent(typeof(ComponentB))).SomeNumber);
        }

        [Fact]
        public void TickSystemUpdatingComponents_MultipleTicks()
        {
            var entity = _context.CreateEntity();
            var component = (ComponentB)entity.CreateComponent<ComponentB>();
            component.SomeNumber = 1;
            entity.AddComponent(component);

            _coordinator.Add(new ComponentBIterateTickSystem());

            _coordinator.Execute(_context);
            _coordinator.Execute(_context);

            Assert.Equal(3, ((ComponentB)entity.GetComponent(typeof(ComponentB))).SomeNumber);
        }

        // TODO OZ-14 test add/remove components in systems

        // TODO OZ-14 test add/remove entities in systems


    }
}
