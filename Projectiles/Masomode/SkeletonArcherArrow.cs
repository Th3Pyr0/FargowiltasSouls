﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.Masomode
{
    public class SkeletonArcherArrow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Venom Arrow");
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.VenomArrow);
            AIType = ProjectileID.VenomArrow;
            Projectile.DamageType = DamageClass.Default;
            Projectile.friendly = false;
            Projectile.arrow = false;
            Projectile.hostile = true;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Venom, 180);
        }
    }
}