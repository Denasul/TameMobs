using HarmonyLib;

namespace Denny_TameMobsNS
{
    [HarmonyPatch(typeof(Worker), "CanHaveCard")]
    public static class Patch_Worker_CanHaveCard_Router
    {
        private static readonly string[] AllowOnGenius =
        {
            "transmission_tower"
        };

        private static readonly string[] AllowOnWorker =
        {

        };

        static bool Prefix(Worker __instance, CardData otherCard, ref bool __result)
        {
            if (__instance == null) return true;
            if (otherCard == null) return true; 

            string workerId = __instance.Id ?? string.Empty;
            string otherId = otherCard.Id ?? string.Empty;

            switch (workerId)
            {
                case "genius":
                    if (AllowOnGenius.Contains(otherId))
                    {
                        __result = true;
                        return false;
                    }
                    return true;

                case "educated_worker":
                    if (AllowOnWorker.Contains(otherId))
                    {
                        __result = true;
                        return false;
                    }
                    return true;

                default:
                    return true;
            }
        }
    }
}