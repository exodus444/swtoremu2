﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GomLib.Models;

namespace GomLib.ModelLoader
{
    public class ItemSetLoader
    {
        const long NameOffset = 0x3311900000000;
        static StringTable strTable;

        static ItemSetLoader()
        {
            strTable = StringTable.Find("str.itm.setbonuses");
        }

        public string ClassName
        {
            get { return "itmSetBonus"; }
        }

        public static Models.ItemSet Load(Models.ItemSet set, GomObjectData obj)
        {
            if (obj == null) { return set; }
            if (set == null) { return null; }

            set.Id = (ulong)obj.ValueOrDefault<long>("itmSetBonusDisplayName", 0);
            set.NameId = (int)obj.ValueOrDefault<long>("itmSetBonusDisplayName", 0);
            set.Name = strTable.GetText(NameOffset + set.NameId, "ItemSet." + set.Id);
            set.Count = (int)obj.ValueOrDefault<long>("itmSetBonusItemCount", 0);

            Dictionary<object, object> setItems = obj.ValueOrDefault<Dictionary<object,object>>("itmSetBonusSetItems", null);
            set.SetItems = new List<ItemSetItem>();
            foreach (var itm_itm in setItems)
            {
                ulong itmId = (ulong)itm_itm.Key;
                ulong primaryItemId = (ulong)itm_itm.Value;

                var itm = ItemLoader.Load(itmId);
                var pItm = ItemLoader.Load(primaryItemId);

                var setItm = new ItemSetItem();
                setItm.Item = itm;
                setItm.PrimaryItem = pItm;
                setItm.ItemSet = set;
                set.SetItems.Add(setItm);
            }

            //List<object> primaryItems = obj.itmSetBonusPrimaryItems;
            //set.PrimaryItems = new List<Item>();
            //foreach (ulong itemNodeId in primaryItems)
            //{
            //    var itm = ItemLoader.Load(itemNodeId);
            //    set.PrimaryItems.Add(itm);
            //}

            Dictionary<object,object> bonusAbilities = obj.ValueOrDefault<Dictionary<object,object>>("itmSetBonusBonuses", null);
            set.Abilities = new List<ItemSetAbility>();
            foreach (var cnt_abl in bonusAbilities)
            {
                int cnt = (int)(long)cnt_abl.Key;
                ulong ablNodeId = (ulong)cnt_abl.Value;

                var abl = AbilityLoader.Load(ablNodeId);

                ItemSetAbility setAbl = new ItemSetAbility();
                setAbl.Ability = abl;
                setAbl.ItemCount = cnt;
                setAbl.ItemSet = set;
                set.Abilities.Add(setAbl);
            }
            return set;
        }

        public void LoadObject(Models.GameObject loadMe, GomObject obj)
        {
            GomLib.Models.ItemSet set = (Models.ItemSet)loadMe;
            Load(set, obj.Data);
        }

        public static void LoadReferences(Models.GameObject obj, GomObject gom)
        {
            // No references to load
        }
    }
}
