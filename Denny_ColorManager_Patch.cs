using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace Denny_TameMobsNS
{

    public static class CustomUIColorRegistry
    {
        [HarmonyPatch(typeof(ColorManager), nameof(ColorManager.GetColor))]
        public static class Patch_ColorManager_GetColor
        {
            static bool Prefix(UIColor col, ref Color __result)
            {
                if (col.ToString() == Denny_TameMobs.BlueprintTabColor.ToString())
                {
                    __result = Denny_TameMobs.BlueprintGroup255Value;
                    return false;
                }

                return true;
            }
        }
    }
}