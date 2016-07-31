using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace BoardGame
{
	namespace Effect
    {
		public class GainSpell : BaseEffect 
		{
            public override void UseEffect()
            {
                GetComponent<CleanupMethod>().Success();
            }
        }
	}
}