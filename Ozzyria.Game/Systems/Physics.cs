using Ozzyria.Model.Components;
using Grecs;
using System.Linq;
using System.Numerics;

namespace Ozzyria.Game.Systems
{
    internal class Physics : TickSystem
    {
        protected EntityQuery query;

        public Physics()
        {
            query = new EntityQuery();
            query.And(typeof(Movement))
                .Or(typeof(Ozzyria.Game.Components.BoundingBox), typeof(Ozzyria.Game.Components.BoundingCircle));
        }

        public override void Execute(float deltaTime, EntityContext context)
        {
            var entities = context.GetEntities(query);
            foreach (var entity in entities)
            {
                var movement = (Movement)entity.GetComponent(typeof(Movement));
                var collision = (Ozzyria.Game.Components.Collision)(entity.GetComponent(typeof(Ozzyria.Game.Components.BoundingBox)) ?? entity.GetComponent(typeof(Ozzyria.Game.Components.BoundingCircle)));
                if (collision.IsDynamic)
                {
                    var possibleCollisions = entities.Where(e => e.id != entity.id && e.GetComponent<Movement>().Layer == movement.Layer);
                    var depthVector = Vector2.Zero;
                    foreach (var collidedEntity in possibleCollisions)
                    {
                        var otherMovement = (Movement)collidedEntity.GetComponent(typeof(Movement));
                        var otherCollision = (Ozzyria.Game.Components.Collision)(collidedEntity.GetComponent(typeof(Ozzyria.Game.Components.BoundingBox)) ?? collidedEntity.GetComponent(typeof(Ozzyria.Game.Components.BoundingCircle)));

                        if (collision is Ozzyria.Game.Components.BoundingCircle && otherCollision is Ozzyria.Game.Components.BoundingCircle)
                        {
                            var result = Ozzyria.Game.Components.Collision.CircleIntersectsCircle((Ozzyria.Game.Components.BoundingCircle)collision, (Ozzyria.Game.Components.BoundingCircle)otherCollision);
                            if (result.Collided)
                            {
                                depthVector += new Vector2(result.NormalX, result.NormalY) * result.Depth;
                            }
                        }
                        else if (collision is Ozzyria.Game.Components.BoundingBox && otherCollision is Ozzyria.Game.Components.BoundingBox)
                        {
                            var result = Ozzyria.Game.Components.Collision.BoxIntersectsBox((Ozzyria.Game.Components.BoundingBox)collision, (Ozzyria.Game.Components.BoundingBox)otherCollision);
                            if (result.Collided)
                            {
                                depthVector += new Vector2(result.NormalX, result.NormalY) * result.Depth;
                            }
                        }
                        else if (collision is Ozzyria.Game.Components.BoundingCircle && otherCollision is Ozzyria.Game.Components.BoundingBox)
                        {
                            var result = Ozzyria.Game.Components.Collision.CircleIntersectsBox((Ozzyria.Game.Components.BoundingCircle)collision, (Ozzyria.Game.Components.BoundingBox)otherCollision);
                            if (result.Collided)
                            {
                                depthVector += new Vector2(result.NormalX, result.NormalY) * result.Depth;
                            }
                        }
                        else if (collision is Ozzyria.Game.Components.BoundingBox && otherCollision is Ozzyria.Game.Components.BoundingCircle)
                        {
                            var result = Ozzyria.Game.Components.Collision.BoxIntersectsCircle((Ozzyria.Game.Components.BoundingBox)collision, (Ozzyria.Game.Components.BoundingCircle)otherCollision);
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
