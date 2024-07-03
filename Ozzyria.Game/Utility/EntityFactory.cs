using Ozzyria.Game.Components;
using Grecs;

namespace Ozzyria.Game.Utility
{
    public class EntityFactory
    {

        public static void CreatePlayer(EntityContext context, int playerId)
        {
            var player = context.CreateEntity();

            var playerTag = (Components.Player)player.CreateComponent(typeof(Components.Player));
            playerTag.PlayerId = playerId;

            var playerLocation = (Components.Location)player.CreateComponent(typeof(Components.Location));
            playerLocation.Area = "test_a"; // TODO OZ-28 pull this from a storage or some kind?

            var thought = (PlayerThought)player.CreateComponent(typeof(PlayerThought));

            var movement = (Movement)player.CreateComponent(typeof(Movement));
            movement.X = 140;
            movement.Y = 140;
            movement.CollisionOffsetY = -8;
            movement.PreviousX = 140;
            movement.PreviousY = 140;

            var stats = (Stats)player.CreateComponent(typeof(Stats));

            var collision = (BoundingCircle)player.CreateComponent(typeof(BoundingCircle));
            collision.Radius = 10;

            var bag = (Bag)player.CreateComponent(typeof(Bag));

            var greenHat = new Entity();
            var hatItem = (Item)greenHat.CreateComponent(typeof(Item));
            hatItem.Name = "Green Hat";
            hatItem.Icon = "green_hat_icon";
            hatItem.ItemId = "green_hat";
            hatItem.EquipmentSlot = "hat"; // TODO OZ-55 update equipment system to more easily tie items and equipment together
            hatItem.IsEquipped = true;
            greenHat.AddComponent(hatItem);
            bag.AddItem(greenHat);

            var bikerJacket = new Entity();
            var bikerJacketItem = (Item)bikerJacket.CreateComponent(typeof(Item));
            bikerJacketItem.Name = "Biker Jacket";
            bikerJacketItem.Icon = "biker_jacket_icon";
            bikerJacketItem.ItemId = "biker_jacket";
            bikerJacketItem.EquipmentSlot = "armor"; // TODO OZ-55 update equipment system to more easily tie items and equipment together
            bikerJacketItem.IsEquipped = true;
            bikerJacket.AddComponent(bikerJacketItem);
            bag.AddItem(bikerJacket);

            var shades = new Entity();
            var shadesItem = (Item)shades.CreateComponent(typeof(Item));
            shadesItem.Name = "Biker Shades";
            shadesItem.Icon = "shades_icon";
            shadesItem.ItemId = "shades";
            shadesItem.EquipmentSlot = "mask"; // TODO OZ-55 update equipment system to more easily tie items and equipment together
            shadesItem.IsEquipped = true;
            shades.AddComponent(shadesItem);
            bag.AddItem(shades);

            var gladius = new Entity();
            var gladiusItem = (Item)gladius.CreateComponent(typeof(Item));
            gladiusItem.Name = "Gladius";
            gladiusItem.Icon = "gladius_icon";
            gladiusItem.ItemId = "gladius";
            gladiusItem.EquipmentSlot = "weapon"; // TODO OZ-55 update equipment system to more easily tie items and equipment together
            gladiusItem.IsEquipped = true;
            gladius.AddComponent(gladiusItem);
            bag.AddItem(gladius);

            // unequipped stuff
            var cowboyHat = new Entity();
            var cowboyHatItem = (Item)cowboyHat.CreateComponent(typeof(Item));
            cowboyHatItem.Name = "Cowboy Hat";
            cowboyHatItem.Icon = "cowboy_hat_icon";
            cowboyHatItem.ItemId = "cowboy_hat";
            cowboyHatItem.EquipmentSlot = "hat"; // TODO OZ-55 update equipment system to more easily tie items and equipment together
            cowboyHatItem.IsEquipped = false;
            cowboyHat.AddComponent(cowboyHatItem);
            bag.AddItem(cowboyHat);

            var cyanArmor = new Entity();
            var cyanArmorItem = (Item)cyanArmor.CreateComponent(typeof(Item));
            cyanArmorItem.Name = "Cyan Armor";
            cyanArmorItem.Icon = "cyan_armor_icon";
            cyanArmorItem.ItemId = "cyan_armor";
            cyanArmorItem.EquipmentSlot = "armor"; // TODO OZ-55 update equipment system to more easily tie items and equipment together
            cyanArmorItem.IsEquipped = false;
            cyanArmor.AddComponent(cyanArmorItem);
            bag.AddItem(cyanArmor);

            var pinkSword = new Entity();
            var pinkSwordItem = (Item)pinkSword.CreateComponent(typeof(Item));
            pinkSwordItem.Name = "Pink Sword";
            pinkSwordItem.Icon = "pink_sword_icon";
            pinkSwordItem.ItemId = "pink_sword";
            pinkSwordItem.EquipmentSlot = "weapon"; // TODO OZ-55 update equipment system to more easily tie items and equipment together
            pinkSwordItem.IsEquipped = false;
            pinkSword.AddComponent(pinkSwordItem);
            bag.AddItem(pinkSword);

            var greySword = new Entity();
            var greySwordItem = (Item)greySword.CreateComponent(typeof(Item));
            greySwordItem.Name = "Grey Sword";
            greySwordItem.Icon = "grey_sword_icon";
            greySwordItem.ItemId = "grey_sword";
            greySwordItem.EquipmentSlot = "weapon"; // TODO OZ-55 update equipment system to more easily tie items and equipment together
            greySwordItem.IsEquipped = false;
            greySword.AddComponent(greySwordItem);
            bag.AddItem(greySword);

            // Skeleton and Initial Equipment Setup
            player.AddComponent(new Animator{NumberOfFrames = 3});
            player.AddComponent(new Skeleton{ Type= SkeletonType.Humanoid });
            player.AddComponent(new Body { BodyId = "Human" });
            player.AddComponent(new Weapon { WeaponType = WeaponType.Sword, WeaponId="gladius" });
            player.AddComponent(new Hat { HatId = "green_hat" });
            player.AddComponent(new Mask { MaskId = "shades" });
            player.AddComponent(new Armor { ArmorId = "biker_jacket" });

            player.AddComponent(playerTag);
            player.AddComponent(playerLocation);
            player.AddComponent(thought);
            player.AddComponent(movement);
            player.AddComponent(stats);
            player.AddComponent(collision);
            player.AddComponent(bag);
        }

