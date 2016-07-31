using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    public static class AdjacencyChecker
    {
        public static List<Enemy.Object> OverlapSphereForEnemies(Vector3 centerOfSphere, float radiusOfSphere = 1.5f)
        {
            List<Enemy.Object> enemies = OverlapSphereForType<Enemy.Object>(centerOfSphere, radiusOfSphere, "Enemies");
            List<Enemy.Object> rampagingEnemies = new List<Enemy.Object>();
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].GetComponent<Enemy.Rampaging>() != null)
                    rampagingEnemies.Add(enemies[i]);
            }

            return rampagingEnemies;
        }

        public static List<T> OverlapSphereForType<T>(Vector3 centerOfSphere, float radiusOfSphere, string layerMaskName)
        {
            LayerMask layer = 1 << LayerMask.NameToLayer(layerMaskName);

            Collider[] colliders = Physics.OverlapSphere(centerOfSphere, radiusOfSphere * Game.Game.unitOfDistance, layer);

            List<T> listOfType = new List<T>();
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].GetComponent<T>() != null)
                    listOfType.Add(colliders[i].GetComponent<T>());
            }

            return listOfType;
        }

        public static bool ByDistance(Vector3 positionA, Vector3 positionB)
        {
            float sqrDistance = (positionA - positionB).sqrMagnitude;
            return Mathf.Sqrt(sqrDistance) < Game.Game.unitOfDistance;
        }
    }
}