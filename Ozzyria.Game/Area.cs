using Ozzyria.Game.ECS;

namespace Ozzyria.Game
{
    public class AreaTemplate
    {
        public string Name { get; set; }
        public string EntityTemplate { get; set; }
    }

    public class Area
    {
        private string _name;
        public SystemCoordinator _coordinator;
        public EntityContext _context;

        public Area(World world, AreaTemplate template)
        {
            _name = template.Name;
            _context = new EntityContext();
            _coordinator = new SystemCoordinator();
            _coordinator
                .Add(new Systems.Player(world))
                .Add(new Systems.Slime())
                .Add(new Systems.Spawner())
                .Add(new Systems.ExperieneOrb())
                .Add(new Systems.Physics())
                .Add(new Systems.Doors())
                .Add(new Systems.Combat())
                .Add(new Systems.Death(_context))
                .Add(new Systems.AreaChange(world, _context));

            world.WorldLoader.LoadContext(_context, template.EntityTemplate);
        }

        public void Update(float deltaTime)
        {
            _coordinator.Execute(deltaTime, _context);
        }
    }
}
