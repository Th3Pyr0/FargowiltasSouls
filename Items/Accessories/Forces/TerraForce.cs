﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using FargowiltasSouls.Toggler;
using FargowiltasSouls.Items.Accessories.Enchantments;

namespace FargowiltasSouls.Items.Accessories.Forces
{
    [AutoloadEquip(EquipType.Shield)]
    public class TerraForce : BaseForce
    {
        public static int[] Enchants => new int[]
        {
            ModContent.ItemType<CopperEnchant>(),
            ModContent.ItemType<TinEnchant>(),
            ModContent.ItemType<IronEnchant>(),
            ModContent.ItemType<LeadEnchant>(),
            ModContent.ItemType<TungstenEnchant>(),
            ModContent.ItemType<ObsidianEnchant>()
        };

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.SetDefault("Terra Force");
            
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "泰拉之力");
            
            string tooltip =
$"[i:{ModContent.ItemType<CopperEnchant>()}] Attacks have a chance to spawn lightning and explosions\n" +
$"[i:{ModContent.ItemType<TinEnchant>()}] Sets your critical strike chance to 10%\n" +
$"[i:{ModContent.ItemType<TinEnchant>()}] Every crit will increase it by 5% up to double your current crit chance\n" +
$"[i:{ModContent.ItemType<IronEnchant>()}] Right Click to guard with your shield\n" +
$"[i:{ModContent.ItemType<IronEnchant>()}] Guard just before being hit to parry the attack\n" +
$"[i:{ModContent.ItemType<IronEnchant>()}] You attract items from a larger range\n" +
$"[i:{ModContent.ItemType<LeadEnchant>()}] Attacks may inflict enemies with Lead Poisoning\n" +
$"[i:{ModContent.ItemType<TungstenEnchant>()}] 300% increased sword size\n" +
$"[i:{ModContent.ItemType<TungstenEnchant>()}] Every quarter second a projectile will be tripled in size\n" +
$"[i:{ModContent.ItemType<ObsidianEnchant>()}]Grants immunity to fire and lava\n" +
$"[i:{ModContent.ItemType<ObsidianEnchant>()}]Your attacks spawn explosions\n" +
"'The land lends its strength'";
            Tooltip.SetDefault(tooltip);

            string tooltip_ch =@"攻击有几率释放闪电击打敌人
将你的基础暴击率设为10%
每次暴击时都会增加5%暴击率，增加的暴击率的最大值为你当前最大暴击率数值x2
被击中后会降低暴击率
右键进行盾牌格挡
扩大你的拾取范围
增加150%剑的尺寸
每过1/4秒便会使一个弹幕的尺寸翻倍
攻击有几率造成铅中毒减益
使你免疫火与岩浆并获得在岩浆中的机动性
你的攻击会引发爆炸
'大地赐予它力量'";
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, tooltip_ch);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            FargoSoulsPlayer modPlayer = player.GetModPlayer<FargoSoulsPlayer>();
            //crit effect improved
            modPlayer.TerraForce = true;
            CopperEnchant.CopperEffect(player);
            TinEnchant.TinEffect(player);
            LeadEnchant.LeadEffect(player);
            TungstenEnchant.TungstenEffect(player);
            //lava immune (obsidian)
            modPlayer.ObsidianEffect();

            if (player.GetToggleValue("IronS"))
            {
                //shield
                modPlayer.IronEffect();
            }
            //magnet
            if (player.GetToggleValue("IronM", false))
            {
                modPlayer.IronEnchantActive = true;
                player.treasureMagnet = true;
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            foreach (int ench in Enchants)
                recipe.AddIngredient(ench);
            recipe.AddTile(ModContent.Find<ModTile>("Fargowiltas", "CrucibleCosmosSheet"));
            recipe.Register();
        }
    }
}
