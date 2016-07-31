using UnityEngine;
using UnityEngine.EventSystems;
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
                float squareDistance = (transform.position - Game.Game.GetCurrentPlayer().transform.position).sqrMagnitude;
                if (Mathf.Sqrt(squareDistance) < Game.Game.unitOfDistance)
                {
                    thisEnemy.EnterCombat();
                }
            }

        }
	}
}