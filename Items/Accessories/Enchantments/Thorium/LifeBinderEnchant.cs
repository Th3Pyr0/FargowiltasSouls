using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using Terraria.Localization;
using ThoriumMod.Items.HealerItems;
using ThoriumMod.Items.ThrownItems;

namespace FargowiltasSouls.Items.Accessories.Enchantments.Thorium
{
    public class LifeBinderEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override bool Autoload(ref string name)
        {
            return ModLoader.GetMod("ThoriumMod") != null;
        }
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Life Binder Enchantment");
            Tooltip.SetDefault(
@"'Vegetation grows from your fingertips'
Healing spells will shortly increase the healed player's maximum life by 50
Your radiant damage has a 15% chance to release a blinding flash of light
The flash heals nearby allies equal to your bonus healing and confuses enemies
Effects of Aloe Leaf and Equalizer
Summons a pet Holy Goat");
            DisplayName.AddTranslation(GameCulture.Chinese, "生命束缚者魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'植物从你的之间生长'
治疗队友将会短暂增加其50最大生命值
光辉伤害有15%概率造成闪光爆炸
闪光爆炸将迷惑敌人并治疗队友(受额外治疗量影响)
拥有芦荟叶和平等护符效果
召唤宠物神圣山羊");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 4;
            item.value = 120000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!Fargowiltas.Instance.ThoriumLoaded) return;

            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();
            ThoriumPlayer thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();

            if (modPlayer.ThoriumSoul) return;

            //life binder set bonus
            thoriumPlayer.mistSet = true;
            //aloe leaf
            thoriumPlayer.aloePlant = true;

            //iridescent set bonus
            thoriumPlayer.iridescentSet = true;

            //equalizer 
            thoriumPlayer.equilibrium = true;
        }

        public override void AddRecipes()
        {
            if (!Fargowiltas.Instance.ThoriumLoaded) return;
            
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ModContent.ItemType<DewBinderMask>());
            recipe.AddIngredient(ModContent.ItemType<DewBinderBreastplate>());
            recipe.AddIngredient(ModContent.ItemType<DewBinderGreaves>());
            recipe.AddIngredient(ModContent.ItemType<IridescentEnchant>());
            recipe.AddIngredient(ModContent.ItemType<AloeLeaf>());
            recipe.AddIngredient(ModContent.ItemType<BloomGuard>());
            recipe.AddIngredient(ModContent.ItemType<SunrayStaff>());
            recipe.AddIngredient(ModContent.ItemType<MorningDew>());
            recipe.AddIngredient(ModContent.ItemType<HolyFire>());
            recipe.AddIngredient(ModContent.ItemType<BudBomb>(), 300);

            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
