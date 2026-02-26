using HarmonyLib;
using System.Linq;

namespace Denny_TameMobsNS
{
    [HarmonyPatch(typeof(CardData), "CanHaveCard")]
    internal static class Patch_TransmissionTower_CanHaveCard
    {
        static bool Prefix(CardData __instance, CardData otherCard, ref bool __result)
        {
            if (otherCard == null) return true;

            if (__instance is not TransmissionTower) return true;

                if (AllowedCardIDs.Contains(otherCard.Id))
                {
                __result = true;
                return false;
                }

                return true;
            
        }

        private static readonly string[] AllowedCardIDs =
        {
            "genius",
            "denny_tesla_coil",
            "copper_bar"
        };
    }
}