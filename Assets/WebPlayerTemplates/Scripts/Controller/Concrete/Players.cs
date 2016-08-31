using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    public class Players : MonoBehaviour 
	{
        private int currentPlayerIndex = 0;
        public List<PlayerImpl> listOfPlayers = new List<PlayerImpl>();

        void Awake()
        {
            Main.players = this;
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