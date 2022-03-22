using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace FargowiltasSouls.Items.Misc
{
    public class BrokenHilt : SoulsItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Broken Hilt");
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.rare = ItemRarityID.Purple;
        }

        public override void SafeModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine line2 in list)
            {
                if (line2.mod == "Terraria" && line2.Name == "ItemName")
                {
                    line2.overrideColor = Main.DiscoColor;
                }
            }
        }
    }
}