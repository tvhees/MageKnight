using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoardGame
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
                if (Game.Turn.Instance.InMovementPhase())
                {
                    float squareDistance = (transform.position - Game.Manager.GetCurrentPlayer().transform.position).sqrMagnitude;

                    if (Mathf.Sqrt(squareDistance) < Game.Manager.unitOfDistance)
                    {
                        Rules.Combat.Instance.AddOrRemoveEnemy(thisEnemy);
                        StartCoroutine(Rules.Combat.Instance.StartCombat()); // Send this enemy to combat
                    }
                }
            }

        }
	}
}