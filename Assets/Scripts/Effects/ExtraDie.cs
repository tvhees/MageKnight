﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace BoardGame
{
	namespace Effect
    {
		public class ExtraDie : BaseEffect 
		{
            public override void UseEffect()
            {
                // Allow an extra mana die to use this turn

                GetComponent<CleanupMethod>().Success();
            }
        }
	}
}