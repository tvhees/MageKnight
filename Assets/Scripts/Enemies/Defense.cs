using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoardGame
{
	namespace Enemy
    {
		public class Defense 
		{
            public Defense()
            {
                strength = 0;
                cold = false;
                fire = false;
                physical = false;
                fortified = false;
            }
            public int strength;

            // Resistances
            public bool cold;
            public bool fire;
            public bool physical;

            // Other
            public bool fortified;
		}
	}
}