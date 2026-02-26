using HarmonyLib;
using UnityEngine;
using System.Linq;

namespace Denny_TameMobsNS
{
    [HarmonyPatch(typeof(NamingStone), "CanHaveCard")]
    public class Patch_NamingStone_CanHaveCard
    {


        static bool Prefix(NamingStone __instance, CardData otherCard, ref bool __result)
        {

            if (otherCard == null) return true;

            if (Denny_TameMobs.TamableMobsIDs.Contains(otherCard.Id))
            {
                __result = true;
                return false;
            }
            else return true;

        }


    }
}