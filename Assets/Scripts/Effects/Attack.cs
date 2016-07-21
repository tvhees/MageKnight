﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace BoardGame
{
	namespace Effect
    {
		public class Attack : BaseEffect 
		{
            public override void UseEffect()
            {
                if (Rules.Combat.Instance.phase == Rules.Combat.Phase.attack)
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