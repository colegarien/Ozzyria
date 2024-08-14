using Ozzyria.Game.Components;
using Grecs;
using System;
using Movement = Ozzyria.Model.Components.Movement;

namespace Ozzyria.Game.Systems
{
    internal class Doors : TickSystem
    {
        protected EntityQuery doorQuery;
        protected EntityQuery playerQuery;

        const int TRIGGER_THRESHOLD = 32;

        public Doors()
        {
            doorQuery = new EntityQuery();
            doorQuery.And(typeof(Door), typeof(Movement));


            // OZ-22 : make doors work for more than just players, potentiall change to require action to use a door
            playerQuery = new EntityQuery();
            playerQuery.And(typeof(Components.Player), typeof(Movement));
        }

        public override void Execute(float deltaTime, EntityContext context)
        {
            foreach (var doorEntity in context.GetEntities(doorQuery))
            {
                var doorMovement = (Movement)doorEntity.GetComponent(typeof(Movement));
                var doorComponent = (Door)doorEntity.GetComponent(typeof(Door));

                foreach(var playerEntity in context.GetEntities(playerQuery))
                {
                    var playerMovement = (Movement)playerEntity.GetComponent(typeof(Movement));


                    // OZ-22 : make doors components work more like a generic "Trigger" system instead of weird distance calculation
                    if (!playerEntity.HasComponent(typeof(AreaChange)) && Math.Abs(playerMovement.X - doorMovement.X) <= TRIGGER_THRESHOLD && Math.Abs(playerMovement.Y - doorMovement.Y) <= TRIGGER_THRESHOLD)
                    {
                        var areaChange = (Components.AreaChange)playerEntity.CreateComponent(typeof(Components.AreaChange));
                        areaChange.NewArea = doorComponent.NewArea;
                        areaChange.NewX = doorComponent.NewX;
                        areaChange.NewY = doorComponent.NewY;

                        playerEntity.AddComponent(areaChange);
                    }
                }
            }
        }
    }
}
