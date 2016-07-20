﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace BoardGame
{
	namespace Effect
    {
		public class ManaToken : BaseEffect 
		{
            public override void UseEffect()
            {
                // Gain a mana token

                GetComponent<CleanupMethod>().Success();
            }
        }
	}
}