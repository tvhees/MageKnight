using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoardGame
{
	namespace Enemy
    {
		public class Reward 
		{
            public Reward()
            {
                fame = 0;
                reputation = 0;
            }

            public int fame;
            public int reputation;

            public static Reward operator +(Reward r1, Reward r2)
            {
                Reward output = new Reward();
                output.fame = r1.fame + r2.fame;
                output.reputation = r1.reputation + r2.reputation;
                return output;
            }
		}
	}
}