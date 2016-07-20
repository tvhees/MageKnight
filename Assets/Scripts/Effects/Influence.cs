using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace BoardGame
{
	namespace Effect
    {
		public class Influence : BaseEffect 
		{
            public override void UseEffect()
            {
                if (Game.Turn.Instance.GetPhase() == Game.Turn.Phase.influence)
                {
                    Rules.Influence.Instance.AddInfluence(intValue);                        
                    GetComponent<CleanupMethod>().Success();
                }
            }
        }
	}
}