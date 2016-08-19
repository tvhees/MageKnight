using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    public class PathDrawer: MonoBehaviour 
	{
        public ObjectPool pathSegmentPool;
        public GameObject getPathSegment { get { return pathSegmentPool.GetObject(); } }

        private Camera mainCamera;

        public List<GameObject> listOfSegments = new List<GameObject>();

        void Awake()
        {
            mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        }

        public void StartPath()
        {
            listOfSegments = new List<GameObject>();
            listOfSegments.Add(null);
        }

        public void ClearPath()
        {
            //Main.commandStack.AddCommand(new ClearMovementPath());
        }

        public void AddSegmentToPath(Vector3 origin, GameObject destinationTile)
        {
            GameObject segment = getPathSegment;

            Vector3 destination = destinationTile.transform.position;
            int cost = destinationTile.GetComponent<Board.Terrain>().movementCost;

            PositionAndOrientSegment(segment, origin, destination);
            OrientAndSetCostText(segment, cost);

            listOfSegments.Add(segment);
        }

        void PositionAndOrientSegment(GameObject segment, Vector3 origin, Vector3 destination)
        {
            Vector3 midPoint = Vector3.Lerp(origin, destination, 0.5f);

            segment.transform.position = midPoint;
            segment.transform.LookAt(destination);
        }

        void OrientAndSetCostText(GameObject segment, int cost)
        {
            TextMesh costText = segment.GetComponentInChildren<TextMesh>();
            costText.transform.rotation = Quaternion.LookRotation(costText.transform.position - mainCamera.transform.position);
            costText.text = cost.ToString();
        }

        public void RemoveLastSegment()
        {
            if(listOfSegments.Count > 1)
                RemoveSegmentAtIndex(listOfSegments.Count - 1);
        }

        public void RemoveSegmentAtIndex(int index)
        {
            Debug.Log(index);
            if (listOfSegments.Count > index)
            {
                GameObject segment = listOfSegments[index];
                pathSegmentPool.ReturnObject(segment);
                listOfSegments.Remove(segment);
            }
        }
	}
}