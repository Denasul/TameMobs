using UnityEngine;
using System;
using System.Linq;
using HarmonyLib;

namespace Denny_TameMobsNS
{
    public class Denny_OccultSigil : Resource
    {
        private static readonly string[] AllowedCardIDs =
        {
            "animal_pen",
            "iron_bar",
            "gold_bar",
            "wizard",
            "mage",
            "denny_sigil_cage",
            "slaughter_house"
        };

        protected override bool CanHaveCard(CardData otherCard)
        {
            if (AllowedCardIDs.Contains(otherCard.Id))
                return true;

            return base.CanHaveCard(otherCard);
        }



    }
}

