using Ozzyria.Model.Components;
using Grecs;
using Ozzyria.Model.Types;
using Ozzyria.Game.Storage;

namespace Ozzyria.Game.Utility
{
    public class EntityFactory
    {

        public static void CreatePlayer(EntityContext context, int playerId, string areaId, ContainerStorage containerStorage)
        {
            var player = context.CreateEntity();

            var playerTag = (Player)player.CreateComponent(typeof(Player));
            playerTag.PlayerId = playerId;

            var playerLocation = (Location)player.CreateComponent(typeof(Location));
            playerLocation.Area = areaId;

            var thought = (PlayerThought)player.CreateComponent(typeof(PlayerThought));

            var movement = (Movement)player.CreateComponent(typeof(Movement));
            movement.X = 140;
            movement.Y = 140;
            movement.CollisionOffsetY = -8;
            movement.PreviousX = 140;
            movement.PreviousY = 140;
            movement.CollisionShape = new CollisionShape
            {
                BoundingCircle = new BoundingCircle
                {
                    Radius = 10
                }
            };

            var stats = (Stats)player.CreateComponent(typeof(Stats));

            var collision = (Collision)player.CreateComponent(typeof(Collision));
            collision.IsDynamic = true;

            var bag = (Bag)player.CreateComponent(typeof(Bag));

            var greenHat = new Entity();
            var hatItem = (Item)greenHat.CreateComponent(typeof(Item));
            hatItem.Name = "Green Hat";
            hatItem.Icon = "green_hat_icon";
            hatItem.ItemId = "green_hat";
            hatItem.EquipmentSlot = "hat"; // TODO OZ-55 update equipment system to more easily tie items and equipment together
            hatItem.IsEquipped = true;
            greenHat.AddComponent(hatItem);
            containerStorage.AddItemToBag(bag, greenHat);

            var bikerJacket = new Entity();
            var bikerJacketItem = (Item)bikerJacket.CreateComponent(typeof(Item));
            bikerJacketItem.Name = "Biker Jacket";
            bikerJacketItem.Icon = "biker_jacket_icon";
            bikerJacketItem.ItemId = "biker_jacket";
            bikerJacketItem.EquipmentSlot = "armor"; // TODO OZ-55 update equipment system to more easily tie items and equipment together
            bikerJacketItem.IsEquipped = true;
            bikerJacket.AddComponent(bikerJacketItem);
            containerStorage.AddItemToBag(bag, bikerJacket);

            var shades = new Entity();
            var shadesItem = (Item)shades.CreateComponent(typeof(Item));
            shadesItem.Name = "Biker Shades";
            shadesItem.Icon = "shades_icon";
            shadesItem.ItemId = "shades";
            shadesItem.EquipmentSlot = "mask"; // TODO OZ-55 update equipment system to more easily tie items and equipment together
            shadesItem.IsEquipped = true;
            shades.AddComponent(shadesItem);
            containerStorage.AddItemToBag(bag, shades);

            var gladius = new Entity();
            var gladiusItem = (Item)gladius.CreateComponent(typeof(Item));
            gladiusItem.Name = "Gladius";
            gladiusItem.Icon = "gladius_icon";
            gladiusItem.ItemId = "gladius";
            gladiusItem.EquipmentSlot = "weapon"; // TODO OZ-55 update equipment system to more easily tie items and equipment together
            gladiusItem.IsEquipped = true;
            gladius.AddComponent(gladiusItem);
            containerStorage.AddItemToBag(bag, gladius);

            // unequipped stuff
            var cowboyHat = new Entity();
            var cowboyHatItem = (Item)cowboyHat.CreateComponent(typeof(Item));
            cowboyHatItem.Name = "Cowboy Hat";
            cowboyHatItem.Icon = "cowboy_hat_icon";
            cowboyHatItem.ItemId = "cowboy_hat";
            cowboyHatItem.EquipmentSlot = "hat"; // TODO OZ-55 update equipment system to more easily tie items and equipment together
            cowboyHatItem.IsEquipped = false;
            cowboyHat.AddComponent(cowboyHatItem);
            containerStorage.AddItemToBag(bag, cowboyHat);

            var cyanArmor = new Entity();
            var cyanArmorItem = (Item)cyanArmor.CreateComponent(typeof(Item));
            cyanArmorItem.Name = "Cyan Armor";
            cyanArmorItem.Icon = "cyan_armor_icon";
            cyanArmorItem.ItemId = "cyan_armor";
            cyanArmorItem.EquipmentSlot = "armor"; // TODO OZ-55 update equipment system to more easily tie items and equipment together
            cyanArmorItem.IsEquipped = false;
            cyanArmor.AddComponent(cyanArmorItem);
            containerStorage.AddItemToBag(bag, cyanArmor);

            var pinkSword = new Entity();
            var pinkSwordItem = (Item)pinkSword.CreateComponent(typeof(Item));
            pinkSwordItem.Name = "Pink Sword";
            pinkSwordItem.Icon = "pink_sword_icon";
            pinkSwordItem.ItemId = "pink_sword";
            pinkSwordItem.EquipmentSlot = "weapon"; // TODO OZ-55 update equipment system to more easily tie items and equipment together
            pinkSwordItem.IsEquipped = false;
            pinkSword.AddComponent(pinkSwordItem);
            containerStorage.AddItemToBag(bag, pinkSword);

            var greySword = new Entity();
            var greySwordItem = (Item)greySword.CreateComponent(typeof(Item));
            greySwordItem.Name = "Grey Sword";
            greySwordItem.Icon = "grey_sword_icon";
            greySwordItem.ItemId = "grey_sword";
            greySwordItem.EquipmentSlot = "weapon"; // TODO OZ-55 update equipment system to more easily tie items and equipment together
            greySwordItem.IsEquipped = false;
            greySword.AddComponent(greySwordItem);
            containerStorage.AddItemToBag(bag, greySword);

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

    }
}
