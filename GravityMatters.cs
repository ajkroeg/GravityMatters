﻿using Harmony;
using BattleTech;
using HBS.Collections;
using UnityEngine;
using System.Reflection;
using System.Linq;
using BattleTech.UI;

namespace GravityMatters
{
    class GravityMatters
    {
        public static float JumpMultiplier;

        private static readonly SimGameState sim = UnityGameInstance.BattleTechGame.Simulation;
        private static readonly CombatGameState combat = UnityGameInstance.BattleTechGame.Combat;

        [HarmonyPatch(typeof(Mech), "JumpDistance", MethodType.Getter)]
        public static class Mech_JumpDistance_Patch
        {
            private static readonly SimGameState sim = UnityGameInstance.BattleTechGame.Simulation;

            public static void Postfix(Mech __instance, ref float __result)
            {
                if (ModInit.Settings.UsePlanetGravityTags == true && !combat.ActiveContract.ContractTypeValue.IsSkirmish)
                {
                    TagSet CurrentSystem = sim.CurSystem.Tags;
                    if (CurrentSystem.Contains("planet_size_small"))
                    {
                        JumpMultiplier = ModInit.Settings.lowgravJumpOverride;
                    }
                    else if (CurrentSystem.Contains("planet_size_large"))
                    {
                        JumpMultiplier = ModInit.Settings.highgravJumpOverride;
                    }
                    else
                    {
                        JumpMultiplier = 1.0f;
                    }
                    __result *= JumpMultiplier;
                }
                else
                {
                    string currentMask = __instance.Combat.MapMetaData.biomeDesignMask.Id;
                    if (currentMask.Contains("DesignMaskBiomeLunarVacuum"))
                    {
                        JumpMultiplier = ModInit.Settings.LunarJumpOverride;
                    }
                    else if (currentMask.Contains("DesignMaskBiomeMartianVacuum"))
                    {
                        JumpMultiplier = ModInit.Settings.MartianJumpOverride;
                    }
                    else
                    {
                        JumpMultiplier = 1.0f;
                    }
                    __result *= JumpMultiplier;
                }
            }
        }
        [HarmonyPatch(typeof(Mech), "CalcJumpHeat")]
        public static class CalcJumpHeat_Patch
        {
            public static void Postfix(Mech __instance, ref float __result)
            {
                if (ModInit.Settings.JumpHeatOverrideFlag == true)
                {
                    __result *= ModInit.Settings.JumpHeatOverrideMultiplier;
                }
                else
                {
                    __result *= (1 / JumpMultiplier);
                }
            }
        }
        public static float WeaponRangeMultiplier;
        
        [HarmonyPatch(typeof(Weapon), "MaxRange", MethodType.Getter)]
        [HarmonyAfter(new string[] { "io.mission.modrepuation" })]

        public static class Weapon_MaxRange_Patch
        {
            private static bool Prepare(HarmonyInstance harmony)
            {
                return ModInit.Settings.BuffWeaponRange;
            }



            public static void Postfix(Weapon __instance, ref float __result)
            {
                if (ModInit.Settings.UsePlanetGravityTags == true && !combat.ActiveContract.ContractTypeValue.IsSkirmish)
                {
                    if (ModInit.Settings.LetGravityAffectEnergyWeps != true && __instance.WeaponCategoryValue.IsEnergy)
                    {
                        return;
                    }
                    TagSet CurrentSystem = sim.CurSystem.Tags;
                    if (CurrentSystem.Contains("planet_size_small"))
                    {
                        WeaponRangeMultiplier = ModInit.Settings.lowgravWeaponOverride;
                    }
                    else if (CurrentSystem.Contains("planet_size_large"))
                    {
                        WeaponRangeMultiplier = ModInit.Settings.highgravWeaponOverride;
                    }
                    else
                    {
                        WeaponRangeMultiplier = 1.0f;
                    }
                    __result *= WeaponRangeMultiplier;
                }
                else
                {
                    if (ModInit.Settings.LetGravityAffectEnergyWeps != true && __instance.WeaponCategoryValue.IsEnergy)
                    {
                        return;
                    }
                    if (combat.MapMetaData.biomeSkin == Biome.BIOMESKIN.lunarVacuum)
                    {
                        WeaponRangeMultiplier = ModInit.Settings.LunarWeaponOverride;
                    }
                    else if (combat.MapMetaData.biomeSkin == Biome.BIOMESKIN.martianVacuum)
                    {
                        WeaponRangeMultiplier = ModInit.Settings.MartianWeaponOverride;
                    }
                    else
                    {
                        WeaponRangeMultiplier = 1.0f;
                    }
                    __result *= WeaponRangeMultiplier;
                }
            }
        }
        
