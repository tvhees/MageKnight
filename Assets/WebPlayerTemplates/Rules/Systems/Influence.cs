using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
	namespace Rules
    {
		public class Influence : Singleton<Influence> 
		{
            private int totalPaid;

            // Add movement points to a player's total
            public void AddInfluence(int value)
            {
                totalPaid += value;
            }

        }
	}
}