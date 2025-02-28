﻿using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using tsorcRevamp.Items.Materials;

namespace tsorcRevamp.Items.Armors.Magic
{
    [LegacyName("AncientDragonScaleGreaves")]
    [AutoloadEquip(EquipType.Legs)]
    public class DragonScaleGreaves : ModItem
    {
        public static float MoveSpeed = 16f;
        public static float ManaCost = 17f;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(MoveSpeed, ManaCost);
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.defense = 5;
            Item.rare = ItemRarityID.LightPurple;
            Item.value = PriceByRarity.fromItem(Item);
        }
        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += MoveSpeed / 100f;
            player.manaCost -= ManaCost / 100f;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.MythrilGreaves);
            recipe.AddIngredient(ModContent.ItemType<DarkSoul>(), 5500);
            recipe.AddTile(TileID.DemonAltar);

            recipe.Register();

            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ItemID.MythrilGreaves);
            recipe2.AddIngredient(ItemID.OrichalcumLeggings);
            recipe2.AddTile(TileID.DemonAltar);

            recipe2.Register();
        }
    }
}

