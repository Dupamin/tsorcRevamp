﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.NPCs.Bosses.JungleWyvern
{
    class JungleWyvernTail : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
        }
        public override void SetDefaults()
        {
            NPC.netAlways = true;
            NPC.boss = true;
            NPC.npcSlots = 1;
            NPC.aiStyle = 6;
            NPC.width = 45;
            NPC.height = 45;
            NPC.knockBackResist = 0f;
            NPC.timeLeft = 1750;
            NPC.damage = 35;
            NPC.defense = 0;
            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.NPCDeath8;
            NPC.lifeMax = 91000000;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.behindTiles = true;
            NPC.value = 70000;
        }


        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }
        public override void AI()
        {
            NPC.TargetClosest();
            if (!Main.npc[(int)NPC.ai[1]].active)
            {
                NPC.life = 0;
                NPC.HitEffect(0, 10.0);
                for (int i = 0; i < 50; i++)
                {
                    Color color = new Color();
                    int dust = Dust.NewDust(new Vector2((float)NPC.position.X, (float)NPC.position.Y), NPC.width, NPC.height, 62, Main.rand.Next(-20, 20) * 2, Main.rand.Next(-20, 20) * 2, 100, color, 10f);
                    Main.dust[dust].noGravity = false;
                    dust = Dust.NewDust(new Vector2((float)NPC.position.X, (float)NPC.position.Y), NPC.width, NPC.height, 62, Main.rand.Next(-20, 20) * 2, Main.rand.Next(-20, 20) * 2, 100, color, 6f);
                    Main.dust[dust].noGravity = false;
                    dust = Dust.NewDust(new Vector2((float)NPC.position.X, (float)NPC.position.Y), NPC.width, NPC.height, 54, Main.rand.Next(-20, 20) * 2, Main.rand.Next(-20, 20) * 2, 100, color, 6f);
                    Main.dust[dust].noGravity = false;
                    dust = Dust.NewDust(new Vector2((float)NPC.position.X, (float)NPC.position.Y), NPC.width, NPC.height, 62, 0, 0, 100, Color.White, 10.0f);
                    Main.dust[dust].noGravity = true;
                    //npc.netUpdate = true; //new
                }

                NPC.active = false;
            }
            if (NPC.position.X > Main.npc[(int)NPC.ai[1]].position.X)
            {
                NPC.spriteDirection = 1;
            }
            if (NPC.position.X < Main.npc[(int)NPC.ai[1]].position.X)
            {
                NPC.spriteDirection = -1;
            }
            int mainDust = Dust.NewDust(new Vector2((float)NPC.position.X, (float)NPC.position.Y + 10), NPC.width, NPC.height, 62, 0, 0, 100, default, 1.0f);
            Main.dust[mainDust].noGravity = true;
        }
        public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            modifiers.FinalDamage *= 2;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Vector2 origin = new Vector2(TextureAssets.Npc[NPC.type].Value.Width / 2, TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2);
            Color alpha = Color.White;
            SpriteEffects effects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, new Vector2(NPC.position.X - Main.screenPosition.X + (float)(NPC.width / 2) - (float)TextureAssets.Npc[NPC.type].Value.Width * NPC.scale / 2f + origin.X * NPC.scale, NPC.position.Y - Main.screenPosition.Y + (float)NPC.height - (float)TextureAssets.Npc[NPC.type].Value.Height * NPC.scale / (float)Main.npcFrameCount[NPC.type] + 4f + origin.Y * NPC.scale + 56f), NPC.frame, alpha, NPC.rotation, origin, NPC.scale, effects, 0f);
            NPC.alpha = 255;
            return true;
        }
    }
}
