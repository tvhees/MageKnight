using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace BoardGame
{
	namespace Effect
    {
		public class Movement : BaseEffect 
		{
            public override void UseEffect()
            {
                Debug.Log("Movement called");
                if (Game.Turn.Instance.GetPhase() == Game.Turn.Phase.movement)
                {
                    Rules.Movement.Instance.AddMovement(intValue);
                }
                else return;

                GetComponent<CleanupMethod>().Success();
            }
        }
	}
}