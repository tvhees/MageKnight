using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
	namespace Effect
    {
		public class Block : BaseEffect 
		{
            public override void UseEffect(Rulesets.Ruleset rules)
            {
                if (Rules.Combat.Instance.phase == Rules.Combat.Phase.block)
                {
                    bool addedSuccessfully = Rules.Combat.Instance.CanAddAttackOrBlock(intValue);
                    if (!addedSuccessfully) return;
                }
                else
                    return;

                GetComponent<CleanupMethod>().Success();
            }
        }
	}
}