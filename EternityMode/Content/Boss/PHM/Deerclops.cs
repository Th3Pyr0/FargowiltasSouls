﻿using FargowiltasSouls.EternityMode.Net;
using FargowiltasSouls.EternityMode.Net.Strategies;
using FargowiltasSouls.EternityMode.NPCMatching;
using FargowiltasSouls.Buffs.Masomode;
using FargowiltasSouls.Items.Accessories.Masomode;
using FargowiltasSouls.Items.Placeables;
using FargowiltasSouls.NPCs;
using FargowiltasSouls.NPCs.EternityMode;
using FargowiltasSouls.Projectiles;
using FargowiltasSouls.Projectiles.Masomode;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using FargowiltasSouls.ItemDropRules.Conditions;
using FargowiltasSouls.Projectiles.Champions;
using Terraria.Localization;
using System;

namespace FargowiltasSouls.EternityMode.Content.Boss.PHM
{
    public class Deerclops : EModeNPCBehaviour
    {
        public override NPCMatcher CreateMatcher() => new NPCMatcher().MatchType(NPCID.Deerclops);

        public int BerserkSpeedupTimer;
        public int TeleportTimer;

        public bool EnteredPhase2;
        public bool EnteredPhase3;

        public bool DroppedSummon;

        public override Dictionary<Ref<object>, CompoundStrategy> GetNetInfo() =>
            new Dictionary<Ref<object>, CompoundStrategy> {
                { new Ref<object>(BerserkSpeedupTimer), IntStrategies.CompoundStrategy },
                { new Ref<object>(TeleportTimer), IntStrategies.CompoundStrategy },

                { new Ref<object>(EnteredPhase2), BoolStrategies.CompoundStrategy },
                { new Ref<object>(EnteredPhase3), BoolStrategies.CompoundStrategy },
            };

        public override void SetDefaults(NPC npc)
        {
            base.SetDefaults(npc);

            npc.buffImmune[BuffID.Frostburn] = true;
            npc.buffImmune[BuffID.Frostburn2] = true;
            npc.buffImmune[BuffID.Chilled] = true;
            npc.buffImmune[BuffID.Frozen] = true;

            npc.lifeMax = (int)Math.Round(npc.lifeMax * 1.25, MidpointRounding.ToEven);
        }

        public override bool CanHitPlayer(NPC npc, Player target, ref int CooldownSlot)
        {
            if (npc.alpha > 0)
                return false;

            return base.CanHitPlayer(npc, target, ref CooldownSlot);
        }

