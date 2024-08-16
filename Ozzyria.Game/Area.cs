using Grecs;
using Ozzyria.Content;
using Ozzyria.Content.Models.Area;
using Ozzyria.Game.Utility;
using Ozzyria.Model.Types;

namespace Ozzyria.Game
{
    public class Area
    {
        private string _areaId;
        public SystemCoordinator _coordinator;
        public EntityContext _context;

        public Area(World world, string areaId)
        {
            _areaId = areaId;
            _context = new EntityContext();
            _coordinator = new SystemCoordinator();
            _coordinator
                .Add(new Systems.Player(world))
                .Add(new Systems.Slime())
                .Add(new Systems.Spawner())
                .Add(new Systems.ExperieneOrb())
                .Add(new Systems.MovementSystem())
                .Add(new Systems.Physics())
                .Add(new Systems.Doors())
                .Add(new Systems.AttackSystem())
                .Add(new Systems.Death(world, _context))
                .Add(new Systems.AreaChange(world, _context));

            // TODO eventually add a persistence layer where this stuff loads into a DB or something only on first run or refresh
            var areaData = AreaData.Retrieve(areaId);
            for (var layer = 0; layer < (areaData?.WallData?.Walls?.Length ?? 0); layer++)
            {
                foreach (var wall in areaData.WallData?.Walls[layer])
                {
                    var wallX = wall.X + (wall.Width / 2f);
                    var wallY = wall.Y + (wall.Height / 2f);
                    EntityFactory.CreateWall(_context, wallX, wallY, layer, (int)wall.Width, (int)wall.Height);
                }
            }

            var prefabPackage = Packages.GetInstance().PrefabPackage;
            for (var layer = 0; layer < (areaData?.PrefabData?.Prefabs?.Length ?? 0); layer++)
            {
                foreach (var prefab in areaData.PrefabData.Prefabs[layer])
                {
                    var prefabDefinition = prefabPackage.GetDefinition(prefab.PrefabId);
                    if (prefabDefinition != null)
                    {
                        Model.Utility.PrefabHydrator.HydrateDefinition(_context, prefabDefinition, prefab.Attributes ?? new ValuePacket());
                    }
                }
            }
        }

        public void Update(float deltaTime)
        {
            _coordinator.Execute(deltaTime, _context);
        }
    }
}
