using HarmonyLib;
using UnityEngine;

namespace Denny_TameMobsNS
{
    [HarmonyPatch(typeof(WorldManager), nameof(WorldManager.Play))]
    public static class Denny_WorldManager_Play_Patch
    {
        static void Postfix(WorldManager __instance)
        {
            Denny_TameMobs.instance.LoadBlueprintGroupSolver();
        }
    }
}