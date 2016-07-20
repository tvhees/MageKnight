using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace BoardGame
{
	namespace Effect
    {
		public class ManaCrystal : BaseEffect 
		{
            public override void UseEffect()
            {
                // Gain a mana crystal

                GetComponent<CleanupMethod>().Success();
            }
        }
	}
}