        public static void CreateSlime(EntityContext context, float x, float y)
        {
            var slime = context.CreateEntity();

            slime.AddComponent(new Animator { NumberOfFrames = 3 });
            slime.AddComponent(new Skeleton { Type = SkeletonType.Slime });
            slime.AddComponent(new Body { BodyId = "Slime" });
            slime.AddComponent(new Weapon { WeaponType = WeaponType.Empty, WeaponId = "" });

            var thought = (SlimeThought)slime.CreateComponent(typeof(SlimeThought));

            var movement = (Movement)slime.CreateComponent(typeof(Movement));
            movement.MAX_SPEED = 50f;
            movement.ACCELERATION = 300f;
            movement.X = x;
            movement.Y = y;
            movement.CollisionOffsetY = -8;
            movement.PreviousX = x;
            movement.PreviousY = y;

            var stats = (Stats)slime.CreateComponent(typeof(Stats));
            stats.Health = 30;
            stats.MaxHealth = 30;

            var collision = (BoundingCircle)slime.CreateComponent(typeof(BoundingCircle));
            collision.Radius = 10;

            slime.AddComponent(thought);
            slime.AddComponent(movement);
            slime.AddComponent(stats);
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

        public static void CreateDoor(EntityContext context, float x, float y, string newArea, float newX, float newY) {
            var door = context.CreateEntity();

            var doorComponent = (Door)door.CreateComponent(typeof(Door));
            doorComponent.NewArea = newArea;
            doorComponent.NewX = newX;
            doorComponent.NewY = newY;
            
            var movement = (Movement)door.CreateComponent<Movement>();
            movement.X = x;
            movement.Y = y;
            movement.PreviousX = x;
            movement.PreviousY = y;

            door.AddComponent(new Animator { Type = ClipType.Stall });
            door.AddComponent(new Skeleton { Type = SkeletonType.Static});
            door.AddComponent(new Body { BodyId = "simple_door" });
            door.AddComponent(doorComponent);
            door.AddComponent(movement);
        }

        public static void CreateExperienceOrb(EntityContext context, float x, float y, int value)
        {
            var orb = context.CreateEntity();

            var thought = (ExperienceOrbThought)orb.CreateComponent(typeof(ExperienceOrbThought));

            var movement = (Movement)orb.CreateComponent(typeof(Movement));
            movement.ACCELERATION = 200f;
            movement.MAX_SPEED = 300f;
            movement.X = x;
            movement.Y = y;
            movement.PreviousX = x;
            movement.PreviousY = y;

            var boost = (ExperienceBoost)orb.CreateComponent(typeof(ExperienceBoost));
            boost.Experience = value;

            orb.AddComponent(new Skeleton { Type = SkeletonType.Static });
            orb.AddComponent(new Animator { Type = ClipType.Stall });
            orb.AddComponent(new Body { BodyId = "exp_orb" });
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
