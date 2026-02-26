using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;

namespace Denny_TameMobsNS
{
    // Epic router to make recipes possible

    [HarmonyPatch(typeof(Resource), "CanHaveCard")]
    public static class Patch_Resource_CanHaveCard_Router
    {

        private static readonly string[] AllowOnGoldBar =
        {
            "animal_pen",
            "wizard"
        };

        private static readonly string[] AllowOnIronBar =
        {
            "animal_pen",
            "wizard",
            "slaughter_house"
        };

        private static readonly string[] AllowOnMagicDust =
        {
            "denny_sigil_cage",
            "slaughter_house"
        };
        
        private static readonly string[] AllowOnHumanBlood =
        {
            "denny_sigil_cage",
            "slaughter_house"
        };


        static bool Prefix(Resource __instance, CardData otherCard, ref bool __result)
        {
            
                if (__instance == null)
                    return true;


                if (otherCard == null) return true;

                string resourceId = __instance.Id ?? string.Empty;
                string otherId = otherCard.Id ?? string.Empty;

                switch (resourceId)
                {
                case "gold_bar":
                    if (AllowOnIronBar.Contains(otherId))
                    {
                        __result = true;
                        return false;
                    }
                    else return true;


                case "iron_bar":
                     if (AllowOnIronBar.Contains(otherId))
                    {
                        __result = true;
                        return false;
                    }
                     else return true;

                case "magic_dust":
                    if (AllowOnMagicDust.Contains(otherId))
                    {
                        __result = true;
                        return false;
                    }
                    else return true;

                case "denny_human_blood":
                    if (AllowOnHumanBlood.Contains(otherId))
                    {
                        __result = true;
                        return false;
                    }
                    else return true;

                default:
                        return true;
                }
            
        }
    }
}