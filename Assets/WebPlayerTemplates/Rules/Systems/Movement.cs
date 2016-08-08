using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    namespace Rules
    {
        public class Movement : Singleton<Movement>
        {
            // Tracking variables
            public bool haveDefeatedEnemies;
            private int progressInMovementPath;
            private int totalPaid;
            private List<HexGrid.Manager> listOfTilesToMoveTo = new List<HexGrid.Manager>(); // List for reference to hextile component
            private List<int> listOfCostsToPay = new List<int>(); // Keep track of running total movecost for payment UI

            public void AddTileToPath(HexGrid.Manager tile)
            {
                if (!tile.isSelected && CanAddToPath(tile)) // If the tile is not already selected, select it and add it to our movement path
                {
                    tile.Select();
                    listOfTilesToMoveTo.Add(tile);

                    int movementCost = tile.GetTerrain().cost;
                    listOfCostsToPay.Add(movementCost);

                    Board.UIManager.Instance.DrawUIForMovementPath(listOfTilesToMoveTo, movementCost.ToString());

                    StartCoroutine(MovePlayer(TotalMovesPaidFor()));

                }
                // If we're removing a hex we need to work backwards down the list and remove any later movement nodes
                else
                {
                        int i = listOfTilesToMoveTo.IndexOf(tile);

                        while (listOfTilesToMoveTo.Count > i)
                        {
                            RemoveLastTileFromPath();
                        }

                        tile.Deselect();
                }
            }

            bool CanAddToPath(HexGrid.Manager tile)
            {
                if (listOfTilesToMoveTo.Count < 1)
                {
                    listOfTilesToMoveTo.Add(Game.GetCurrentPlayer().currentTile); // If we don't have a path yet, start it at the player's current tile
                    listOfCostsToPay.Add(0); // We need a dummy cost to keep indices consistent with the list of tiles
                }

                // Store positions of previous and current movement nodes
                Vector3 positionOfPathEnd = listOfTilesToMoveTo.GetLast().transform.position; // Check distance from the end of the current path
                Vector3 positionOfNewTile = tile.transform.position; // Draw to the new tile

                float distanceOfMovement = Mathf.Abs((positionOfNewTile - positionOfPathEnd).magnitude);
                if (distanceOfMovement < Game.unitOfDistance) // We can usually only move one tile away
                {
                    return true; // This tile can be added to the path
                }
                else
                    return false; // Too far away from the end of the current path
            }

            void RemoveLastTileFromPath()
            {
                // Remove hex and cost attributes
                HexGrid.Manager lastHex = listOfTilesToMoveTo.GetLast();
                lastHex.GetComponent<HexGrid.Manager>().Deselect();
                listOfTilesToMoveTo.RemoveLast();
                listOfCostsToPay.RemoveLast();

                Board.UIManager.Instance.DeleteLastPathArrow();
            }

            // Add movement points to a player's total
            public void AddMovement(int value)
            {
                totalPaid += value;

                StartCoroutine(MovePlayer(TotalMovesPaidFor())); // Move as many tiles as the player has now paid for.
            }

            int TotalMovesPaidFor()
            {
                int n = AdditionalMovesPaidFor() + progressInMovementPath;
                Board.UIManager.Instance.ColourMovementPath(progressInMovementPath, n); // Colour the moves that have been paid for
                return n;
            }


            // Calculate how many of the queued movements they player has payed enough movement points for
            int AdditionalMovesPaidFor()
            {
                int movesPaidFor = 0;
                for (int i = progressInMovementPath + 1; i < listOfCostsToPay.Count; i++)
                {
                    int tileCost = listOfCostsToPay[i];
                    if (totalPaid >= tileCost)
                    {
                        totalPaid -= tileCost;
                        movesPaidFor++;
                    }
                }

                return movesPaidFor;
            }

            IEnumerator MovePlayer(int numberOfTilesToMove)
            {
                if (numberOfTilesToMove == 0) // If we're not moving anywhere we can safely skip this method
                    yield break;

                // Get the player's moving object component because we're going to be moving
                MovingObject player = Game.GetCurrentPlayer().GetComponent<MovingObject>();

                // Create a list to hold any enemies we have to fight
                List<Enemy.Object> enemiesToFight = new List<Enemy.Object>();

                bool combatOccurred = false;

                for (int i = progressInMovementPath; i < numberOfTilesToMove; i++)
                {
                    // The player is currently at tile i
                    int nextTileIndex = i + 1; // We want to look at the cost, position etc of the tile we're moving to
                    HexGrid.Manager nextTile = listOfTilesToMoveTo[nextTileIndex];

                    // Look for rampaging enemies adjacent to our destination - the player always has the option to fight these
                    List<Enemy.Object> enemiesAdjacentToNextMove = AdjacencyChecker.OverlapSphereForEnemies(player.transform.position);
                    foreach (Enemy.Object enemy in enemiesAdjacentToNextMove)
                    {
                        // If the enemy is adjacent NOW as well as where we're moving, then we must be moving past them - the player is forced to fight these
                        if (AdjacencyChecker.ByDistance(player.transform.position, enemy.transform.position))
                            enemiesToFight.Add(enemy);
                    }

                    // Check if there's enemies ON our destination
                    Enemy.Object[] enemiesAtDestination = nextTile.GetComponentsInChildren<Enemy.Object>();

                    if (enemiesAtDestination.Length > 0)
                    {
                        // We are assaulting a building and must successfully fight BEFORE we can complete the move
                        enemiesToFight.AddRange(enemiesAtDestination);

                        yield return StartCombatPhase(enemiesToFight);
                        combatOccurred = true;

                        if (nextTile.GetComponentInChildren<Enemy.Object>() != null) // If there are still enemies on our destination we can't move there
                        {
                            EndMovementPhase();
                            break;
                        }
                    }

                    // The tile ahead is empty, we move on to it.
                    yield return StartCoroutine(player.SetTargetPos(nextTile.transform.position, true));
                    progressInMovementPath = nextTileIndex;

                    // Store the current tile the player is occupying
                    player.GetComponent<Players.Player>().currentTile = nextTile;

                    if (enemiesToFight.Count > 0 && !combatOccurred) // We moved past rampaging enemies but haven't fought anything yet
                    {
                        yield return StartCombatPhase(enemiesToFight);
                        combatOccurred = true;
                        break; // Combat prevents further movement
                    }

                    // If we fought nothing, clear the list of potential enemies and continue to next movement
                    enemiesToFight.Clear();
                }

                if (combatOccurred)
                    EndMovementPhase();
            }

            void GetRampagingEnemiesAdjacentToDestination()
            {

            }

            IEnumerator StartCombatPhase(List<Enemy.Object> enemies)
            {
                foreach (Enemy.Object enemy in enemies)
                    Combat.Instance.AddOrRemoveEnemy(enemy);

                
            }

            public void EndMovementPhase()
            {
                totalPaid = 0;
                progressInMovementPath = 0;

                foreach (HexGrid.Manager tile in listOfTilesToMoveTo)
                {
                    tile.Deselect();
                }

                listOfTilesToMoveTo.Clear();
                listOfCostsToPay.Clear();

                Board.UIManager.Instance.DeleteArrowPath();
            }
        }
    }
}