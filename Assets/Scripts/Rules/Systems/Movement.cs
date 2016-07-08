using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace BoardGame
{
    namespace Rules
    {
        public class Movement : Singleton<Movement>
        {

            // Game Rules
            private float maxDistance = 4f;

            // Tracking variables
            private int m_totalCost;
            private int m_totalPaid;
            private List<HexGrid.Manager> m_hexPath = new List<HexGrid.Manager>(); // List for reference to hextile component
            private List<int> m_pathCosts = new List<int>(); // Keep track of running total movecost for payment UI

            public bool ChangeCost(bool removeHex, HexGrid.Manager hex)
            {
                // If we're adding new hexes to movement path they go at the end of the list
                if (!removeHex)
                {
                    return AddNewNode(hex); // Tell the tile that it is selected iff we manage to add it as a movement node 
                }
                // If we're removing a hex we need to work backwards down the list and remove any later movement nodes
                else
                {
                    int i = m_hexPath.IndexOf(hex);

                    while (m_hexPath.Count > i)
                    {
                        DeleteLastNode();
                    }

                    return false; // Confirm that the tile is deselected
                }
            }

            bool AddNewNode(HexGrid.Manager hex)
            {
                // Store positions of previous and current movement nodes
                Vector3 posA;
                if (m_hexPath.Count < 1)
                    posA = Game.Manager.Instance.GetCurrentPlayer().transform.position; // Player position is always the first node
                else
                    posA = m_hexPath.GetLast().transform.position; // Draw from the last tile added to movement path
                Vector3 posB = hex.transform.position; // Draw to the new tile

                Vector3 direction = posB - posA;
                if (direction.sqrMagnitude < maxDistance) // We can usually only move one tile away
                {
                    // Add new hex to the end of the list and show the total movement cost above the new arrow
                    m_hexPath.Add(hex);
                    m_totalCost += hex.GetTerrain().GetCost();
                    m_pathCosts.Add(m_totalCost);

                    Board.UIManager.Instance.DrawPath(posA, posB, m_totalCost.ToString());
                    ColourPath();

                    return true;
                }
                else
                    return false;
            }

            void DeleteLastNode()
            {
                // Remove hex and cost attributes
                HexGrid.Manager lastHex = m_hexPath.GetLast();
                lastHex.GetComponent<HexGrid.Manager>().Deselect();
                m_hexPath.RemoveLast();
                m_totalCost -= lastHex.GetTerrain().GetCost();
                m_pathCosts.RemoveLast();

                Board.UIManager.Instance.DeleteLast();
            }

            public void AddMovement(int value)
            {
                m_totalPaid += value;

                ColourPath();
            }

            void ColourPath()
            {
                int nodesPaid = 0;
                for (int i = 0; i < m_pathCosts.Count; i++)
                {
                    if (m_pathCosts[i] <= m_totalPaid)
                    {
                        Board.UIManager.Instance.ColourNode(i, Color.red);
                        nodesPaid++;
                    }
                    else
                    {
                        Board.UIManager.Instance.ColourNode(i, Color.white);
                    }
                }

                if(nodesPaid > 0)
                    StartCoroutine(MovePlayer(nodesPaid));
            }

            IEnumerator MovePlayer(int n)
            {
                MovingObject player = Game.Manager.Instance.GetCurrentPlayer().GetComponent<MovingObject>();
                for (int i = 0; i < n; i++)
                {
                    yield return StartCoroutine(player.SetTargetPos(m_hexPath[i].transform.position, true));
                }


            }
        }
    }
}