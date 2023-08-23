using Ozzyria.Game.ECS;

namespace Ozzyria.Game
{
    public class Area
    {
        private string _name;
        public SystemCoordinator _coordinator;
        public EntityContext _context;

        public Area(World world, string name, string contextTemplate)
        {
            _name = name;
            _context = new EntityContext();
            _coordinator = new SystemCoordinator();
            _coordinator
                .Add(new Systems.Player(world))
                .Add(new Systems.Slime())
                .Add(new Systems.Spawner())
                .Add(new Systems.ExperieneOrb())
                .Add(new Systems.Physics())
                .Add(new Systems.Combat())
                .Add(new Systems.Death(_context))
                .Add(new Systems.AreaChange(world, _context))
                .Add(new Systems.AnimationStateSync(_context))
                .Add(new Systems.Animation());

            world.WorldLoader.LoadContext(_context, contextTemplate);
        }

        public void Update(float deltaTime)
        {
            _coordinator.Execute(deltaTime, _context);
        }
    }
}
