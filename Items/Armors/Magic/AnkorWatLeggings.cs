﻿using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using tsorcRevamp.Items.Materials;

namespace tsorcRevamp.Items.Armors.Magic
{
    [AutoloadEquip(EquipType.Legs)]
    public class AnkorWatLeggings : ModItem
    {
        public static float Dmg = 16f;
        public static float MoveSpeed = 10f;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(Dmg, MoveSpeed);
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.defense = 10;
            Item.rare = ItemRarityID.Yellow;
            Item.value = PriceByRarity.fromItem(Item);
        }
        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += MoveSpeed / 100f;
            player.GetDamage(DamageClass.Magic) += Dmg / 100f;

            if(player.HasBuff(BuffID.ShadowDodge))
            {
                player.moveSpeed += MoveSpeed / 100f;
                player.GetDamage(DamageClass.Magic) += Dmg / 100f;
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

