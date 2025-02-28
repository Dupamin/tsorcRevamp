using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using tsorcRevamp.Buffs.Debuffs;
using tsorcRevamp.Items.Potions;

namespace tsorcRevamp.NPCs.Enemies
{
    class BasiliskWalker : ModNPC
    {
        float shotTimer;
        int chargeDamage = 0;
        bool chargeDamageFlag = false;
        int hypnoticDisruptorDamage = 15;
        int bioSpitDamage = 10;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 12;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
        }
        public override void SetDefaults()
        {
            NPC.npcSlots = 2;
            AnimationType = 28;
            NPC.knockBackResist = 0.3f;
            NPC.damage = 23;
            NPC.defense = 8;
            NPC.height = 50;
            NPC.width = 24;
            NPC.lifeMax = 100;
            NPC.HitSound = SoundID.NPCHit20;
            NPC.DeathSound = SoundID.NPCDeath5;
            NPC.value = 500; // health / 2
            NPC.lavaImmune = true;

            if (Main.hardMode)
            {
                NPC.lifeMax = 250;
                NPC.defense = 20;
                NPC.value = 1250; // health / 2 : was 70
                NPC.damage = 33;
                hypnoticDisruptorDamage = 23;
                bioSpitDamage = 18;
            }

            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Banners.BasiliskWalkerBanner>();
            UsefulFunctions.AddAttack(NPC, 140, ModContent.ProjectileType<Projectiles.Enemy.EnemyBioSpitBall>(), bioSpitDamage, 8, SoundID.Item20 with { Volume = 0.2f, Pitch = 0.3f }, telegraphColor: Color.GreenYellow);
            UsefulFunctions.AddAttack(NPC, 240, ModContent.ProjectileType<Projectiles.Enemy.HypnoticDisrupter>(), hypnoticDisruptorDamage, 3, SoundID.Item24 with { Volume = 0.6f, Pitch = -0.5f }, weight: 0.08f, telegraphColor: Color.Purple);
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            Player P = spawnInfo.Player; //These are mostly redundant with the new zone definitions, but it still works.
            bool Meteor = P.ZoneMeteor;
            bool Jungle = P.ZoneJungle;
            bool Dungeon = P.ZoneDungeon;
            bool Corruption = (P.ZoneCorrupt || P.ZoneCrimson);
            bool AboveEarth = P.ZoneOverworldHeight;
            bool InBrownLayer = P.ZoneDirtLayerHeight;
            bool InGrayLayer = P.ZoneRockLayerHeight;
            // P.townNPCs > 0f // is no town NPCs nearby

            if (spawnInfo.Invasion)
            {
                return 0;
            }

            if (spawnInfo.Water) return 0f;

            if (!Main.hardMode && !Main.dayTime && (Corruption || Jungle) && AboveEarth && P.townNPCs <= 0f && tsorcRevampWorld.NewSlain.ContainsKey(new NPCDefinition(NPCID.SkeletronHead)) && Main.rand.NextBool(24)) return 1;


            //new chance to spawn in the corruption or crimson below ground (poison and cursed aren't activated until EoW and Skeletron respectively for balance; now we'll finally have a unique mod npc that fits well in these zones)
            if (!Main.hardMode && P.ZoneCorrupt && !Main.dayTime && !AboveEarth && Main.rand.NextBool(10)) return 1;

            if (!Main.hardMode && P.ZoneCorrupt && Main.dayTime && !AboveEarth && Main.rand.NextBool(20)) return 1;

            //higher chance to spawn in the crimson 
            if (!Main.hardMode && P.ZoneCrimson && !Main.dayTime && Main.rand.NextBool(5)) return 1;

            if (!Main.hardMode && P.ZoneCrimson && Main.dayTime && Main.rand.NextBool(10)) return 1;//10 is 3%, 5 is 6%

            //meteor not desert
            if (!Main.hardMode && Meteor && !Dungeon && !Main.dayTime && !P.ZoneUndergroundDesert && (InBrownLayer || InGrayLayer) && Main.rand.NextBool(5)) return 1;

            if (!Main.hardMode && Meteor && !Dungeon && Main.dayTime && !P.ZoneUndergroundDesert && InGrayLayer && Main.rand.NextBool(10)) return 1;
            //meteor and desert
            if (!Main.hardMode && Meteor && !Dungeon && !Main.dayTime && P.ZoneUndergroundDesert && (InBrownLayer || InGrayLayer) && Main.rand.NextBool(12)) return 1;

            if (!Main.hardMode && Meteor && !Dungeon && Main.dayTime && P.ZoneUndergroundDesert && InGrayLayer && Main.rand.NextBool(24)) return 1;
            //jungle
            if (!Main.hardMode && Jungle && Main.dayTime && !Dungeon && InGrayLayer && Main.rand.NextBool(80)) return 1; //was 200

            if (!Main.hardMode && Jungle && !Main.dayTime && !Dungeon && InGrayLayer && Main.rand.NextBool(60)) return 1; //was 850

            //hard mode
            if (Main.hardMode && P.townNPCs <= 0f && !Main.dayTime && (Meteor || Jungle || Corruption) && !Dungeon && (AboveEarth || InBrownLayer || InGrayLayer) && Main.rand.NextBool(45)) return 1;

            if (Main.hardMode && P.townNPCs <= 0f && Main.dayTime && (Meteor || Jungle || Corruption) && !Dungeon && (InBrownLayer || InGrayLayer) && Main.rand.NextBool(55)) return 1;

            return 0;
        }

