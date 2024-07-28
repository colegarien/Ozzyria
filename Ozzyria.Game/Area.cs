using Grecs;
using Ozzyria.Content.Models.Area;
using Ozzyria.Game.Utility;

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
                .Add(new Systems.Death(_context))
                .Add(new Systems.AreaChange(world, _context));

            // TODO eventually add a persistence layer where this stuff loads into a DB or something only on first run or refresh
            var areaData = AreaData.Retrieve(areaId);
            for (var layer = 0; layer < (areaData?.WallData?.Walls?.Length ?? 0); layer++) {
                foreach (var wall in areaData.WallData?.Walls[layer])
                {
                    EntityFactory.CreateBoxColliderArea(_context, wall.X, wall.Y, wall.X + wall.Width, wall.Y + wall.Height);
                }
            }

            for (var layer = 0; layer < (areaData?.PrefabData?.Prefabs?.Length ?? 0); layer ++)
            {
                foreach (var prefab in areaData.PrefabData.Prefabs[layer])
                {
                    // TODO stop hard-coding this and make them more dynamic (see MainForm.cs in Gryp for these value mappings)
                    switch (prefab.PrefabId) {
                        case "slime_spawner":
                            EntityFactory.CreateSlimeSpawner(_context, prefab.X, prefab.Y);
                            break;
                        case "door":
                            EntityFactory.CreateDoor(_context, prefab.X, prefab.Y, prefab.Attributes?["new_area_id"] ?? "", float.Parse(prefab.Attributes?["new_area_x"] ?? "0"), float.Parse(prefab.Attributes?["new_area_y"] ?? "0"));
                            break;
                        case "exp_orb":
                            EntityFactory.CreateExperienceOrb(_context, prefab.X, prefab.Y, int.Parse(prefab.Attributes?["amount"] ?? "0"));
                            break;
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
