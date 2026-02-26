using HarmonyLib;
using UnityEngine;
using System.Linq;

namespace Denny_TameMobsNS
{
    [HarmonyPatch(typeof(AnimalPen), "CanHaveCard")]
    public class Patch_CardData_CanHaveCard
    {
        private static readonly string[] AllowedCardIDs =
        {
            "wheat",
            "egg",
            "magic_dust",
            "soil",
            "denny_occult_sigil",
            "gold_bar",
            "iron_bar",
            "wizard",
            "mage"
        };


        static bool Prefix(AnimalPen __instance, CardData otherCard, ref bool __result)
        {

            if (otherCard == null) return true;

            __result = AnimalPenCanHaveMethod(otherCard, __instance);
            return false;

        }

        private static bool AnimalPenCanHaveMethod(CardData otherCard, AnimalPen thisCard)
        {
            if (thisCard.Id == "animal_cage" && otherCard.Id == "animal_cage")
            {
                return true;
            }

            if (AllowedCardIDs.Contains(otherCard.Id)) return true;

            //uncomment this to allow TamableMobs int the normal AnimalPen
            //if (Denny_TameMobs.TamableMobsIDs.Contains(otherCard.Id)) return true; 

            int num = thisCard.GetChildCount() + (1 + otherCard.GetChildCount());

            if (!thisCard.IsForFish)
            {
                return otherCard is Animal && otherCard.MyCardType != CardType.Fish && num <= thisCard.MaxAnimalCount;
            }
            return otherCard is Animal && otherCard.MyCardType == CardType.Fish && num <= thisCard.MaxAnimalCount;
        }

    }
}