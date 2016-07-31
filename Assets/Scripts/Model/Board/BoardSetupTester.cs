using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
	namespace Board
    {
		public class BoardSetupTester : MonoBehaviour 
		{
            void Start()
            {
                BoardSetup boardSetup = BoardSetupDatabase.GetBoardSetup("FullConquest");
                if (boardSetup != null) Debug.Log(string.Format("Scenario Name: {0}, Scenario Description: {1}", boardSetup.scenarioName, boardSetup.scenarioDescription));
            }
		}
	}
}