using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using tsorcRevamp.Items;
using Terraria.UI;
using Terraria.GameContent.UI;
using System.Collections;
using tsorcRevamp.UI;
using System;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using static tsorcRevamp.MethodSwaps;
using System.IO;
using Terraria.ModLoader.IO;
using Terraria.Graphics.Shaders;
using Terraria.Graphics.Effects;

namespace tsorcRevamp {

   
    public class tsorcRevamp : Mod {

        public static ModHotKey toggleDragoonBoots;
        public static ModHotKey reflectionShiftKey;
        public static bool isAdventureMap = false;
        public static int DarkSoulCustomCurrencyId;
        public static List<int> KillAllowed;
        public static List<int> PlaceAllowed;
        public static List<int> Unbreakable;
        public static List<int> IgnoredTiles;
        public static List<int> CrossModTiles;
        public static List<int> PlaceAllowedModTiles;

        internal BonfireUIState BonfireUIState;
        private UserInterface _bonfireUIState;
        internal DarkSoulCounterUIState DarkSoulCounterUIState;
        private UserInterface _darkSoulCounterUIState;
        internal UserInterface EmeraldHeraldUserInterface;


        public static Effect TheAbyssEffect;
        //public static Effect AttraidiesEffect;

        public override void Load() {
            toggleDragoonBoots = RegisterHotKey("Dragoon Boots", "Z");
            reflectionShiftKey = RegisterHotKey("Reflection Shift", "O");

            DarkSoulCustomCurrencyId = CustomCurrencyManager.RegisterCurrency(new DarkSoulCustomCurrency(ModContent.ItemType<SoulShekel>(), 99999L));

            BonfireUIState = new BonfireUIState();
            if (!Main.dedServ) BonfireUIState.Activate();
            _bonfireUIState = new UserInterface();
            if (!Main.dedServ) _bonfireUIState.SetState(BonfireUIState);

            DarkSoulCounterUIState = new DarkSoulCounterUIState();
            //if (!Main.dedServ) DarkSoulCounterUIState.Activate();
            _darkSoulCounterUIState = new UserInterface();
            if (!Main.dedServ) _darkSoulCounterUIState.SetState(DarkSoulCounterUIState);

            
            ApplyMethodSwaps();
            PopulateArrays();
            if(!Main.dedServ) TransparentTextureHandler.TransparentTextureFix();

            IL.Terraria.Player.Update += Player_Update;
            IL.Terraria.Player.Update += Chest_Patch;
            if (!Main.dedServ) {
                tsorcRevamp Instance = this;
                TheAbyssEffect = Instance.GetEffect("Effects/ScreenFilters/TheAbyssShader");
                Filters.Scene["tsorcRevamp:TheAbyss"] = new Filter(new ScreenShaderData(new Terraria.Ref<Effect>(TheAbyssEffect), "TheAbyssShaderPass").UseImage("Images/Misc/noise"), EffectPriority.Low);

                //AttraidiesEffect = Instance.GetEffect("Effects/ScreenFilters/AttraidiesShader");
                //Filters.Scene["tsorcRevamp:AttraidiesShader"] = new Filter(new ScreenShaderData(new Terraria.Ref<Effect>(AttraidiesEffect), "AttraidiesShaderPass").UseImage("Images/Misc/noise"), EffectPriority.Low);
               
                EmeraldHeraldUserInterface = new UserInterface();
            }
        }


