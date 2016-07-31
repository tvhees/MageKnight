using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
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

                images = new List<Sprite>();
            }
            public int strength;

            public List<Sprite> images;

            // Resistances
            public bool cold;
            public bool fire;
            public bool physical;

            // Other
            public bool fortified;

            // Define addition operation
            public static Defense operator +(Defense d1, Defense d2)
            {
                Defense output = new Defense();
                output.strength = d1.strength + d2.strength;

                output.cold = d1.cold || d2.cold;
                output.fire = d1.fire || d2.fire;
                output.physical = d1.physical || d2.physical;
                output.fortified = d1.fortified || d2.fortified;

                return output;
            }
		}
	}
}