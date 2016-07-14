using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoardGame
{
	namespace Enemy
    {
		public class Rampaging : MonoBehaviour 
		{
            public float AttackDistance = 4f;
            private Object thisEnemy;
            void Awake()
            {
                thisEnemy = GetComponent<Object>();
            }
            // Rampaging enemies will halt movement and fight when passed on two adjacent hexes
            public void Provoke()
            {
                float squareDistance = (transform.position - Game.Manager.Instance.GetCurrentPlayer().transform.position).sqrMagnitude;

                if (squareDistance < AttackDistance)
                {
                    Rules.Combat.Instance.AddOrRemoveEnemy(thisEnemy);
                    StartCoroutine(Rules.Combat.Instance.StartCombat());// Send this enemy to combat
                }
            }

        }
	}
}