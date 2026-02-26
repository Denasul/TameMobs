using HarmonyLib;
using UnityEngine;

namespace Denny_TameMobsNS
{
    [HarmonyPatch(typeof(CardData), nameof(CardData.HasEnergyInput))]
    public static class Patch_CardData_HasEnergyInput
    {
        static bool Prefix(CardData __instance, ref bool __result, CardConnector connectedNode = null)
        {

            var wm = WorldManager.instance;

            if (wm == null || wm.CurrentBoard == null)
                return true;


            int generatorCount = wm.GetCardCount("denny_tesla_tower");

            if (generatorCount > 0)
            {
                __result = true;
                return false; 
            }

            return true;
        }
    }
}