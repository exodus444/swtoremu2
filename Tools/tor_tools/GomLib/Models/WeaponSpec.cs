﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GomLib.Models
{
    /// <summary>Available Weapon Specs from /server/tbl/cbtWeaponTable.tbl</summary>
    public enum WeaponSpec
    {
        Undefined = 0,        // WeaponSpecs not listed in the weapon spec table, e.g. lightsaber_purple, itm.npc.blaster.grunt.level4, itm.npc.ranged
        NpcRangedEnergy = 1,// wpn.npc.ranged_energy
        NpcMeleeKinetic = 2,// wpn.npc.melee_kinetic
        NpcMeleeEnergy = 3, // wpn.npc.melee_energy
        Lightsaber = 4,     // wpn.pc.lightsaber
        TrainingSaber = 5,  // wpn.pc.trainingsaber
        Polesaber = 6,      // wpn.pc.polesaber
        Unarmed = 7,        // wpn.pc.unarmed
        Gauntlet = 8,       // wpn.pc.gauntlet
        Vibroblade = 9,     // wpn.pc.vibroblade
        Electrostaff = 10,  // wpn.pc.electrostaff
        Vibroknife = 11,    // wpn.pc.vibroknife
        Pistol = 12,        // wpn.pc.pistol
        Rifle = 13,         // wpn.pc.rifle
        SniperRifle = 14,   // wpn.pc.sniper_rifle
        AssaultCannon = 15, // wpn.pc.assault_cannon
        Shotgun = 16,       // wpn.pc.shotgun
        TestRanged1h = 17,  // wpn.test.ranged_1h
        TestRanged2h = 18,  // wpn.test.ranged_2h
        TestMelee1h = 19,   // wpn.test.melee_1h
        TestMelee2h = 20,   // wpn.test.melee_2h
        Test21 = 21,        // wpn.test.regression.weapon_ranged
        Test22 = 22,        // wpn.test.npc.ranged_staged
        VibrobladeTech = 23,// wpn.pc.vibroblade_tech
        ElectrostaffTech = 24// wpn.pc.electrostaff_tech
    }

    public static class WeaponSpecExtensions
    {
        public static WeaponSpec ToWeaponSpec(this string str)
        {
            if (String.IsNullOrEmpty(str)) return WeaponSpec.Undefined;
            switch (str)
            {
                case "wpn.npc.ranged_energy": return WeaponSpec.NpcRangedEnergy;
                case "wpn.npc.melee_kinetic": return WeaponSpec.NpcMeleeKinetic;
                case "wpn.npc.melee_energy": return WeaponSpec.NpcMeleeEnergy;
                case "wpn.pc.lightsaber": return WeaponSpec.Lightsaber;
                case "wpn.pc.trainingsaber": return WeaponSpec.TrainingSaber;
                case "wpn.pc.polesaber": return WeaponSpec.Polesaber;
                case "wpn.pc.unarmed": return WeaponSpec.Unarmed;
                case "wpn.pc.gauntlet": return WeaponSpec.Gauntlet;
                case "wpn.pc.vibroblade": return WeaponSpec.Vibroblade;
                case "wpn.pc.electrostaff": return WeaponSpec.Electrostaff;
                case "wpn.pc.vibroknife": return WeaponSpec.Vibroknife;
                case "wpn.pc.pistol": return WeaponSpec.Pistol;
                case "wpn.pc.rifle": return WeaponSpec.Rifle;
                case "wpn.pc.sniper_rifle": return WeaponSpec.SniperRifle;
                case "wpn.pc.assault_cannon": return WeaponSpec.AssaultCannon;
                case "wpn.pc.shotgun": return WeaponSpec.Shotgun;
                case "wpn.test.ranged_1h": return WeaponSpec.TestRanged1h;
                case "wpn.test.ranged_2h": return WeaponSpec.TestRanged2h;
                case "wpn.test.melee_1h": return WeaponSpec.TestMelee1h;
                case "wpn.test.melee_2h": return WeaponSpec.TestMelee2h;
                case "wpn.test.regression.weapon_ranged": return WeaponSpec.Test21;
                case "wpn.test.npc.ranged_staged": return WeaponSpec.Test22;
                case "wpn.pc.vibroblade_tech": return WeaponSpec.VibrobladeTech;
                case "wpn.pc.electrostaff_tech": return WeaponSpec.ElectrostaffTech;
                case "lightsaber_purple":
                case "itm.npc.blaster.grunt.level4":
                case "itm.npc.ranged":
                    return WeaponSpec.Undefined;
                default: throw new InvalidOperationException("Unknown WeaponSpec: " + str);
            }
        }

        public static WeaponSpec ToWeaponSpec(ulong val)
        {
            // THESE VALUES ARE DEFINED IN cbtWeaponPrototype
            switch (val)
            {
                case 16140928756886587319: return WeaponSpec.NpcRangedEnergy;
                case 16140975835053173911: return WeaponSpec.NpcMeleeKinetic;
                case 16141148174188008413: return WeaponSpec.NpcMeleeEnergy;
                case 16141073364047128647:
                case 16140959248801439990:
                    return WeaponSpec.Lightsaber;
                case 16140904775591778297: return WeaponSpec.Polesaber;
                case 16140980641957308349:
                case 16141080658808713365: return WeaponSpec.Unarmed;
                case 16141014140383536168: return WeaponSpec.Vibroblade;
                case 16140993574079637585: return WeaponSpec.TrainingSaber;
                case 16141001868656241281: return WeaponSpec.Electrostaff;
                case 16141171601309861681: return WeaponSpec.Vibroknife;
                case 16140947982327943081: return WeaponSpec.Pistol;
                case 16140939612183949602: return WeaponSpec.Rifle;
                case 16140945769811276372: return WeaponSpec.SniperRifle;
                case 16141054056412565729: return WeaponSpec.AssaultCannon;
                case 16141126804575299650: return WeaponSpec.Shotgun;
                case 16140967610640264697: return WeaponSpec.VibrobladeTech;
                case 16141077019582649497: return WeaponSpec.ElectrostaffTech;
                case 16141022257089501878: return WeaponSpec.TestRanged1h;
                case 16141150613398334960: return WeaponSpec.Gauntlet;
                case 0: return WeaponSpec.Undefined;
                default: throw new InvalidOperationException("Unknown WeaponSpec: " + val);
            }
        }

        public static bool HasStats(this WeaponSpec spec)
        {
            switch (spec)
            {
                case WeaponSpec.AssaultCannon:
                case WeaponSpec.Electrostaff:
                case WeaponSpec.ElectrostaffTech:
                case WeaponSpec.Lightsaber:
                case WeaponSpec.Gauntlet:
                case WeaponSpec.Pistol:
                case WeaponSpec.Polesaber:
                case WeaponSpec.Rifle:
                case WeaponSpec.Shotgun:
                case WeaponSpec.SniperRifle:
                case WeaponSpec.TrainingSaber:
                case WeaponSpec.Vibroblade:
                case WeaponSpec.VibrobladeTech:
                case WeaponSpec.Vibroknife:
                    return true;
                default:
                    return false;
            }
        }
    }
}
