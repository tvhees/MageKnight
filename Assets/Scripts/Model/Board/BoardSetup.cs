using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
	namespace Board
    {
		public class BoardSetup : ScriptableObject 
		{
            public string scenarioName;
            
            public string scenarioDescription;

            public TextAsset hexTileData;
            public TextAsset enemyData;
            public TextAsset cardData;
		}
	}
}