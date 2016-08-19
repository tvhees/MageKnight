using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Boardgame
{
    public class Main : Singleton<Main>
    {
        public static float scalingFactor = 50f;
        public static GameImpl game;
        public static Board.Board board;
        public static Cards.Cards cards;
        public static Players players;
        public static Model.Turn turn;
        public static Rulesets.Ruleset rules { get { return game.rules; } }
        public static Board.ScenarioDatabase scenarioDatabase;
        public static Commands.Stack commandStack;
        public static Cards.CardOffer cardOffer;

        public GameObject frontEndMenu;
        public GameObject inGameUI;
        public GameObject statsDisplay;
        public Camera shopCamera;
        public Camera mainCamera;
        public ToolTip toolTip;

        private int numberOfPlayers = 1;
        private string scenarioName = "Solo Conquest";
        [SerializeField]
        private Board.Scenario scenario;

        void Awake()
        {
            game = FindObjectOfType<GameImpl>();
            board = FindObjectOfType<Board.Board>();
            cards = FindObjectOfType<Cards.Cards>();
            players = FindObjectOfType<Players>();
            turn = FindObjectOfType<Model.Turn>();
            commandStack = FindObjectOfType<Commands.Stack>();
            cardOffer = FindObjectOfType<Cards.CardOffer>();
        }

        public void SelectPlayerCount(Dropdown playerCountDropdown)
        {
            numberOfPlayers = playerCountDropdown.value + 1;
        }

        public int NumberOfPlayers { get { return numberOfPlayers; } }

        public void ChooseScenario(Text scenarioName)
        {
            this.scenarioName = scenarioName.text;
            scenario = scenarioDatabase.GetScriptableObject(this.scenarioName);
        }

        public void NewGame()
        {
            if (numberOfPlayers < scenario.minPlayers)
                Debug.Log(string.Format("This scenario requires at least {0} players.", scenario.minPlayers));
            else if (numberOfPlayers > scenario.maxPlayers)
                Debug.Log(string.Format("This scenario supports a maximum of {0} players.", scenario.maxPlayers));
            else
            {
                ActivateGameMenu();
                game.StartScenario(scenario);
            }
        }

        public void DestroyGame()
        {
            game.EndScenario();
        }

        void ActivateGameMenu()
        {
            frontEndMenu.SetActive(false);
            inGameUI.SetActive(true);
            statsDisplay.SetActive(true);
        }
    }
}