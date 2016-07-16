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
            public bool haveDefeatedEnemies;
            private int m_playerPosition;
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
                    if (Game.Turn.Instance.InMovementPhase())
                    {
                        int i = m_hexPath.IndexOf(hex);

                        while (m_hexPath.Count > i)
                        {
                            DeleteLastNode();
                        }

                        if (i == 0)
                            Game.Turn.Instance.MoveBackward();

                        return false; // Confirm that the tile is deselected
                    }
                    else
                        return true; // We weren't in the movement phase, this tile could not be deselected
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
                if (Mathf.Sqrt(direction.sqrMagnitude) < Game.Manager.unitOfDistance) // We can usually only move one tile away
                {
                    if (Game.Turn.Instance.InMovementPhase()) // Check that we are in the movement phase
                    {    // Add new hex to the end of the list and show the total movement cost above the new arrow
                        m_hexPath.Add(hex);
                        m_totalCost += hex.GetTerrain().GetCost();
                        m_pathCosts.Add(m_totalCost);

                        Board.UIManager.Instance.DrawPath(posA, posB, m_totalCost.ToString());
                        ColourPath();

                        return true;
                    }
                    else
                        return false; // Not in the movement phase!
                }
                else
                    return false; // Too far away!
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
                haveDefeatedEnemies = false; // We haven't fought anything yet

                // Create a list to hold any enemies we have to fight
                List<Enemy.Object> enemiesToFight = new List<Enemy.Object>();

                // Get the player's moving object component
                MovingObject player = Game.Manager.Instance.GetCurrentPlayer().GetComponent<MovingObject>();

                for (int i = m_playerPosition; i < n; i++)
                {
                    // Check if we're moving past any rampaging enemies and add them to enemies list
                    List<Enemy.Object> enemiesAdjacentToNextMove = AdjacencyChecker.OverlapSphereForEnemies(player.transform.position);
                    foreach (Enemy.Object enemy in enemiesAdjacentToNextMove)
                    {
                        // If the enemy is adjacent NOW as well as where we're moving, then we must be moving past them
                        if (AdjacencyChecker.ByDistance(player.transform.position, enemy.transform.position))
                            enemiesToFight.Add(enemy);
                    }

                    // Check if there's enemies where we want to move
                    Enemy.Object[] enemiesAtDestination = m_hexPath[i].GetComponentsInChildren<Enemy.Object>();

                    if (enemiesAtDestination.Length > 0)
                    {
                        // We must fight before moving
                        enemiesToFight.AddRange(enemiesAtDestination);

                        yield return StartCombatPhase(enemiesToFight);

                        if (!haveDefeatedEnemies)
                            yield break; // Stop all movement if we haven't killed all the enemies.
                    }

                    // If we have killed required enemies OR not fought any, continue to next hex
                    yield return StartCoroutine(player.SetTargetPos(m_hexPath[i].transform.position, true));
                    m_playerPosition = i;

                    if (enemiesToFight.Count > 0 && !haveDefeatedEnemies) // We moved past rampaging enemies but haven't fought anything yet
                    {
                        yield return StartCombatPhase(enemiesToFight);

                        yield break; // Rampaging enemies always prevent further movement
                    }

                    // If we fought nothing, continue to next movement

                    // We need to delete the node here!
                }
            }

            IEnumerator StartCombatPhase(List<Enemy.Object> enemies)
            {
                foreach (Enemy.Object enemy in enemies)
                    Combat.Instance.AddOrRemoveEnemy(enemy);

                yield return StartCoroutine(Combat.Instance.StartCombat());
            }
        }
    }
}