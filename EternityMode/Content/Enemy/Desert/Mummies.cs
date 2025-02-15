﻿using FargowiltasSouls.Buffs.Masomode;
using FargowiltasSouls.EternityMode.Net;
using FargowiltasSouls.EternityMode.Net.Strategies;
using FargowiltasSouls.EternityMode.NPCMatching;
using FargowiltasSouls.NPCs;
using FargowiltasSouls.Projectiles;
using FargowiltasSouls.Projectiles.Masomode;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.EternityMode.Content.Enemy.Desert
{
    public class Mummies : EModeNPCBehaviour
    {
        public override NPCMatcher CreateMatcher() => new NPCMatcher().MatchTypeRange(
            NPCID.Mummy,
            NPCID.BloodMummy,
            NPCID.DarkMummy,
            NPCID.LightMummy
        );

        public override void AI(NPC npc)
        {
            base.AI(npc);

            EModeGlobalNPC.Aura(npc, 500, BuffID.Slow, false, 0);
        }

        public override void OnHitPlayer(NPC npc, Player target, int damage, bool crit)
        {
            base.OnHitPlayer(npc, target, damage, crit);

            target.GetModPlayer<FargoSoulsPlayer>().AddBuffNoStack(BuffID.Webbed, 60);
        }

        public override void OnKill(NPC npc)
        {
            base.OnKill(npc);

            if (Main.rand.NextBool(5))
            {
                for (int i = 0; i < 4; i++)
                    FargoSoulsUtil.NewNPCEasy(npc.GetSource_FromAI(), npc.Center, NPCID.WallCreeper, velocity: new Vector2(Main.rand.NextFloat(-5f, 5f), -Main.rand.NextFloat(10f)));
            }
        }
    }
}
