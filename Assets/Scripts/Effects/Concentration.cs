using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace BoardGame
{
	namespace Effect
    {
		public class Concentration : BaseEffect 
		{
            public override void UseEffect()
            {
                // Get mana tokens or power a card

                GetComponent<CleanupMethod>().Success();
            }
        }
	}
}