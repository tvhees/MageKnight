using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace BoardGame
{
	namespace Effect
    {
		public class Improvisation : BaseEffect 
		{
            public override void UseEffect()
            {
                // Discard a card


                if (Game.Turn.Instance.GetPhase() == Game.Turn.Phase.movement) Rules.Movement.Instance.AddMovement(intValue);
                else if (Rules.Combat.Instance.phase == Rules.Combat.Phase.attack || Rules.Combat.Instance.phase == Rules.Combat.Phase.block)
                {
                    bool addedSuccessfully = Rules.Combat.Instance.CanAddAttackOrBlock(intValue);
                    if (!addedSuccessfully) return;
                }
                else if (Game.Turn.Instance.GetPhase() == Game.Turn.Phase.influence) Rules.Influence.Instance.AddInfluence(intValue);
                else return;

                GetComponent<CleanupMethod>().Success();
            }
        }
	}
}