﻿using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Weapons.Ranged
{
    public class Blackhawk : ModItem
    {
        public override bool Autoload(ref string name) => !ModContent.GetInstance<tsorcRevampConfig>().LegacyMode;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blackhawk");
            Tooltip.SetDefault("Deals etra damage to hallowed creatures");
        }

        public override void SetDefaults()
        {
            item.damage = 40;
            item.ranged = true;
            item.width = 48;
            item.height = 34;
            item.useTime = 14;
            item.useAnimation = 14;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 4;
            item.value = 200000;
            item.scale = 0.9f;
            item.rare = ItemRarityID.LightRed;
            item.crit = 5;
            item.UseSound = SoundID.Item40;
            item.shoot = mod.ProjectileType("SentenzaShot");
            item.shootSpeed = 22f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Revolver);
            recipe.AddIngredient(ItemID.SoulofNight, 10);
            recipe.AddIngredient(mod.GetItem("DarkSoul"), 6000);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        // Help, my gun isn't being held at the handle! Adjust these 2 numbers until it looks right.
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2, 0);
        }
    }
}
