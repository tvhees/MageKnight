using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    public class Players: MonoBehaviour 
	{
        public GameObject playerPrefab;

        private int currentPlayerIndex = 0;
        private List<PlayerImpl> listOfPlayers = new List<PlayerImpl>();

        public void CreatePlayers(int numberOfPlayers)
        {
            for (int i = 0; i < numberOfPlayers; i++)
                CreatePlayer();
        }

        public void CreatePlayer()
        {
            PlayerImpl player = gameObject.AddComponent<PlayerImpl>();
            player.playerObject = transform.InstantiateChild(playerPrefab);
            player.tile = GameObject.FindGameObjectWithTag("PortalTile");
            listOfPlayers.Add(player);
        }
        
        public PlayerImpl currentPlayer
        {
            get { return listOfPlayers[currentPlayerIndex]; }
        }

        public PlayerImpl nextPlayer
        {
            get { return listOfPlayers[(int)Mathf.Repeat(currentPlayerIndex + 1, listOfPlayers.Count)]; }
        }
	}
}