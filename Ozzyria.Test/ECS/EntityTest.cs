using Ozzyria.Game.ECS;
using Ozzyria.Test.ECS.Stub;
using Xunit;

namespace Ozzyria.Test.ECS
{
    public class EntityTest
    {
        private readonly EntityContext _context;
        private readonly Entity _entity;

        private Entity lastEventEntity;
        private IComponent lastEventComponent;

        public EntityTest()
        {
            _context = new EntityContext();
            _entity = _context.CreateEntity();

            lastEventEntity = null;
            lastEventComponent = null;
        }

        public void HandleComponentEvent(Entity entity, IComponent component)
        {
            lastEventEntity = entity;
            lastEventComponent = component;
        }

        [Fact]
        public void createComponentTest()
        {
            var one = _entity.CreateComponent<ComponentA>();
            var two = _entity.CreateComponent(typeof(ComponentA));
            Assert.IsType<ComponentA>(one);
            Assert.IsType<ComponentA>(two);
        }

        [Fact]
        public void addComponentTest()
        {
            var component = _entity.CreateComponent<ComponentA>();
            _entity.AddComponent(component);

            Assert.True(_entity.HasComponent(typeof(ComponentA)));
            Assert.Same(component, _entity.GetComponents()[0]);
        }

        [Fact]
        public void addComponent_MultipleTest()
        {
            var componentA = _entity.CreateComponent<ComponentA>();
            var componentB = _entity.CreateComponent<ComponentB>();
            _entity.AddComponent(componentA);
            _entity.AddComponent(componentB);

            Assert.Same(componentA, _entity.GetComponent(typeof(ComponentA)));
            Assert.Same(componentB, _entity.GetComponent(typeof(ComponentB)));
        }

        [Fact]
        public void removeComponentTest()
        {
            var componentA = _entity.CreateComponent<ComponentA>();
            var componentB = _entity.CreateComponent<ComponentB>();
            _entity.AddComponent(componentA);
            _entity.AddComponent(componentB);

            _entity.RemoveComponent(componentA);

            Assert.Null(_entity.GetComponent(typeof(ComponentA)));
            Assert.Same(componentB, _entity.GetComponent(typeof(ComponentB)));
            Assert.Same(componentB, _entity.GetComponents()[0]);
        }

        [Fact]
        public void removeAllComponents()
        {
            var componentA = _entity.CreateComponent<ComponentA>();
            var componentB = _entity.CreateComponent<ComponentB>();
            _entity.AddComponent(componentA);
            _entity.AddComponent(componentB);

            _entity.RemoveAllComponents();

            Assert.Null(_entity.GetComponent(typeof(ComponentA)));
            Assert.Null(_entity.GetComponent(typeof(ComponentB)));
        }

        [Fact]
        public void testComponetAddedEvent()
        {
            _entity.OnComponentAdded += HandleComponentEvent;
            var componentA = _entity.CreateComponent<ComponentA>();

            _entity.AddComponent(componentA);

            Assert.Same(_entity, lastEventEntity);
            Assert.Same(componentA, lastEventComponent);
        }

        [Fact]
        public void testComponetRemovedEvent()
        {
            _entity.OnComponentRemoved += HandleComponentEvent;
            var componentA = _entity.CreateComponent<ComponentA>();

            _entity.AddComponent(componentA);
            _entity.RemoveComponent(componentA);

            Assert.Same(_entity, lastEventEntity);
            Assert.Same(componentA, lastEventComponent);
        }
    }
}
