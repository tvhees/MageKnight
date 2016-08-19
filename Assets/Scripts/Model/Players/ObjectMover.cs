using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    public class ObjectMover: MonoBehaviour 
	{
        public float translationSpeed;

        private List<Vector3> destinations = new List<Vector3>();
        public bool moving { get; private set; }

        void Update()
        {
            if (destinations.Count == 0)
            {
                moving = false;
                return;
            }
            else
                moving = true;

            transform.position = Vector3.MoveTowards(transform.position, destinations[0], translationSpeed * Time.deltaTime);

            float sqrDistance = (transform.position - destinations[0]).sqrMagnitude;
            if (sqrDistance < Mathf.Epsilon)
            {
                destinations.RemoveAt(0);
            }
        }

        public void AddDestination(Vector3 destination)
        {
            destinations.Add(destination);
        }
	}
}