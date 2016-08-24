using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    public class Players: MonoBehaviour 
	{
        public GameObject playerPrefab;
        public GameObject playerViewPrefab;

        private int currentPlayerIndex = 0;
        private List<PlayerImpl> listOfPlayers = new List<PlayerImpl>();

        public void CreatePlayers()
        {
            for (int i = 0; i < Main.Instance.NumberOfPlayers; i++)
                CreatePlayer();
        }

        public void CreatePlayer()
        {
            PlayerImpl player = gameObject.AddComponent<PlayerImpl>();
            listOfPlayers.Add(player);

            player.SetObject(transform.InstantiateChild(playerPrefab));
            player.AddObjectMover();
            player.tile = GameObject.FindGameObjectWithTag("PortalTile");

            player.SetBelongings(transform.InstantiateChild(playerViewPrefab, 100f * Vector3.left).GetComponentInChildren<Player.Belongings>());
            List<GameObject> playerDeck = Main.cards.CreatePlayerDeck();
            
            foreach (GameObject card in playerDeck)
                player.belongings.GainCardToDeck(card.GetComponent<Cards.MovementAndDisplay>());
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