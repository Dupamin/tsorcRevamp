﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using tsorcRevamp.Items.Weapons.Melee.Shortswords;

namespace tsorcRevamp.Projectiles.Melee.Shortswords
{
    class YellowTailFishbone : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = 4;
            Projectile.scale = 1f;
            Projectile.alpha = 50;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 60;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 120;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation(); // projectile faces sprite right
            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 33, Projectile.velocity.X * -0.3f, Projectile.velocity.Y * -.3f, 30, Color.LightYellow, 1.3f);
            Main.dust[dust].noGravity = true;
            Projectile.ai[0]++;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player owner = Main.player[Projectile.owner];
            for (int d = 0; d < 20; d++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 26, Projectile.velocity.X * 1f, Projectile.velocity.Y * 1f, 30, default(Color), 1.5f);
                Main.dust[dust].velocity.X = +Main.rand.Next(-50, 51) * 0.1f;
                Main.dust[dust].velocity.Y = +Main.rand.Next(-50, 51) * 0.1f;
                Main.dust[dust].noGravity = true;
            }
            owner.statLife += YellowTail.Heal;
            owner.HealEffect(YellowTail.Heal);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCDeath9 with { Volume = 0.5f }, Projectile.position);
            for (int d = 0; d < 15; d++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 5, Projectile.velocity.X * 1.2f, Projectile.velocity.Y * 1.2f, 30, default(Color), 1.4f);
                Main.dust[dust].noGravity = true;

            }
            for (int d = 0; d < 5; d++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 96, Projectile.velocity.X * 0.8f, Projectile.velocity.Y * 0.8f, 30, default(Color), 1.2f);
                Main.dust[dust].noGravity = true;
            }
            return true;
        }

        float colorMult = 0f;

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)Terraria.GameContent.TextureAssets.Projectile[Projectile.type];

            if (Projectile.ai[0] > 10)
            {
                colorMult += 0.12f;
            }

            Main.spriteBatch.Draw(texture, Projectile.Center, Color.White * (1 - colorMult));
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 18, 14), Color.White * (1 - colorMult), Projectile.rotation, new Vector2(9, 7), Projectile.scale, SpriteEffects.None, 0);

            return false;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.ScalingArmorPenetration += YellowTail.ArmorPen / 100f;
        }

    }
}
