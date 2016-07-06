using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoardGame
{
	namespace Building
    {
		public class Spawner : MonoBehaviour 
		{
            public Enemy.Factory.EnemyType enemyType;

            public Enemy.Object Spawn()
            {
                return Enemy.Manager.Instance.GetEnemy(enemyType.ToString());
            }

		}
	}
}