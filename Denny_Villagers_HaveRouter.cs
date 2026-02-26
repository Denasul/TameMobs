using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;

namespace Denny_TameMobsNS
{
    [HarmonyPatch(typeof(BaseVillager), "CanHaveCard")]
    public static class Patch_BaseVillager_CanHaveCard_Router
    {
        // same as Resources router but for Villagers

        private static readonly string[] AllowOnWizard =
        {
            "animal_pen",
            "denny_occult_sigil",
            "denny_sigil_cage"
        };

        private static readonly string[] AllowOnMage =
        {
            "animal_pen",
            "denny_occult_sigil",
            "denny_sigil_cage"
        };

        private static readonly string[] AllowOnBuilder =
        {
            
        };



        static bool Prefix(BaseVillager __instance, CardData otherCard, ref bool __result)
        {
            
                if (__instance == null)
                    return true;


                if (otherCard == null) return true;

                string villagerId = __instance.Id ?? string.Empty;
                string otherId = otherCard.Id ?? string.Empty;

                switch (villagerId)
                {
                case "wizard":
                    if (AllowOnWizard.Contains(otherId))
                    {
                        __result = true;
                        return false;
                    }
                    else return true;

                case "mage":
                    if (AllowOnMage.Contains(otherId))
                    {
                        __result = true;
                        return false;
                    }
                    else return true;


                case "builder":
                     if (AllowOnBuilder.Contains(otherId))
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