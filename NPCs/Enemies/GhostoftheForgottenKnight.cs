using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using tsorcRevamp.Items.Materials;

namespace tsorcRevamp.NPCs.Enemies
{
    class GhostoftheForgottenKnight : ModNPC
    {
        public int spearDamage = 15;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 16;
        }
        public override void SetDefaults()
        {
            NPC.npcSlots = 3;
            AnimationType = 28;
            NPC.aiStyle = -1;
            NPC.height = 40;
            NPC.width = 20;
            NPC.damage = 30;
            NPC.defense = 22;
            NPC.lifeMax = 150;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.lavaImmune = true;
            NPC.value = 450;
            NPC.knockBackResist = 0.0f;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Banners.GhostOfTheForgottenKnightBanner>();
            if (Main.hardMode)
            {
                NPC.lifeMax = 200;
                NPC.defense = 32;
                NPC.value = 650;
                NPC.damage = 40;
                spearDamage = 25;
                topSpeed = 1.5f;
            }

            if (tsorcRevampWorld.SuperHardMode)
            {
                NPC.lifeMax = 900;
                NPC.defense = 70;
                NPC.damage = 50;
                NPC.value = 1000;
                spearDamage = 45;
                topSpeed = 2f;
            }
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {

            if (!Main.hardMode && NPC.downedBoss3 && spawnInfo.Player.ZoneDungeon)
            {
                return 0.2f; //.16 should be 8%
            }
            if (Main.hardMode && spawnInfo.Player.ZoneDungeon)
            {
                return 0.17f;
            }
            if (tsorcRevampWorld.SuperHardMode && spawnInfo.Player.ZoneDungeon)
            {
                return 0.08f; //.08% is 4.28%
            }

            return 0;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (Main.rand.NextBool(2))
            {
                target.AddBuff(BuffID.Weak, 60 * 60, false);
            }
        }

        float spearTimer = 0;
        float topSpeed = 1.2f;
        public override void AI()
        {
            tsorcRevampAIs.FighterAI(NPC, topSpeed, .05f, 0.2f, true, enragePercent: 0.2f, enrageTopSpeed: 2.4f);

            bool canFire = NPC.Distance(Main.player[NPC.target].Center) < 1600 && Collision.CanHitLine(NPC.Center, 0, 0, Main.player[NPC.target].Center, 0, 0);
            tsorcRevampAIs.SimpleProjectile(NPC, ref spearTimer, 180, ModContent.ProjectileType<Projectiles.Enemy.BlackKnightSpear>(), 20, 8, canFire, true, SoundID.Item17);

            if (NPC.justHit && spearTimer <= 149 && Main.rand.NextBool(4))
            {
                spearTimer = 0f;
            }
        }

        #region Draw Spear Texture
        static Texture2D spearTexture;
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (spearTexture == null)
            {
                spearTexture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/Enemy/BlackKnightGhostSpear");
            }
            if (spearTimer >= 150)
            {
                Lighting.AddLight(NPC.Center, Color.White.ToVector3() * 0.3f); //Pick a color, any color. The 0.5f tones down its intensity by 50%
                if (Main.rand.NextBool(3))
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Smoke, NPC.velocity.X, NPC.velocity.Y);
                }
                SpriteEffects effects = NPC.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                if (NPC.spriteDirection == -1)
                {
                    spriteBatch.Draw(spearTexture, NPC.Center - Main.screenPosition, new Rectangle(0, 0, spearTexture.Width, spearTexture.Height), drawColor, -MathHelper.PiOver2, new Vector2(8, 38), NPC.scale, effects, 0); // facing left (8, 38 work)
                }
                else
                {
                    spriteBatch.Draw(spearTexture, NPC.Center - Main.screenPosition, new Rectangle(0, 0, spearTexture.Width, spearTexture.Height), drawColor, MathHelper.PiOver2, new Vector2(8, 38), NPC.scale, effects, 0); // facing right, first value is height, higher number is higher
                }
            }
        }
        #endregion

        #region Gore
        public override void OnKill()
        {
            if (!Main.dedServ)
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Black Knight Gore 1").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Black Knight Gore 2").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Black Knight Gore 3").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Black Knight Gore 2").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Black Knight Gore 3").Type, 1f);
            }
        }
        #endregion

        public override void ModifyNPCLoot(NPCLoot npcLoot) {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Ranged.Thrown.RoyalThrowingSpear>(), 4, 25, 35));
            npcLoot.Add(new CommonDrop(ModContent.ItemType<Items.Weapons.Ranged.Thrown.EphemeralThrowingSpear>(), 5, 25, 30, 2));
            npcLoot.Add(new CommonDrop(ModContent.ItemType<Items.Potions.HealingElixir>(), 5, 1, 1, 2));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EphemeralDust>(), 1, 3, 9));
            npcLoot.Add(ItemDropRule.Common(ItemID.IronskinPotion, 35));
            npcLoot.Add(ItemDropRule.Common(ItemID.GreaterHealingPotion, 35));
            npcLoot.Add(new CommonDrop(ItemID.HunterPotion, 100, 1, 1, 8));
            npcLoot.Add(new CommonDrop(ItemID.RegenerationPotion, 100, 1, 1, 6));
            npcLoot.Add(new CommonDrop(ItemID.ShinePotion, 100, 1, 1, 30));
            npcLoot.Add(ItemDropRule.Common(ItemID.BattlePotion, 30));
            npcLoot.Add(ItemDropRule.Common(ItemID.GoldenKey, 20));
            IItemDropRule hmCondition = new LeadingConditionRule(new Conditions.IsHardmode());
            hmCondition.OnSuccess(ItemDropRule.Common(ItemID.SoulofNight));
            npcLoot.Add(hmCondition);
        }
    }
}