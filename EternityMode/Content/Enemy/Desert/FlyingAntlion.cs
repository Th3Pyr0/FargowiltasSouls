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
    public class FlyingAntlion : EModeNPCBehaviour
    {
        public override NPCMatcher CreateMatcher() => new NPCMatcher().MatchType(NPCID.FlyingAntlion);

        public override void OnSpawn(NPC npc)
        {
            base.OnSpawn(npc);

            if (Main.rand.NextBool(3))
                EModeGlobalNPC.Horde(npc, Main.rand.Next(5));
        }
    }
}