        public override bool PreAI(NPC npc)
        {
            bool result = base.PreAI(npc);

            EModeGlobalNPC.deerBoss = npc.whoAmI;

            if (FargoSoulsWorld.SwarmActive)
                return result;

            //localai[3] seems to be invul timer, up to 30

            const int MaxBerserkTime = 600;

            BerserkSpeedupTimer -= 1;

            if (npc.localAI[3] > 0 || EnteredPhase3)
                npc.localAI[2]++; //cry about it

            const int TeleportThreshold = 660;

            if (npc.ai[0] != 0)
            {
                npc.alpha -= 10;
                if (npc.alpha < 0)
                    npc.alpha = 0;

                if (EnteredPhase3)
                    npc.localAI[2]++;
            }

            TeleportTimer++;
            if (EnteredPhase3)
                TeleportTimer++;

            switch ((int)npc.ai[0])
            {
                case 0: //walking at player
                    if (TeleportTimer < TeleportThreshold)
                    {
                        if (EnteredPhase3)
                            npc.position.X += npc.velocity.X;

                        if (npc.velocity.Y == 0)
                        {
                            if (EnteredPhase2)
                                npc.position.X += npc.velocity.X;
                            if (BerserkSpeedupTimer > 0)
                                npc.position.X += npc.velocity.X * 4f * BerserkSpeedupTimer / MaxBerserkTime;
                        }
                    }

                    if (EnteredPhase2)
                    {
                        if (!EnteredPhase3 && npc.life < npc.lifeMax * .33)
                        {
                            npc.ai[0] = 3;
                            npc.ai[1] = 0;
                            npc.netUpdate = true;
                            break;
                        }

                        if (TeleportTimer > TeleportThreshold)
                        {
                            npc.velocity.X *= 0.9f;
                            npc.dontTakeDamage = true;
                            npc.localAI[1] = 0; //reset walls attack counter

                            if (EnteredPhase3 && Main.LocalPlayer.active && !Main.LocalPlayer.ghost && !Main.LocalPlayer.dead)
                            {
                                FargoSoulsUtil.AddDebuffFixedDuration(Main.LocalPlayer, BuffID.Darkness, 2);
                                FargoSoulsUtil.AddDebuffFixedDuration(Main.LocalPlayer, BuffID.Blackout, 2);
                            }

                            if (npc.alpha == 0)
                            {
                                Terraria.Audio.SoundEngine.PlaySound(SoundID.Roar, npc.Center, 0);

                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    for (int i = 0; i < 6; i++)
                                    {
                                        Vector2 spawnPos = Main.player[npc.target].Center + Main.rand.NextVector2CircularEdge(16 * 30, 16 * 30);
                                        Projectile.NewProjectile(npc.GetSource_FromThis(), spawnPos, Vector2.Zero, ModContent.ProjectileType<DeerclopsHand>(), 0, 0f, Main.myPlayer, npc.target);
                                    }
                                }
                            }

                            npc.alpha += 5;
                            if (npc.alpha > 255)
                            {
                                npc.alpha = 255;

                                npc.localAI[3] = 30;

                                if (npc.HasPlayerTarget) //teleport
                                {
                                    float distance = 16 * 14 * Math.Sign(npc.Center.X - Main.player[npc.target].Center.X);
                                    distance *= -1f; //alternate back and forth

                                    if (TeleportTimer == TeleportThreshold + 10) //introduce randomness
                                    {
                                        if (Main.rand.NextBool())
                                            distance *= -1f;
                                        if (Main.netMode == NetmodeID.Server)
                                            NetMessage.SendData(MessageID.SyncNPC, number: npc.whoAmI);
                                    }

                                    npc.Bottom = Main.player[npc.target].Bottom + distance * Vector2.UnitX;

                                    npc.direction = Math.Sign(Main.player[npc.target].Center.X - npc.Center.X);
                                    npc.velocity.X = 3.4f * npc.direction;
                                    npc.velocity.Y = 0;

                                    int addedThreshold = 180;
                                    if (EnteredPhase3)
                                        addedThreshold -= 30;
                                    if (FargoSoulsWorld.MasochistModeReal)
                                        addedThreshold -= 30;

                                    if (TeleportTimer > TeleportThreshold + addedThreshold)
                                    {
                                        TeleportTimer = 0;
                                        npc.velocity.X = 0;
                                        npc.ai[0] = 4;
                                        npc.ai[1] = 0;
                                        NetSync(npc);
                                        if (Main.netMode == NetmodeID.Server)
                                            NetMessage.SendData(MessageID.SyncNPC, number: npc.whoAmI);
                                    }
                                }
                            }
                            else
                            {
                                TeleportTimer = TeleportThreshold;

                                if (npc.localAI[3] > 0)
                                    npc.localAI[3] -= 3; //remove visual effect
                            }

                            return false;
                        }
                    }
                    else if (npc.life < npc.lifeMax * .66)
                    {
                        npc.ai[0] = 3;
                        npc.ai[1] = 0;
                        npc.netUpdate = true;
                    }
                    break;

                case 1: //ice wave, npc.localai[1] counts them, attacks at ai1=30, last spike 52, ends at ai1=80
                    if (npc.ai[1] < 30)
                    {
                        if (FargoSoulsWorld.MasochistModeReal)
                        {
                            npc.ai[1] += 0.5f;
                            npc.frameCounter += 0.5;
                        }
                    }
                    break;

                case 2: //debris attack
                    break;

                case 3: //roar at 30, ends at ai1=60
                    if (!FargoSoulsWorld.MasochistModeReal && npc.ai[1] < 30)
                    {
                        npc.ai[1] -= 0.5f;
                        npc.frameCounter -= 0.5;
                    }

                    if (EnteredPhase2)
                    {
                        npc.localAI[1] = 0; //ensure this is always the same
                        npc.localAI[3] = 30; //go invul
                    }
                    else if (npc.life < npc.lifeMax * .66)
                    {
                        EnteredPhase2 = true;
                        NetSync(npc);
                    }

                    if (EnteredPhase3)
                    {
                        if (!Main.dedServ)
                            Main.LocalPlayer.GetModPlayer<FargoSoulsPlayer>().Screenshake = 2;

                        if (npc.ai[1] > 30) //roaring
                        {
                            if (npc.HasValidTarget) //fly over player
                                npc.position = Vector2.Lerp(npc.position, Main.player[npc.target].Center - 450 * Vector2.UnitY, 0.2f);

                            Main.dayTime = false;
                            Main.time = 16200; //midnight for cool factor
                        }
                    }
                    else if (npc.life < npc.lifeMax * .33)
                    {
                        EnteredPhase3 = true;
                        NetSync(npc);
                    }

                    if (EnteredPhase3 || FargoSoulsWorld.MasochistModeReal)
                        BerserkSpeedupTimer = MaxBerserkTime;
                    break;

                case 4: //both sides ice wave, attacks at ai1=50, last spike 70, ends at ai1=90
                    {
                        int cooldown = 100;
                        if (EnteredPhase3)
                            cooldown *= 2;
                        if (TeleportTimer > TeleportThreshold - cooldown)
                            TeleportTimer = TeleportThreshold - cooldown;

                        int threshold = 0;
                        if (EnteredPhase2)
                            threshold = 30;

                        if (EnteredPhase2 && npc.ai[1] < threshold)
                            npc.ai[1]++;
                    }
                    break;

                case 6: //trying to return home
                    npc.TargetClosest();

                    if (npc.ai[1] > 120 && (!npc.HasValidTarget || npc.Distance(Main.player[npc.target].Center) > 1600))
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient) //force despawn
                        {
                            npc.ai[0] = 8f;
                            npc.ai[1] = 0.0f;
                            npc.localAI[1] = 0.0f;
                            npc.netUpdate = true;
                        }
                    }
                    break;

