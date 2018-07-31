﻿using FargowiltasSouls.NPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace FargowiltasSouls.Buffs.Souls
{
    public class SolarFlare : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Solar Flare");
            Main.buffNoSave[Type] = true;
            canBeCleared = false;
            Main.debuff[Type] = true;
        }

        public override bool Autoload(ref string name, ref string texture)
        {
            texture = "FargowiltasSouls/Buffs/PlaceholderDebuff";
            return true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<FargoGlobalNPC>().SolarFlare = true;

            if (npc.buffTime[buffIndex] < 3)
            {
                Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("Explosion"), 1000, 0f, Main.myPlayer);
            }
        }

        public override bool ReApply(NPC npc, int time, int buffIndex)
        {
            return true;
        }
    }
}