using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using tsorcRevamp.Items.Materials;

namespace tsorcRevamp.Items.Weapons.Melee.Broadswords
{
    public class AtmaWeapon : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Yellow;
            Item.damage = 66;
            Item.width = 96;
            Item.height = 96;
            Item.knockBack = 9f;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = PriceByRarity.LightPurple_6;
            Item.shoot = ModContent.ProjectileType<Projectiles.Nothing>();
            tsorcInstancedGlobalItem instancedGlobal = Item.GetGlobalItem<tsorcInstancedGlobalItem>();
            instancedGlobal.slashColor = Microsoft.Xna.Framework.Color.Cyan;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.CobaltSword, 1);
            recipe.AddIngredient(ItemID.MythrilSword, 1);
            recipe.AddIngredient(ItemID.AdamantiteSword, 1);
            recipe.AddIngredient(ModContent.ItemType<DarkSoul>(), 15000);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }

        //When I woke up one morning I found this number written on the back of my hand, again and again. I don't know what it does or why, but it works.
        //Jk, it's just 50 (the damage boost) / 55 (the weapon's base damage). It's used to convert the damage boost we come up with into a fraction of the base damage, since that's what add
        //It would've been easier to just use flat, but then it would have worked strangely with multiplicitive damage boosts or debuffs...
        static float multiplier = 0.90909090909f;
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            damage += ((float)player.statLife / (float)player.statLifeMax2) * multiplier;
        }
    }
}
