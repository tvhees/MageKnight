using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace BoardGame
{
	namespace Effect
    {
		public class Threaten : BaseEffect 
		{
            public override void UseEffect()
            {
                if (Game.Turn.Instance.GetPhase() == Game.Turn.Phase.influence)
                {
                    GetComponent<Button>().player.AddReputation(-1);
                    Rules.Influence.Instance.AddInfluence(intValue);                        
                }
                else return;

                GetComponent<CleanupMethod>().Success();
            }
        }
	}
}