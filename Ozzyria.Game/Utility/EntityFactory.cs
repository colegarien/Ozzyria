using Ozzyria.Game.Components;
using Ozzyria.Game.ECS;

namespace Ozzyria.Game.Utility
{
    public class EntityFactory
    {

        public static void CreatePlayer(EntityContext context, int playerId)
        {
            var player = context.CreateEntity();

            var renderable = (Renderable)player.CreateComponent(typeof(Renderable));
            renderable.IsDynamic = true;
            renderable.Z = (int)ZLayer.Middleground;

            var playerTag = (Components.Player)player.CreateComponent(typeof(Components.Player));
            playerTag.PlayerId = playerId;

            var playerLocation = (Components.Location)player.CreateComponent(typeof(Components.Location));
            playerLocation.Area = "test_a"; // TODO OZ-28 pull this from a storage or some kind?

            var thought = (PlayerThought)player.CreateComponent(typeof(PlayerThought));

            var movement = (Movement)player.CreateComponent(typeof(Movement));
            movement.X = 140;
            movement.Y = 140;

            var stats = (Stats)player.CreateComponent(typeof(Stats));
            var combat = (Components.Combat)player.CreateComponent(typeof(Components.Combat));

            var equippedGear = (EquippedGear)player.CreateComponent(typeof(EquippedGear));
            equippedGear.Hat = "green_hat";
            equippedGear.Body = "body_white";
            equippedGear.Armor = "biker_jacket";
            equippedGear.Mask = "shades";
            equippedGear.Weapon = "gladius";
            equippedGear.WeaponEffect = "basic_slash";

            var collision = (BoundingCircle)player.CreateComponent(typeof(BoundingCircle));
            collision.Radius = 10;

            var animationState = player.CreateComponent(typeof(AnimationState));

            player.AddComponent(animationState);
            player.AddComponent(equippedGear);
            player.AddComponent(renderable);
            player.AddComponent(playerTag);
            player.AddComponent(playerLocation);
            player.AddComponent(thought);
            player.AddComponent(movement);
            player.AddComponent(stats);
            player.AddComponent(combat);
            player.AddComponent(collision);
        }

        public static void CreateSlime(EntityContext context, float x, float y)
        {
            var slime = context.CreateEntity();

            var renderable = (Renderable)slime.CreateComponent(typeof(Renderable));
            renderable.IsDynamic = true;
            renderable.Z = (int)ZLayer.Middleground;

            var thought = (SlimeThought)slime.CreateComponent(typeof(SlimeThought));

            var movement = (Movement)slime.CreateComponent(typeof(Movement));
            movement.MAX_SPEED = 50f;
            movement.ACCELERATION = 300f;
            movement.X = x;
            movement.Y = y;

            var stats = (Stats)slime.CreateComponent(typeof(Stats));
            stats.Health = 30;
            stats.MaxHealth = 30;

            var combat = (Components.Combat)slime.CreateComponent(typeof(Components.Combat));

            var equippedGear = (EquippedGear)slime.CreateComponent(typeof(EquippedGear));
            equippedGear.Body = "slime";

            var collision = (BoundingCircle)slime.CreateComponent(typeof(BoundingCircle));
            collision.Radius = 10;

            var animationState = slime.CreateComponent(typeof(AnimationState));

            slime.AddComponent(animationState);
            slime.AddComponent(equippedGear);
            slime.AddComponent(renderable);
            slime.AddComponent(thought);
            slime.AddComponent(movement);
            slime.AddComponent(stats);
            slime.AddComponent(combat);
            slime.AddComponent(collision);

        }

        public static void CreateSlimeSpawner(EntityContext context, float x, float y)
        {
            var spawner = context.CreateEntity();

            var component = (SlimeSpawner)spawner.CreateComponent(typeof(SlimeSpawner));
            component.X = x;
            component.Y = y;
            component.ThinkDelay = new Delay { DelayInSeconds = 5f };

            spawner.AddComponent(component);
        }

        public static void CreateExperienceOrb(EntityContext context, float x, float y, int value)
        {
            var orb = context.CreateEntity();

            var renderable = (Renderable)orb.CreateComponent(typeof(Renderable));
            renderable.IsDynamic = false;
            renderable.StaticClip = "static_exp_orb";
            renderable.Z = (int)ZLayer.Items;

            var thought = (ExperienceOrbThought)orb.CreateComponent(typeof(ExperienceOrbThought));

            var movement = (Movement)orb.CreateComponent(typeof(Movement));
            movement.ACCELERATION = 200f;
            movement.MAX_SPEED = 300f;
            movement.X = x;
            movement.Y = y;

            var boost = (ExperienceBoost)orb.CreateComponent(typeof(ExperienceBoost));
            boost.Experience = value;

            orb.AddComponent(renderable);
            orb.AddComponent(thought);
            orb.AddComponent(movement);
            orb.AddComponent(boost);
        }

        public static void CreateBoxColliderArea(EntityContext context, float left, float top, float right, float bottom)
        {
            var width = (int)(right - left);
            var height = (int)(bottom - top);

            var centerX = left + (width / 2f);
            var centerY = top + (height / 2f);

            var box = context.CreateEntity();

            var movement = (Movement)box.CreateComponent<Movement>();
            movement.X = centerX;
            movement.Y = centerY;
            movement.PreviousX = centerX;
            movement.PreviousY = centerY;


            var bounds = (BoundingBox)box.CreateComponent<BoundingBox>();
            bounds.IsDynamic = false;
            bounds.Width = width;
            bounds.Height = height;

            box.AddComponent(movement);
            box.AddComponent(bounds);
        }

        public static void CreateCircleCollider(EntityContext context, float x, float y, float radius)
        {
            var circle = context.CreateEntity();

            var movement = (Movement)circle.CreateComponent<Movement>();
            movement.X = x;
            movement.Y = y;
            movement.PreviousX = x;
            movement.PreviousY = y;


            var bounds = (BoundingCircle)circle.CreateComponent<BoundingCircle>();
            bounds.IsDynamic = false;
            bounds.Radius = radius;

            circle.AddComponent(movement);
            circle.AddComponent(bounds);

        }

    }
}
