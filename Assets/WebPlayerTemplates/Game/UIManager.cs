using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    namespace Board
    {
        public class UIManager : Singleton<UIManager>
        {
            public GameObject arrowPrefab;
            public GameObject costNumberPrefab;
            private List<GameObject> listOfArrows = new List<GameObject>();
            private List<GameObject> listOfMovementCostNumbers = new List<GameObject>();


            // Draw a path of arrows across relevant hexes
            public void DrawUIForMovementPath(List<HexGrid.Manager> listOfTilesToDrawAcross, string movementCostAsString)
            {
                int i = listOfTilesToDrawAcross.Count - 1;

                Vector3 start = listOfTilesToDrawAcross[i - 1].transform.position;
                Vector3 end = listOfTilesToDrawAcross[i].transform.position;
                Vector3 midPoint = (start + end) * 0.5f; // Place arrows and numbers at midpoint between tiles
                DrawArrow(start, end, midPoint);
                DrawCostNumber(midPoint, movementCostAsString);                
            }

            void DrawArrow(Vector3 arrowTail, Vector3 arrowHead, Vector3 arrowMidpoint)
            {
                Quaternion arrowRotation = Quaternion.LookRotation(Vector3.Cross((arrowHead - arrowTail), Vector3.up)); // arrow prefab is already rotated 90 degrees so we need to make it 'look' at the orthogonal direction
                GameObject arrow = Instantiate(arrowPrefab, arrowMidpoint, arrowRotation) as GameObject;
                listOfArrows.Add(arrow);
            }

            void DrawCostNumber(Vector3 numberPosition, string movementCost)
            {
                Quaternion numberRotation = Quaternion.Euler(60f, 30f, 0f); // rotate to face the camera;
                GameObject number = Instantiate(costNumberPrefab, numberPosition, numberRotation) as GameObject;
                number.GetComponent<TextMesh>().text = movementCost;
                listOfMovementCostNumbers.Add(number);
            }

            public void DeleteArrowPath()
            {
                for (int i = 0; i < listOfArrows.Count; i++)
                {
                    Destroy(listOfArrows[i]);
                    Destroy(listOfMovementCostNumbers[i]);
                }

                listOfArrows.Clear();
                listOfMovementCostNumbers.Clear();
            }

            public void ColourMovementPath(int progressInMovementPath, int totalNumberOfCostsPaid)
            {
                for (int i = progressInMovementPath; i < listOfMovementCostNumbers.Count; i++)
                {
                    if (i < totalNumberOfCostsPaid)
                    {
                        ColourPathCost(i, Color.red);
                    }
                    else
                    {
                        ColourPathCost(i, Color.white);
                    }
                }
            }

            void ColourPathCost(int i, Color colour)
            {
                if (i < listOfMovementCostNumbers.Count)
                    listOfMovementCostNumbers[i].GetComponent<TextMesh>().color = colour;
            }

            public void DeleteLastPathArrow()
            {
                // Destroy associated arrow
                GameObject arrow = listOfArrows.GetLast();
                Destroy(arrow);
                listOfArrows.RemoveLast();

                // Destroy associated number
                GameObject number = listOfMovementCostNumbers.GetLast();
                Destroy(number);
                listOfMovementCostNumbers.RemoveLast();
            }
        }
    }
}