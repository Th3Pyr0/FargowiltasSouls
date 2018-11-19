using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using ThoriumMod;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace FargowiltasSouls.Items.Accessories.Enchantments.Thorium
{
    public class DreamWeaverEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");
        
        public override bool Autoload(ref string name)
        {
            return ModLoader.GetLoadedMods().Contains("ThoriumMod");
        }
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dream Weaver Enchantment");
            Tooltip.SetDefault(
@"'Manifest your dearest dreams through your allies, Bind the enemies of your future in temporal agony'
Pressing the 'Special Ability' key will spend 200 mana and place you within the Dream
While in the Dream, healed allies will become briefly invulnerable and be cured of all debuffs
Pressing the 'Special Ability' key will spend 200 mana and bend the very fabric of reality
Enemies will be heavily slowed and take 15% more damage from all sources
Allies will receive greatly increased movement and attack speed
Summons a cute lil' maid to clean");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 10;
            item.value = 400000;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color?(new Color(255, 128, 0));
                }
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!Fargowiltas.Instance.ThoriumLoaded) return;
            
            DreamEffect(player);
        }
        
        private void DreamEffect(Player player)
        {
            ThoriumPlayer thoriumPlayer = player.GetModPlayer<ThoriumPlayer>(thorium);
            //all allies invuln
            thoriumPlayer.dreamHoodSet = true;
            //enemies slowed and take more dmg
            thoriumPlayer.dreamSet = true;
            //maid pet
            thoriumPlayer.maidPet = true;
        }
        
        private readonly string[] items =
        {
            "DreamWeaversHelmet",
            "DreamWeaversHood",
            "DreamWeaversTabard",
            "DreamWeaversTreads",
            "DragonHeartWand",
            "SnackLantern",
            "ChristmasCheer",
            "MoleculeStabilizer",
            "DreamCatcher",
            "SimpleBroom"
        };

        public override void AddRecipes()
        {
            if (!Fargowiltas.Instance.ThoriumLoaded) return;
            
            ModRecipe recipe = new ModRecipe(mod);
            
            foreach (string i in items) recipe.AddIngredient(thorium.ItemType(i));

            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
