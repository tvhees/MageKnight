using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoardGame
{
	namespace Building
    {
		public class Garrison : MonoBehaviour 
		{
            public Enemy.Factory.EnemyType enemyType;
            private Enemy.Object garrisonEnemy;

            void Awake()
            {
                garrisonEnemy = Enemy.Manager.Instance.GetEnemy(enemyType.ToString());
                garrisonEnemy.transform.SetParent(transform);
                garrisonEnemy.transform.localPosition = Vector3.zero;
            }

		}
	}
}