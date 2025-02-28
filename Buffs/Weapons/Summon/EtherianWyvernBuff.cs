using Terraria;
using Terraria.ModLoader;
using tsorcRevamp.Projectiles.Summon;
using tsorcRevamp.Projectiles.Summon.EtherianWyvern;

namespace tsorcRevamp.Buffs.Weapons.Summon
{
    public class EtherianWyvernBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // If the minions exist reset the buff time, otherwise remove the buff from the player
            if (player.ownedProjectileCounts[ModContent.ProjectileType<EtherianWyvernProjectile>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}