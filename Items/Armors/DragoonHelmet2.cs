﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Armors
{
    [AutoloadEquip(EquipType.Head)]
    public class DragoonHelmet2 : ModItem
    {
        public override string Texture => "tsorcRevamp/Items/Armors/DragoonHelmet";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dragoon Helmet II");
            Tooltip.SetDefault("Harmonized with Sky and Fire\n+200 Mana\nPotion use has a 15 second shorter cooldown.");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 12;
            Item.defense = 15;
            Item.value = 10000;
            Item.rare = ItemRarityID.Orange;
        }

        public override void UpdateEquip(Player player)
        {
            player.statManaMax2 += 200;
            player.pStone = true;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<DragoonArmor2>() && legs.type == ModContent.ItemType<DragoonGreaves2>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Harmonized with the four elements: fire, water, earth and air, including +6 life regen and 38% boost to all stats";
            player.lavaImmune = true;
            player.fireWalk = true;
            player.breath = 9999999;
            player.waterWalk = true;
            player.noKnockback = true;
            player.GetDamage(DamageClass.Generic) += 0.38f;
            player.GetCritChance(DamageClass.Ranged) += 38;
            player.GetCritChance(DamageClass.Melee) += 38;
            player.GetCritChance(DamageClass.Magic) += 38;
            player.GetCritChance(DamageClass.Throwing) += 38;
            player.GetAttackSpeed(DamageClass.Melee) += 0.38f;
            player.moveSpeed += 0.38f;
            player.manaCost -= 0.38f;
            player.lifeRegen += 6;

            //player.wings = 34; // looks like Jim's Wings
            //player.wingsLogic = 34;
            player.wingTimeMax = 180;

        }

        public override bool WingUpdate(Player player, bool inUse)
        {
            return base.WingUpdate(player, inUse);
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }

        public override void AddRecipes()
        {
            Terraria.Recipe recipe = CreateRecipe();
            recipe.AddIngredient(Mod.Find<ModItem>("DragoonHelmet").Type, 1);
            recipe.AddIngredient(ModContent.ItemType<DragonEssence>(), 10);
            recipe.AddIngredient(Mod.Find<ModItem>("DyingWindShard").Type, 10);
            recipe.AddIngredient(Mod.Find<ModItem>("DarkSoul").Type, 40000);
            recipe.AddTile(TileID.DemonAltar);

            recipe.Register();
        }
    }
}
