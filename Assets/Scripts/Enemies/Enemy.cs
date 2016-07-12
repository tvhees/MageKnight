using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoardGame
{
	namespace Enemy
    {
		public class Enemy 
		{
            public Enemy()
            {
                attack = new Attack();
                defense = new Defense();
                reward = new Reward();
            }

            public string name;

            public Sprite image;

            public Attack attack;

            public Defense defense;

            public Reward reward;

		}
	}
}