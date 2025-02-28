﻿using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Potions
{
    public class StrengthPotion : ModItem
    {
        public static int Defense = 12;
        public static float DamageBoost = 12f;
        public static float AttackSpeedBoost = 10f;
        public static int BadLifeRegen = 5;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(DamageBoost, Defense, AttackSpeedBoost, BadLifeRegen, 1f + tsorcRevampPlayer.MeleeBonusMultiplier);
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 26;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;
            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = 1000;
            Item.buffType = ModContent.BuffType<Buffs.Strength>();
            Item.buffTime = 28800;
        }
        public override bool? UseItem(Player player)
        {
            int currentBuff = 0;
            foreach (int buffType in player.buffType)
            {
                if (buffType == ModContent.BuffType<Buffs.Battlefront>() || buffType == ModContent.BuffType<Buffs.DemonDrug>() || buffType == ModContent.BuffType<Buffs.ArmorDrug>())
                {
                    player.DelBuff(currentBuff);
                }
                currentBuff++;
            }
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(4);
            recipe.AddIngredient(ItemID.Teacup, 4);
            recipe.AddIngredient(ItemID.Deathweed, 4);
            recipe.AddIngredient(ItemID.Diamond, 4);
            recipe.AddIngredient(ItemID.SoulofNight, 4);
            recipe.AddTile(TileID.Bottles);

            recipe.Register();
        }
    }
}
