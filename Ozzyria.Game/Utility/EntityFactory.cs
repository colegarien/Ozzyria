using Ozzyria.Model.Components;
using Grecs;
using Ozzyria.Model.Types;
using Ozzyria.Game.Storage;
using Ozzyria.Content;

namespace Ozzyria.Game.Utility
{
    public class EntityFactory
    {

        public static void CreatePlayer(EntityContext context, int playerId, string areaId, ContainerStorage containerStorage)
        {
            var prefabDefinition = Packages.GetInstance().PrefabPackage.GetDefinition("player");
            var playerEntity = Model.Utility.PrefabHydrator.HydrateDefinitionAtLocation(context, prefabDefinition, 140, 140, 1, new ValuePacket
            {
                { "player::playerId", playerId.ToString() },
                { "location::area", areaId },

            });

            var bag = (Bag)playerEntity.GetComponent(typeof(Bag));

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
        }

        public static void CreateWall(EntityContext context, float x, float y, int layer, int width, int height)
        {
            var prefabDefinition = Packages.GetInstance().PrefabPackage.GetDefinition("wall");
            Model.Utility.PrefabHydrator.HydrateDefinitionAtLocation(context, prefabDefinition, x, y, layer, new ValuePacket
            {
                { "movement::collisionShape::boundingBox::width", width.ToString() },
                { "movement::collisionShape::boundingBox::height", height.ToString() },
            });
        }

        public static void CreateExperienceOrb(EntityContext context, float x, float y, int layer, int value)
        {
            var prefabDefinition = Packages.GetInstance().PrefabPackage.GetDefinition("exp_orb");
            Model.Utility.PrefabHydrator.HydrateDefinitionAtLocation(context, prefabDefinition, x, y, layer, new ValuePacket
            {
                { "exp_boost::experience", value.ToString() },
            });
        }

    }
}
