using System.Collections.Generic;
using HarmonyLib;

namespace Denny_TameMobsNS
{
    public static class TameUtil
    {
        private const string TamedKey = "denny_tamemobs.tamed";

        public static bool IsTamed(CardData card)
        {
            if (card == null) return false;
            if (card.LeftoverExtraData == null) return false;

            for (int i = 0; i < card.LeftoverExtraData.Count; i++)
            {
                ExtraCardData e = card.LeftoverExtraData[i];
                if (e != null && e.AttributeId == TamedKey)
                    return e.BoolValue; 
            }

            return false;
        }

        public static void SetTamedTag(CardData card, bool tamed)
        {
            if (card == null) return;

            if (card.LeftoverExtraData == null)
                card.LeftoverExtraData = new List<ExtraCardData>();

            for (int i = 0; i < card.LeftoverExtraData.Count; i++)
            {
                ExtraCardData e = card.LeftoverExtraData[i];
                if (e != null && e.AttributeId == TamedKey)
                {
                    e.BoolValue = tamed;
                    return;
                }
            }

            card.LeftoverExtraData.Add(new ExtraCardData(TamedKey, tamed));
        }



        public static void TameMob(Enemy enemyBro)
        {
            if (enemyBro == null) return;

            if (enemyBro.gameObject.GetComponent<Denny_TamedMob>() != null)
                return;

            enemyBro.CurrentTarget = null;
            enemyBro.BaseAttackType = AttackType.None;
            enemyBro.CanAttack = false;
            enemyBro.CanHaveInventory = false;
            enemyBro.IsAggressive = false;


            string baseName = enemyBro.CustomName;

            if (string.IsNullOrEmpty(baseName))
            {
                baseName = !string.IsNullOrEmpty(enemyBro.nameOverride)
                    ? enemyBro.nameOverride
                    : enemyBro.FullName;

                // Strip repeated prefixes if they already exist
                while (!string.IsNullOrEmpty(baseName) && baseName.StartsWith(Denny_TameMobs.TamedMobPrefix))
                    baseName = baseName.Substring(Denny_TameMobs.TamedMobPrefix.Length);
            }

            UpdateCardVisuals(enemyBro);

            // Mark as tamed component
            var tamedMobScript = enemyBro.gameObject.AddComponent<Denny_TamedMob>();
            tamedMobScript.cardID = enemyBro.Id;
            tamedMobScript.myCardData = enemyBro;
            tamedMobScript.myGameCard = enemyBro.MyGameCard;
        }

        public static void UpdateCardVisuals(Enemy enemyBro)
        {
            if (IsTamed(enemyBro))
            {
                if (!string.IsNullOrEmpty(enemyBro.CustomName))
                    enemyBro.nameOverride = enemyBro.CustomName;
                else
                    enemyBro.nameOverride = Denny_TameMobs.TamedMobPrefix + CardDataNameAccess.GetInternalName(enemyBro) + Denny_TameMobs.TamedMobSuffix;
            }

            else
            {
                if (!string.IsNullOrEmpty(enemyBro.CustomName))
                    enemyBro.nameOverride = enemyBro.CustomName;
            }

            enemyBro.MyGameCard.UpdateCardPalette();
        }
    }

    public static class CardDataNameAccess
    {
        private static readonly AccessTools.FieldRef<CardData, string> _nameRef =
            AccessTools.FieldRefAccess<CardData, string>("_name");

        // GETTER
        public static string GetInternalName(CardData cd)
        {
            return _nameRef(cd);
        }

        // SETTER
        public static void SetInternalName(CardData cd, string value)
        {
            _nameRef(cd) = value;
        }
    }
}