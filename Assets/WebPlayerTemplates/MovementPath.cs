using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Player
{
    public class MovementPath 
	{
        public int count { get { return listOfDestinations.Count; } }

        private List<GameObject> listOfDestinations;
        private List<GameObject> listOfPaidDestinations;
        private PlayerImpl player;

        public MovementPath(PlayerImpl player)
        {
            this.player = player;
            listOfDestinations = new List<GameObject>();
            listOfDestinations.Add(player.tile);
        }

        public GameObject lastTileInPath { get { return listOfDestinations.GetLast(); } }

        public Vector3 lastPositionInPath { get { return lastTileInPath.transform.position; } }

        public GameObject tileAtIndexInPath(int i)
        {
            return listOfDestinations[i];
        }

        public bool CanMoveToTile(GameObject tile)
        {
            if (Contains(tile))
                return false;
            else if (Vector3.Magnitude(tile.transform.position - lastPositionInPath) > player.variables.movementRange)
                return false;
            else
                return true;
        }

        public bool Contains(GameObject tile)
        {
            return listOfDestinations.Contains(tile);
        }

        public void AddDestinationTile(GameObject tile)
        {
            listOfDestinations.Add(tile);
        }

        public void RemoveDestinationTile(GameObject tile)
        {
            if(listOfDestinations.Contains(tile))
                listOfDestinations.Remove(tile);
        }

        public void RemoveLastTile()
        {
            if (listOfDestinations.Count > 0)
                listOfDestinations.RemoveLast();
        }
    }
}