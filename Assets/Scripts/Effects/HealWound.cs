using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace BoardGame
{
	namespace Effect
    {
		public class HealWound : BaseEffect 
		{
            public override void UseEffect()
            {
                if (Rules.Healing.Instance.totalPaid < intValue) return;

                GetComponent<CleanupMethod>().ThrowAway(GetComponentInParent<Card.Object>());
                Rules.Healing.Instance.AddHealing(-intValue);
            }
        }
	}
}