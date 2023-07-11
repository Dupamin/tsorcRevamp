﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using tsorcRevamp.Items.Materials;
using tsorcRevamp.NPCs.Bosses.SuperHardMode.Fiends;

namespace tsorcRevamp.Items.BossItems
{
    public class BossRematchTome : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.useTime = 1;
            Item.useAnimation = 1;
            Item.UseSound = SoundID.Item11;
            Item.useTurn = true;
            Item.noMelee = true;
            Item.value = 10000;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.rare = ItemRarityID.Green;
            Item.shootSpeed = 24f;
            Item.shoot = ModContent.ProjectileType<Projectiles.BlackFirelet>();
        }

        int index = 0;
        List<NPCDefinition> keys;
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            string selectedBoss;
            if(tsorcRevampWorld.NewSlain == null || tsorcRevampWorld.NewSlain.Keys.Count == 0)
            {
                selectedBoss = Language.GetTextValue("Mods.tsorcRevamp.Items.BossRematchTome.None");
            }
            else
            {
                keys = new List<NPCDefinition>(tsorcRevampWorld.NewSlain.Keys);
                RemoveBannedBosses(keys);
                if(keys.Count == 0)
                {
                    selectedBoss = Language.GetTextValue("Mods.tsorcRevamp.Items.BossRematchTome.None");
                }
                else
                {
                    NPC temp = new NPC();
                    temp.SetDefaults(keys[index].Type);

                    selectedBoss = temp.GivenOrTypeName;

                    if (selectedBoss.Contains("Slogra"))
                    {
                        selectedBoss = Language.GetTextValue("Mods.tsorcRevamp.Items.BossRematchTome.Duo1");
                    }
                }
            }
            
            tooltips.Add(new TooltipLine(ModContent.GetInstance<tsorcRevamp>(), "selectedboss", Language.GetTextValue("Mods.tsorcRevamp.Items.BossRematchTome.Selected") + selectedBoss));
            base.ModifyTooltips(tooltips);
        }



       

        

        public static List<int> PreHardmodeBossIDs = new List<int>
        {
            ModContent.NPCType<NPCs.Special.LeonhardPhase1>(),
            ModContent.NPCType<NPCs.Enemies.RedKnight>(),
            NPCID.EyeofCthulhu,
            NPCID.BrainofCthulhu,
            NPCID.EaterofWorldsHead,
            ModContent.NPCType<NPCs.Bosses.AncientOolacileDemon>(),
            NPCID.KingSlime,
            ModContent.NPCType<NPCs.Bosses.Slogra>(),
            NPCID.QueenBee,
            NPCID.Skeleton,
            NPCID.Deerclops,
            ModContent.NPCType<NPCs.Bosses.JungleWyvern.JungleWyvernHead>(),
            ModContent.NPCType<NPCs.Bosses.AncientDemon>(),
            NPCID.WallofFlesh
        };
                
        public static List<int> HardmodeBossIDs = new List<int>
        {
            NPCID.QueenSlimeBoss,
            ModContent.NPCType<NPCs.Bosses.TheRage>(),
            ModContent.NPCType<NPCs.Bosses.WyvernMage.WyvernMage>(),
            ModContent.NPCType<NPCs.Bosses.TheSorrow>(),
            ModContent.NPCType<NPCs.Bosses.Serris.SerrisX>(),
            ModContent.NPCType<NPCs.Bosses.Death>(),
            ModContent.NPCType<NPCs.Bosses.TheHunter>(),
            NPCID.TheDestroyer,
            ModContent.NPCType<NPCs.Bosses.PrimeV2.TheMachine>(),
            ModContent.NPCType<NPCs.Bosses.Cataluminance>(),
            NPCID.Plantera,
            NPCID.Golem,
            NPCID.HallowBoss,
            ModContent.NPCType<NPCs.Bosses.Okiku.FirstForm.DarkShogunMask>(),
            ModContent.NPCType<NPCs.Bosses.Okiku.FinalForm.Attraidies>()
        };

        
        public static List<int> SHMBossIDs = new List<int>
        {
            ModContent.NPCType<NPCs.Bosses.SuperHardMode.HellkiteDragon.HellkiteDragonHead>(),
            ModContent.NPCType<NPCs.Bosses.SuperHardMode.Witchking>(),
            NPCID.MoonLordCore,
            ModContent.NPCType<NPCs.Bosses.SuperHardMode.Fiends.WaterFiendKraken>(),
            ModContent.NPCType<NPCs.Bosses.SuperHardMode.Fiends.EarthFiendLich>(),
            ModContent.NPCType<NPCs.Bosses.SuperHardMode.Fiends.FireFiendMarilith>(),
            ModContent.NPCType<NPCs.Bosses.SuperHardMode.Blight>(),
            ModContent.NPCType<NPCs.Bosses.SuperHardMode.AbysmalOolacileSorcerer>(),
            ModContent.NPCType<NPCs.Bosses.SuperHardMode.Artorias>(),
            ModContent.NPCType<NPCs.Bosses.SuperHardMode.Seath.SeathTheScalelessHead>(),
            ModContent.NPCType<NPCs.Bosses.SuperHardMode.GhostWyvernMage.WyvernMageShadow>(),
            ModContent.NPCType<NPCs.Bosses.SuperHardMode.Chaos>(),
            ModContent.NPCType<NPCs.Bosses.SuperHardMode.DarkCloud>(),
            ModContent.NPCType<NPCs.Bosses.SuperHardMode.Gwyn>(),
        };

        //Calls PhaseBossIDs for each phase, and glues the lists together with a tuple.
        public static Tuple<List<NPCDefinition>, List<NPCDefinition>, List<NPCDefinition>> GetDownedBossIDs()
        {
            return new Tuple<List<NPCDefinition>, List<NPCDefinition>, List<NPCDefinition>>(PhaseBossIDs(PreHardmodeBossIDs), PhaseBossIDs(HardmodeBossIDs), PhaseBossIDs(SHMBossIDs));
        }

        //Returns a list of all bosses downed during one phase of the game
        public static List<NPCDefinition> PhaseBossIDs(List<int> idList)
        {
            List<NPCDefinition> result = new List<NPCDefinition>();
            foreach (int id in idList)
            {
                if (tsorcRevampWorld.NewSlain.ContainsKey(new NPCDefinition(id)))
                {
                    result.Add(new NPCDefinition(id));
                }
                else
                {
                    result.Add(new NPCDefinition(NPCID.Bunny));
                }
            }

            return result;
        }

        //TODO: Make it work like the Grand Design
        public override bool? UseItem(Player player)
        {
            keys = new List<NPCDefinition>(tsorcRevampWorld.NewSlain.Keys);
            if(player.whoAmI != Main.myPlayer || Main.netMode == NetmodeID.Server)
            {
                return false;
            }

            RemoveBannedBosses(keys);
            if (tsorcRevampWorld.NewSlain == null || tsorcRevampWorld.NewSlain.Keys.Count == 0 || keys.Count == 0)
            {
                UsefulFunctions.BroadcastText(Language.GetTextValue("Mods.tsorcRevamp.Items.BossRematchTome.None"));
                return true;
            }

            if (player.altFunctionUse == 2)
            {                
                if (keys.Count > 1)
                {
                    if (index >= keys.Count - 1)
                    {
                        index = 0;
                    }
                    else
                    {
                        index++;
                    }
                }
                NPC temp = new NPC();
                temp.SetDefaults(keys[index].Type);

                string selectedBoss = temp.GivenOrTypeName;

                if (selectedBoss.Contains("Slogra"))
                {
                    selectedBoss = Language.GetTextValue("Mods.tsorcRevamp.Items.BossRematchTome.Duo1");
                }
                if (keys[index].Type == ModContent.NPCType<NPCs.Bosses.Okiku.FirstForm.DarkShogunMask>())
                {
                    selectedBoss = Language.GetTextValue("Mods.tsorcRevamp.Items.BossRematchTome.Attraidies1");
                }
                if (keys[index].Type == ModContent.NPCType<NPCs.Bosses.Okiku.FinalForm.Attraidies>())
                {
                    selectedBoss = Language.GetTextValue("Mods.tsorcRevamp.Items.BossRematchTome.Attraidies2");
                }

                UsefulFunctions.BroadcastText(Language.GetTextValue("Mods.tsorcRevamp.Items.BossRematchTome.Selected") + selectedBoss);
            }
            else
            {
                
                if (!tsorcRevampWorld.BossAlive)
                {
                    if (keys[index].Type == ModContent.NPCType<NPCs.Bosses.Slogra>())
                    {
                        if (Main.netMode == NetmodeID.SinglePlayer)
                        {
                            NPC.NewNPCDirect(Item.GetSource_FromThis(), player.Center + new Vector2(0, -300), ModContent.NPCType<NPCs.Bosses.Gaibon>());
                        }
                        else
                        {
                            ModPacket spawnNPCPacket = ModContent.GetInstance<tsorcRevamp>().GetPacket();
                            spawnNPCPacket.Write(tsorcPacketID.SpawnNPC);
                            spawnNPCPacket.Write(ModContent.NPCType<NPCs.Bosses.Gaibon>());
                            spawnNPCPacket.WriteVector2(player.Center + new Vector2(0, -300));
                            spawnNPCPacket.Send();
                        }
                    }
                    if (Main.netMode == NetmodeID.SinglePlayer)
                    {
                        NPC.NewNPCDirect(Item.GetSource_FromThis(), player.Center + new Vector2(0, -300), keys[index].Type);
                    }
                    else
                    {
                        ModPacket spawnNPCPacket = ModContent.GetInstance<tsorcRevamp>().GetPacket();
                        spawnNPCPacket.Write(tsorcPacketID.SpawnNPC);
                        spawnNPCPacket.Write(keys[index].Type);
                        spawnNPCPacket.WriteVector2(player.Center + new Vector2(0, -300));
                        spawnNPCPacket.Send();
                    }
                }
                else
                {
                    UsefulFunctions.BroadcastText(Language.GetTextValue("Mods.tsorcRevamp.Items.BossRematchTome.Forbidden"));
                }
            }
            return base.UseItem(player);
        }

        public void RemoveBannedBosses(List<NPCDefinition> keys)
        {
            //These are pieces or alternate phases of bosses which automatically get spawned by their 'parent' boss and should not be spawned on their own
            List<int> bannedBosses = new List<int>();
            bannedBosses.Add(ModContent.NPCType<NPCs.Bosses.Gaibon>());
            bannedBosses.Add(ModContent.NPCType<NPCs.Bosses.WyvernMage.MechaDragonHead>());
            bannedBosses.Add(ModContent.NPCType<NPCs.Bosses.Serris.SerrisX>());
            bannedBosses.Add(ModContent.NPCType<LichKingDisciple>());
            bannedBosses.Add(ModContent.NPCType<LichKingSerpentHead>());
            bannedBosses.Add(ModContent.NPCType<NPCs.Bosses.SuperHardMode.DarkCloudMirror>());
            bannedBosses.Add(ModContent.NPCType<NPCs.Bosses.SuperHardMode.GhostWyvernMage.GhostDragonHead>());
            bannedBosses.Add(NPCID.EaterofWorldsBody);
            bannedBosses.Add(NPCID.EaterofWorldsTail);

            //These are bugged and need to be fixed
            bannedBosses.Add(ModContent.NPCType<NPCs.Bosses.JungleWyvern.JungleWyvernHead>()); //Head is detached from its body
            bannedBosses.Add(ModContent.NPCType<NPCs.Bosses.SuperHardMode.HellkiteDragon.HellkiteDragonHead>()); //Head is detached from its body
            bannedBosses.Add(ModContent.NPCType<NPCs.Bosses.SuperHardMode.Seath.SeathTheScalelessHead>()); //Head is detached from its body
            bannedBosses.Add(ModContent.NPCType<NPCs.Bosses.Serris.SerrisHead>()); //Just spawns the head, nothing more
            bannedBosses.Add(ModContent.NPCType<NPCs.Enemies.RedKnight>()); //Bugged loot

            //Check if the keys list has any of the banned bosses, and if so remove them
            for (int i = 0; i < bannedBosses.Count; i++)
            {
                if (keys.Contains(new NPCDefinition(bannedBosses[i])))
                {
                    keys.Remove(new NPCDefinition(bannedBosses[i]));
                }
            }
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 speed, int type, int damage, float knockBack)
        {
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<DarkSoul>(), 25);
            recipe.AddIngredient(ItemID.Book, 1);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register(); 

            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ModContent.ItemType<DarkSoul>(), 25);
            recipe2.AddIngredient(ItemID.SpellTome, 1);
            recipe2.AddTile(TileID.DemonAltar);
            recipe2.Register();
        }
    }
}