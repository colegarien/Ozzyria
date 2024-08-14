using Ozzyria.Game.Components;
using Grecs;
using System.Linq;
using System.Numerics;
using Movement = Ozzyria.Model.Components.Movement;
using Ozzyria.Model.Extensions;
using Ozzyria.Model.Types;

namespace Ozzyria.Game.Systems
{
    internal class Physics : TickSystem
    {
        protected EntityQuery query;

        public Physics()
        {
            query = new EntityQuery();
            query.And(typeof(Movement))
                .Or(typeof(BoundingBox), typeof(BoundingCircle));
        }

        public override void Execute(float deltaTime, EntityContext context)
        {
            var entities = context.GetEntities(query);
            foreach (var entity in entities)
            {
                var movement = (Movement)entity.GetComponent(typeof(Movement));
                var collision = (Collision)(entity.GetComponent(typeof(BoundingBox)) ?? entity.GetComponent(typeof(BoundingCircle)));
                if (collision.IsDynamic)
                {
                    var possibleCollisions = entities.Where(e => e.id != entity.id && e.GetComponent<Movement>().Layer == movement.Layer);
                    var depthVector = Vector2.Zero;
                    foreach (var collidedEntity in possibleCollisions)
                    {
                        var otherMovement = (Movement)collidedEntity.GetComponent(typeof(Movement));
                        var otherCollision = (Collision)(collidedEntity.GetComponent(typeof(BoundingBox)) ?? collidedEntity.GetComponent(typeof(BoundingCircle)));

                        if (collision is BoundingCircle && otherCollision is BoundingCircle)
                        {
                            var result = Collision.CircleIntersectsCircle((BoundingCircle)collision, (BoundingCircle)otherCollision);
                            if (result.Collided)
                            {
                                depthVector += new Vector2(result.NormalX, result.NormalY) * result.Depth;
                            }
                        }
                        else if (collision is BoundingBox && otherCollision is BoundingBox)
                        {
                            var result = Collision.BoxIntersectsBox((BoundingBox)collision, (BoundingBox)otherCollision);
                            if (result.Collided)
                            {
                                depthVector += new Vector2(result.NormalX, result.NormalY) * result.Depth;
                            }
                        }
                        else if (collision is BoundingCircle && otherCollision is BoundingBox)
                        {
                            var result = Collision.CircleIntersectsBox((BoundingCircle)collision, (BoundingBox)otherCollision);
                            if (result.Collided)
                            {
                                depthVector += new Vector2(result.NormalX, result.NormalY) * result.Depth;
                            }
                        }
                        else if (collision is BoundingBox && otherCollision is BoundingCircle)
                        {
                            var result = Collision.BoxIntersectsCircle((BoundingBox)collision, (BoundingCircle)otherCollision);
                            if (result.Collided)
                            {
                                depthVector += new Vector2(result.NormalX, result.NormalY) * result.Depth;
                            }
                        }
                    }
                    movement.X += depthVector.X;
                    movement.Y += depthVector.Y;
                }
            }
        }
    }
}
