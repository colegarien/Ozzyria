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
                                animator.Type = ClipType.Loop;
                                animator.Frame = 0;
                                animator.NumberOfFrames = 3;
                                break;
                            case ClipType.Loop:
                                animator.Frame = 0;
                                break;
                        }
                    }
                }

                if (entity.HasComponent(typeof(AttackIntent))) {
                    // fall in and out of combat animation
                    var intent = entity.GetComponent<AttackIntent>();
                    if (animator.CurrentPose != SkeletonPose.Attack)
                    {
                        animator.CurrentPose = SkeletonPose.Attack;
                        animator.Type = ClipType.Decay;
                        animator.Frame = intent.Frame;
                        animator.NumberOfFrames = intent.DecayFrame;
                    }
                }
                else if (animator.CurrentPose != SkeletonPose.Walk && entity.HasComponent(typeof(Movement)) && entity.GetComponent<Movement>().IsMoving())
                {
                    animator.CurrentPose = SkeletonPose.Walk;
                    animator.Type = ClipType.Decay;
                    animator.Frame = 0;
                    animator.NumberOfFrames = 3;
                }
                else if(animator.CurrentPose != SkeletonPose.Idle && !(entity.HasComponent(typeof(Movement)) && entity.GetComponent<Movement>().IsMoving()))
                {
                    animator.CurrentPose = SkeletonPose.Idle;
                    animator.Type = ClipType.Loop;
                    animator.Frame = 0;
                    animator.NumberOfFrames = 3;
                }
            }
        }
    }
}
