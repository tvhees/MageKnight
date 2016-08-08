using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
	namespace Enemy
    {
		public class Rampaging : MonoBehaviour 
		{
            private Object thisEnemy;
            void Awake()
            {
                thisEnemy = GetComponent<Object>();
            }
            // Rampaging enemies will halt movement and fight when passed on two adjacent hexes
            public void Provoke()
            {
                Game.GetRules().Provoke(new EffectData());
            }

        }
	}
}