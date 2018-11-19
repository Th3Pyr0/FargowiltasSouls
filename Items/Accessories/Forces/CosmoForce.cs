using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Items.Accessories.Forces
{
    public class CosmoForce : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Force of Cosmos");

            /*string tooltip =
@"'Been around since the Big Bang'
A meteor shower initiates every few seconds while using any weapon
Hurting enemies has a chance to spawn buff boosters
Reach maxed buff boosters to gain drastically increased attack speed
Solar shield allows you to dash through enemies
Attacks may inflict the Solar Flare debuff
Double tap down to direct your guardian
Press the Freeze Key to freeze time for 5 seconds
There is a 60 second cooldown for this effect, a sound effect plays when it's back
Double tap down to toggle stealth, reducing chance for enemies to target you but slowing movement
You also spawn a vortex to draw in and massively damage enemies when you enter stealth
Summons a Companion Cube Pet";

            if(thorium != null)
            {
                tooltip += "Releases a strange alien creature";
            }

            Tooltip.SetDefault(tooltip);*/
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 10;
            item.value = 600000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>(mod);
            //shower works for all, meme speed works for all, solar flare all
            modPlayer.CosmoForce = true; //check all
            //meteor shower
            modPlayer.MeteorEffect(75);
            //boosters and meme speed
            modPlayer.NebulaEffect();
            //solar shields and flare debuff
            modPlayer.SolarEffect();
            //guardian and time freeze
            modPlayer.StardustEffect();
            //stealth, voids, pet
            modPlayer.VortexEffect(hideVisual);

            modPlayer.AddPet("Suspicious Eye Pet", hideVisual, BuffID.SuspiciousTentacle, ProjectileID.SuspiciousTentacle);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "MeteorEnchant");
            recipe.AddIngredient(null, "SolarEnchant");
            recipe.AddIngredient(null, "VortexEnchant");
            recipe.AddIngredient(null, "NebulaEnchant");
            recipe.AddIngredient(null, "StardustEnchant");

            //bow of light
            recipe.AddIngredient(ItemID.SuspiciousLookingTentacle);

            if (Fargowiltas.Instance.FargosLoaded)
                recipe.AddTile(ModLoader.GetMod("Fargowiltas"), "CrucibleCosmosSheet");
            else
                recipe.AddTile(TileID.LunarCraftingStation);

            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}