using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.NPCs.Bosses.Serris
{
    class SerrisBody : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 3;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frozen] = true;
        }
        public override void SetDefaults()
        {
            NPC.netAlways = true;
            NPC.npcSlots = 1;
            NPC.width = 38;
            NPC.height = 56;
            NPC.aiStyle = 6;
            NPC.timeLeft = 750;
            NPC.damage = 23;
            NPC.defense = 40;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath5;
            NPC.dontTakeDamage = true;
            NPC.lavaImmune = true;
            NPC.knockBackResist = 0;
            NPC.lifeMax = 91000000;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.behindTiles = true;
            NPC.boss = true;
            NPC.value = 460;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }

        bool SpeedBoost = false;
        public override void AI()
        {
            //tsorcRevampGlobalNPC.AIWorm(npc, ModContent.NPCType<SerrisHead>(), SerrisHead.bodyTypes, ModContent.NPCType<SerrisTail>(), 16, -2f, 12f, 0.6f, true, false, true, true, true);


            if (NPC.position.X > Main.npc[(int)NPC.ai[1]].position.X)
            {
                NPC.spriteDirection = -1;
            }
            if (NPC.position.X < Main.npc[(int)NPC.ai[1]].position.X)
            {
                NPC.spriteDirection = 1;
            }
            if (!Main.npc[(int)NPC.ai[1]].active || Main.npc[(int)NPC.ai[1]].life <= 0)
            {
                NPC.life = 0;
                NPC.HitEffect(0, 10.0);
                NPC.active = false;

                Vector2 vector8 = new Vector2(NPC.position.X + (NPC.width * 0.5f), NPC.position.Y + (NPC.height / 2)); if (!Main.dedServ)
                {
                    Gore.NewGore(NPC.GetSource_Death(), vector8, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Serris Gore 2").Type, 1f);
                }
            }

            if (Main.npc[(int)NPC.ai[2]].active && Main.npc[(int)NPC.ai[2]].dontTakeDamage && Main.npc[(int)NPC.ai[2]].type == ModContent.NPCType<NPCs.Bosses.Serris.SerrisHead>())
            {
                SpeedBoost = true;
                return;
            }
            else
            {
                SpeedBoost = false;
            }

        }
        public override bool CheckActive()
        {
            return false;
        }
        public override void FindFrame(int currentFrame)
        {
            int num = 1;
            if (!Main.dedServ)
            {
                num = TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type];
            }
            NPC.frameCounter += 1.0;
            if (SpeedBoost)
            {
                if (NPC.frameCounter >= 0 && NPC.frameCounter < 5)
                {
                    NPC.frame.Y = num;
                }
                if (NPC.frameCounter >= 5 && NPC.frameCounter < 10)
                {
                    NPC.frame.Y = num * 2;
                }
                if (NPC.frameCounter >= 10)
                {
                    NPC.frameCounter = 0;
                }
            }
            else
            {
                NPC.frame.Y = 0;
                NPC.frameCounter = 0;
            }
        }
    }
}