                default:
                    break;
            }

            //FargoSoulsUtil.PrintAI(npc);

            EModeUtils.DropSummon(npc, "DeerThing2", NPC.downedDeerclops, ref DroppedSummon);

            return result;
        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            base.ModifyNPCLoot(npc, npcLoot);

            LeadingConditionRule emodeRule = new LeadingConditionRule(new EModeDropCondition());
            emodeRule.OnSuccess(FargoSoulsUtil.BossBagDropCustom(ModContent.ItemType<Deerclawps>()));
            emodeRule.OnSuccess(FargoSoulsUtil.BossBagDropCustom(ItemID.FrozenCrate, 5));
            npcLoot.Add(emodeRule);
        }

        public override void OnHitPlayer(NPC npc, Player target, int damage, bool crit)
        {
            base.OnHitPlayer(npc, target, damage, crit);

            target.AddBuff(BuffID.Frostburn, 90);
            target.AddBuff(ModContent.BuffType<MarkedforDeath>(), 600);
            target.AddBuff(ModContent.BuffType<Hypothermia>(), 1200);
        }

        public override void LoadSprites(NPC npc, bool recolor)
        {
            base.LoadSprites(npc, recolor);

            //LoadNPCSprite(recolor, npc.type);
            //LoadBossHeadSprite(recolor, 14);
            //LoadGoreRange(recolor, 303, 308);
        }
    }
}
