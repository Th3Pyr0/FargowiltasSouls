﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FargowiltasSouls.Buffs.Masomode;

namespace FargowiltasSouls.Projectiles.BossWeapons
{
    public class HentaiSpearBigDeathray : Deathrays.BaseDeathray
    {
        public HentaiSpearBigDeathray() : base(60, "PhantasmalDeathrayML", hitboxModifier: 1.5f) { }

        int dustTimer;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.SetDefault("Phantasmal Deathray");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            CooldownSlot = -1;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;

            Projectile.GetGlobalProjectile<FargoSoulsGlobalProjectile>().CanSplit = false;
            Projectile.GetGlobalProjectile<FargoSoulsGlobalProjectile>().TimeFreezeImmune = true;
            Projectile.GetGlobalProjectile<FargoSoulsGlobalProjectile>().DeletionImmuneRank = 2;

            Projectile.hide = true;
            Projectile.penetrate = -1;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindProjectiles.Add(index);
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!Main.dedServ && Main.LocalPlayer.active)
                Main.LocalPlayer.GetModPlayer<FargoSoulsPlayer>().Screenshake = 2;

            Vector2? vector78 = null;
            if (Projectile.velocity.HasNaNs() || Projectile.velocity == Vector2.Zero)
            {
                Projectile.velocity = -Vector2.UnitY;
            }
            int byIdentity = FargoSoulsUtil.GetProjectileByIdentity(Projectile.owner, (int)Projectile.ai[1], ModContent.ProjectileType<HentaiSpearWand>());
            if (byIdentity != -1)
            {
                Projectile.timeLeft = 2;

                float itemrotate = player.direction < 0 ? MathHelper.Pi : 0;
                if (Math.Abs(player.itemRotation) > Math.PI / 2)
                    itemrotate = itemrotate == 0 ? MathHelper.Pi : 0;
                Projectile.velocity = (player.itemRotation + itemrotate).ToRotationVector2();
                Projectile.Center = player.Center + Main.rand.NextVector2Circular(5, 5);

                Projectile.position += Projectile.velocity * 164 * 1.3f / 4f; //offset by part of spear's length (wand)
                Projectile.position += Projectile.velocity * 164 * 1.3f * 0.75f; //part of penetrator's length (ray)

                Projectile.damage = player.GetWeaponDamage(player.HeldItem);
                Projectile.knockBack = player.GetWeaponKnockback(player.HeldItem, player.HeldItem.knockBack);

                /*Projectile.Center = Main.projectile[player.heldProj].Center + Main.rand.NextVector2Circular(5, 5);
                Projectile.timeLeft = 2;

                Projectile.velocity = Vector2.Normalize(Main.projectile[player.heldProj].velocity);
                Projectile.position += Projectile.velocity * 164 * 1.3f * 0.75f; //part of penetrator's length

                Projectile.damage = Main.projectile[player.heldProj].damage;
                Projectile.knockBack = Main.projectile[player.heldProj].knockBack;*/
            }
            else if (Projectile.localAI[0] > 5) //leeway for mp lag
            {
                Projectile.Kill();
                return;
            }
            if (Projectile.velocity.HasNaNs() || Projectile.velocity == Vector2.Zero)
            {
                Projectile.velocity = -Vector2.UnitY;
            }
            if (Projectile.localAI[0] == 0f)
            {
                if (!Main.dedServ)
                    Terraria.Audio.SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot(FargowiltasSouls.Instance, "Sounds/Zombie_104"), Projectile.Center);
            }
            float num801 = 10f;

            if (Projectile.localAI[0] == maxTime / 2)
            {
                if (Projectile.owner == Main.myPlayer && !(player.controlUseTile && player.altFunctionUse == 2 && player.HeldItem.type == ModContent.ItemType<Items.Weapons.FinalUpgrades.HentaiSpear>()))
                    Projectile.localAI[0] += 1f; //if stop firing, proceed to die
                else
                    Projectile.localAI[0] -= 1f; //otherwise, stay (also for multiplayer!)
            }
            else
            {
                Projectile.localAI[0] += 1f;
            }

            if (Projectile.localAI[0] >= maxTime)
            {
                Projectile.Kill();
                return;
            }
            //Projectile.scale = num801;
            Projectile.scale = (float)Math.Sin(Projectile.localAI[0] * 3.14159274f / maxTime) * 1.5f * num801;
            if (Projectile.scale > num801)
            {
                Projectile.scale = num801;
            }
            //float num804 = Projectile.velocity.ToRotation();
            //num804 += Projectile.ai[0];
            //Projectile.rotation = num804 - 1.57079637f;
            //float num804 = Main.npc[(int)Projectile.ai[1]].ai[3] - 1.57079637f;
            //if (Projectile.ai[0] != 0f) num804 -= (float)Math.PI;
            //Projectile.rotation = num804;
            //num804 += 1.57079637f;
            //Projectile.velocity = num804.ToRotationVector2();
            float num805 = 3f;
            float num806 = (float)Projectile.width;
            Vector2 samplingPoint = Projectile.Center;
            if (vector78.HasValue)
            {
                samplingPoint = vector78.Value;
            }
            float[] array3 = new float[(int)num805];
            //Collision.LaserScan(samplingPoint, Projectile.velocity, num806 * Projectile.scale, 3000f, array3);
            for (int i = 0; i < array3.Length; i++)
                array3[i] = 3000f;
            float num807 = 0f;
            int num3;
            for (int num808 = 0; num808 < array3.Length; num808 = num3 + 1)
            {
                num807 += array3[num808];
                num3 = num808;
            }
            num807 /= num805;
            float amount = 0.5f;
            Projectile.localAI[1] = MathHelper.Lerp(Projectile.localAI[1], num807, amount);
            /*Vector2 vector79 = Projectile.Center + Projectile.velocity * (Projectile.localAI[1] - 14f);
            for (int num809 = 0; num809 < 2; num809 = num3 + 1)
            {
                float num810 = Projectile.velocity.ToRotation() + ((Main.rand.Next(2) == 1) ? -1f : 1f) * 1.57079637f;
                float num811 = (float)Main.rand.NextDouble() * 2f + 2f;
                Vector2 vector80 = new Vector2((float)Math.Cos((double)num810) * num811, (float)Math.Sin((double)num810) * num811);
                int num812 = Dust.NewDust(vector79, 0, 0, 244, vector80.X, vector80.Y, 0, default(Color), 1f);
                Main.dust[num812].noGravity = true;
                Main.dust[num812].scale = 1.7f;
                num3 = num809;
            }
            if (Main.rand.NextBool(5))
            {
                Vector2 value29 = Projectile.velocity.RotatedBy(1.5707963705062866, default(Vector2)) * ((float)Main.rand.NextDouble() - 0.5f) * (float)Projectile.width;
                int num813 = Dust.NewDust(vector79 + value29 - Vector2.One * 4f, 8, 8, 244, 0f, 0f, 100, default(Color), 1.5f);
                Dust dust = Main.dust[num813];
                dust.velocity *= 0.5f;
                Main.dust[num813].velocity.Y = -Math.Abs(Main.dust[num813].velocity.Y);
            }*/
            //DelegateMethods.v3_1 = new Vector3(0.3f, 0.65f, 0.7f);
            //Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.velocity * Projectile.localAI[1], (float)Projectile.width * Projectile.scale, new Utils.PerLinePoint(DelegateMethods.CastLight));

            Projectile.position -= Projectile.velocity;
            float oldRot = Projectile.rotation;
            Projectile.rotation = Projectile.velocity.ToRotation() - 1.57079637f;

            if (++Projectile.ai[0] > 60)
            {
                Projectile.ai[0] = 0;

                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item84, player.Center);

                if (Projectile.owner == Main.myPlayer)
                {
                    const int ringMax = 10;
                    const float speed = 12f;
                    const float rotation = 0.5f;
                    for (int i = 0; i < ringMax; i++)
                    {
                        Vector2 vel = speed * Projectile.velocity.RotatedBy(2 * Math.PI / ringMax * i);
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center, vel, ModContent.ProjectileType<HentaiSphereRing>(),
                            Projectile.damage, Projectile.knockBack, Projectile.owner, rotation, speed);
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center, vel, ModContent.ProjectileType<HentaiSphereRing>(),
                            Projectile.damage, Projectile.knockBack, Projectile.owner, -rotation, speed);
                    }
                }
            }

            if (--dustTimer < -1)
            {
                dustTimer = 50;

                float diff = MathHelper.WrapAngle(Projectile.rotation - oldRot);
                //if (npc.HasPlayerTarget && Math.Abs(MathHelper.WrapAngle(npc.DirectionTo(Main.player[npc.target].Center).ToRotation() - Projectile.velocity.ToRotation())) < Math.Abs(diff)) diff = 0;
                diff *= 15f;

                const int ring = 220; //LAUGH
                for (int i = 0; i < ring; ++i)
                {
                    Vector2 speed = Projectile.velocity.RotatedBy(diff) * 24f;

                    Vector2 vector2 = (-Vector2.UnitY.RotatedBy(i * 3.14159274101257 * 2 / ring) * new Vector2(8f, 16f)).RotatedBy(Projectile.velocity.ToRotation() + diff);
                    int index2 = Dust.NewDust(Main.player[Projectile.owner].Center, 0, 0, 111, 0.0f, 0.0f, 0, new Color(), 1f);
                    Main.dust[index2].scale = 2.5f;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].position = Main.player[Projectile.owner].Center;
                    Main.dust[index2].velocity = vector2 * 2.5f + speed;

                    index2 = Dust.NewDust(Main.player[Projectile.owner].Center, 0, 0, 111, 0.0f, 0.0f, 0, new Color(), 1f);
                    Main.dust[index2].scale = 2.5f;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].position = Main.player[Projectile.owner].Center;
                    Main.dust[index2].velocity = vector2 * 1.75f + speed * 2;
                }
            }


            Projectile.frameCounter += Main.rand.Next(3);
            if (++Projectile.frameCounter > 3)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame > 15)
                    Projectile.frame = 0;
            }

            if (Main.rand.NextBool(10))
                Projectile.spriteDirection *= -1;
        }

        public override void PostAI()
        {
            base.PostAI();
            Projectile.hide = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 1; //balanceing
            target.AddBuff(ModContent.BuffType<CurseoftheMoon>(), 600);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.velocity == Vector2.Zero)
            {
                return false;
            }

            SpriteEffects spriteEffects = Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Texture2D texture2D19 = FargowiltasSouls.Instance.Assets.Request<Texture2D>("Projectiles/Deathrays/Mutant/MutantDeathray_" + Projectile.frame.ToString(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Texture2D texture2D20 = FargowiltasSouls.Instance.Assets.Request<Texture2D>("Projectiles/Deathrays/Mutant/MutantDeathray2_" + Projectile.frame.ToString(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Texture2D texture2D21 = FargowiltasSouls.Instance.Assets.Request<Texture2D>("Projectiles/Deathrays/" + texture + "3", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            float num223 = Projectile.localAI[1];
            Color color44 = new Color(255, 255, 255, 100) * 0.9f;
            SpriteBatch arg_ABD8_0 = Main.spriteBatch;
            Texture2D arg_ABD8_1 = texture2D19;
            Vector2 arg_ABD8_2 = Projectile.Center - Main.screenPosition;
            Rectangle? sourceRectangle2 = null;
            arg_ABD8_0.Draw(arg_ABD8_1, arg_ABD8_2, sourceRectangle2, color44, Projectile.rotation, texture2D19.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
            num223 -= (texture2D19.Height / 2 + texture2D21.Height) * Projectile.scale;
            Vector2 value20 = Projectile.Center;
            value20 += Projectile.velocity * Projectile.scale * texture2D19.Height / 2f;
            if (num223 > 0f)
            {
                float num224 = 0f;
                Rectangle rectangle7 = new Rectangle(0, 0, texture2D20.Width, 30);
                while (num224 + 1f < num223)
                {
                    if (num223 - num224 < rectangle7.Height)
                    {
                        rectangle7.Height = (int)(num223 - num224);
                    }

                    Main.EntitySpriteDraw(texture2D20, value20 - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(rectangle7), color44, Projectile.rotation, new Vector2(rectangle7.Width / 2, 0f), Projectile.scale, spriteEffects, 0);
                    num224 += rectangle7.Height * Projectile.scale;
                    value20 += Projectile.velocity * rectangle7.Height * Projectile.scale;
                    rectangle7.Y += 30;
                    if (rectangle7.Y + rectangle7.Height > texture2D20.Height)
                    {
                        rectangle7.Y = 0;
                    }
                }
            }
            SpriteBatch arg_AE2D_0 = Main.spriteBatch;
            Texture2D arg_AE2D_1 = texture2D21;
            Vector2 arg_AE2D_2 = value20 - Main.screenPosition;
            sourceRectangle2 = null;
            arg_AE2D_0.Draw(arg_AE2D_1, arg_AE2D_2, sourceRectangle2, color44, Projectile.rotation, texture2D21.Frame(1, 1, 0, 0).Top(), Projectile.scale, spriteEffects, 0);
            return false;
        }
    }
}