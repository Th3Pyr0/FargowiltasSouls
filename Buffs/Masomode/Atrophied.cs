using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Buffs.Masomode
{
    public class Atrophied : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Atrophied");
            Description.SetDefault("Your muscles are deteriorating");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "萎缩");
            Description.AddTranslation((int)GameCulture.CultureName.Chinese, "你的肌肉正在退化");
        }

        public override void Update(Player player, ref int buffIndex)
        {
            //melee silence hopefully plus damage reduced 99%, -all crit just in case
            player.GetModPlayer<FargoSoulsPlayer>().Atrophied = true;
            if (player.HeldItem.DamageType == DamageClass.Melee || player.HeldItem.DamageType == DamageClass.Throwing)
                player.GetModPlayer<FargoSoulsPlayer>().AttackSpeed -= 0.5f;
        }
    }
}