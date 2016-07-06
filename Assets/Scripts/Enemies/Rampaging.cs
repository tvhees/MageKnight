using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoardGame
{
	namespace Enemy
    {
		public class Rampaging : MonoBehaviour 
		{
            // Rampaging enemies will be fought immediately when clicked on instead of moving on to the hex first
            void OnMouseUpAsButton()
            {
                Provoke();
            }

            // Rampaging enemies will halt movement and fight when passed on two adjacent hexes
            public void Provoke()
            {
                // Send this enemy to combat
            }

        }
	}
}