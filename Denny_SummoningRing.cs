using System;
using UnityEngine;

namespace Denny_TameMobsNS
{
    public class Denny_SummoningRing : CardData
    {
        public override bool DetermineCanHaveCardsWhenIsRoot => true;

        protected override bool CanHaveCard(CardData otherCard)
        {
            if (otherCard == null)
                return false;

            if (!Denny_TameMobs.TamableMobsIDs.Contains(otherCard.Id))
                return false;

            int childCount = this.MyGameCard.GetChildCount();

            // Slot 1: any tamable mob
            if (childCount == 0)
                return true;

            // Slot 2: must match first mob id
            if (childCount == 1)
            {
                GameCard first = this.MyGameCard.Child;
                return first != null
                    && first.CardData != null
                    && first.CardData.Id == otherCard.Id;
            }

            return false;
        }

        public override void UpdateCard()
        {
            if (this.MyGameCard.GetChildCount() == 2)
            {
                this.MyGameCard.StartTimer(
                    120f,
                    new TimerAction(this.SummonCreature),
                    SokLoc.Translate("tamemobs_action_summoning"),
                    base.GetActionId("SummonCreature"),
                    true, false, false
                );
            }
            else if (this.MyGameCard.GetChildCount() > 2)
            {
                // Never allow more than 2 children
                GameCard extra = this.MyGameCard.TryGetNthChild(3);
                if (extra != null)
                    extra.RemoveFromParent();

                this.MyGameCard.CancelTimer(base.GetActionId("SummonCreature"));
            }
            else
            {
                this.MyGameCard.CancelTimer(base.GetActionId("SummonCreature"));
            }

            base.UpdateCard();
        }

        [TimedAction("denny_summon_creature")]
        public void SummonCreature()
        {
            GameCard parentA = this.MyGameCard.Child;
            if (parentA == null || parentA.CardData == null)
                return;

            GameCard parentB = parentA.Child;
            if (parentB == null || parentB.CardData == null)
                return;

           
            if (parentA.CardData.Id != parentB.CardData.Id)
                return;

            string summonId = parentA.CardData.Id;

            // Spawn the new creature
            CardData spawned = WorldManager.instance.CreateCard(
                base.transform.position,
                summonId,
                true,  // faceUp
                true,  // playSound
                true   // idk lol
            );

            // Make it tamed
            if (spawned != null)
            {
                TameUtil.SetTamedTag(spawned, true);

                if (spawned is Enemy enemy)
                {
                    TameUtil.TameMob(enemy);
                    TameUtil.UpdateCardVisuals(enemy);
                }

                // Send ONLY the child out, keep parents inside
                if (spawned.MyGameCard != null)
                    WorldManager.instance.StackSendCheckTarget(this.MyGameCard, spawned.MyGameCard, this.OutputDir, null, true, -1);
            }
        }
    }
}