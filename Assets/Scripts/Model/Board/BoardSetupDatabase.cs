using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    namespace Board
    {
        public class BoardSetupDatabase
        {
            static private Dictionary<string, BoardSetup> boardSetups;

            static private bool databaseIsLoaded = false;

            static private void ValidateDatabase()
            {
                if (boardSetups == null) boardSetups = new Dictionary<string, BoardSetup>();
                if (!databaseIsLoaded) LoadDatabase();
            }

            static public void LoadDatabase()
            {
                if (databaseIsLoaded) return;
                databaseIsLoaded = true;
                LoadDatabaseForce();
            }

            static public void LoadDatabaseForce()
            {
                ValidateDatabase();
                BoardSetup[] resources = Resources.LoadAll<BoardSetup>(@"Scenarios");
                foreach (var boardSetup in resources)
                {
                    if (!boardSetups.ContainsValue(boardSetup)) boardSetups.Add(boardSetup.scenarioName, boardSetup);
                }
            }

            static public void ClearDatabase()
            {
                databaseIsLoaded = false;
                boardSetups.Clear();
            }

            static public BoardSetup GetBoardSetup(string name)
            {
                ValidateDatabase();

                BoardSetup boardSetup;
                if (boardSetups.TryGetValue(name, out boardSetup)) return ScriptableObject.Instantiate(boardSetup) as BoardSetup;
                else return null;
            }
        }
    }
}