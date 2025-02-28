﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using tsorcRevamp.Buffs.Debuffs;

namespace tsorcRevamp.Items.Tools
{
    public class PowerWithin : ModItem
    {
        public static float DamageIncrease = 10f;
        public static float StaminaRegen = 30f;
        public static int Duration = 30;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(DamageIncrease, StaminaRegen, Duration);
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.rare = ItemRarityID.Orange;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.value = 15000;

        }

        public override void HoldItem(Player player)
        {
            if (player.itemAnimation != 0)
            {
                float slowdownX = player.velocity.X * .9f;

                player.velocity.X = slowdownX;


            }
        }

        public override void UseStyle(Player player, Rectangle rectangle)
        {
            if (player.itemTime == 0)
            {
                player.itemTime = (int)(Item.useTime / PlayerLoader.UseTimeMultiplier(player, Item));
                player.AddBuff(ModContent.BuffType<Crippled>(), 60);
                player.AddBuff(ModContent.BuffType<GrappleMalfunction>(), 60);
            }

            if (player.itemTime < (int)(Item.useTime / PlayerLoader.UseTimeMultiplier(player, Item)) / 2)
            {
                Item.useStyle = ItemUseStyleID.HoldUp;

                if (Main.rand.NextBool(2))
                {
                    if (player.direction == 1)
                    {
                        Dust dust = Main.dust[Dust.NewDust(new Vector2(player.position.X + 10, player.position.Y - 4), 10, 10, 6, player.velocity.X, player.velocity.Y, 100, default, Main.rand.NextFloat(1f, 1.5f))];
                        dust.noGravity = true;
                    }
                    else
                    {
                        Dust dust = Main.dust[Dust.NewDust(new Vector2(player.position.X - 2, player.position.Y - 4), 10, 10, 6, player.velocity.X, player.velocity.Y, 100, default, Main.rand.NextFloat(1f, 1.5f))];
                        dust.noGravity = true;
                    }
                }
            }

            if (player.itemTime >= (int)(Item.useTime / PlayerLoader.UseTimeMultiplier(player, Item)) / 2)
            {
                Item.useStyle = ItemUseStyleID.Shoot;
            }

            if (player.itemTime == 1)
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item100 with { Volume = 1f, PitchVariance = 0.3f }, player.Center);

                player.AddBuff(ModContent.BuffType<Buffs.PowerWithin>(), Duration * 60);

                for (int q = 0; q < 30; q++)
                {
                    int z = Dust.NewDust(player.Center, 40, 56, 270, 0f, 0f, 120, default(Color), 1f);
                    Main.dust[z].noGravity = true;
                    Main.dust[z].velocity *= 2.75f;
                    Main.dust[z].fadeIn = 1.3f;
                    Vector2 vectorother = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                    vectorother.Normalize();
                    vectorother *= (float)Main.rand.Next(50, 100) * 0.2f;
                    Main.dust[z].velocity = vectorother;
                    vectorother.Normalize();
                    vectorother *= 10f;
                    Main.dust[z].position = player.Center - vectorother;
                }

                for (int q = 0; q < 30; q++)
                {
                    int z = Dust.NewDust(player.Center, 40, 56, 270, 0f, 0f, 120, default(Color), 1f);
                    Main.dust[z].noGravity = true;
                    Main.dust[z].velocity *= 2.75f;
                    Main.dust[z].fadeIn = 1.3f;
                    Vector2 vectorother = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                    vectorother.Normalize();
                    vectorother *= (float)Main.rand.Next(50, 100) * 0.12f;
                    Main.dust[z].velocity = vectorother;
                    vectorother.Normalize();
                    vectorother *= 30f;
                    Main.dust[z].position = player.Center - vectorother;
                }

            }
        }
    }
}