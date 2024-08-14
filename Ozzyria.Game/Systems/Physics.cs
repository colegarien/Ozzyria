using Ozzyria.Model.Components;
using Grecs;
using System.Linq;
using System.Numerics;
using Ozzyria.Model.Extensions;

namespace Ozzyria.Game.Systems
{
    internal class Physics : TickSystem
    {
        protected EntityQuery query;

        public Physics()
        {
            query = new EntityQuery();
            query.And(typeof(Movement), typeof(Collision));
        }

        public override void Execute(float deltaTime, EntityContext context)
        {
            var entities = context.GetEntities(query);
            foreach (var entity in entities)
            {
                var movement = (Movement)entity.GetComponent(typeof(Movement));
                var collision = (Collision)entity.GetComponent(typeof(Collision));
                if (collision.IsDynamic)
                {
                    var possibleCollisions = entities.Where(e => e.id != entity.id && e.GetComponent<Movement>().Layer == movement.Layer);
                    var depthVector = Vector2.Zero;
                    foreach (var collidedEntity in possibleCollisions)
                    {
                        var otherMovement = (Movement)collidedEntity.GetComponent(typeof(Movement));
                        var result = movement.CheckCollision(otherMovement);
                        if (result.Collided)
                        {
                            depthVector += new Vector2(result.NormalX, result.NormalY) * result.Depth;
                        }
                    }

                    movement.X += depthVector.X;
                    movement.Y += depthVector.Y;
                }
            }
        }
    }
}
