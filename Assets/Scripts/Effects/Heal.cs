using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace BoardGame
{
	namespace Effect
    {
		public class Heal : BaseEffect 
		{
            public override void UseEffect()
            {
                if (Game.Turn.Instance.GetPhase() != Game.Turn.Phase.combat)
                {
                    Rules.Healing.Instance.AddHealing(intValue);
                }
                else
                    return;

                GetComponent<CleanupMethod>().Success();
            }
        }
	}
}