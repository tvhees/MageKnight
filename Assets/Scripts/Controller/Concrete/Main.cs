using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Boardgame
{
    public class Main : Singleton<Main>
    {
        public GameImpl game;
        public Players players;
        public Board.ScenarioDatabase scenarioDatabase;
        public GameObject frontEndMenu;
        public GameObject inGameUI;
        public PathDrawer pathDrawer;

        private int numberOfPlayers = 1;
        private string scenarioName = "Solo Conquest";
        [SerializeField]
        private Board.Scenario scenario;

        public void SelectPlayerCount(Dropdown playerCountDropdown)
        {
            numberOfPlayers = playerCountDropdown.value + 1;
        }

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
                game.StartScenario(scenario, numberOfPlayers);
                ActivateGameMenu();
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
        }
    }
}