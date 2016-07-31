using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace BoardGame
{
	namespace Effect
    {
		public class Reputation : BaseEffect 
		{
            public override void UseEffect()
            {
                GetComponentInParent<Card.Object>().owningPlayer.AddReputation(intValue);

                GetComponent<CleanupMethod>().Success();
            }
        }
	}
}