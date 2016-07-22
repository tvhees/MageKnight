using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace BoardGame
{
	namespace Effect
    {
		public class TakeWound : BaseEffect 
		{
            public override void UseEffect()
            {
                GetComponentInParent<Card.Object>().owningPlayer.TakeWound(Card.Object.Location.hand);


                GetComponent<CleanupMethod>().Success();
            }
        }
	}
}