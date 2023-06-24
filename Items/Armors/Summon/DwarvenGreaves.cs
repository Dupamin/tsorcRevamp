﻿using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using tsorcRevamp.Items.Materials;

namespace tsorcRevamp.Items.Armors.Summon
{
    [AutoloadEquip(EquipType.Legs)]
    class DwarvenGreaves : ModItem
    {
        public static int MinionSlots = 1;
        public static float MoveSpeed = 19f;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(MinionSlots, MoveSpeed);
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.height = Item.width = 18;
            Item.defense = 12;
            Item.rare = ItemRarityID.Yellow;
            Item.value = PriceByRarity.fromItem(Item);
        }
        public override void UpdateEquip(Player player)
        {
            player.maxMinions += MinionSlots;

            player.moveSpeed += MoveSpeed / 100f;

            if (player.HasBuff(BuffID.ShadowDodge))
            {
                player.moveSpeed += MoveSpeed / 100f;
            }
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.HallowedGreaves, 1);
            recipe.AddIngredient(ModContent.ItemType<DarkSoul>(), 10000);
            recipe.AddTile(TileID.DemonAltar);

            recipe.Register();
        }
    }
}
