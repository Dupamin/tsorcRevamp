﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using tsorcRevamp.Buffs.Debuffs;
using tsorcRevamp.Items.Materials;
using tsorcRevamp.Items.Weapons.Magic;
using tsorcRevamp.Utilities;

namespace tsorcRevamp.NPCs.Bosses.SuperHardMode
{

    [AutoloadBossHead]
    class Blight : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn2] = true;
        }
        public override void SetDefaults()
        {
            NPC.npcSlots = 5;
            NPC.width = 40;
            NPC.height = 110;
            NPC.aiStyle = -1;
            NPC.timeLeft = 22500;
            NPC.damage = 53;
            NPC.defense = 90;
            NPC.HitSound = SoundID.NPCHit3;
            NPC.DeathSound = SoundID.Zombie53;
            // npc.DeathSound = SoundID.NPCDeath43;
            NPC.lifeMax = 250000;
            NPC.knockBackResist = 0f;
            NPC.scale = 1f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.value = 650000;
            NPC.rarity = 37;
            NPC.friendly = false;
            NPC.alpha = 255;
            NPC.boss = true;
            despawnHandler = new NPCDespawnHandler(LangUtils.GetTextValue("NPCs.Blight.DespawnHandler"), new Color(255, 50, 50), DustID.Firework_Blue);
        }

        int phantomSeekerDamage = 58;
        int cometDamage = 50;
        int darkAstronomyDamage = 60;
        int antimatterCannonDamage = 70;

        //chaos
        int holdTimer = 0;
        int chargeDamage = 0;
        bool chargeDamageFlag = false;


        int phase = 700;
        int attackindex = 0;
        float spazzlevel;
        float targetspazzlevel;
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (Main.rand.NextBool(4))
            {
                target.AddBuff(BuffID.BrokenArmor, 3 * 60, false); //broken armor
                target.AddBuff(BuffID.Poisoned, 60 * 60, false); //poisoned
                target.AddBuff(BuffID.Bleeding, 30 * 60, false); //bleeding
            }

            if (Main.rand.NextBool(2))
            {
                target.AddBuff(BuffID.BrokenArmor, 3 * 60, false); //broken armor
                target.AddBuff(BuffID.CursedInferno, 3 * 60, false); //cursed inferno
                //player.AddBuff("Powerful Curse Buildup", 18000, false); //chance to lose -20 life for 5 minutes
                target.AddBuff(ModContent.BuffType<CurseBuildup>(), 300 * 60, false);
            }
        }

        NPCDespawnHandler despawnHandler;
        public override void AI()
        {
            despawnHandler.TargetAndDespawn(NPC.whoAmI);
            int num54;




            //If it's too far away, target the closest player and charge them
            if (Math.Abs(Main.player[NPC.target].position.X - NPC.position.X) > 2800 || Math.Abs(Main.player[NPC.target].position.Y - NPC.position.Y) > 2200)
            {
                if (Main.rand.NextBool(450))
                {
                    chargeDamageFlag = true;
                    Vector2 vector8 = new Vector2(NPC.position.X + (NPC.width * 0.5f), NPC.position.Y + (NPC.height / 2));
                    float rotation = (float)Math.Atan2(vector8.Y - (Main.player[NPC.target].position.Y + (Main.player[NPC.target].height * 0.5f)), vector8.X - (Main.player[NPC.target].position.X + (Main.player[NPC.target].width * 0.5f)));
                    NPC.velocity.X = (float)(Math.Cos(rotation) * 12) * -1;
                    NPC.velocity.Y = (float)(Math.Sin(rotation) * 12) * -1;
                    NPC.ai[1] = 1f;
                    NPC.netUpdate = true;
                }
                if (chargeDamageFlag == true)
                {
                    NPC.damage = 130;
                    chargeDamage++;
                }
                if (chargeDamage >= 50)
                {
                    chargeDamageFlag = false;
                    NPC.damage = 80;
                    chargeDamage = 0;
                }
            }

            if (Main.player[NPC.target].position.Y - 100 > NPC.position.Y)
            {

                Color color = new Color();
                int dust = Dust.NewDust(new Vector2((float)NPC.position.X, (float)NPC.position.Y), NPC.width, NPC.height, 15, NPC.velocity.X, NPC.velocity.Y, 250, color, 5f);
                Main.dust[dust].noGravity = true;


                NPC.directionY = 1;
            }
            else
            {

                Color color = new Color();
                int dust = Dust.NewDust(new Vector2((float)NPC.position.X, (float)NPC.position.Y), NPC.width, NPC.height, 15, NPC.velocity.X, NPC.velocity.Y, 250, color, 4f);
                Main.dust[dust].noGravity = false;


                NPC.directionY = -1;
            }

            if (Main.player[NPC.target].position.X - 250 > NPC.position.X)
            {

                Color color = new Color();
                int dust = Dust.NewDust(new Vector2((float)NPC.position.X, (float)NPC.position.Y), NPC.width, NPC.height, 15, NPC.velocity.X, NPC.velocity.Y, 250, color, 2f);
                Main.dust[dust].noGravity = false;


                NPC.direction = 1;
            }
            else
            {

                Color color = new Color();
                int dust = Dust.NewDust(new Vector2((float)NPC.position.X, (float)NPC.position.Y), NPC.width, NPC.height, 15, NPC.velocity.X, NPC.velocity.Y, 250, color, 5f);
                Main.dust[dust].noGravity = false;


                NPC.direction = -1;
            }

            NPC.spriteDirection = 1;

            NPC.frame.Width = 40;


            if (attackindex == 0)
            {
                NPC.frame.Y = 58;
            }

            NPC.ai[1] += (Main.rand.Next(2, 5) * 0.1f) * NPC.scale;
            if (NPC.ai[1] >= 8f)
            {
                if (Main.rand.NextBool(450))
                {
                    chargeDamageFlag = true;
                    Vector2 vector8 = new Vector2(NPC.position.X + (NPC.width * 0.5f), NPC.position.Y + (NPC.height / 2));
                    float rotation = (float)Math.Atan2(vector8.Y - (Main.player[NPC.target].position.Y + (Main.player[NPC.target].height * 0.5f)), vector8.X - (Main.player[NPC.target].position.X + (Main.player[NPC.target].width * 0.5f)));
                    NPC.velocity.X = (float)(Math.Cos(rotation) * 12) * -1;
                    NPC.velocity.Y = (float)(Math.Sin(rotation) * 12) * -1;
                    NPC.ai[1] = 1f;
                    NPC.netUpdate = true;
                }
                if (chargeDamageFlag == true)
                {
                    NPC.damage = 130;
                    chargeDamage++;
                }
                if (chargeDamage >= 50)
                {
                    chargeDamageFlag = false;
                    NPC.damage = 80;
                    chargeDamage = 0;
                }
            }


            if (NPC.direction == -1 && NPC.velocity.X > -2f)
            {
                NPC.velocity.X = NPC.velocity.X - 0.1f;
                if (NPC.velocity.X > 2f)
                {
                    NPC.velocity.X = NPC.velocity.X - 0.1f;
                }
                else
                {
                    if (NPC.velocity.X > 0f)
                    {
                        NPC.velocity.X = NPC.velocity.X + 0.05f;
                    }
                }
                if (NPC.velocity.X < -2f)
                {
                    NPC.velocity.X = -2f;
                }
            }
            else
            {
                if (NPC.direction == 1 && NPC.velocity.X < 2f)
                {
                    NPC.velocity.X = NPC.velocity.X + 0.1f;
                    if (NPC.velocity.X < -2f)
                    {
                        NPC.velocity.X = NPC.velocity.X + 0.1f;
                    }
                    else
                    {
                        if (NPC.velocity.X < 0f)
                        {
                            NPC.velocity.X = NPC.velocity.X - 0.05f;
                        }
                    }
                    if (NPC.velocity.X > 2f)
                    {
                        NPC.velocity.X = 2f;
                    }
                }
            }
            if (NPC.directionY == -1 && (double)NPC.velocity.Y > -1.5)
            {
                NPC.velocity.Y = NPC.velocity.Y - 0.05f;

                if ((double)NPC.velocity.Y < -1.5)
                {
                    NPC.velocity.Y = -1.5f;
                }
            }
            else
            {
                if (NPC.directionY == 1 && (double)NPC.velocity.Y < 1.5)
                {
                    NPC.velocity.Y = NPC.velocity.Y + 0.05f;
                    if ((double)NPC.velocity.Y > 1.5)
                    {
                        NPC.velocity.Y = 1.5f;
                    }
                }
            }

            //Deal with dat crazy spazzin' out;
            spazzlevel += (targetspazzlevel - spazzlevel) / 60f;

            //Cycle through all attacks linearly		
            if (phase > 1000)
            {
                phase = 0;

                //Chill for a few seconds after either of these attacks, because their projectiles linger
                if (attackindex == 2 || attackindex == 3)
                {
                    phase += 150;
                    attackindex = 0;
                }
                else
                {
                    attackindex = Main.rand.Next(1, 5);
                }
            }


            phase++;

            // If we're almost dead, activate the Cataclysm
            if (NPC.life < NPC.lifeMax / 20)
            {
                attackindex = 5;
            }

            //Actual attacks
            //Condemnation - Phantom Seeker
            if (attackindex == 1)
            {
                targetspazzlevel = 0;


                if ((Main.GameUpdateCount % 60) < 1)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            num54 = Projectile.NewProjectile(NPC.GetSource_FromThis(), new Vector2(NPC.position.X + 20, NPC.position.Y + 50), new Vector2(Main.rand.Next(-5, 5), Main.rand.Next(-5, 5)), ModContent.ProjectileType<Projectiles.PhantomSeeker>(), phantomSeekerDamage, 0f, Main.myPlayer); //Phantom Seeker
                            Main.projectile[num54].timeLeft = 400;
                            Main.projectile[num54].rotation = Main.rand.Next(700) / 100f;
                            Main.projectile[num54].ai[0] = NPC.target;
                        } 
                    }
                }
            }


            //Antimatter - Black Comet
            else if (attackindex == 3)
            {
                targetspazzlevel = 10;
                if ((Main.GameUpdateCount % 5) < 1)
                {
                    float posX = Main.player[NPC.target].position.X + Main.rand.Next(-1400, 1400);
                    /** int spread = Main.rand.Next(3);
                     if (spread < 2) {
                         if (spread == 1){
                             posX += 1200;
                         } else
                         {
                             posX -= 1200;
                         }
                      }**/
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        num54 = Projectile.NewProjectile(NPC.GetSource_FromThis(), posX, Main.player[NPC.target].position.Y - 650, 0, 5, ModContent.ProjectileType<Projectiles.Comet>(), cometDamage, 0f, Main.myPlayer); //Comet
                        Main.projectile[num54].ai[1] = 5.5f; //Velocity
                    }
                }
            }


            //Dark Astronomy - Black Spiral
            else if (attackindex == 2)
            {
                targetspazzlevel = 25;

                if ((Main.GameUpdateCount % 60) < 1)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            num54 = Projectile.NewProjectile(NPC.GetSource_FromThis(), new Vector2(NPC.position.X + 20, NPC.position.Y + 50), new Vector2(0, 0), ModContent.ProjectileType<Projectiles.PhantomSpiral>(), darkAstronomyDamage, 0f, Main.myPlayer); //Phantom Spiral
                            Main.projectile[num54].timeLeft = 1000;
                            Main.projectile[num54].rotation = Main.rand.Next(700) / 100f;
                            Main.projectile[num54].ai[0] = NPC.whoAmI;
                            Main.projectile[num54].ai[1] = Main.rand.Next(200, 2500);
                        }
                    }
                }
            }

            //Annihilation - Antimatter Cannon
            else if (attackindex == 4)
            {
                float j = (float)Math.Atan2((double)(NPC.position.X - Main.player[NPC.target].position.X), (double)(NPC.position.Y - Main.player[NPC.target].position.Y + 48));

                targetspazzlevel = 50;
                NPC.velocity.Y = 0;
                NPC.velocity.X = 0;
                if ((Main.GameUpdateCount % 5) < 1 && phase > 100)
                {
                    phase += 10;
                    for (int i = 0; i < 6; i++)
                    {
                        int s = Main.rand.Next(2, 10);
                        float m = (float)Math.Sin(j) * -s;
                        float n = (float)Math.Cos(j) * -s;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            num54 = Projectile.NewProjectile(NPC.GetSource_FromThis(), new Vector2(NPC.position.X + Main.rand.Next(-25, 25), NPC.position.Y + Main.rand.Next(50, 150)), new Vector2(m, n), ModContent.ProjectileType<Projectiles.Comet>(), antimatterCannonDamage, 0f, Main.myPlayer); //Antimatter Cannon
                            Main.projectile[num54].scale = (Main.rand.Next(50, 100)) / 75f;
                            Main.projectile[num54].timeLeft = 300;
                            Main.projectile[num54].ai[1] = 10; //Velocity
                        }
                    }
                }
            }
            else
            {
                targetspazzlevel = 0;
            }

            //chaos defense move
            if (holdTimer > 0)
            {
                holdTimer--;
            }

            if (Vector2.Distance(NPC.Center, Main.player[NPC.target].Center) > 1600)
            {
                NPC.defense = 9999;
                if (holdTimer <= 0 && Main.netMode != NetmodeID.Server)
                {
                    Main.NewText(LangUtils.GetTextValue("NPCs.Blight.OutOfRange"), 45, 75, 255);
                    holdTimer = 200;
                }
            }
            else
            {
                NPC.defense = 90;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {

            Texture2D Noise_Texture = TextureAssets.Npc[this.NPC.type].Value;
            Random rand1 = new Random((int)Main.GameUpdateCount);
            int height = this.NPC.frame.Height;
            int width = this.NPC.frame.Width;
            int offsetx = this.NPC.frame.X;
            int offsety = this.NPC.frame.Y;
            float targetscale = 1f;
            Rectangle fromrect = new Rectangle(offsetx, offsety, width, height);
            Vector2 PC;
            SpriteEffects mydirection;
            if (this.NPC.spriteDirection >= 0) mydirection = SpriteEffects.FlipHorizontally;
            else mydirection = SpriteEffects.None;
            for (int i = 0; i < 5; i++)
            {

                PC = this.NPC.position;
                PC.Y += this.NPC.height * 2.5f;

                PC.X += rand1.Next((int)-spazzlevel, (int)spazzlevel);
                PC.Y += rand1.Next((int)-spazzlevel, (int)spazzlevel);
                Color targetColor = new Color(0, 0, 0, 0);
                spriteBatch.Draw(
                            Noise_Texture,
                            PC - Main.screenPosition,
                            fromrect,
                            targetColor,
                            this.NPC.rotation,//Main.rand.Next(600)/100, 
                            new Vector2(0, 0),
                            targetscale * 1.04f,
                            mydirection,
                            0f);
            }

            rand1 = new Random((int)Main.GameUpdateCount);
            for (int i = 0; i < 5; i++)
            {
                PC = this.NPC.position;

                PC.Y += this.NPC.height * 0.1f;

                PC.X += rand1.Next((int)-spazzlevel, (int)spazzlevel);
                PC.Y += rand1.Next((int)-spazzlevel, (int)spazzlevel);
                Color targetColor = new Color(0, 0, 0, 0);
                spriteBatch.Draw(
                            Noise_Texture,
                            PC - Main.screenPosition,
                            fromrect,
                            targetColor,
                            this.NPC.rotation,//Main.rand.Next(600)/100, 
                            new Vector2(0, 0),
                            targetscale,
                            mydirection,
                            0f);
            }

            return false;
        }
        public override bool CheckActive()
        {
            return false;
        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<Items.BossBags.BlightBag>()));
            npcLoot.Add(ItemDropRule.ByCondition(tsorcRevamp.tsorcItemDropRuleConditions.NonExpertFirstKillRule, ModContent.ItemType<GuardianSoul>()));
            IItemDropRule notExpertCondition = new LeadingConditionRule(new Conditions.NotExpert());
            notExpertCondition.OnSuccess(ItemDropRule.Common(ModContent.ItemType<DivineSpark>()));
            notExpertCondition.OnSuccess(ItemDropRule.Common(ModContent.ItemType<SoulOfBlight>(), 1, 2, 4));
            npcLoot.Add(notExpertCondition);
        }
    }
}
