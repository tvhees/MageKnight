using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoardGame
{
	namespace Enemy
    {
		public class Attack 
		{
            public Attack()
            {
                strength = 0;
                cold = false;
                fire = false;
                swiftness = false;
                brutal = false;
                poison = false;
                paralyze = false;
                summoner = false;
                images = new List<Sprite>();
            }

            public List<Sprite> images;

            public int strength;
            
            // Elements
            public bool cold;
            public bool fire;
            
            // Multipliers
            public bool swiftness;
            public bool brutal;
            public bool poison;

            // Other
            public bool paralyze;
            public bool summoner;
		}
	}
}