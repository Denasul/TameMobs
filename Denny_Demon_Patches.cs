using HarmonyLib;

// Script makes sure that if the demon / demon_lord are tamed and you kill them, you don't get asked to click the "continue" button

namespace Denny_TameMobsNS
{
    [HarmonyPatch(typeof(Demon), nameof(Demon.Die))]
    public static class Patch_Demon_Die
    {
        public static bool Prefix(Demon __instance)
        {
            if (__instance == null) return true;

            if (TameUtil.IsTamed(__instance))
            {
                __instance.TryDropItems();


                __instance.MyGameCard.GetAllCardsInStack().Remove(__instance.MyGameCard);
                __instance.MyGameCard.DestroyCard(true, true);

                return false;
            }

            return true; 
        }
    }
}