using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Denny_TameMobsNS
{
    // This script adds the Ideas and Rumors to the Booster Packs

    public static class BoosterInjector
    {
        public static void InjectAllCardBags()
        {
            InjectIntoLocations();
            InjectIntoCities();
        }

        private static void InjectIntoLocations()
        {
            var wm = WorldManager.instance;
            if (wm == null || wm.GameDataLoader == null)
            {
                Debug.LogWarning("[Denny] WorldManager or GameDataLoader not ready.");
                return;
            }

            var gdl = wm.GameDataLoader;

            var booster = gdl.BoosterpackDatas
                .FirstOrDefault(b => b.BoosterId == "locations");

            if (booster == null)
            {
                Debug.LogWarning("[Denny] Booster 'locations' not found.");
                return;
            }

            booster.CardBags ??= new List<CardBag>();

            // Prevent duplicate insertion
            bool alreadyInserted = booster.CardBags.Any(bag =>
                bag?.Chances?.Any(c => c?.Id == "denny_rumor_blood_extraction") == true
            );

            if (alreadyInserted)
            {
                Debug.Log("[Denny] Custom bag already injected.");
                return;
            }

            var myBag = new CardBag
            {
                CardBagType = CardBagType.Chances,
                CardsInPack = 1,
                UseFallbackBag = false,
                Chances = new List<CardChance>
                {
                    new CardChance { Id = "denny_rumor_blood_extraction", Chance = 1 },
                    new CardChance { Id = "denny_rumor_taming", Chance = 1 },

                    new CardChance { Id = "denny_blueprint_infernal_slaughterhouse", Chance = 1 },
                    new CardChance { Id = "denny_blueprint_blood_extractor", Chance = 1 },
                    new CardChance { Id = "denny_blueprint_summoning_ring", Chance = 1 },
                    new CardChance { Id = "denny_blueprint_occult_sigil", Chance = 1 },
                    new CardChance { Id = "denny_blueprint_sigil_cage", Chance = 1 }
                }
            };

            booster.CardBags.Insert(0, myBag);

            Debug.Log("[Denny] Injected custom CardBag into 'locations'.");
        }

        private static void InjectIntoCities()
        {
            var wm = WorldManager.instance;
            if (wm == null || wm.GameDataLoader == null)
            {
                Debug.LogWarning("[Denny] WorldManager or GameDataLoader not ready.");
                return;
            }

            var gdl = wm.GameDataLoader;

            var booster = gdl.BoosterpackDatas
                .FirstOrDefault(b => b.BoosterId == "cities_industry");

            if (booster == null)
            {
                Debug.LogWarning("[Denny] Booster 'cities_industry' not found.");
                return;
            }

            booster.CardBags ??= new List<CardBag>();

            // Prevent duplicate insertion
            bool alreadyInserted = booster.CardBags.Any(bag =>
                bag?.Chances?.Any(c => c?.Id == "denny_blueprint_tesla_tower") == true
            );

            if (alreadyInserted)
            {
                Debug.Log("[Denny] Custom bag already injected.");
                return;
            }

            var myBag = new CardBag
            {
                CardBagType = CardBagType.Chances,
                CardsInPack = 1,
                UseFallbackBag = false,
                Chances = new List<CardChance>
                {
                    new CardChance { Id = "denny_blueprint_tesla_tower", Chance = 1 },
                    new CardChance { Id = "denny_blueprint_tesla_coil", Chance = 1 },
                    new CardChance { Id = "denny_blueprint_copper_coil", Chance = 1 }
                }
            };

            booster.CardBags.Insert(0, myBag);

            Debug.Log("[Denny] Injected custom CardBag into 'cities_industry'.");
        }
    }
}