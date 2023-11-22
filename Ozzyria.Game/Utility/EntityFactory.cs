using Ozzyria.Game.Components;
using Ozzyria.Game.ECS;
using Ozzyria.Game.Systems;
using System.ComponentModel;
using System.Drawing;

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
            movement.PreviousX = 140;
            movement.PreviousY = 140;

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

            var bag = (Bag)player.CreateComponent(typeof(Bag));

            var greenHat = new Entity();
            var hatItem = (Item)greenHat.CreateComponent(typeof(Item));
            hatItem.Name = "Green Hat";
            hatItem.Icon = "green_hat_icon";
            hatItem.ItemId = "green_hat";
            hatItem.EquipmentSlot = "hat"; // TODO UI gross
            hatItem.IsEquipped = true;
            greenHat.AddComponent(hatItem);
            bag.AddItem(greenHat);

            var bikerJacket = new Entity();
            var bikerJacketItem = (Item)bikerJacket.CreateComponent(typeof(Item));
            bikerJacketItem.Name = "Biker Jacket";
            bikerJacketItem.Icon = "biker_jacket_icon";
            bikerJacketItem.ItemId = "biker_jacket";
            bikerJacketItem.EquipmentSlot = "armor"; // TODO UI gross
            bikerJacketItem.IsEquipped = true;
            bikerJacket.AddComponent(bikerJacketItem);
            bag.AddItem(bikerJacket);

            var shades = new Entity();
            var shadesItem = (Item)shades.CreateComponent(typeof(Item));
            shadesItem.Name = "Biker Shades";
            shadesItem.Icon = "shades_icon";
            shadesItem.ItemId = "shades";
            shadesItem.EquipmentSlot = "mask"; // TODO UI gross
            shadesItem.IsEquipped = true;
            shades.AddComponent(shadesItem);
            bag.AddItem(shades);

            var gladius = new Entity();
            var gladiusItem = (Item)gladius.CreateComponent(typeof(Item));
            gladiusItem.Name = "Gladius";
            gladiusItem.Icon = "gladius_icon";
            gladiusItem.ItemId = "gladius";
            gladiusItem.EquipmentSlot = "weapon"; // TODO UI gross
            gladiusItem.IsEquipped = true;
            gladius.AddComponent(gladiusItem);
            bag.AddItem(gladius);

            // unequipped stuff
            var cowboyHat = new Entity();
            var cowboyHatItem = (Item)cowboyHat.CreateComponent(typeof(Item));
            cowboyHatItem.Name = "Cowboy Hat";
            cowboyHatItem.Icon = "cowboy_hat_icon";
            cowboyHatItem.ItemId = "cowboy_hat";
            cowboyHatItem.EquipmentSlot = "hat"; // TODO UI gross
            cowboyHatItem.IsEquipped = false;
            cowboyHat.AddComponent(cowboyHatItem);
            bag.AddItem(cowboyHat);

            var cyanArmor = new Entity();
            var cyanArmorItem = (Item)cyanArmor.CreateComponent(typeof(Item));
            cyanArmorItem.Name = "Cyan Armor";
            cyanArmorItem.Icon = "cyan_armor_icon";
            cyanArmorItem.ItemId = "cyan_armor";
            cyanArmorItem.EquipmentSlot = "armor"; // TODO UI gross
            cyanArmorItem.IsEquipped = false;
            cyanArmor.AddComponent(cyanArmorItem);
            bag.AddItem(cyanArmor);

            var pinkSword = new Entity();
            var pinkSwordItem = (Item)pinkSword.CreateComponent(typeof(Item));
            pinkSwordItem.Name = "Pink Sword";
            pinkSwordItem.Icon = "pink_sword_icon";
            pinkSwordItem.ItemId = "pink_sword";
            pinkSwordItem.EquipmentSlot = "weapon"; // TODO UI gross
            pinkSwordItem.IsEquipped = false;
            pinkSword.AddComponent(pinkSwordItem);
            bag.AddItem(pinkSword);

            var greySword = new Entity();
            var greySwordItem = (Item)greySword.CreateComponent(typeof(Item));
            greySwordItem.Name = "Grey Sword";
            greySwordItem.Icon = "grey_sword_icon";
            greySwordItem.ItemId = "grey_sword";
            greySwordItem.EquipmentSlot = "weapon"; // TODO UI gross
            greySwordItem.IsEquipped = false;
            greySword.AddComponent(greySwordItem);
            bag.AddItem(greySword);

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
            player.AddComponent(bag);
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
            movement.PreviousX = x;
            movement.PreviousY = y;

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

            // TODO OZ-22 make doors have their own graphic
            var renderable = (Renderable)door.CreateComponent(typeof(Renderable));
            renderable.IsDynamic = false;
            renderable.StaticClip = "static_door";
            renderable.Z = (int)ZLayer.Items;

            door.AddComponent(doorComponent);
            door.AddComponent(movement);
            door.AddComponent(renderable);
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
            movement.PreviousX = x;
            movement.PreviousY = y;

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
