﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.NPCs.Enemies.SuperHardMode
{
    class IceSkeleton : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 15;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn2] = true;
        }
        public override void SetDefaults()
        {
            AnimationType = NPCID.AngryBones;
            NPC.width = 18;
            NPC.height = 40;
            NPC.knockBackResist = .3f;
            NPC.value = 4000; // 163
            NPC.timeLeft = 750;
            NPC.damage = 70;
            NPC.defense = 73;
            NPC.HitSound = SoundID.NPCHit2;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.lifeMax = 1000;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Banners.IceSkeletonBanner>();
        }

        int dashCounter = 0;
        public override void AI()
        {
            tsorcRevampAIs.FighterAI(NPC, 5, 0.1f, enragePercent: 0.95f, enrageTopSpeed: 10);
            tsorcRevampAIs.LeapAtPlayer(NPC, 5, 5, 4, 200);
            dashCounter++;

            if (dashCounter > 120)
            {
                UsefulFunctions.DustRing(NPC.Center, 32, DustID.BlueCrystalShard, 12, 4);
                Lighting.AddLight(NPC.Center, Color.Orange.ToVector3() * 5);
            }

            if (dashCounter > 150 && Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height))
            {
                NPC.velocity = UsefulFunctions.Aim(NPC.Center, Main.player[NPC.target].Center, 15);
                dashCounter = 0;
            }
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            Player p = spawnInfo.Player;
            if (tsorcRevampWorld.SuperHardMode && p.ZoneSnow)
            {
                return 0.15f;
            }
            if (tsorcRevampWorld.SuperHardMode && spawnInfo.SpawnTileX > Main.maxTilesX * 0.7f)
            {
                if (p.ZoneDirtLayerHeight)
                {
                    if (!Main.dayTime) { return 0.2f; }
                    else return 0.067f;
                }
                else if (p.ZoneRockLayerHeight) { return 0.2f; } // was 0.1f
            }
            return 0f;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                if (!Main.dedServ)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Ice Skelly Head").Type, 1.1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Ice Skelly Vert").Type, 1.1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Ice Skelly Vert").Type, 1.1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Ice Skelly Piece").Type, 1.1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Ice Skelly Piece").Type, 1.1f);
                }
            }
        }
    }
}