        [HarmonyPatch(typeof(Weapon), "LongRange", MethodType.Getter)]
        [HarmonyAfter(new string[] { "io.mission.modrepuation" })]

        public static class Weapon_LongRange_Patch
        {
            private static bool Prepare(HarmonyInstance harmony)
            {
                if (ModInit.Settings.BuffAllRangeBrackets == true && ModInit.Settings.BuffWeaponRange == true)
                {
                    return true;
                }
                else
                    return false;
            }



            public static void Postfix(Weapon __instance, ref float __result)
            {
                if (ModInit.Settings.UsePlanetGravityTags == true && !combat.ActiveContract.ContractTypeValue.IsSkirmish)
                {
                    if (!ModInit.Settings.LetGravityAffectEnergyWeps && (__instance.WeaponCategoryValue.IsEnergy || __instance.Type == WeaponType.Laser || __instance.Type == WeaponType.PPC || __instance.Type == WeaponType.COIL))
                    {
                        return;
                    }
                    TagSet CurrentSystem = sim.CurSystem.Tags;
                    if (CurrentSystem.Contains("planet_size_small"))
                    {
                        WeaponRangeMultiplier = ModInit.Settings.lowgravWeaponOverride;
                    }
                    else if (CurrentSystem.Contains("planet_size_large"))
                    {
                        WeaponRangeMultiplier = ModInit.Settings.highgravWeaponOverride;
                    }
                    else
                    {
                        WeaponRangeMultiplier = 1.0f;
                    }
                    __result *= WeaponRangeMultiplier;
                }
                else
                {

                    if (!ModInit.Settings.LetGravityAffectEnergyWeps && (__instance.WeaponCategoryValue.IsEnergy || __instance.Type == WeaponType.Laser || __instance.Type == WeaponType.PPC || __instance.Type == WeaponType.COIL))
                    {
                        return;
                    }
                    if (combat.MapMetaData.biomeSkin == Biome.BIOMESKIN.lunarVacuum)
                    {
                        WeaponRangeMultiplier = ModInit.Settings.LunarWeaponOverride;
                    }
                    else if (combat.MapMetaData.biomeSkin == Biome.BIOMESKIN.martianVacuum)
                    {
                        WeaponRangeMultiplier = ModInit.Settings.MartianWeaponOverride;
                    }
                    else
                    {
                        WeaponRangeMultiplier = 1.0f;
                    }
                    __result *= WeaponRangeMultiplier;
                }
            }
        }
        [HarmonyPatch(typeof(Weapon), "MediumRange", MethodType.Getter)]
        [HarmonyAfter(new string[] { "io.mission.modrepuation" })]

        public static class Weapon_MediumRange_Patch
        {
            private static bool Prepare(HarmonyInstance harmony)
            {
                if (ModInit.Settings.BuffAllRangeBrackets == true && ModInit.Settings.BuffWeaponRange == true)
                {
                    return true;
                }
                else
                    return false;
            }



