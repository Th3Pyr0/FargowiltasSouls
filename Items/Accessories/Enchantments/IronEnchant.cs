using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class IronEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");
        public int timer;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Iron Enchantment");

            string tooltip = "'Strike while the iron is hot'\n";

            if(thorium != null)
            {
                tooltip += "While in combat, you generate a 20 life shield\n";
            }

            tooltip += @"
Allows the player to dash into the enemy
Right Click to guard with your shield
You attract items from a larger range";

            Tooltip.SetDefault(tooltip); 
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 2;
            item.value = 40000;
            item.shieldSlot = 5;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            //EoC Shield
            player.dash = 2;
            player.GetModPlayer<FargoPlayer>(mod).IronEffect();

            if (!Fargowiltas.Instance.ThoriumLoaded) return;

            ThoriumPlayer thoriumPlayer = (ThoriumPlayer)player.GetModPlayer(thorium, "ThoriumPlayer");
            thoriumPlayer.metallurgyShield = true;
            if (!thoriumPlayer.outOfCombat)
            {
                timer++;
                if (timer >= 30)
                {
                    int num = 20;
                    if (thoriumPlayer.shieldHealth < num)
                    {
                        CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), new Color(51, 255, 255), 1, false, true);
                        thoriumPlayer.shieldHealth++;
                    }
                    timer = 0;
                    return;
                }
            }
            else
            {
                timer = 0;
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IronHelmet);
            recipe.AddIngredient(ItemID.IronChainmail);
            recipe.AddIngredient(ItemID.IronGreaves);

            if(Fargowiltas.Instance.ThoriumLoaded)
            {      
                recipe.AddIngredient(thorium.ItemType("IronShield"));
                recipe.AddIngredient(thorium.ItemType("ThoriumShield"));
                recipe.AddIngredient(ItemID.EoCShield);
                recipe.AddIngredient(ItemID.IronBroadsword);
                recipe.AddIngredient(thorium.ItemType("OpalStaff"));
                recipe.AddIngredient(ItemID.IronAnvil);
                recipe.AddIngredient(ItemID.ZebraSwallowtailButterfly);
            }
            else
            {
                recipe.AddIngredient(ItemID.EoCShield);
                recipe.AddIngredient(ItemID.IronBroadsword);
                recipe.AddIngredient(ItemID.IronAnvil);
            }

            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
