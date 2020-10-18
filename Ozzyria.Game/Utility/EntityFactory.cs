using Ozzyria.Game.Component;

namespace Ozzyria.Game.Utility
{
    public class EntityFactory
    {

        public static Entity CreatePlayer(int id)
        {
            var player = new Entity { Id = id };
            player.AttachComponent(new Renderable { Sprite = SpriteType.Player });
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
            slime.AttachComponent(new Renderable { Sprite = SpriteType.Slime });
            slime.AttachComponent(new SlimeThought());
            slime.AttachComponent(new Movement { MAX_SPEED = 50f, ACCELERATION = 300f, X = x, Y = y });
            slime.AttachComponent(new Stats { Health = 30, MaxHealth = 30 });
            slime.AttachComponent(new Combat());
            slime.AttachComponent(new BoundingCircle { Radius = 10 });

            return slime;
        }

        public static Entity CreateExperienceOrb(float x, float y, int value)
        {
            var orb = new Entity();
            orb.AttachComponent(new Renderable { Sprite = SpriteType.Particle });
            orb.AttachComponent(new ExperienceOrbThought());
            orb.AttachComponent(new Movement { ACCELERATION = 200f, MAX_SPEED = 300f, X = x, Y = y });
            orb.AttachComponent(new ExperienceBoost { Experience = value });

            return orb;
        }

        public static Entity CreateBoxCollider(float x, float y, int w, int h)
        {
            var box = new Entity();
            box.AttachComponent(new Movement() { X = x, Y = y, PreviousX = x, PreviousY = y });
            box.AttachComponent(new BoundingBox() { IsDynamic = false, Width = w, Height = h });

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
