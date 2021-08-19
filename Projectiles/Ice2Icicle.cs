﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace tsorcRevamp.Projectiles {
    class Ice2Icicle : ModProjectile {
        public override void SetDefaults() {
            projectile.width = 24;
            projectile.height = 64;
            projectile.friendly = true;
            projectile.penetrate = 5;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 90;
        }
        public override void AI() {
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90);

            if (projectile.timeLeft <= 30)
            {
                projectile.alpha += 6;
            }

            //keep a portion of the projectile's velocity when spawned, so we canmake sure it has the right knockback
            if (projectile.ai[0] == 0)
            {
                projectile.velocity.X *= 0.001f;
                projectile.ai[0] = 1;
            }
        }
    }
}