        public override void AI()
        {
            tsorcRevampAIs.FighterAI(NPC, 1, 0.03f, canTeleport: false, randomSound: SoundID.Mummy, soundFrequency: 1000, enragePercent: 0.2f, enrageTopSpeed: 2);

            //MAKE SOUND WHEN JUMPING/HOVERING
            if (Main.rand.NextBool(12) && NPC.velocity.Y <= -1f)
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item24 with { Volume = 0.2f, Pitch = 0.1f }, NPC.Center);
            }

            //JUSTHIT CODE
            Player player = Main.player[NPC.target];
            if (NPC.justHit && NPC.Distance(player.Center) < 150)
            {
                NPC.GetGlobalNPC<tsorcRevampGlobalNPC>().ProjectileTimer = 0f;
            }
            if (NPC.justHit && NPC.Distance(player.Center) < 150 && Main.rand.NextBool(2))
            {
                shotTimer = 80f;
                NPC.velocity.Y = Main.rand.NextFloat(-6f, -3f);
                NPC.velocity.X = NPC.velocity.X + (float)NPC.direction * Main.rand.NextFloat(-5f, -3f);
                NPC.netUpdate = true;
            }
            if (NPC.justHit && NPC.Distance(player.Center) > 150 && Main.rand.NextBool(2))
            {
                NPC.velocity.Y = Main.rand.NextFloat(-5f, -2f);
                NPC.velocity.X = NPC.velocity.X + (float)NPC.direction * Main.rand.NextFloat(-5f, 3f);
                NPC.netUpdate = true;
            }

            //Shift toward the player randomly
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (Main.rand.NextBool(200) && NPC.Distance(player.Center) > 260)
                {
                    chargeDamageFlag = true;
                    Vector2 vector8 = new Vector2(NPC.position.X + (NPC.width * 0.5f), NPC.position.Y + (NPC.height / 2));
                    float rotation = (float)Math.Atan2(vector8.Y - (Main.player[NPC.target].position.Y + (Main.player[NPC.target].height * 0.5f)), vector8.X - (Main.player[NPC.target].position.X + (Main.player[NPC.target].width * 0.5f)));
                    NPC.velocity.X = (float)(Math.Cos(rotation) * 10) * -1;
                    NPC.velocity.Y = (float)(Math.Sin(rotation) * 10) * -1;
                    NPC.netUpdate = true;
                }
                if (chargeDamageFlag == true)
                {
                    NPC.damage = 26;
                    chargeDamage++;
                }
                if (chargeDamage >= 55)
                {
                    chargeDamageFlag = false;
                    NPC.damage = 23;
                    chargeDamage = 0;
                }

            }
        }



        #region Find Frame
        public override void FindFrame(int currentFrame)
        {
            int num = 1;
            if (!Main.dedServ)
            {
                num = TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type];
            }
            if (NPC.velocity.Y == 0f)
            {
                if (NPC.direction == 1)
                {
                    NPC.spriteDirection = 1;
                }
                if (NPC.direction == -1)
                {
                    NPC.spriteDirection = -1;
                }
                if (NPC.velocity.X == 0f)
                {
                    NPC.frame.Y = 0;
                    NPC.frameCounter = 0.0;
                }
                else
                {
                    NPC.frameCounter += (double)(Math.Abs(NPC.velocity.X) * .2f);
                    //npc.frameCounter += 1.0;
                    if (NPC.frameCounter > 10)
                    {
                        NPC.frame.Y = NPC.frame.Y + num;
                        NPC.frameCounter = 0;
                    }
                    if (NPC.frame.Y / num >= Main.npcFrameCount[NPC.type])
                    {
                        NPC.frame.Y = num * 1;
                    }
                }
            }
            else
            {
                NPC.frameCounter = 0.0;
                NPC.frame.Y = num;
                NPC.frame.Y = 0;
            }
        }

        #endregion



        #region Debuffs
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Hunter, 3 * 60, false);

            if (Main.rand.NextBool(2))
            {
                target.AddBuff(BuffID.Poisoned, 5 * 60, false);
            }
            if (tsorcRevampWorld.NewSlain.ContainsKey(new NPCDefinition(NPCID.EaterofWorldsHead)))
            {
                target.AddBuff(ModContent.BuffType<CurseBuildup>(), 300 * 60, false); //-20 life if counter hits 100
                target.GetModPlayer<tsorcRevampPlayer>().CurseLevel += 5;
            }
            if (Main.rand.NextBool(10))
            {
                target.AddBuff(BuffID.BrokenArmor, 10 * 60, false);
            }

        }
        #endregion

        public override void OnKill()
        {

            Item.NewItem(NPC.GetSource_Loot(), NPC.getRect(), ItemID.Heart, 1);
            Item.NewItem(NPC.GetSource_Loot(), NPC.getRect(), ItemID.Heart, 1);
            Item.NewItem(NPC.GetSource_Loot(), NPC.getRect(), ItemID.Heart, 1);

            if (!Main.dedServ)
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Parasite Zombie Gore 1").Type, 1.1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Parasite Zombie Gore 2").Type, 1.1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Parasite Zombie Gore 3").Type, 1.1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Parasite Zombie Gore 2").Type, 1.1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Parasite Zombie Gore 1").Type, 1.1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Parasite Zombie Gore 3").Type, 1.1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Parasite Zombie Gore 2").Type, 1.1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Parasite Zombie Gore 3").Type, 1.1f);

                for (int i = 0; i < 10; i++)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Blood Splat").Type, 1.1f);
                }
            }
        }


        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            IItemDropRule hmCondition = new LeadingConditionRule(new Conditions.IsHardmode());
            hmCondition.OnFailedConditions(new CommonDrop(ItemID.HealingPotion, 15, 1, 1, 3));
            npcLoot.Add(hmCondition);
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BloodredMossClump>(), 3));
            npcLoot.Add(new CommonDrop(ItemID.ManaRegenerationPotion, 25, 1, 1, 3));
        }
    }
}