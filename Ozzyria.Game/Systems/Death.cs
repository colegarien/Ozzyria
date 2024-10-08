﻿using Ozzyria.Model.Components;
using Grecs;
using Ozzyria.Game.Utility;
using Ozzyria.Model.CodeGen.Packages;
using Ozzyria.Content;
using System.Reflection.Emit;

namespace Ozzyria.Game.Systems
{
    internal class Death : TriggerSystem
    {
        protected World _world;
        protected PrefabPackage _prefabPackage;
        public Death(World world, EntityContext context) : base(context)
        {
            _world = world;
            _prefabPackage = Packages.GetInstance().PrefabPackage;
        }

        public override void Execute(EntityContext context, Entity[] entities)
        {
            foreach (var entity in entities)
            {
                var movement = (Movement)entity.GetComponent(typeof(Movement));
                EntityFactory.CreateExperienceOrb(context, movement.X, movement.Y, movement.Layer, 10);
                if (entity.HasComponent(typeof(Model.Components.Player)))
                {
                    // TODO OZ-30 : create Graveyard component and add a AssignedGraveyard to players, then just revive them there
                    // TODO OZ-30 : probably just remove thought component, change renderable to corpse then make a "respawn" system to handle player respawn at a graveyard

                    var playerId = ((Model.Components.Player)entity.GetComponent(typeof(Model.Components.Player))).PlayerId;

                    // reset player
                    context.DestroyEntity(entity);
                    EntityFactory.CreatePlayer(context, playerId, _world.WorldState.PlayerAreaTracker[playerId], _world.WorldState.ContainerStorage);
                }
                else
                {
                    context.DestroyEntity(entity);
                }
            }
        }

        protected override bool Filter(Entity entity)
        {
            return true;
        }

        protected override QueryListener GetListener(EntityContext context)
        {
            var query = new EntityQuery().And(typeof(Dead), typeof(Movement));
            return context.CreateListener(query);
        }
    }
}
