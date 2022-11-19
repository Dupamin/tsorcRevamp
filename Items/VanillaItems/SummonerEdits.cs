using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using tsorcRevamp.Items.Armors.Summon;

namespace tsorcRevamp.Items.VanillaItems
{
    class SummonerEdits : GlobalItem
    {

        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.RainbowWhip)
            {
                item.damage = 100;
            }
            if (item.type == ItemID.ScytheWhip)
            {
                item.damage = 80;
            }
            if (item.type == ItemID.MaceWhip)
            {
                item.damage = 110;
            }
            if (item.type == ItemID.BoneWhip)
            {
                item.damage = 24;
            }
            if (item.type == ItemID.Smolstar)//Blade Staff
            {
                item.damage = 1;//Powerful tag whips were added
            }

            //Lunar items
            if (item.type == ItemID.StardustDragonStaff)
            {
                item.damage = 35;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.RainbowWhip)
            {
                int ttindex = tooltips.FindIndex(t => t.Name == "Tooltip0");
                if (ttindex != -1)
                {
                    tooltips.RemoveAt(ttindex);
                    tooltips.Insert(ttindex, new TooltipLine(Mod, "TagNerfed", "10 summon tag damage"));
                }
                int ttindex2 = tooltips.FindIndex(t => t.Name == "Tooltip1");
                if (ttindex2 != -1)
                {
                    tooltips.RemoveAt(ttindex2);
                    tooltips.Insert(ttindex2, new TooltipLine(Mod, "TagCritNerfed", "5% summon tag critical strike chance"));
                }
            }
            /*if (item.type == ItemID.MaceWhip)
            {
                int ttindex1 = tooltips.FindIndex(t => t.Name == "Tooltip1");
                if (ttindex1 != -1)
                {
                    tooltips.RemoveAt(ttindex1);
                    tooltips.Insert(ttindex1, new TooltipLine(Mod, "TagCritNerfed", "4% summon tag critical strike chance"));
                }
            }*/
            if (item.type == ItemID.EmpressBlade && !tsorcRevampWorld.Slain.ContainsKey(ModContent.NPCType<NPCs.Bosses.SuperHardMode.Artorias>()))
            {
                int ttindex2 = tooltips.FindLastIndex(t => t.Name == "Tooltip0");
                if (ttindex2 != -1)
                {
                    tooltips.Insert(ttindex2 + 1, new TooltipLine(Mod, "Nerfed", "The full power of this blade has been sealed by an ancient knight\nDefeat him to partially unlock its power!"));
                }
            } else
            if (item.type == ItemID.EmpressBlade && !tsorcRevampWorld.Slain.ContainsKey(ModContent.NPCType<NPCs.Bosses.SuperHardMode.Chaos>()))
            {
                int ttindex2 = tooltips.FindLastIndex(t => t.Name == "Tooltip0");
                if (ttindex2 != -1)
                {
                    tooltips.Insert(ttindex2 + 1, new TooltipLine(Mod, "Nerfed2", "The full power of this blade has been sealed by the embodiment of chaos\nDefeat it to fully unlock its power!"));
                }
            }
        }
        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            base.ModifyWeaponDamage(item, player, ref damage);
            if (tsorcRevampWorld.Slain != null)
            {
                if (item.type == ItemID.EmpressBlade & !tsorcRevampWorld.Slain.ContainsKey(ModContent.NPCType<NPCs.Bosses.SuperHardMode.Artorias>()))
                {
                    damage *= 0.75f;
                }
                else if (item.type == ItemID.EmpressBlade & !tsorcRevampWorld.Slain.ContainsKey(ModContent.NPCType<NPCs.Bosses.SuperHardMode.Chaos>()))
                {
                    damage *= 0.88f;
                }
            }
        }
        public override string IsArmorSet(Item head, Item body, Item legs)
        {
            if (head.type == ModContent.ItemType<OldChainCoif>() && body.type == ItemID.FlinxFurCoat && legs.type == ModContent.ItemType<OldChainGreaves>())
            {
                return "FlinxFurChained";
            }
            else return base.IsArmorSet(head, body, legs);
        }
        public override void UpdateArmorSet(Player player, string set)
        {
            if (set == "FlinxFurChained")
            {
                player.setBonus = "Increases your max number of minions by 1";

                player.maxMinions += 1;
            }
        }

    }
}


