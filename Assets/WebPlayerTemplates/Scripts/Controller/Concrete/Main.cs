using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Boardgame
{
    public class GameSetupEvent : UnityEvent<Board.Scenario, int> { }
    public class GameEvent : UnityEvent { }
    [System.Serializable]
    public class TurnEvent : UnityEvent<PlayerImpl> { }

    [RequireComponent(typeof(Board.ScenarioDatabase))]
    public class Main : Singleton<Main>
    {
        public static Rulesets.BaseRuleset rules;
        public static Commands.Stack commandStack;
        public static Cards.Cards cards;
        public static Cards.Shop cardShop;
        public static Players players;
        public static Model.Turn turn;
        public static ToolTip toolTip;
        public static Camera shopCamera;

        public static GameSetupEvent gameSetup = new GameSetupEvent();
        public static GameEvent gameStart = new GameEvent();
        public static TurnEvent turnStart = new TurnEvent();
        public static TurnEvent turnEnd = new TurnEvent();

        private string scenarioName = "Solo Conquest";
        private Board.Scenario scenario;
        private Board.ScenarioDatabase scenarioDatabase;

        public void Start()
        {
            

            rules = FindObjectOfType<Rulesets.BaseRuleset>();

            scenarioDatabase = GetComponent<Board.ScenarioDatabase>();
            SetScenario();
            NewGame();
        }

        void SetScenario()
        {
            scenario = scenarioDatabase.GetScriptableObject(scenarioName);
        }

        #region Menu settings
        public void ChooseScenario(Text scenarioText)
        {
            scenarioName = scenarioText.text;
            SetScenario();
        }
        #endregion

        #region Starting games
        public void NewGame()
        {
            if (Network.connections.Length + 1 < scenario.minPlayers)
                Debug.Log(string.Format("This scenario requires at least {0} players.", scenario.minPlayers));
            else if (Network.connections.Length + 1 > scenario.maxPlayers)
                Debug.Log(string.Format("This scenario supports a maximum of {0} players.", scenario.maxPlayers));
            else
            {
                gameStart.Invoke();
                gameSetup.Invoke(scenario, Network.connections.Length);
                NewTurn();
            }
        }

        public void NewTurn()
        {
            turnStart.Invoke(players.currentPlayer);
        }
        #endregion
    }
}