using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Board
{
    [CreateAssetMenu(menuName = "Scenario/MapShape", fileName = "MapShape", order = 2)]
    [System.Serializable]
    public class MapShape: ScriptableObject 
	{
        public HexTile startTile;
        public int startingCountryside;
        public HexVector[] listOfTilePositions;
	}
}