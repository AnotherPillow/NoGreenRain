using HarmonyLib;
using StardewModdingAPI;
using StardewValley.Menus;
using StardewValley;
using System.Runtime.CompilerServices;

namespace No_Green_Rain
{

    internal sealed class ModEntry : Mod
    {

        // private ModConfig Config;
        private static ModConfig Config;
        public override void Entry(IModHelper helper)
        {
            ModEntry.Config = this.Helper.ReadConfig<ModConfig>();

            var harmony = new Harmony(this.ModManifest.UniqueID);

            harmony.Patch(
               original: AccessTools.Method(typeof(Utility), nameof(Utility.isGreenRainDay)),
               prefix: new HarmonyMethod(typeof(ModEntry), nameof(Utility_IsGreenRainDay_Prefix))
            );
            harmony.Patch(
                original: AccessTools.Method(typeof(Game1), nameof(Game1.getWeatherModificationsForDate)),
                postfix: new HarmonyMethod(typeof(ModEntry), nameof(Game1_GetWeatherModificationsForDate_PostFix))
            );
        }

        public static bool Utility_IsGreenRainDay_Prefix(ref bool __result)
        {
            __result = Config.AlwaysGreenRain;
            return false;

        }

        public static void Game1_GetWeatherModificationsForDate_PostFix(ref string __result)
        {
            bool alwaysGreenRain = Config.AlwaysGreenRain;
            bool alternativeIsSun = Config.AlternativeIsSun;

            bool willBeGreenRain = __result == "GreenRain";

            if (alwaysGreenRain)
            {
                __result = "GreenRain";
            } else if (alternativeIsSun && willBeGreenRain)
            {
                __result = "Sun";
            } else if (willBeGreenRain)
            {
                __result = "Rain";
            }
        }
    }
}