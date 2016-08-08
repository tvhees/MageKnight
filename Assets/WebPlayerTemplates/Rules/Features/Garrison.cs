using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
	namespace Building
    {
		public class Garrison : MonoBehaviour 
		{
            public Enemy.Factory.EnemyType enemyType;
            private Enemy.Object garrisonEnemy;

            public void Init()
            {
                // Get an enemy token to live in this garrison.
                // Need to do this AFTER instantiation of the building is finished
                // so that the correct position reference is passed to the enemy
                garrisonEnemy = Enemy.Manager.Instance.GetEnemy(enemyType.ToString(), transform);
            }

		}
	}
}