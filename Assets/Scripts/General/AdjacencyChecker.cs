using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoardGame
{
    public static class AdjacencyChecker
    {
        public static List<Enemy.Object> OverlapSphereForEnemies(Vector3 centerOfSphere)
        {
            LayerMask enemyLayer = 1 << LayerMask.NameToLayer("Enemies");

            Collider[] enemyColliders = Physics.OverlapSphere(centerOfSphere, 1.5f * Game.Manager.unitOfDistance, enemyLayer);

            List<Enemy.Object> rampagingEnemies = new List<Enemy.Object>();
            for (int i = 0; i < enemyColliders.Length; i++)
            {
                if (enemyColliders[i].GetComponent<Enemy.Rampaging>())
                    rampagingEnemies.Add(enemyColliders[i].GetComponent<Enemy.Object>());
            }

            return rampagingEnemies;
        }

        public static bool ByDistance(Vector3 positionA, Vector3 positionB)
        {
            float sqrDistance = (positionA - positionB).sqrMagnitude;
            return Mathf.Sqrt(sqrDistance) < Game.Manager.unitOfDistance;
        }
    }
}