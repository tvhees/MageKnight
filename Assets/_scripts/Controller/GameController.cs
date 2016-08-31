using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Other.Data;
using Other.Utility;

namespace Boardgame
{
    public class GameController: MonoBehaviour 
	{
        public Game game = new Game();
        public Board board;
        public PlayerControl localPlayer;
        public List<PlayerControl> players = new List<PlayerControl>();

        void OnEnable()
        {
            PlayerControl.Started.AddListener(OnPlayerStarted);
            PlayerControl.StartedLocal.AddListener(OnPlayerStartedLocal);

            Scenario scenario = ScenarioDatabase.GetScriptableObject("Solo Conquest");
            board = new Board(scenario);
        }

        #region Event Handlers
        void OnPlayerStarted(PlayerControl sender)
        {
            players.Add(sender);
        }

        void OnPlayerStartedLocal(PlayerControl sender)
        {
            localPlayer = sender;
        }
        #endregion
    }
}