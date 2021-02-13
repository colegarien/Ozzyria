using Ozzyria.Game.Component;

namespace Ozzyria.Game.Utility
{
    public class EntityFactory
    {

        public static Entity CreatePlayer(int id)
        {
            var player = new Entity { Id = id };
            player.AttachComponent(new Renderable { Sprite = SpriteType.Player, Z = Renderable.Z_MIDDLEGROUND });
            player.AttachComponent(new PlayerThought());
            player.AttachComponent(new Movement() { X = 140, Y = 140 });
            player.AttachComponent(new Stats());
            player.AttachComponent(new Combat());
            player.AttachComponent(new Input());
            player.AttachComponent(new BoundingCircle { Radius = 10 });

            return player;
        }

        public static Entity CreateSlime(float x, float y)
        {
            var slime = new Entity();
            slime.AttachComponent(new Renderable { Sprite = SpriteType.Slime, Z = Renderable.Z_MIDDLEGROUND });
            slime.AttachComponent(new SlimeThought());
            slime.AttachComponent(new Movement { MAX_SPEED = 50f, ACCELERATION = 300f, X = x, Y = y });
            slime.AttachComponent(new Stats { Health = 30, MaxHealth = 30 });
            slime.AttachComponent(new Combat());
            slime.AttachComponent(new BoundingCircle { Radius = 10 });

            return slime;
        }

        public static Entity CreateSlimeSpawner(float x, float y)
        {
            var spawner = new Entity();
            spawner.AttachComponent(new SlimeSpawner
            {
                X = x,
                Y = y,
                ThinkDelay = new Delay
                {
                    DelayInSeconds = 5f
                }
            });

            return spawner;
        }

        public static Entity CreateExperienceOrb(float x, float y, int value)
        {
            var orb = new Entity();
            orb.AttachComponent(new Renderable { Sprite = SpriteType.Particle, Z = Renderable.Z_ITEMS });
            orb.AttachComponent(new ExperienceOrbThought());
            orb.AttachComponent(new Movement { ACCELERATION = 200f, MAX_SPEED = 300f, X = x, Y = y });
            orb.AttachComponent(new ExperienceBoost { Experience = value });

            return orb;
        }

        public static Entity CreateBoxColliderArea(float left, float top, float right, float bottom)
        {
            var width = (int)(right - left);
            var height = (int)(bottom - top);

            var centerX = left + (width / 2f);
            var centerY = top + (height / 2f);

            var box = new Entity();
            box.AttachComponent(new Movement() { X = centerX, Y = centerY, PreviousX = centerX, PreviousY = centerY });
            box.AttachComponent(new BoundingBox() { IsDynamic = false, Width = width, Height = height });

            return box;
        }

        public static Entity CreateCircleCollider(float x, float y, float radius)
        {
            var circle = new Entity();
            circle.AttachComponent(new Movement() { X = x, Y = y, PreviousX = x, PreviousY = y });
            circle.AttachComponent(new BoundingCircle() { IsDynamic = false, Radius = radius });

            return circle;
        }

    }
}
