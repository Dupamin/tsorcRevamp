﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Weapons.Melee.Broadswords
{
    class SeveringDusk : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("A blade honed sharp by the magic of the coming night." +
                "\nRight click to dash, consuming stamina and making you immune for a moment" +
                "\nStriking enemies while dashing creates a demon spirit from their torn soul"); */
        }
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Blue;
            Item.damage = 100;
            Item.width = 50;
            Item.height = 52;
            Item.knockBack = 5;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 12;
            Item.useTime = 12;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = PriceByRarity.Blue_1;
            Item.shoot = ModContent.ProjectileType<Projectiles.Nothing>();
        }

        int dashTimer;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if(player.altFunctionUse == 2)
            {
                tsorcRevampStaminaPlayer playerStamina = player.GetModPlayer<tsorcRevampStaminaPlayer>();
                if (playerStamina.staminaResourceCurrent > 30)
                {
                    playerStamina.staminaResourceCurrent -= 30;
                    player.velocity = UsefulFunctions.GenerateTargetingVector(player.Center, Main.MouseWorld, 45);
                    player.immuneTime = 30;
                    dashTimer = 20;
                }
            }
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
        }

        public override void HoldItem(Player player)
        {
            if(dashTimer > 0)
            {
                player.immune = true;
                dashTimer--;
                if(dashTimer == 0)
                {
                    player.velocity *= 0.1f;
                }
                for(int i = 0; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i].active && !Main.npc[i].friendly && Main.npc[i].Distance(player.Center) < 70)
                    {
                        //Main.npc[i].StrikeNPC((int)player.GetTotalDamage(DamageClass.Melee).ApplyTo(Item.damage), 0, 0, true);
                    }
                }
            }
            base.HoldItem(player);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
    }
}
