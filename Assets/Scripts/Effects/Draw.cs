using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace BoardGame
{
	namespace Effect
    {
		public class Draw : BaseEffect 
		{
            public override void UseEffect()
            {
                GetComponent<Button>().player.DrawCards(intValue);
                GetComponent<CleanupMethod>().Success();
            }
        }
	}
}