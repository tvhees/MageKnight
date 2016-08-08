using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
	namespace Effect
    {
		public class Ranged : BaseEffect 
		{
            public override void UseEffect(Rulesets.Ruleset rules)
            {
                if (Rules.Combat.Instance.phase == Rules.Combat.Phase.attack || Rules.Combat.Instance.phase == Rules.Combat.Phase.ranged)
                {
                    bool addedSuccessfully = Rules.Combat.Instance.CanAddAttackOrBlock(intValue);

                    Debug.Log(addedSuccessfully);

                    if (!addedSuccessfully) return;
                }
                else
                    return;

                GetComponent<CleanupMethod>().Success();
            }
        }
	}
}