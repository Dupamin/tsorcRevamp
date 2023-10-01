﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using tsorcRevamp.Items.Weapons.Melee.Runeterra;
using tsorcRevamp.Projectiles.Melee.Runeterra;

namespace tsorcRevamp.Buffs.Runeterra.Melee
{
    public class NightbringerDash : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            var dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.Torch, Scale: 3f);
            dust.noGravity = true;

            player.immune = true;

            if (player.buffTime[buffIndex] >= (int)(PlasmaWhirlwind.DashDuration * 60))
            {
                player.GetModPlayer<tsorcRevampPlayer>().SweepingBladeTimer = 2;
            }
            if (player.velocity.X > 0) 
            {
                player.direction = 1;
            } else
            { player.direction = -1; }
        }
    }
}
