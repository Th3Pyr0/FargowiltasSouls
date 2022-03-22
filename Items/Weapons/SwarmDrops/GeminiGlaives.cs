﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using FargowiltasSouls.Projectiles.BossWeapons;
using Terraria.DataStructures;

namespace FargowiltasSouls.Items.Weapons.SwarmDrops
{
    public class GeminiGlaives : SoulsItem
    {
        private int lastThrown = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gemini Glaives");
            Tooltip.SetDefault("Fire different glaives depending on mouse click" +
                "\nAlternating clicks will enhance attacks" +
                "\n'The compressed forms of defeated foes..'");

            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "被打败的敌人的压缩形态..");
        }

        public override void SetDefaults()
        {
            Item.damage = 340;
            Item.DamageType = DamageClass.Melee;
            Item.width = 30;
            Item.height = 30;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3;
            Item.value = Item.sellPrice(0, 25);
            Item.rare = ItemRarityID.Purple;
            Item.shootSpeed = 20;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Retiglaive>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<Spazmaglaive>()] > 0)
                return false;

            if (player.altFunctionUse == 2)
            {
                Item.shoot = ModContent.ProjectileType<Retiglaive>();
                Item.shootSpeed = 15f;
            }
            else
            {
                Item.shoot = ModContent.ProjectileType<Spazmaglaive>();
                Item.shootSpeed = 45f;
            }
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (lastThrown != type)
                damage = (int)(damage * 1.2); //additional damage boost for switching

            for (int i = -1; i <= 1; i++)
            {
                Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(30) * i), type, damage, knockback, player.whoAmI, lastThrown);
            }

            lastThrown = type;
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(null, "TwinRangs")
            .AddIngredient(null, "AbomEnergy", 10)
            .AddIngredient(ModContent.Find<ModItem>("Fargowiltas", "EnergizerTwins"))
            .AddTile(ModContent.Find<ModTile>("Fargowiltas", "CrucibleCosmosSheet"))
            
            .Register();
        }
    }
}