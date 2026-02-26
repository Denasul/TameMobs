using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

namespace Denny_TameMobsNS
{
    public static class Denny_TamingEnemy_Patches
    {
        private const string TamingItemID = "denny_occult_sigil";


        // Re-apply tame on LOAD

        [HarmonyPatch(typeof(CardData), "SetExtraCardData", new Type[] { typeof(List<ExtraCardData>) })]
        public static class Patch_CardData_SetExtraCardData
        {
            static void Postfix(CardData __instance, List<ExtraCardData> extraData)
            {
                if (__instance is not Enemy enemy) return;
                if (!TameUtil.IsTamed(enemy)) return;

                TameUtil.TameMob(enemy);
            }
        }


        // Tame when sigil is ON TOP

        [HarmonyPatch(typeof(Enemy), nameof(Enemy.UpdateCard))]
        public static class Patch_Enemy_UpdateCard_TameOnOccultSigil
        {
            static void Postfix(Enemy __instance)
            {
                if (__instance == null) return;

                TameUtil.UpdateCardVisuals(__instance);

                if (TameUtil.IsTamed(__instance))
                    return;

                if (__instance.HasCardOnTop(TamingItemID, out CardData sigil))
                {
                    TameUtil.SetTamedTag(__instance, true);
                    TameUtil.TameMob(__instance);

                    if (sigil?.MyGameCard != null)
                        sigil.MyGameCard.DestroyCard(true, true);

                    Debug.Log($"[Denny_TameMobs] Tamed '{__instance.Id}' using {TamingItemID}");
                }
            }
        }



            // Allow placing the sigil on mobs

            [HarmonyPatch]
        public static class Patch_Mob_CanHaveCard
        {
            static MethodBase TargetMethod()
            {
                return AccessTools.Method(typeof(Mob), "CanHaveCard", new[] { typeof(CardData) });
            }

            private static GameCard GetRootCardSkippingFoundation(GameCard start)
            {
                if (start == null) return null;

                GameCard root = start.GetRootCard();
                if (root == null) return null;

                if (root.CardData is HeavyFoundation && root.HasChild)
                    root = root.Child;

                return root;
            }

            private static Denny_SigilCage GetContainingSigilCage(Mob mob, out GameCard cageRootCard)
            {
                cageRootCard = null;
                if (mob?.MyGameCard == null || !mob.MyGameCard.HasParent)
                    return null;

                GameCard root = GetRootCardSkippingFoundation(mob.MyGameCard);
                if (root?.CardData is Denny_SigilCage cage)
                {
                    cageRootCard = root;
                    return cage;
                }

                return null;
            }

            private static int CountMobsInCageStack(GameCard cageRootCard)
            {
                // cageRootCard is the cage itself at the top of the stack.
                // Walk down Child links and count Mob cards.
                int count = 0;
                GameCard cur = cageRootCard;

                while (cur != null && cur.HasChild)
                {
                    cur = cur.Child;
                    if (cur?.CardData is Mob)
                        count++;
                }

                return count;
            }

            private static int GetMaxCreatureCount(Denny_SigilCage cage)
            {
                if (cage == null) return 0;


                // reads MaxCreatureCount from Sigil Cage

                var t = cage.GetType();

                FieldInfo f = AccessTools.Field(t, "MaxCreatureCount");
                if (f != null && f.FieldType == typeof(int))
                    return (int)f.GetValue(cage);

                PropertyInfo p = AccessTools.Property(t, "MaxCreatureCount");
                if (p != null && p.PropertyType == typeof(int))
                    return (int)p.GetValue(cage);

                return 0;
            }

            static bool Prefix(Mob __instance, CardData otherCard, ref bool __result)
            {
                if (__instance == null || otherCard == null)
                    return true;

                if (otherCard is NamingStone && TameUtil.IsTamed(__instance))
                {
                    __result = true;
                    return false;
                }

                // Mob-on-mob stacking:
                // Only allowed when the TARGET mob is in a Sigil Cage AND the cage is not full.
                if (otherCard is Mob)
                {
                    GameCard cageRootCard;
                    Denny_SigilCage cage = GetContainingSigilCage(__instance, out cageRootCard);

                    if (cage == null || cageRootCard == null)
                    {
                        __result = false;
                        return false;
                    }

                    int max = GetMaxCreatureCount(cage);
                    int current = CountMobsInCageStack(cageRootCard);

                    __result = (max <= 0) ? false : (current + 1) <= max;
                    return false;
                }

                // intercept sigil logic
                if (otherCard.Id != TamingItemID)
                    return true;

                bool allowed =
                    Denny_TameMobs.TamableMobsIDs != null &&
                    Denny_TameMobs.TamableMobsIDs.Contains(__instance.Id) &&
                    !TameUtil.IsTamed(__instance);

                __result = allowed;
                return false;
            }
        }

    }
}