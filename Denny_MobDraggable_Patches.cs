using HarmonyLib;

namespace Denny_TameMobsNS
{
    // Took so long to figure out how to patch this getter (Ty ChatGPT) 
    [HarmonyPatch(typeof(Mob), "get_CanBeDragged")]
    public static class Patch_Mob_get_CanBeDragged
    {
        static bool Prefix(Mob __instance, ref bool __result)
        {

            if (!Denny_TameMobs.dragUntamedMobs.Value)
            {
                if (!TameUtil.IsTamed(__instance)) return true;
            }

            if (__instance != null && Denny_TameMobs.TamableMobsIDs.Contains(__instance.Id))
            {
                __result = true;
                return false;
            }

            return true; 
        }
    }

    [HarmonyPatch(typeof(Mob), "get_CanMove")]
    public static class Patch_Mob_get_CanMove
    {
        static bool Prefix(Mob __instance, ref bool __result)
        {
            if (__instance != null && Denny_TameMobs.TamableMobsIDs.Contains(__instance.Id))
            {
                __result = MobDragUtil.CanMoveLikeTameMob(__instance) && !__instance.MyGameCard.BeingDragged;
                return false; 
            }

            return true;
        }
    }

    internal static class MobDragUtil
    {
        public static bool CanMoveLikeTameMob(Mob mob) // pretty much the nomal method that Animals have
        {
            if (mob == null || mob.MyGameCard == null)
                return false;

            if (IsInAnyPenOrMagnet(mob))
                return false;

            if (mob.MyGameCard.HasParent || mob.MyGameCard.HasChild)
                return false;

            if (mob.MyGameCard.GetCardWithStatusInStack() != null)
                return false;

            return true;
        }

        public static bool IsInAnyPenOrMagnet(Mob mob)
        {
            // Mirrors your original InAnimalPen logic

            return GetRootStructure(mob) is AnimalPen
                || GetRootStructure(mob) is BreedingPen
                || GetRootStructure(mob) is ResourceMagnet
                || GetRootStructure(mob) is SlaughterHouse
                || GetRootStructure(mob) is PettingZoo;
        }

        private static CardData GetRootStructure(Mob mob)
        {
            if (mob == null || mob.MyGameCard == null)
                return null;

            if (!mob.MyGameCard.HasParent)
                return null;

            GameCard root = mob.MyGameCard.GetRootCard();

            // HeavyFoundation special-case 
            if (root.CardData is HeavyFoundation && root.HasChild)
                root = root.Child;

            return root.CardData;
        }
    }
}