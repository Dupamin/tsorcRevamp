﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace tsorcRevamp.Items
{
    class DisabledSale : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Red;
            Item.value = 999999999;
        }
    }
}
