using System;
using UnityEngine;

namespace Denny_TameMobsNS
{
    // A butchery-equivalent for tamed mobs
    public class Denny_TamedSlaughterHouse : CardData
    {

        private const int MaxCreatureCount = 5;

        public override bool DetermineCanHaveCardsWhenIsRoot => true;

        public override bool CanHaveCardsWhileHasStatus()
        {
            return true;
        }

        private static readonly float timeToButcher = 45f;

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

        protected override bool CanHaveCard(CardData otherCard)
        {
            if (otherCard == null) return false;

            // Only allow tamed mobs
            if (otherCard is not Mob mob) return false;
            if (!TameUtil.IsTamed(mob)) return false;

            int total = base.GetChildCount() + (1 + otherCard.GetChildCount());
            return total <= MaxCreatureCount;
        }

        public override void UpdateCard()
        {
            if (MyGameCard.HasChild && MyGameCard.Child?.CardData is Mob mob && TameUtil.IsTamed(mob))
            {
                MyGameCard.StartTimer(
                    timeToButcher,
                    new TimerAction(this.SlaughterTamedMob),
                    SokLoc.Translate("tamemobs_action_slaughtering"),
                    base.GetActionId("SlaughterTamedMob"),
                    true, false, false
                );
            }
            else
            {
                MyGameCard.CancelTimer(base.GetActionId("SlaughterTamedMob"));
            }

            base.UpdateCard();
        }

        [TimedAction("denny_slaughter_tamed_mob")]
        public void SlaughterTamedMob()
        {
            if (!MyGameCard.HasChild) return;

            GameCard child = MyGameCard.Child;
            if (child?.CardData is not Mob mob) return;
            if (!TameUtil.IsTamed(mob)) return;

            base.RemoveFirstChildFromStack();

            // Let vanilla death pipeline handle drops, smoke, effects, etc.
            mob.Die();

            // more juice
            WorldManager.instance.CreateSmoke(transform.position);
        }
    }
}