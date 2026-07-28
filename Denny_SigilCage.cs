using UnityEngine;
using System;
using HarmonyLib;

namespace Denny_TameMobsNS
{
    public class Denny_SigilCage : CardData
    {
        public int MaxCreatureCount = 5;

        private static readonly string[] AllowedCardIDs =
        {
            "mage",
            "wizard",
            "magic_dust",
            "denny_occult_sigil",
            "denny_human_blood"
        };


        protected override bool CanHaveCard(CardData otherCard)
        {
            if (AllowedCardIDs.Contains(otherCard.Id)) return true;

            int num = base.GetChildCount() + (1 + otherCard.GetChildCount());

            if (Denny_TameMobs.TamableMobsIDs.Contains(otherCard.Id) && num <= MaxCreatureCount) return true;
            else return false;
        }

        protected override void Awake()
        {
            EnergyConnectors.Add(new CardConnectorData
            {
                EnergyConnectionStrength = ConnectionType.Transport,
                EnergyConnectionType = CardDirection.input,
                EnergyConnectionAmount = 3
            });

            EnergyConnectors.Add(new CardConnectorData
            {
                EnergyConnectionStrength = ConnectionType.Transport,
                EnergyConnectionType = CardDirection.output,
                EnergyConnectionAmount = 1
            });

            this.MyGameCard.CreateCardConnectors();

            base.Awake();
        }



    }
}

