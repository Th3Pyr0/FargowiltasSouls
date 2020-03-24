using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using Terraria.Localization;
using ThoriumMod.Items.HealerItems;
using ThoriumMod.Items.Painting;

namespace FargowiltasSouls.Items.Accessories.Enchantments.Thorium
{
    public class EbonEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override bool Autoload(ref string name)
        {
            return ModLoader.GetMod("ThoriumMod") != null;
        }
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ebon Enchantment");
            Tooltip.SetDefault(
@"'Great for brooding'
Corrupts your radiant powers, causing them to take on dark forms and deal additional effects");
            DisplayName.AddTranslation(GameCulture.Chinese, "黑檀魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'适合沉思'
腐化你的光辉之力, 使它们转化为暗面形态并造成额外效果");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 1;
            item.value = 40000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!Fargowiltas.Instance.ThoriumLoaded) return;

            ThoriumPlayer thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();
            //set bonus
            thoriumPlayer.darkAura = true;
        }

        public override void AddRecipes()
        {
            if (!Fargowiltas.Instance.ThoriumLoaded) return;
            
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ModContent.ItemType<EbonHood>());
            recipe.AddIngredient(ModContent.ItemType<EbonCloak>());
            recipe.AddIngredient(ModContent.ItemType<EbonLeggings>());
            recipe.AddIngredient(ModContent.ItemType<LeechBolt>());
            recipe.AddIngredient(ModContent.ItemType<ShadowWand>());
            recipe.AddIngredient(ModContent.ItemType<DarkHeart>());
            recipe.AddIngredient(ModContent.ItemType<EaterOfPain>());
            recipe.AddIngredient(ModContent.ItemType<BrainCoral>());
            recipe.AddIngredient(ModContent.ItemType<AToastPaint>());
            recipe.AddIngredient(ItemID.RedAdmiralButterfly);

            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
