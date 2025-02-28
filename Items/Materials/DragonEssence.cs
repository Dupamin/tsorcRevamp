﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Materials
{
    class DragonEssence : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 20;
            Item.rare = ItemRarityID.Orange;
            Item.value = 1000;
            Item.maxStack = Item.CommonMaxStack;
        }
    }
}
