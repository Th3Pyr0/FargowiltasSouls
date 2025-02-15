using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Buffs.Boss
{
    public class DeviPresence : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Deviant Presence");
            Description.SetDefault("Friendly NPCs take massively increased damage");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "戴维安驾到");
            Description.AddTranslation((int)GameCulture.CultureName.Chinese, "大幅增加友方NPC受到的伤害");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            
            Terraria.ID.BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<FargoSoulsPlayer>().DevianttPresence = true;
        }
    }
}
