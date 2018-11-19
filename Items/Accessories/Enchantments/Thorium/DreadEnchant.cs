using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using ThoriumMod;
using Microsoft.Xna.Framework;

namespace FargowiltasSouls.Items.Accessories.Enchantments.Thorium
{
    public class DreadEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");
        
        public override bool Autoload(ref string name)
        {
            return ModLoader.GetLoadedMods().Contains("ThoriumMod");
        }
        
        public override string Texture => "FargowiltasSouls/Items/Placeholder";
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dread Enchantment");
            Tooltip.SetDefault(
@"'Infused with souls of the damned'
Your boots vibrate at an unreal frequency, increasing movement speed significantly
While moving, your melee damage and critical strike chance are increased
15% increased movement and maximum speed
Running builds up momentum and increases movement speed
Crashing into an enemy releases all stored momentum, catapulting the enemy
Flail weapons have a chance to release rolling spike balls on hit that apply cursed flames to damaged enemies
Your symphonic damage empowers all nearby allies with: Vile Flames
Damage done against curse flamed enemies is increased by 8%
Doubles the range of your empowerments effect radius
Your symphonic damage will empower all nearby allies with: Movement Speed II");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 7;
            item.value = 200000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!Fargowiltas.Instance.ThoriumLoaded) return;
            
            DreadEffect(player);
        }
        
        private void DreadEffect(Player player)
        {
            ThoriumPlayer thoriumPlayer = player.GetModPlayer<ThoriumPlayer>(thorium);
            //dread set bonus
            player.moveSpeed += 0.8f;
            player.maxRunSpeed += 10f;
            player.runAcceleration += 0.05f;
            if (player.velocity.X > 0f || player.velocity.X < 0f)
            {
                player.meleeDamage += 0.35f;
                player.meleeCrit += 26;
                player.endurance += 0.1f;
                for (int i = 0; i < 2; i++)
                {
                    int num = Dust.NewDust(new Vector2(player.position.X, player.position.Y) - player.velocity * 0.5f, player.width, player.height, 65, 0f, 0f, 0, default(Color), 1.75f);
                    int num2 = Dust.NewDust(new Vector2(player.position.X, player.position.Y) - player.velocity * 0.5f, player.width, player.height, 75, 0f, 0f, 0, default(Color), 1f);
                    Main.dust[num].noGravity = true;
                    Main.dust[num2].noGravity = true;
                    Main.dust[num].noLight = true;
                    Main.dust[num2].noLight = true;
                }
            }
            //crash boots
            player.moveSpeed += 0.0015f * thoriumPlayer.momentum;
            player.maxRunSpeed += 0.0025f * thoriumPlayer.momentum;
            if (player.velocity.X > 0f || player.velocity.X < 0f)
            {
                if (thoriumPlayer.momentum < 180)
                {
                    thoriumPlayer.momentum++;
                }
                if (thoriumPlayer.momentum > 60 && Collision.SolidCollision(player.position, player.width, player.height + 4))
                {
                    int num = Dust.NewDust(new Vector2(player.position.X - 2f, player.position.Y + player.height - 2f), player.width + 4, 4, 6, 0f, 0f, 100, default(Color), 0.625f + 0.0075f * thoriumPlayer.momentum);
                    Main.dust[num].noGravity = true;
                    Main.dust[num].noLight = true;
                    Dust dust = Main.dust[num];
                    dust.velocity *= 0f;
                }
            }
            //cursed core
            thoriumPlayer.cursedCore = true;
            //corrupt woofer
            thoriumPlayer.subwooferCursed = true;
            thoriumPlayer.bardRangeBoost += 450;
            //music player
            thoriumPlayer.musicPlayer = true;
            thoriumPlayer.MP3MovementSpeed = 2;
        }
        
        private readonly string[] items =
        {
            "DreadSkull",
            "DreadChestPlate",
            "DreadGreaves",
            "CrashBoots",
            "CursedCore",
            "CorruptSubwoofer",
            "TunePlayerMovementSpeed"
        };

        public override void AddRecipes()
        {
            if (!Fargowiltas.Instance.ThoriumLoaded) return;
            
            ModRecipe recipe = new ModRecipe(mod);
            
            foreach (string i in items) recipe.AddIngredient(thorium.ItemType(i));

            recipe.AddIngredient(ItemID.ChainGuillotines);
            recipe.AddIngredient(thorium.ItemType("ImpactDrill"));
            recipe.AddIngredient(thorium.ItemType("DreadLauncher"));

            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
