using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoardGame
{
	namespace Players
    {
		public class Strength 
		{
            public Strength()
            {
                physical = 0;
                cold = 0;
                fire = 0;
                coldfire = 0;
            }

            public int AddStrength(int value, string type)
            {
                switch (type)
                {
                    case "physical":
                        physical += value;
                        return physical;
                    case "cold":
                        cold += value;
                        return cold;
                    case "fire":
                        fire += value;
                        return fire;
                    case "coldfire":
                        coldfire += value;
                        return coldfire;
                }

                return 0;
            }

            public int Total()
            {
                return physical + cold + fire + coldfire;
            }

            public int physical;

            // Elements
            public int cold;
            public int fire;
            public int coldfire;
        }

    }
}