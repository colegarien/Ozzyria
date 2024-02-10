using Microsoft.Xna.Framework;
using Ozzyria.Game.Components;
using Ozzyria.Game.ECS;

namespace Ozzyria.MonoGameClient.Rendering.EntityRenderer
{
    internal class BodyRenderer : EntityRenderPipeline
    {
        protected override bool CanRender(Entity entity)
        {
            return entity.HasComponent(typeof(Body));
        }

        protected override void DoRender(GraphicsPipeline graphicsPipeline, Entity entity, Skeleton skeleton)
        {
            var body = (Body)entity.GetComponent(typeof(Body));
            switch (body.BodyType)
            {
                case BodyType.Human:
                    var drawable = Drawable.PLAYER_RIGHT;
                    if (skeleton.Pose == SkeletonPose.Attack && skeleton.Frame == 1)
                    {
                        if (skeleton.Direction == Direction.Right)
                        {
                            drawable = Drawable.PLAYER_RIGHT_HIGH;
                        }
                        else if (skeleton.Direction == Direction.Left)
                        {
                            drawable = Drawable.PLAYER_LEFT_HIGH;
                        }
                        else if (skeleton.Direction == Direction.Up)
                        {
                            drawable = Drawable.PLAYER_UP_HIGH;
                        }
                        else if (skeleton.Direction == Direction.Down)
                        {
                            drawable = Drawable.PLAYER_DOWN_HIGH;
                        }
                    }
                    else
                    {
                        if (skeleton.Direction == Direction.Right)
                        {
                            drawable = Drawable.PLAYER_RIGHT;
                        }
                        else if (skeleton.Direction == Direction.Left)
                        {
                            drawable = Drawable.PLAYER_LEFT;
                        }
                        else if (skeleton.Direction == Direction.Up)
                        {
                            drawable = Drawable.PLAYER_UP;
                        }
                        else if (skeleton.Direction == Direction.Down)
                        {
                            if (skeleton.Frame == 0)
                            {
                                drawable = Drawable.PLAYER_DOWN_HIGH;
                            }
                            else if (skeleton.Frame == 1)
                            {
                                drawable = Drawable.PLAYER_DOWN;
                            }
                            else
                            {
                                drawable = Drawable.PLAYER_DOWN_LOW;
                            }
                        }
                    }
                    PushDrawable(graphicsPipeline, entity, skeleton, drawable, Color.White);
                    break;
                case BodyType.Slime:
                    PushDrawable(graphicsPipeline, entity, skeleton, Drawable.SLIME_LEFT, Color.White);
                    break;
            }
        }
    }
}
