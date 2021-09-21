using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.NPCs.Bosses.Fiends
{
	class LichKingSerpentHead : ModNPC
	{
		public override void SetDefaults()
		{

			Main.npcFrameCount[npc.type] = 1;
			animationType = 10;
			npc.netAlways = true;
			npc.npcSlots = 50;
			npc.width = 40;
			npc.height = 40;
			npc.boss = true;
			npc.aiStyle = 6;
			npc.defense = 20;
			npc.timeLeft = 22500;
			npc.damage = 310;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath5;
			npc.lifeMax = 120000;
			npc.knockBackResist = 0;
			npc.lavaImmune = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.behindTiles = true;
			npc.value = 40000;
			despawnHandler = new NPCDespawnHandler(DustID.GreenFairy);
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lich King Serpent");
		}
		bool TailSpawned = false;

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.damage = (int)(npc.damage * 1.3 / 2);
			npc.defense = npc.defense += 12;
			npc.lifeMax = (int)(npc.lifeMax * 1.3 / 2);
		}

		NPCDespawnHandler despawnHandler;
		public override void AI()
		{
			despawnHandler.TargetAndDespawn(npc.whoAmI);
			if (!TailSpawned)
			{
				int Previous = npc.whoAmI;
				for (int num36 = 0; num36 < 44; num36++)
				{
					int lol = 0;
					if (num36 >= 0 && num36 < 43)
					{
						lol = NPC.NewNPC((int)npc.position.X + (npc.width / 2), (int)npc.position.Y + (npc.width / 2), ModContent.NPCType<LichKingSerpentBody>(), npc.whoAmI);
					}
					else
					{
						lol = NPC.NewNPC((int)npc.position.X + (npc.width / 2), (int)npc.position.Y + (npc.width / 2), ModContent.NPCType<LichKingSerpentTail>(), npc.whoAmI);
					}
					Main.npc[lol].realLife = npc.whoAmI;
					Main.npc[lol].ai[2] = (float)npc.whoAmI;
					Main.npc[lol].ai[1] = (float)Previous;
					Main.npc[Previous].ai[0] = (float)lol;
					NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, lol, 0f, 0f, 0f, 0);
					Previous = lol;
				}
				TailSpawned = true;
			}
		}

		private static int ClosestSegment(NPC head, params int[] segmentIDs)
		{
			List<int> segmentIDList = new List<int>(segmentIDs);
			Vector2 targetPos = Main.player[head.target].Center;
			int closestSegment = head.whoAmI; //head is default, updates later
			float minDist = 1000000f; //arbitrarily large, updates later
			for (int i = 0; i < Main.npc.Length; i++)
			{ //iterate through every NPC
				NPC npc = Main.npc[i];
				if (npc != null && npc.active && segmentIDList.Contains(npc.type))
				{ //if the npc is part of the wyvern
					float targetDist = (npc.Center - targetPos).Length();
					if (targetDist < minDist)
					{ //if we're closer than the previously closer segment (or closer than 1,000,000 if it's the first iteration, so always)
						minDist = targetDist; //update minDist. future iterations will compare against the updated value
						closestSegment = i; //and set closestSegment to the whoAmI of the closest segment
					}
				}
			}
			return closestSegment; //the whoAmI of the closest segment
		}

		public override bool SpecialNPCLoot()
		{
			int closestSegmentID = ClosestSegment(npc, ModContent.NPCType<LichKingSerpentBody>(), ModContent.NPCType<LichKingSerpentTail>());
			npc.position = Main.npc[closestSegmentID].position; //teleport the head to the location of the closest segment before running npcloot
			return false;
		}
		public override bool CheckActive()
		{
			return false;
		}
		public override void NPCLoot()
		{
			Vector2 vector8 = new Vector2(npc.position.X + (npc.width * 0.5f), npc.position.Y + (npc.height / 2));
			if (npc.life <= 0)
			{
				Gore.NewGore(vector8, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), mod.GetGoreSlot("Gores/Lich King Serpent Head Gore"), 1f);
				Gore.NewGore(vector8, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), mod.GetGoreSlot("Gores/Lich King Serpent Body Gore"), 1f);
				Gore.NewGore(vector8, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), mod.GetGoreSlot("Gores/Lich King Serpent Body Gore"), 1f);
				Gore.NewGore(vector8, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), mod.GetGoreSlot("Gores/Lich King Serpent Body Gore"), 1f);
				Gore.NewGore(vector8, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), mod.GetGoreSlot("Gores/Lich King Serpent Body Gore"), 1f);
				Gore.NewGore(vector8, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), mod.GetGoreSlot("Gores/Lich King Serpent Body Gore"), 1f);
				Gore.NewGore(vector8, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), mod.GetGoreSlot("Gores/Lich King Serpent Body Gore"), 1f);
				Gore.NewGore(vector8, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), mod.GetGoreSlot("Gores/Lich King Serpent Body Gore"), 1f);
				Gore.NewGore(vector8, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), mod.GetGoreSlot("Gores/Lich King Serpent Body Gore"), 1f);
				Gore.NewGore(vector8, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), mod.GetGoreSlot("Gores/Lich King Serpent Tail Gore"), 1f);
			}

			if (!tsorcRevampWorld.Slain.ContainsKey(npc.type))
			{
				Item.NewItem(npc.getRect(), ModContent.ItemType<Items.DarkSoul>(), 2000);
			}
		}
	}
}