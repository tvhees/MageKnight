using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    public class PathDrawer: MonoBehaviour 
	{
        public GameObject pathSegmentPrefab;

        private Camera mainCamera;

        private List<GameObject> listOfSegments = new List<GameObject>();

        void Awake()
        {
            mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        }

        public void StartPath()
        {
            ClearPath();
            listOfSegments.Add(null);
        }

        public void ClearPath()
        {
            listOfSegments.Clear();
        }

        public void AddSegmentToPath(Vector3 origin, Vector3 destination)
        {
            GameObject segment = transform.InstantiateChild(pathSegmentPrefab);

            PositionAndOrientSegment(segment, origin, destination);
            OrientCostText(segment);

            listOfSegments.Add(segment);
        }

        void PositionAndOrientSegment(GameObject segment, Vector3 origin, Vector3 destination)
        {
            Vector3 midPoint = Vector3.Lerp(origin, destination, 0.5f);

            segment.transform.position = midPoint;
            segment.transform.LookAt(destination);
        }

        void OrientCostText(GameObject segment)
        {
            TextMesh cost = segment.GetComponentInChildren<TextMesh>();
            cost.transform.LookAt(mainCamera.transform);
        }

        public void RemoveLastSegment()
        {
            if (listOfSegments.Count > 0)
            {
                Destroy(listOfSegments.GetLast());
                listOfSegments.RemoveLast();
            }
        }
	}
}