            public static void Postfix(Weapon __instance, ref float __result)
            {
                if (ModInit.Settings.UsePlanetGravityTags == true && !combat.ActiveContract.ContractTypeValue.IsSkirmish)
                {
                    if (!ModInit.Settings.LetGravityAffectEnergyWeps && (__instance.WeaponCategoryValue.IsEnergy || __instance.Type == WeaponType.Laser || __instance.Type == WeaponType.PPC || __instance.Type == WeaponType.COIL))
                    {
                        return;
                    }
                    TagSet CurrentSystem = sim.CurSystem.Tags;
                    if (CurrentSystem.Contains("planet_size_small"))
                    {
                        WeaponRangeMultiplier = ModInit.Settings.lowgravWeaponOverride;
                    }
                    else if (CurrentSystem.Contains("planet_size_large"))
                    {
                        WeaponRangeMultiplier = ModInit.Settings.highgravWeaponOverride;
                    }
                    else
                    {
                        WeaponRangeMultiplier = 1.0f;
                    }
                    __result *= WeaponRangeMultiplier;
                }
                else
                {

                    if (!ModInit.Settings.LetGravityAffectEnergyWeps && (__instance.WeaponCategoryValue.IsEnergy || __instance.Type == WeaponType.Laser || __instance.Type == WeaponType.PPC || __instance.Type == WeaponType.COIL))
                    {
                        return;
                    }
                    if (combat.MapMetaData.biomeSkin == Biome.BIOMESKIN.lunarVacuum)
                    {
                        WeaponRangeMultiplier = ModInit.Settings.LunarWeaponOverride;
                    }
                    else if (combat.MapMetaData.biomeSkin == Biome.BIOMESKIN.martianVacuum)
                    {
                        WeaponRangeMultiplier = ModInit.Settings.MartianWeaponOverride;
                    }
                    else
                    {
                        WeaponRangeMultiplier = 1.0f;
                    }
                    __result *= WeaponRangeMultiplier;
                }
            }
        }
        [HarmonyPatch(typeof(Weapon), "ShortRange", MethodType.Getter)]
        [HarmonyAfter(new string[] { "io.mission.modrepuation" })]

        public static class Weapon_ShortRange_Patch
        {
            private static bool Prepare(HarmonyInstance harmony)
            {
                if (ModInit.Settings.BuffAllRangeBrackets == true && ModInit.Settings.BuffWeaponRange == true)
                {
                    return true;
                }
                else
                    return false;
            }



            public static void Postfix(Weapon __instance, ref float __result)
            {
                if (ModInit.Settings.UsePlanetGravityTags == true && !combat.ActiveContract.ContractTypeValue.IsSkirmish)
                {
                    if (!ModInit.Settings.LetGravityAffectEnergyWeps && (__instance.WeaponCategoryValue.IsEnergy || __instance.Type == WeaponType.Laser || __instance.Type == WeaponType.PPC || __instance.Type == WeaponType.COIL))
                    {
                        return;
                    }
                    TagSet CurrentSystem = sim.CurSystem.Tags;
                    if (CurrentSystem.Contains("planet_size_small"))
                    {
                        WeaponRangeMultiplier = ModInit.Settings.lowgravWeaponOverride;
                    }
                    else if (CurrentSystem.Contains("planet_size_large"))
                    {
                        WeaponRangeMultiplier = ModInit.Settings.highgravWeaponOverride;
                    }
                    else
                    {
                        WeaponRangeMultiplier = 1.0f;
                    }
                    __result *= WeaponRangeMultiplier;
                }
                else
                {
                    if (!ModInit.Settings.LetGravityAffectEnergyWeps && (__instance.WeaponCategoryValue.IsEnergy || __instance.Type == WeaponType.Laser || __instance.Type == WeaponType.PPC || __instance.Type == WeaponType.COIL))
                    {
                        return;
                    }
                    if (combat.MapMetaData.biomeSkin == Biome.BIOMESKIN.lunarVacuum)
                    {
                        WeaponRangeMultiplier = ModInit.Settings.LunarWeaponOverride;
                    }
                    else if (combat.MapMetaData.biomeSkin == Biome.BIOMESKIN.martianVacuum)
                    {
                        WeaponRangeMultiplier = ModInit.Settings.MartianWeaponOverride;
                    }
                    else
                    {
                        WeaponRangeMultiplier = 1.0f;
                    }
                    __result *= WeaponRangeMultiplier;
                }
            }
        }
    }
}
