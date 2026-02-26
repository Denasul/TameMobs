using System;
using UnityEngine;



namespace Denny_TameMobsNS
{
    public class Denny_BloodExtractor : CardData
    {
        private const int MaxCorpseCount = 6;
        private const float ExtractSeconds = 30f;

        private const string CorpseId = "corpse";
        private const string BoneId = "bone";

        private const string BloodId = "denny_human_blood";

        public override bool DetermineCanHaveCardsWhenIsRoot => true;

        public override bool CanHaveCardsWhileHasStatus()
        {
            return true;
        }

        protected override bool CanHaveCard(CardData otherCard)
        {
            if (otherCard == null)
                return false;

            // Only allow corpses
            if (!string.Equals(otherCard.Id, CorpseId, StringComparison.Ordinal))
                return false;

            // Enforce maximum stack size
            int num = base.GetChildCount() + (1 + otherCard.GetChildCount());
            return num <= MaxCorpseCount;
        }

        public override void UpdateCard()
        {
            if (this.MyGameCard != null
                && this.MyGameCard.HasChild
                && this.MyGameCard.Child != null
                && this.MyGameCard.Child.CardData != null
                && string.Equals(this.MyGameCard.Child.CardData.Id, CorpseId, StringComparison.Ordinal))
            {
                this.MyGameCard.StartTimer(
                    ExtractSeconds,
                    new TimerAction(this.ExtractFromCorpse),
                    SokLoc.Translate("tamemobs_action_decomposing"),
                    base.GetActionId("ExtractFromCorpse"),
                    true, false, false
                );
            }
            else
            {
                this.MyGameCard.CancelTimer(base.GetActionId("ExtractFromCorpse"));
            }

            base.UpdateCard();
        }

        [TimedAction("denny_extract_from_corpse")]
        public void ExtractFromCorpse()
        {
            if (this.MyGameCard == null || !this.MyGameCard.HasChild)
                return;

            GameCard corpseCard = this.MyGameCard.Child;
            if (corpseCard == null || corpseCard.CardData == null)
                return;

            if (!string.Equals(corpseCard.CardData.Id, CorpseId, StringComparison.Ordinal))
                return;

            // Remove the corpse
            base.RemoveFirstChildFromStack();
            corpseCard.DestroyCard(false, true);

            int bones = Roll012();
            int blood = Roll012();

            SpawnOutputs(BoneId, bones);
            SpawnOutputs(BloodId, blood);

            WorldManager.instance.CreateSmoke(base.transform.position);
        }

        private int Roll012()
        {
            // 10%: 0, 80%: 1, 10%: 2
            float r = UnityEngine.Random.value;
            if (r < 0.10f) return 0;
            if (r < 0.90f) return 1;
            return 2;
        }

        private void SpawnOutputs(string cardId, int amount)
        {
            if (amount <= 0)
                return;

            for (int i = 0; i < amount; i++)
            {
                CardData created = WorldManager.instance.CreateCard(
                    base.transform.position,
                    cardId,
                    faceUp: true,
                    checkAddToStack: false, // <- Don't stack blood on top of bone
                    playSound: true
                );

                if (created != null && created.MyGameCard != null)
                {
                    WorldManager.instance.StackSendCheckTarget(
                        this.MyGameCard,
                        created.MyGameCard,
                        this.OutputDir,
                        null,
                        true,
                        -1
                    );
                }
            }
        }
    }
}