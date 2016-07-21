using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace BoardGame
{
	namespace Effect
    {
		public class ManaDraw : BaseEffect 
		{
            public override void UseEffect()
            {
                // Set die from source to any colour, get two tokens, don't reroll at end of turn

                GetComponent<CleanupMethod>().Success();
            }
        }
	}
}