        public override void UpdateUI(GameTime gameTime) {
            if (BonfireUIState.Visible) {
                _bonfireUIState?.Update(gameTime);
            }
            if (DarkSoulCounterUIState.Visible)
            {
                _darkSoulCounterUIState?.Update(gameTime);
            }

            EmeraldHeraldUserInterface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1) {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "tsorcRevamp: BonfireUI",
                    delegate {
                        if (BonfireUIState.Visible) {

                            _bonfireUIState.Draw(Main.spriteBatch, new GameTime());
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }

            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (resourceBarIndex != -1) {
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "tsorcRevamp: Dark Soul Counter UI",
                    delegate {
                        _darkSoulCounterUIState.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }

            int inventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
            if (inventoryIndex != -1)
            {
                layers.Insert(inventoryIndex, new LegacyGameInterfaceLayer(
                    "tsorcRevamp: Emerald Herald UI",
                    delegate {
                        EmeraldHeraldUserInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }


        private void PopulateArrays() {
            #region KillAllowed list
            KillAllowed = new List<int>() {
                2, //grass
                3, //small plants
                4, // torch
                5, //tree trunk
                6, //iron
                7, //copper
                9, //silver
                10, //closed door
                11, //open door
                12, //Heart crystal
                13, //bottles and jugs
                14, //table
                15, //chairs
                16, //anvil
                17, //furnance
                18, //workbench
                20, //sapling
                21, //chests
                23, //corruption grass
                24, //small corruption plants
                27, //sunflower
                28, //pot
                29, //piggy bank
                31, //shadow orb
                32, //corruption barbs
                33, //candle
                34, //bronze chandellier
                35, //silver c
                36, //gold c
                37, //meteorite
                42, //chain lantern
                49, //water candle
                50, //books
                51, //cobweb
                52, //vines
                53, //sand
                55, //Sign 
                //56, //obsidian (removed at tim's request)
                60, //jungle grass
                61, //small jungle plants
                62, //jungle vines
                67, //Amethyst 
                69, //thorns
                72, //mushroom stalks
                71, //small mushrooms
                73, //plants
                74, //plants
                78, //clay pot
                79, //bed
                80, //cactus
                81, //corals
                82, //new herbs
                83, //grown herbs
                84, //bloomed herbs
                85, //tombstone
                86, //loom
                87, //piano
                88, //drawer
                89, //bench
                90, //bathtub
                91, //banner
                92, //lamp post
                93, //tiki torch
                94, //keg
                95, //chinese lantern
                96, //cooking pot
                97, //safe
                98, //skull candle
                99, //trash can
                100, //candlabra
                101, //bookcase
                102, //throne
                103, //bowl
                104, //grandfather clock
                105, //statue
                106, //sawmill
                107, //cobalt
                108, //mythril
                109, //Hallowed Grass
                110, //Hallowed Plants 
                111, //adamantite
                112, //ebonsand
                114, //tinkerer's workbench
                115, //Hallowed Vines 
                116, //pearlsand
                125, //crystal ball
                126, //discoball
                128, //mannequin
                129, //crystal shard
                133, //adamantite forge
                134, //mythril anvil
                138, //boulder
                141, //explosives
                165, //ambient objects (stalagmites / stalactites, icicles)
                172, //sinks
                173, //platinum candelabra
                174, //platinum candle
                184, //moss growth
                185, //ambient objects (small rocks, small coin stashes, gem stashes)
                186, //ambient objects (bone piles, large rocks, large coin stashes)
                187, //ambient objects (mossy / lava rocks, spider eggs, misc bg furniture (cave tents, etc)) sword shrine
                201, //tall grass (crimson)
                205, //crimson vines
                207, //water fountain
                211, //Chlorophyte Ore
                218, //meat grinder
                227, //strange plant
                228, //dye vat
                233, ////ambient objects (background leafy jungle plants)
                238, //Plantera Bulb
                240, //trophies
                242, //big paintings
                245, //tall paintings
                246, //wide paintings
                270, //firefly in a bottle
                271, //lightning bug in a bottle
                275, //bunny cage
                276, //squirrel cage
                277, //mallard cage
                278, //duck cage
                279, //bird cage
                280, //blue jay cage
                281, //cardinal cage
                282, //fish bowl
                283, //heavy work bench
                285, //snail cage
                286, //glowing snail cage
                287, //ammo box
                288, //monarch jar
                289, //purple emperor jar
                290, //red admiral jar
                291, //ulysses jar
                292, //sulphur jar
                293, //tree nymph jar
                294, //zebra swallowtail jar
                295, //julia jar
                296, //scorpion cage
                297, //black scorpion cage
                298, //frog cage
                299, //mouse cage
                300, //bone welder
                301, //flesh cloning vat
                302, //glass kiln
                307, //steampunk boiler
                309, //penguin cage
                310, //worm cage
                316, //blue jellyfish jar
                317, //green jellyfish jar
                318, //pink jellyfish jar
                319, //ship in a bottle
                310, //seaweed planter
                324, //seashell variants
                337, //number and letter statues
                339, //grasshopper cage
                352, //crimson thorn
                354, //bewitching table
                355, //alchemy table
                358, //gold bird cage
                359, //gold bunny cage
                360, //gold butterfly jar
                361, //gold frog cage
                362, //gold grasshopper cage
                363, //gold mouse cage
                364, //gold worm cage
                372, //peace candle
                377, //sharpening station
                378, //target dummy
                382, //flower vines
                390, //lava lamp
                391, //enchanted nightcrawler cage
                392, //buggy cage
                393, //grubby cage
                394, //sluggy cage
                413, //red squirrel cage
                414, //gold squirrel cage
                463, //defenders forge
                TileID.Titanium,
                TileID.Pumpkins, //the harvestable kind, not the block

            };

            #endregion
            //--------
            #region PlaceAllowed list
            PlaceAllowed = new List<int>() {
                4,  //torch
                10, //Closed Door
                11, //Open Door  
                13, //bottles
                15, //chairs
                16, //anvil
                17, //furnace
                18, //workbench
                20, //sapling
                21, //chests
                27, //sunflower
                28, //pot
                29, //piggy bank
                33, //candle
                34, //bronze chandellier
                35, //silver chandellier
                36, //gold chandellier
                42, //chain lantern
                49, //water candle
                50, //books
                73, //plants
                74, //plants
                78, //clay pot
                79, //bed
                81, //corals
                82, //new herbs
                83, //grown herbs
                84, //bloomed herbs
                85, //tombstone
                86, //loom
                87, //piano
                88, //drawer
                89, //bench
                90, //bathtub
                91, //banner
                92, //lamp post
                93, //tiki torch
                94, //keg
                95, //chinese lantern
                96, //cooking pot
                97, //safe
                98, //skull candle
                99, //trash can
                100, //candlabra
                101, //bookcase
                102, //throne
                103, //bowl
                104, //grandfather clock
                105, //statue
                106, //sawmill
                114, //tinkerer's workbench
                125, //crystal ball
                126, //discoball
                128, //mannequin
                129, //crystal shard
                132, //lever
                133, //adamantite forge
                134, //mythril anvil
                149, //festive lights
                172, //sinks
                173, //platinum candelabra
                174, //platinum candle
                207, //water fountain
                218, //meat grinder
                228, //dye vat
                240, //trophies
                242, //big paintings
                245, //tall paintings
                246, //wide paintings
                270, //firefly in a bottle
                271, //lightning bug in a bottle
                275, //bunny cage
                276, //squirrel cage
                277, //mallard cage
                278, //duck cage
                279, //bird cage
                280, //blue jay cage
                281, //cardinal cage
                282, //fish bowl
                283, //heavy work bench
                285, //snail cage
                286, //glowing snail cage
                287, //ammo box
                288, //monarch jar
                289, //purple emperor jar
                290, //red admiral jar
                291, //ulysses jar
                292, //sulphur jar
                293, //tree nymph jar
                294, //zebra swallowtail jar
                295, //julia jar
                296, //scorpion cage
                297, //black scorpion cage
                298, //frog cage
                299, //mouse cage
                300, //bone welder
                301, //flesh cloning vat
                302, //glass kiln
                307, //steampunk boiler
                309, //penguin cage
                310, //worm cage
                316, //blue jellyfish jar
                317, //green jellyfish jar
                318, //pink jellyfish jar
                319, //ship in a bottle
                310, //seaweed planter
                324, //seashell variants
                337, //number and letter statues
                339, //grasshopper cage
                354, //bewitching table
                355, //alchemy table
                358, //gold bird cage
                359, //gold bunny cage
                360, //gold butterfly jar
                361, //gold frog cage
                362, //gold grasshopper cage
                363, //gold mouse cage
                364, //gold worm cage
                372, //peace candle
                377, //sharpening station
                378, //target dummy
                390, //lava lamp
                391, //enchanted nightcrawler cage
                392, //buggy cage
                393, //grubby cage
                394, //sluggy cage
                413, //red squirrel cage
                414, //gold squirrel cage
                463, //defender's forge
                TileID.Pumpkins //the harvestable kind, not the block

            };
            #endregion
            //--------
            #region Unbreakable list
            Unbreakable = new List<int>() {
                19, //Wood platform
                55, //sign
                132, //lever
                130, //active stone block
                131, //inactive stone block
                135, //pressure plates
                136, //switch
                137, //dart trap
                TileID.LunarMonolith //410
            };
            #endregion
            //--------
            #region IgnoredTiles list
            IgnoredTiles = new List<int>() {
                3, //tall grass
                24, //tall grass (corruption)
                32, //corruption thorn
                51, //cobweb
                52, //vines
                61, //tall grass (jungle)
                62, //jungle vines
                69, //jungle thorn
                73, //tall grass (misc)
                74, //tall jungle plants
                82, //plants (growing)
                83, //plants (matured)
                84, //plants (blooming)
                85, //tombstone
                115, //hallowed vines
                129, //crystal shard
                165, //ambient objects (stalagmites / stalactites, icicles)
                184, //moss growth
                185, //ambient objects (small rocks, small coin stashes, gem stashes)
                186, //ambient objects (bone piles, large rocks, large coin stashes)
                187, //ambient objects (mossy / lava rocks, spider eggs, misc bg furniture (cave tents, etc))
                201, //tall grass (crimson)
                205, //crimson vines
                227, //strange plant
                233, //ambient objects (background leafy jungle plants)
                324, //sea shells
                352, //crimson thorn
                382 //flower vines
            };
            #endregion
            //--------
            #region CrossModTiles list
            CrossModTiles = new List<int>();
            Mod MagicStorage = ModLoader.GetMod("MagicStorage");
            if (MagicStorage != null) {
                CrossModTiles.Add(MagicStorage.TileType("CraftingAccess"));
                CrossModTiles.Add(MagicStorage.TileType("RemoteAccess"));
                CrossModTiles.Add(MagicStorage.TileType("StorageAccess"));
                CrossModTiles.Add(MagicStorage.TileType("StorageComponent"));
                CrossModTiles.Add(MagicStorage.TileType("StorageHeart"));
                CrossModTiles.Add(MagicStorage.TileType("StorageUnit"));
                CrossModTiles.Add(MagicStorage.TileType("StorageConnector"));

            }
            #endregion
            //--------
            #region PlaceAllowedModTiles list
            PlaceAllowedModTiles = new List<int>()
            {
                TileType("EnemyBannerTile"),
                TileType("SweatyCyclopsForge")

            };

            #endregion
            //--------
        }

        public override void Unload() {
            toggleDragoonBoots = null;
            KillAllowed = null;
            PlaceAllowed = null;
            Unbreakable = null;
            IgnoredTiles = null;
            tsorcRevampWorld.Slain = null;
            //the following sun and moon texture changes are failsafes. they should be set back to default in PreSaveAndQuit 
            Main.sunTexture = ModContent.GetTexture("Terraria/Sun");
            Main.sun2Texture = ModContent.GetTexture("Terraria/Sun2");
            Main.sun3Texture = ModContent.GetTexture("Terraria/Sun3");
            for (int i = 0; i < Main.moonTexture.Length; i++) {
                Main.moonTexture[i] = ModContent.GetTexture("Terraria/Moon_" + i);
            }
        }
        public override void AddRecipes() {
            ModRecipeHelper.AddModRecipes();

            if (ModContent.GetInstance<tsorcRevampConfig>().AdventureModeItems) {
                RecipeHelper.EditRecipes(); 
            }
        }

        public override void AddRecipeGroups() {
            ModRecipeHelper.AddRecipeGroups();
        }

        public override void PreSaveAndQuit() {
            Main.sunTexture = ModContent.GetTexture("Terraria/Sun");
            Main.sun2Texture = ModContent.GetTexture("Terraria/Sun2");
            Main.sun3Texture = ModContent.GetTexture("Terraria/Sun3");
            for (int i = 0; i < Main.moonTexture.Length; i++) {
                Main.moonTexture[i] = ModContent.GetTexture("Terraria/Moon_" + i);
            }
        }

        private void Player_Update(ILContext il) {
            ILCursor cursor = new ILCursor(il);

            if (!cursor.TryGotoNext(MoveType.Before,
                                    i => i.MatchLdfld("Terraria.Player", "statManaMax2"),
                                    i => i.MatchLdcI4(400))) {
                Logger.Fatal("Could not find instruction to patch (Player_Update)");
                return;
            }

            cursor.Next.Next.Operand = int.MaxValue;
        }

        private void Chest_Patch(ILContext il) {
            ILCursor c = new ILCursor(il);

            if (!c.TryGotoNext(instr => instr.MatchLdcR4(1f) && instr.Next.Next.Next.Next.Next.Next.MatchStfld(typeof(Player).GetField("chest")))) {
                throw new Exception("Could not find instruction to patch (Chest_Patch)");
            }

            c.FindNext(out ILCursor[] cursors, instr => instr.MatchLdcR4(1f));
            c = cursors[0];

            c.Index++;
            c.EmitDelegate<Func<float, float>>((volume) => {
                if (Main.LocalPlayer.GetModPlayer<tsorcRevampPlayer>().chestBankOpen
                || Main.LocalPlayer.GetModPlayer<tsorcRevampPlayer>().chestPiggyOpen) {
                    // Return 0 volume if one is open so the sound is silent
                    return 0f;
                }

                return volume;
            });
        }

        public override void PostDrawInterface(SpriteBatch spriteBatch) {
            tsorcRevampPlayer modPlayer = Main.LocalPlayer.GetModPlayer<tsorcRevampPlayer>();
            modPlayer.Draw(spriteBatch);
        }

        

        public override void HandlePacket(BinaryReader reader, int whoAmI) {
            int message = reader.ReadByte(); //(byte) 1;

            //Sync Soul Slot
            if (message == tsorcPacketID.SyncSoulSlot)
            {
                byte player = reader.ReadByte(); //player.whoAmI;
                tsorcRevampPlayer modPlayer = Main.player[player].GetModPlayer<tsorcRevampPlayer>();
                modPlayer.SoulSlot.Item = ItemIO.Receive(reader);
                if (Main.netMode == NetmodeID.Server)
                {
                    modPlayer.SendSingleItemPacket(1, modPlayer.SoulSlot.Item, -1, whoAmI);
                }
            }

            /**
            //For synced random
            //Recieves the seed from the server, and passes it off to UsefulFunctions.RecieveRandPacket which uses it to instantiate the new random generator
            else if (message == tsorcPacketID.SyncRandom)
            {
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    byte seed = reader.ReadByte();
                    Main.NewText("Client recieved seed:" + seed);
                    UsefulFunctions.RecieveRandPacket(seed);
                }
            }**/

            else
            {
                Logger.InfoFormat("[tsorcRevamp] Sync failed. Unknown message ID: {0}", message);
            }
        }

        public override void PostSetupContent()
        {

            #region Boss Checklist Compatibility

            Mod bossChecklist = ModLoader.GetMod("BossChecklist"); //See https://github.com/JavidPack/BossChecklist/wiki/Support-using-Mod-Call for instructions
            if (bossChecklist != null)
            {

                // AddBoss, bossname, order or value in terms of vanilla bosses, inline method for retrieving downed value.
                /*
                public const float SlimeKing = 1f;
                public const float EyeOfCthulhu = 2f;
                public const float EaterOfWorlds = 3f;
                public const float QueenBee = 4f;
                public const float Skeletron = 5f;
                public const float WallOfFlesh = 6f;
                public const float TheTwins = 7f;
                public const float TheDestroyer = 8f;
                public const float SkeletronPrime = 9f;
                public const float Plantera = 10f;
                public const float Golem = 11f;
                public const float DukeFishron = 12f;
                public const float LunaticCultist = 13f;
                public const float Moonlord = 14f;*/



                // PRE-HM

                bossChecklist.Call(
                    "AddBoss", // Name of the call
                    3.9f, // Tier (look above)
                    new List<int>() { ModContent.NPCType<NPCs.Bosses.Slogra>(), ModContent.NPCType<NPCs.Bosses.Gaibon>() },
                    this, // Mod
                    "Slogra and Gaibon", // Boss Name
                    (Func<bool>)(() => tsorcRevampWorld.Slain.ContainsKey(ModContent.NPCType<NPCs.Bosses.Slogra>())), // Downed variable (the one keeping track the boss has been defeated once)
                    ModContent.ItemType<Items.BossItems.TomeOfSlograAndGaibon>(),
                    0,
                    new List<int> { ModContent.ItemType<Items.BossBags.SlograBag>(), ModContent.ItemType<Items.Accessories.PoisonbiteRing>(), ModContent.ItemType<Items.Accessories.BloodbiteRing>() }, // List containing all the loot to show in the bestiary
                    $"Found in the depths of the Meteor Temple.", // Guide to fight the boss
                    "", // Despawning Message
                    "tsorcRevamp/NPCs/Bosses/Boss Checklist Replacement Sprites/SlograAndGaibon");


                bossChecklist.Call(
                    "AddBoss", // Name of the call
                    5.01f, // Tier (look above)
                    new List<int>() { ModContent.NPCType<NPCs.Bosses.JungleWyvern.JungleWyvernHead>(), ModContent.NPCType<NPCs.Bosses.JungleWyvern.JungleWyvernBody>(), ModContent.NPCType<NPCs.Bosses.JungleWyvern.JungleWyvernBody2>(), ModContent.NPCType<NPCs.Bosses.JungleWyvern.JungleWyvernBody3>(), ModContent.NPCType<NPCs.Bosses.JungleWyvern.JungleWyvernLegs>(), ModContent.NPCType<NPCs.Bosses.JungleWyvern.JungleWyvernTail>() },
                    this, // Mod
                    "Jungle Wyvern", // Boss Name
                    (Func<bool>)(() => tsorcRevampWorld.Slain.ContainsKey(ModContent.NPCType<NPCs.Bosses.JungleWyvern.JungleWyvernHead>())), // Downed variable (the one keeping track the boss has been defeated once)
                    ModContent.ItemType<Items.BossItems.JungleFeather>(),
                    0,
                    new List<int> { ModContent.ItemType<Items.BossBags.JungleWyvernBag>(), ItemID.Sapphire, ItemID.Ruby, ItemID.Topaz, ItemID.Diamond, ItemID.Emerald, ItemID.Amethyst, ItemID.NecroHelmet, ItemID.NecroBreastplate, ItemID.NecroGreaves }, // List containing all the loot to show in the bestiary
                    $"Found in the depths of the Forgotten City.", // Guide to fight the boss
                    "", // Despawning Message
                    "tsorcRevamp/NPCs/Bosses/Boss Checklist Replacement Sprites/JungleWyvern");




                // HM

                bossChecklist.Call(
                    "AddBoss", // Name of the call
                    6.2f, // Tier (look above)
                    new List<int>() { ModContent.NPCType<NPCs.Bosses.TheRage>() },
                    this, // Mod
                    "The Rage", // Boss Name
                    (Func<bool>)(() => tsorcRevampWorld.Slain.ContainsKey(ModContent.NPCType<NPCs.Bosses.TheRage>())), // Downed variable (the one keeping track the boss has been defeated once)
                    ModContent.ItemType<Items.BossItems.FieryEgg>(),
                    0,
                    new List<int> { ModContent.ItemType<Items.BossBags.TheRageBag>(), ModContent.ItemType<Items.CrestOfFire>(), ItemID.CobaltDrill }, // List containing all the loot to show in the bestiary
                    $"Found in the depths of the earth.", // Guide to fight the boss
                    "");


                bossChecklist.Call(
                    "AddBoss", // Name of the call
                    6.4f, // Tier (look above)
                    new List<int>() { ModContent.NPCType<NPCs.Bosses.TheSorrow>() },
                    this, // Mod
                    "The Sorrow", // Boss Name
                    (Func<bool>)(() => tsorcRevampWorld.Slain.ContainsKey(ModContent.NPCType<NPCs.Bosses.TheSorrow>())), // Downed variable (the one keeping track the boss has been defeated once)
                    ModContent.ItemType<Items.BossItems.WateryEgg>(),
                    0,
                    new List<int> { ModContent.ItemType<Items.BossBags.TheSorrowBag>(), ModContent.ItemType<Items.CrestOfWater>(), ItemID.AdamantiteDrill }, // List containing all the loot to show in the bestiary
                    $"Follow the clues given after beating the previous boss.", // Guide to fight the boss
                    "");


                bossChecklist.Call(
                    "AddBoss", // Name of the call
                    6.5f, // Tier (look above)
                    new List<int>() { ModContent.NPCType<NPCs.Bosses.TheHunter>() },
                    this, // Mod
                    "The Hunter", // Boss Name
                    (Func<bool>)(() => tsorcRevampWorld.Slain.ContainsKey(ModContent.NPCType<NPCs.Bosses.TheHunter>())), // Downed variable (the one keeping track the boss has been defeated once)
                    ModContent.ItemType<Items.BossItems.GrassyEgg>(),
                    0,
                    new List<int> { ModContent.ItemType<Items.BossBags.TheHunterBag>(), ModContent.ItemType<Items.CrestOfEarth>(), ItemID.Drax, ItemID.WaterWalkingBoots }, // List containing all the loot to show in the bestiary
                    $"Follow the clues given after beating the previous boss.", // Guide to fight the boss
                    "");



                bossChecklist.Call(
                    "AddBoss", // Name of the call
                    7.1f, // Tier (look above)
                    new List<int>() { ModContent.NPCType<NPCs.Bosses.Death>() },
                    this, // Mod
                    "Death", // Boss Name
                    (Func<bool>)(() => tsorcRevampWorld.Slain.ContainsKey(ModContent.NPCType<NPCs.Bosses.Death>())), // Downed variable (the one keeping track the boss has been defeated once)
                    0,
                    0,
                    new List<int> { ModContent.ItemType<Items.BossBags.DeathBag>(), ModContent.ItemType<Items.Potions.HolyWarElixir>(), ModContent.ItemType<Items.Weapons.Magic.WallTome>(), ModContent.ItemType<Items.GuardianSoul>(), ModContent.ItemType<Items.Weapons.Magic.BarrierTome>(), ItemID.MidnightRainbowDye }, // List containing all the loot to show in the bestiary
                    $"???", // Guide to fight the boss
                    "");


                bossChecklist.Call(
                    "AddBoss", // Name of the call
                    7.2f, // Tier (look above)
                    new List<int>() { ModContent.NPCType<NPCs.Bosses.WyvernMage.WyvernMage>() },
                    this, // Mod
                    "Wyvern Mage & Mecha Wyvern", // Boss Name
                    (Func<bool>)(() => tsorcRevampWorld.Slain.ContainsKey(ModContent.NPCType<NPCs.Bosses.WyvernMage.WyvernMage>())), // Downed variable (the one keeping track the boss has been defeated once)
                    ModContent.ItemType<Items.BossItems.WingOfTheFallen>(),
                    0,
                    new List<int> { ModContent.ItemType<Items.BossBags.WyvernMageBag>(), ModContent.ItemType<Items.Potions.HolyWarElixir>(), ModContent.ItemType<Items.Weapons.Melee.LionheartGunblade>(), ModContent.ItemType<Items.Weapons.Magic.LampTome>(), ModContent.ItemType<Items.Accessories.GemBox>(), ModContent.ItemType<Items.Accessories.PoisonbiteRing>(), ModContent.ItemType<Items.Accessories.BloodbiteRing>() }, // List containing all the loot to show in the bestiary
                    $"Found high atop a mountain in a great fortress.", // Guide to fight the boss
                    "", // Despawning Message
                    "tsorcRevamp/NPCs/Bosses/Boss Checklist Replacement Sprites/WyvernMage");



                bossChecklist.Call(
                    "AddBoss", // Name of the call
                    7.3f, // Tier (look above)
                    new List<int>() { ModContent.NPCType<NPCs.Bosses.Serris.SerrisX>() },
                    this, // Mod
                    "Serris", // Boss Name
                    (Func<bool>)(() => tsorcRevampWorld.Slain.ContainsKey(ModContent.NPCType<NPCs.Bosses.Serris.SerrisX>())), // Downed variable (the one keeping track the boss has been defeated once)
                    ModContent.ItemType<Items.BossItems.SerrisBait>(),
                    0,
                    new List<int> { ModContent.ItemType<Items.BossBags.SerrisBag>(), ModContent.ItemType<Items.Potions.DemonDrugPotion>(), ModContent.ItemType<Items.Potions.ArmorDrugPotion>(), ModContent.ItemType<Items.GuardianSoul>(), ModContent.ItemType<Items.Weapons.Magic.BarrierTome>() }, // List containing all the loot to show in the bestiary
                    $"???", // Guide to fight the boss
                    "", // Despawning Message
                    "tsorcRevamp/NPCs/Bosses/Boss Checklist Replacement Sprites/Serris");





                // SHM


                //stuff

                #endregion
            }
        }

    }
    public class tsorcPacketID
    {
        //Bytes because packets use bytes
        public const byte SyncSoulSlot = 1;
        public const byte SyncRandom = 2;
    }

    //config moved to separate file

    public class TilePlaceCode : GlobalItem {

        public override bool CanUseItem(Item item, Player player) {
            if (ModContent.GetInstance<tsorcRevampConfig>().AdventureMode) {
                if (item.createWall > 0) {
                    return false; //prevent placing walls
                }
                if (item.createTile > -1) {

                    if (tsorcRevamp.PlaceAllowed.Contains(item.createTile))
                    {
                        return true; //allow placing tiles in PlaceAllowed
                    }

                    else if (tsorcRevamp.CrossModTiles.Contains(item.createTile))
                    {
                        return true; //allow placing certain tiles from other mods
                    }

                    else if (tsorcRevamp.PlaceAllowedModTiles.Contains(item.createTile))
                    {
                        return true; //allow placing certain tiles from this mod
                    }

                    else return false; //disallow using item if it places other tiles
                }
                return true; //allow using items if they do not create tiles
            }
            return base.CanUseItem(item, player); //use default value
        }
    }

    public class TileKillCode : GlobalTile {

        public override bool CanKillTile(int x, int y, int type, ref bool blockDamaged) {


            if (ModContent.GetInstance<tsorcRevampConfig>().AdventureMode) {

                bool right = !Main.tile[x + 1, y].active() || tsorcRevamp.IgnoredTiles.Contains(Main.tile[x + 1, y].type);
                bool left = !Main.tile[x - 1, y].active() || tsorcRevamp.IgnoredTiles.Contains(Main.tile[x - 1, y].type);
                bool below = !Main.tile[x, y - 1].active() || tsorcRevamp.IgnoredTiles.Contains(Main.tile[x, y - 1].type);
                bool above = !Main.tile[x, y + 1].active() || tsorcRevamp.IgnoredTiles.Contains(Main.tile[x, y + 1].type);
                if (x < 10 || x > Main.maxTilesX - 10) {//sanity
                    return false;
                }
                else if (y < 10 || y > Main.maxTilesY - 10) {//sanity 
                    return false;
                }
                else if (Main.tile[x, y] == null) {//sanity
                    return false;
                }
                else if (tsorcRevamp.KillAllowed.Contains(type)) {//always allow KillAllowed
                    return true;
                }
                else if (tsorcRevamp.CrossModTiles.Contains(type)) {//allow breaking placeable modded tiles from other mods
                    return true;
                }
                else if (tsorcRevamp.PlaceAllowedModTiles.Contains(type)) {//allow breaking placeable modded tiles
                    return true;
                }
                else if (tsorcRevamp.Unbreakable.Contains(type)) {//always disallow Unbreakable	
                    return false;
                }
                else if (right && left) {//if a tile has no neighboring tiles horizontally, allow breaking
                    return true;
                }
                else if (below && above) {//if a tile has no neighboring tiles vertically, allow breaking
                    return true;
                }
                else return false; //disallow breaking tiles otherwise
            }
            return base.CanKillTile(x, y, type, ref blockDamaged); //use default value
        }

        public override bool CanExplode(int x, int y, int type) {
            if (ModContent.GetInstance<tsorcRevampConfig>().AdventureMode) {
                bool right = !Main.tile[x + 1, y].active() || tsorcRevamp.IgnoredTiles.Contains(Main.tile[x + 1, y].type);
                bool left = !Main.tile[x - 1, y].active() || tsorcRevamp.IgnoredTiles.Contains(Main.tile[x - 1, y].type);
                bool below = !Main.tile[x, y - 1].active() || tsorcRevamp.IgnoredTiles.Contains(Main.tile[x, y - 1].type);
                bool above = !Main.tile[x, y + 1].active() || tsorcRevamp.IgnoredTiles.Contains(Main.tile[x, y + 1].type);
                bool CanDestroy = false;
                if (type == TileID.Ebonsand || type == TileID.Amethyst || type == TileID.ShadowOrbs) { //shadow temple / corruption chasm stuff that gets blown up
                    CanDestroy = true;
                }

                //check cankilltiles stuff
                if ((right && left) || (above && below) || tsorcRevamp.KillAllowed.Contains(type) || (x < 10 || x > Main.maxTilesX - 10) || (y < 10 || y > Main.maxTilesY - 10) || (!Main.tile[x, y].active())) {
                    CanDestroy = true;
                }
                if (Main.tileDungeon[Main.tile[x, y].type]
                    || type == TileID.Silver
                    || type == TileID.Cobalt
                    || type == TileID.Mythril
                    || type == TileID.Adamantite
                    || (tsorcRevamp.Unbreakable.Contains(type))
                ) {
                    CanDestroy = false;
                }
                if (!Main.hardMode && type == TileID.Hellstone) {
                    CanDestroy = false;
                }
                return CanDestroy;
            }
            else return base.CanExplode(x, y, type);

        }

        public override bool Slope(int i, int j, int type) {
            if (ModContent.GetInstance<tsorcRevampConfig>().AdventureMode) {
                return false;
            }
            else return base.Slope(i, j, type);
        }
    }

    public class WallKillCode : GlobalWall {
        public override void KillWall(int i, int j, int type, ref bool fail) {
            if (ModContent.GetInstance<tsorcRevampConfig>().AdventureMode) {
                fail = true;
            }
        }
        public override bool CanExplode(int i, int j, int type) {
            if (ModContent.GetInstance<tsorcRevampConfig>().AdventureMode) {
                return false;
            }
            return base.CanExplode(i, j, type);
        }
    }
    public class MiscGlobalTile : GlobalTile {

        bool vortexNotif = false;
        bool nebulaNotif = false;
        bool stardustNotif = false;
        bool solarNotif = false;
       
        public override void NearbyEffects(int i, int j, int type, bool closer) {
            if (ModContent.GetInstance<tsorcRevampConfig>().AdventureMode) {
                Player player = Main.LocalPlayer;
                var pos = new Vector2(i + 0.5f, j); // the + .5f makes the effect reach from equal distance to left and right
                var distance = Math.Abs(Vector2.Distance(player.Center, (pos * 16)));

                if (Main.tile[i, j].type == TileID.LunarMonolith && distance <= 800f && !player.dead && Main.tile[i, j].frameY > 54) { //frameY > 54 means enabled
                    
                        int style = Main.tile[i, j].frameX / 36;
                    switch (style)
                    {
                        case 0:
                            if (!NPC.AnyNPCs(NPCID.LunarTowerVortex) && !tsorcRevampWorld.DownedVortex)
                            {
                                if (tsorcRevampWorld.SuperHardMode)
                                {
                                    int p = NPC.NewNPC((i * 16) + 8, (j * 16) - 64, NPCID.LunarTowerVortex, 1);
                                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, p);
                                    NPC.TowerActiveVortex = true;
                                    NPC.ShieldStrengthTowerVortex = NPC.ShieldStrengthTowerMax;
                                    NetMessage.SendData(MessageID.UpdateTowerShieldStrengths);
                                }
                                else
                                {
                                    if (!vortexNotif)
                                    {
                                        Main.NewText("The power of this monolith is bound to Attraidies", Color.Teal);
                                        Main.NewText("Defeating him and returning here may allow you to release it...", Color.Teal);
                                        vortexNotif = true;
                                    }
                                }
                            }
                            break;

                        case 1:
                            if (!NPC.AnyNPCs(NPCID.LunarTowerNebula) && !tsorcRevampWorld.DownedNebula)
                            {
                                if (tsorcRevampWorld.SuperHardMode)
                                {
                                    int p = NPC.NewNPC((i * 16) + 8, (j * 16) - 64, NPCID.LunarTowerNebula, 1);
                                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, p);
                                    NPC.TowerActiveNebula = true;
                                    NPC.ShieldStrengthTowerNebula = NPC.ShieldStrengthTowerMax;
                                    NetMessage.SendData(MessageID.UpdateTowerShieldStrengths);
                                }
                                else
                                {
                                    if (!nebulaNotif)
                                    {
                                        Main.NewText("The power of this monolith is bound to Attraidies", Color.Pink);
                                        Main.NewText("Defeating him and returning here may allow you to release it...", Color.Pink);
                                        nebulaNotif = true;
                                    }
                                }
                            }
                            break;

                        case 2:
                            if (!NPC.AnyNPCs(NPCID.LunarTowerStardust) && !tsorcRevampWorld.DownedStardust)
                            {
                                if (tsorcRevampWorld.SuperHardMode)
                                {
                                    int p = NPC.NewNPC((i * 16) + 8, (j * 16) - 64, NPCID.LunarTowerStardust, 1);
                                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, p);
                                    NPC.TowerActiveStardust = true;
                                    NPC.ShieldStrengthTowerStardust = NPC.ShieldStrengthTowerMax;
                                    NetMessage.SendData(MessageID.UpdateTowerShieldStrengths);
                                }
                                else
                                {
                                    if (!stardustNotif)
                                    {
                                        Main.NewText("The power of this monolith is bound to Attraidies", Color.LightBlue);
                                        Main.NewText("Defeating him and returning here may allow you to release it...", Color.LightBlue);
                                        stardustNotif = true;
                                    }
                                }
                            }
                            break;

                        case 3:
                            if (!NPC.AnyNPCs(NPCID.LunarTowerSolar) && !tsorcRevampWorld.DownedSolar)
                            {
                                if (tsorcRevampWorld.SuperHardMode)
                                {
                                    int p = NPC.NewNPC((i * 16) + 8, (j * 16) - 64, NPCID.LunarTowerSolar, 1);
                                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, p);
                                    NPC.TowerActiveSolar = true;
                                    NPC.ShieldStrengthTowerSolar = NPC.ShieldStrengthTowerMax;
                                    NetMessage.SendData(MessageID.UpdateTowerShieldStrengths);
                                }
                                else
                                {
                                    if (!solarNotif)
                                    {
                                        Main.NewText("The power of this monolith is bound to Attraidies", Color.OrangeRed);
                                        Main.NewText("Defeating him and returning here may allow you to release it...", Color.OrangeRed);
                                        solarNotif = true;
                                    }
                                }
                            }
                            break;
                    }                    
                } 
            }

            base.NearbyEffects(i, j, type, closer);
        }
    }

    //tConfig played nice with partially transparent textures, tModloader doesn't. This class helps fix that
    //Handles premultiplication, allowing textures with alpha transparency to be displayed correctly
    //To add a texture, add an enum for it under TransparentTextureType, and a KeyValuePair for it in the TransparentTextures dictionary like the others below
    //You can then get your premultiplied Texture2D from it wherever it's needed via TransparentTextureHandler.TransparentTextures[TransparentTextureHandler.TransparentTextureType.(YourEnum)]; 
    public static class TransparentTextureHandler
    {
        public static Dictionary<TransparentTextureType, Texture2D> TransparentTextures;
        public enum TransparentTextureType
        {
            PhasedMatterBlast,
            AntiGravityBlast,
            EnemyPlamaOrb,
            ManaShield,
            CrazedOrb,
            MasterBuster,
            AntiMaterialRound,
            GlaiveBeam,
            GlaiveBeamItemGlowmask,
            GlaiveBeamHeldGlowmask,
            GenericLaser,
            GenericLaserTargeting,
            DarkLaser,
            DarkLaserTargeting,
            PulsarGlowmask,
            GWPulsarGlowmask,
            PolarisGlowmask,
            ToxicCatalyzerGlowmask,
            VirulentCatalyzerGlowmask,
            BiohazardGlowmask,
            HealingElixirGlowmask,
            ShatteredMoonlightGlowmask,
            GreySlashGlowmask,
            DarkDivineSpark,
            UltimaWeapon,
            UltimaWeaponGlowmask,
            DarkUltimaWeapon,
            DarkUltimaWeaponGlowmask,
            ReflectionShift,
            PhazonRound

        }          
        
        //All textures with transparency will have to get run through this function to get premultiplied
        public static void TransparentTextureFix()
        {
            //Generates the dictionary of textures
            TransparentTextures = new Dictionary<TransparentTextureType, Texture2D>()
            {
                {TransparentTextureType.PhasedMatterBlast, ModContent.GetTexture("tsorcRevamp/Projectiles/Enemy/Okiku/PhasedMatterBlast")},
                {TransparentTextureType.AntiGravityBlast, ModContent.GetTexture("tsorcRevamp/Projectiles/Enemy/AntiGravityBlast")},
                {TransparentTextureType.EnemyPlamaOrb, ModContent.GetTexture("tsorcRevamp/Projectiles/Enemy/EnemyPlasmaOrb")},
                {TransparentTextureType.ManaShield, ModContent.GetTexture("tsorcRevamp/Projectiles/ManaShield")},
                {TransparentTextureType.CrazedOrb, ModContent.GetTexture("tsorcRevamp/Projectiles/Enemy/Okiku/CrazedOrb")},
                {TransparentTextureType.MasterBuster, ModContent.GetTexture("tsorcRevamp/Projectiles/MasterBuster")},
                {TransparentTextureType.AntiMaterialRound, ModContent.GetTexture("tsorcRevamp/Projectiles/AntiMaterialRound")},
                {TransparentTextureType.GlaiveBeam, ModContent.GetTexture("tsorcRevamp/Projectiles/GlaiveBeamLaser")},
                {TransparentTextureType.GlaiveBeamItemGlowmask, ModContent.GetTexture("tsorcRevamp/Items/Weapons/Ranged/GlaiveBeam_Glowmask")},
                {TransparentTextureType.GlaiveBeamHeldGlowmask, ModContent.GetTexture("tsorcRevamp/Items/Weapons/Ranged/GlaiveBeamHeld_Glowmask")},
                {TransparentTextureType.GenericLaser, ModContent.GetTexture("tsorcRevamp/Projectiles/GenericLaser")},
                {TransparentTextureType.GenericLaserTargeting, ModContent.GetTexture("tsorcRevamp/Projectiles/GenericLaserTargeting")},
                {TransparentTextureType.DarkLaser, ModContent.GetTexture("tsorcRevamp/Projectiles/Enemy/Okiku/DarkLaser")},
                {TransparentTextureType.DarkLaserTargeting, ModContent.GetTexture("tsorcRevamp/Projectiles/Enemy/Okiku/DarkLaserTargeting")},
                {TransparentTextureType.PulsarGlowmask, ModContent.GetTexture("tsorcRevamp/Items/Weapons/Ranged/Pulsar_Glowmask")},
                {TransparentTextureType.GWPulsarGlowmask, ModContent.GetTexture("tsorcRevamp/Items/Weapons/Ranged/GWPulsar_Glowmask")},
                {TransparentTextureType.PolarisGlowmask, ModContent.GetTexture("tsorcRevamp/Items/Weapons/Ranged/Polaris_Glowmask")},
                {TransparentTextureType.ToxicCatalyzerGlowmask, ModContent.GetTexture("tsorcRevamp/Items/Weapons/Ranged/ToxicCatalyzer_Glowmask")},
                {TransparentTextureType.VirulentCatalyzerGlowmask, ModContent.GetTexture("tsorcRevamp/Items/Weapons/Ranged/VirulentCatalyzer_Glowmask")},
                {TransparentTextureType.BiohazardGlowmask, ModContent.GetTexture("tsorcRevamp/Items/Weapons/Ranged/Biohazard_Glowmask")},
                {TransparentTextureType.HealingElixirGlowmask, ModContent.GetTexture("tsorcRevamp/Items/Potions/HealingElixir_Glowmask")},
                {TransparentTextureType.DarkDivineSpark, ModContent.GetTexture("tsorcRevamp/Projectiles/Enemy/DarkCloud/DarkDivineSpark")},
                {TransparentTextureType.ShatteredMoonlightGlowmask, ModContent.GetTexture("tsorcRevamp/Projectiles/ShatteredMoonlight_Glowmask")},
                {TransparentTextureType.GreySlashGlowmask, ModContent.GetTexture("tsorcRevamp/Projectiles/Enemy/GreySlash_Glowmask")},
                {TransparentTextureType.UltimaWeapon, ModContent.GetTexture("tsorcRevamp/Items/Weapons/Melee/UltimaWeaponTransparent")},
                {TransparentTextureType.UltimaWeaponGlowmask, ModContent.GetTexture("tsorcRevamp/Items/Weapons/Melee/UltimaWeaponGlowmask")},
                {TransparentTextureType.DarkUltimaWeapon, ModContent.GetTexture("tsorcRevamp/NPCs/Bosses/SuperHardMode/DarkUltimaWeapon")},
                {TransparentTextureType.DarkUltimaWeaponGlowmask, ModContent.GetTexture("tsorcRevamp/NPCs/Bosses/SuperHardMode/DarkUltimaWeaponGlowmask")},
                {TransparentTextureType.ReflectionShift, ModContent.GetTexture("tsorcRevamp/Items/Accessories/ReflectionShift")},
                {TransparentTextureType.PhazonRound, ModContent.GetTexture("tsorcRevamp/Projectiles/PhazonRound")}

            };

            //Runs each entry through the XNA's premultiplication function
           foreach(Texture2D textureEntry in TransparentTextures.Values)
            {
                Color[] buffer = new Color[textureEntry.Width * textureEntry.Height];
                textureEntry.GetData(buffer);
                for (int j = 0; j < buffer.Length; j++)
                {
                    buffer[j] = Color.FromNonPremultiplied(buffer[j].R, buffer[j].G, buffer[j].B, buffer[j].A);
                }
                textureEntry.SetData(buffer);
            }
        }
    };
}