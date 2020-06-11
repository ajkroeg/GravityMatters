using Harmony;
using BattleTech;
using HBS.Collections;
using MechEngineer;
using Org.BouncyCastle.Crypto.Parameters;

namespace GravityMatters
{
    class GravityMatters
    {
        public static float JumpMultiplier;
        [HarmonyPatch(typeof(Mech), "JumpDistance", MethodType.Getter)]
        public static class Mech_JumpDistance_Patch
        {
            private static readonly SimGameState sim = UnityGameInstance.BattleTechGame.Simulation;

            public static void Postfix(Mech __instance, ref float __result)
            {
                if (ModInit.Settings.UsePlanetGravityTags == true)
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
    }
}
