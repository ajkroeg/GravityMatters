using Harmony;
using System;
using System.Reflection;
using Newtonsoft.Json;

namespace GravityMatters
{

    public static class ModInit
    {
        public static GravityMattersSettings Settings = new GravityMattersSettings();
        public const string HarmonyPackage = "us.tbone.GravityMatters";
        public static void Init(string directory, string settingsJSON)
        {
            try
            {
                ModInit.Settings = JsonConvert.DeserializeObject<GravityMattersSettings>(settingsJSON);
            }
            catch (Exception)
            {
                
                ModInit.Settings = new GravityMattersSettings();
            }
            var harmony = HarmonyInstance.Create(HarmonyPackage);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

    }
    public class GravityMattersSettings
    {
        public bool UsePlanetGravityTags = false;
        public float LunarJumpOverride = 1.2f;
        public float MartianJumpOverride = 1.2f;
        public float lowgravJumpOverride = 1.2f;
        public float highgravJumpOverride = 0.8f;
        public bool JumpHeatOverrideFlag = false;
        public float JumpHeatOverrideMultiplier = 0.83f;
    }
}