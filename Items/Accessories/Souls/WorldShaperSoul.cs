using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using static Terraria.ID.ItemID;
using ThoriumMod;

namespace FargowiltasSouls.Items.Accessories.Souls
{
    //[AutoloadEquip(EquipType.Back)]
    public class WorldShaperSoul : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("World Shaper Soul");
            Tooltip.SetDefault(
@"'Limitless possibilities'
Increased block and wall placement speed by 50% 
Near infinite block placement and mining reach
Mining speed doubled 
Auto paint and actuator effect 
Provides light 
Grants the ability to enable Builder Mode:
Anything that creates a tile will not be consumed 
No enemies can spawn
Effect can be disabled in Soul Toggles menu");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            item.value = 750000;
            item.expert = true;
            item.rare = -12;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();
            //placing speed up
            player.tileSpeed += 0.5f;
            player.wallSpeed += 0.5f;
            //toolbox
            Player.tileRangeX += 50;
            Player.tileRangeY += 50;
            //gizmo pack
            player.autoPaint = true;
            //pick speed
            player.pickSpeed -= 0.50f;
            //mining helmet
            if (Soulcheck.GetValue("Shine Buff")) Lighting.AddLight(player.Center, 0.8f, 0.8f, 0f);
            //presserator
            player.autoActuator = true;

            if (Soulcheck.GetValue("Builder Mode"))
            {
                FargoWorld.BuilderMode = true;
                modPlayer.BuilderMode = true;
            }

            //cell phone
            player.accWatch = 3;
            player.accDepthMeter = 1;
            player.accCompass = 1;
            player.accFishFinder = true;
            player.accDreamCatcher = true;
            player.accOreFinder = true;
            player.accStopwatch = true;
            player.accCritterGuide = true;
            player.accJarOfSouls = true;
            player.accThirdEye = true;
            player.accCalendar = true;
            player.accWeatherRadio = true;

            /*if (!Fargowiltas.Instance.ThoriumLoaded) return;

            ThoriumPlayer thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();
            thoriumPlayer.geodeShine = true;
            Lighting.AddLight(player.position, 1.2f, 0.8f, 1.2f);
            //pets
            modPlayer.AddPet("Inspiring Lantern Pet", hideVisual, thorium.BuffType("SupportLanternBuff"), thorium.ProjectileType("SupportLantern"));
            thoriumPlayer.lanternPet = true;
            modPlayer.AddPet("Lock Box Pet", hideVisual, thorium.BuffType("LockBoxBuff"), thorium.ProjectileType("LockBoxPet"));
            thoriumPlayer.LockBoxPet = true;
            //mining speed, spelunker, dangersense, light, hunter, pet
            modPlayer.MinerEffect(hideVisual, .5f);*/
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            //if (!Fargowiltas.Instance.ThoriumLoaded)
            //{

                recipe.AddIngredient(Toolbelt);
                recipe.AddIngredient(Toolbox);
                recipe.AddIngredient(ArchitectGizmoPack);
                recipe.AddIngredient(ActuationAccessory);
                recipe.AddIngredient(LaserRuler);
                recipe.AddIngredient(RoyalGel);
                recipe.AddIngredient(PutridScent);
                recipe.AddIngredient(CellPhone);
                recipe.AddIngredient(GravityGlobe);

                recipe.AddRecipeGroup("FargowiltasSouls:AnyDrax");
                recipe.AddIngredient(ShroomiteDiggingClaw);
                recipe.AddIngredient(Picksaw);
                recipe.AddIngredient(LaserDrill);
                recipe.AddIngredient(DrillContainmentUnit);
                
            //}
            
            /*
             * geode enchant
            GnomeKingPickaxe
            impact drill
            TerrariumCanyonSplitter
            */

            if (Fargowiltas.Instance.FargosLoaded)
                recipe.AddTile(ModLoader.GetMod("Fargowiltas"), "CrucibleCosmosSheet");
            else
                recipe.AddTile(TileID.LunarCraftingStation);
                
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
