using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
	namespace Enemy
    {
		public class EnemyDeprecated 
		{
            public string name;
            public Sprite image;
            public Sprite backImage;

            public Attack attack;
            public Defense defense;
            public Reward reward;

            public EnemyDeprecated()
            {
                attack = new Attack();
                defense = new Defense();
                reward = Reward.NullReward();
            }
		}
	}
}