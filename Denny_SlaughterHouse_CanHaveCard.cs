using HarmonyLib;

namespace Denny_TameMobsNS
{
    [HarmonyPatch(typeof(SlaughterHouse), "CanHaveCard")]
    internal static class Patch_SlaughterHouse_CanHaveCard
    {
        private static readonly string[] AllowedCardIDs =
        {
            "magic_dust",
            "denny_occult_sigil",
            "iron_bar",
            "denny_human_blood"
        };

        static bool Prefix(SlaughterHouse __instance, CardData otherCard, ref bool __result)
        {
            if (otherCard == null) return true;

            if (AllowedCardIDs.Contains(otherCard.Id))
            {
                __result = true;
                return false; 
            }

            return true; 
        }
    }
}