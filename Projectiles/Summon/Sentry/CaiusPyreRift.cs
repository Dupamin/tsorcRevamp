using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Projectiles.Summon.Sentry
{
    public class CaiusPyreRift : FloatingSentryProjectile
    {
        public override int ShotCooldown => 1 * 60;
        public override int SentryShotCooldownReductionOnSpawn => 10;
        public override int ProjectileFrameCount => 6;
        public override int ProjectileWidth => 24;
        public override int ProjectileHeight => 84;
        public override DamageClass ProjectileDamageType => DamageClass.MagicSummonHybrid;
        public override bool ContactDamage => false;
        public override bool CanShoot => true;
        public override int ShotProjectileType => ModContent.ProjectileType<CaiusPyreFireball>();
        public override float ProjectileInitialVelocity => 0;
        public override bool PlaysSoundOnShot => true;
        public override SoundStyle ShootSoundStyle => SoundID.Item20;
        public override float ShootSoundVolume => 1.5f;
        public override bool SpawnsDust => true;
        public override int ProjectileDustID => DustID.RedTorch;
    }
}