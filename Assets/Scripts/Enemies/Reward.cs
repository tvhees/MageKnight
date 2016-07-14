using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoardGame
{
	namespace Enemy
    {
		public class Reward 
		{
            public int fame;
            public int reputation;

            // Base constructor
            private Reward(int fame, int reputation)
            {
                this.fame = 0;
                this.reputation = 0;
            }

            // Create reward with 0 fame or reputation
            public static Reward NullReward()
            {
                return new Reward(0, 0);
            }

            // Create reward with specified fame and reputation
            public static Reward RewardFromInt(int fame, int reputation)
            {
                return new Reward(fame, reputation);
            }

            // Add two Reward classes together
            public static Reward operator +(Reward inputRewardOne, Reward inputRewardTwo)
            {
                Reward output = NullReward();
                output.fame = inputRewardOne.fame + inputRewardTwo.fame;
                output.reputation = inputRewardOne.reputation + inputRewardTwo.reputation;
                return output;
            }
		}
	}
}