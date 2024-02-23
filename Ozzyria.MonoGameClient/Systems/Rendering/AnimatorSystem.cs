using Ozzyria.Game.Components;
using Grecs;

namespace Ozzyria.MonoGameClient.Systems.Rendering
{
    internal class AnimatorSystem : TickSystem
    {
        EntityQuery _query;
        public AnimatorSystem()
        {
            _query = new EntityQuery().And(typeof(Animator));
        }

        public override void Execute(float deltaTime, EntityContext context)
        {
            var animators = context.GetEntities(_query);
            foreach (var entity in animators)
            {
                var animator = (Animator)entity.GetComponent(typeof(Animator));
                animator.FrameTimer += deltaTime;
                if (animator.FrameTimer >= animator.TimePerFrame)
                {
                    animator.FrameTimer -= animator.TimePerFrame;
                    animator.Frame++;
                    if (animator.Frame >= animator.NumberOfFrames)
                    {
                        switch (animator.Type)
                        {
                            case ClipType.Stall:
                                animator.Frame = animator.NumberOfFrames - 1;
                                break;
                            case ClipType.Decay:
                                animator.CurrentPose = SkeletonPose.Idle;
                                animator.Frame = 0;
                                animator.NumberOfFrames = 3;
                                break;
                            case ClipType.Loop:
                                animator.Frame = 0;
                                break;
                        }
                    }
                }

                if (entity.HasComponent(typeof(Combat))) {
                    // fall in and out of combat animation
                    var combat = (Combat)entity.GetComponent(typeof(Combat));
                    if (combat.StartedAttack && animator.CurrentPose != SkeletonPose.Attack)
                    {
                        animator.CurrentPose = SkeletonPose.Attack;
                        animator.Type = ClipType.Decay;
                        animator.Frame = 0;
                        animator.FrameTimer = 0;
                        animator.NumberOfFrames = 3;
                    }
                }
            }
        }
    }
}
