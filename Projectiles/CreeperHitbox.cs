﻿using Terraria;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles
{
    public class CreeperHitbox : ModProjectile
    {
        public override string Texture => "FargowiltasSouls/Projectiles/Explosion";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Creeper");
        }

        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 1;
            Projectile.hide = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 0;
        }

        public override bool? CanDamage()
        {
            Projectile.maxPenetrate = 1;
            return true;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            hitDirection = Main.player[Projectile.owner].Center.X > target.Center.X ? -1 : 1;
        }
    }
}