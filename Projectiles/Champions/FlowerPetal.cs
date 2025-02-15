using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.Champions
{
    public class FlowerPetal : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_221";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flower Petal");
            Main.projFrames[Projectile.type] = 3;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.timeLeft = 240;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.alpha = 0;
            Projectile.hide = true;
            CooldownSlot = 1;
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 0 && Projectile.timeLeft > 105)
                Projectile.timeLeft = 105;

            if (Projectile.localAI[0] == 0f)
            {
                Projectile.localAI[0] = 1f;
                Projectile.scale = Main.rand.NextFloat(1.5f, 2f);
                Projectile.frame = Main.rand.Next(3);
                Projectile.hide = false;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item8, Projectile.Center);
            }

            if (++Projectile.localAI[1] > 30 && Projectile.localAI[1] < 100)
            {
                Projectile.velocity *= 1.06f;
            }

            Projectile.rotation += Projectile.velocity.X * 0.01f;

            int dust = Dust.NewDust(Projectile.Center, 0, 0, 86);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].scale *= 2f;
            Main.dust[dust].velocity *= 0.1f;

            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 86, Projectile.velocity.X, Projectile.velocity.Y);
        }

        public override void Kill(int timeLeft)
        {
            if (Projectile.ai[0] == 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                for (int i = -1; i <= 1; i++) //split
                {
                    Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Projectile.velocity.RotatedBy(MathHelper.ToRadians(5) * i) / 2f,
                        Projectile.type, Projectile.damage, 0f, Main.myPlayer, 1f);
                }
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (FargoSoulsWorld.EternityMode)
                target.AddBuff(ModContent.BuffType<Buffs.Masomode.Purified>(), 300);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * Projectile.Opacity;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;

            Color color26 = lightColor;
            color26 = Projectile.GetAlpha(color26);

            SpriteEffects effects = Projectile.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
            {
                Color color27 = Color.White * Projectile.Opacity * 0.75f * 0.5f;
                color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
                Vector2 value4 = Projectile.oldPos[i];
                float num165 = Projectile.oldRot[i];
                Main.EntitySpriteDraw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color27, num165, origin2, Projectile.scale, effects, 0);
            }

            Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), Projectile.GetAlpha(lightColor), Projectile.rotation, origin2, Projectile.scale, effects, 0);
            return false;
        }
    }
}