﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using tsorcRevamp.Items.Materials;

namespace tsorcRevamp.Items.Pets
{
    class MiakodaFull : ModItem
    {
        public static float DamageReduction = 3f;
        public static int BaseHealing = 2;
        public static float MaxHPHealPercent = 2f;
        public static int HealCooldown = 12;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(DamageReduction, BaseHealing, MaxHPHealPercent, HealCooldown);
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.DD2PetGhost);
            Item.shoot = ModContent.ProjectileType<Projectiles.Pets.MiakodaFull>();
            Item.buffType = ModContent.BuffType<Buffs.MiakodaFull>();
        }

        public override void UseStyle(Player player, Rectangle rectangle)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600, true);
            }
        }

        public override void AddRecipes()
        {
            {
                Recipe recipe = CreateRecipe();
                recipe.AddIngredient(ModContent.ItemType<MiakodaCrescent>());
                recipe.AddIngredient(ModContent.ItemType<DarkSoul>(), 100);
                recipe.AddTile(TileID.DemonAltar);

                recipe.Register();
            }
            {
                Recipe recipe = CreateRecipe();
                recipe.AddIngredient(ModContent.ItemType<MiakodaNew>());
                recipe.AddIngredient(ModContent.ItemType<DarkSoul>(), 100);
                recipe.AddTile(TileID.DemonAltar);

                recipe.Register();
            }
        }
    }
}
