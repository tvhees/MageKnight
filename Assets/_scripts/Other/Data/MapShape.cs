using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Other.Utility;

namespace Other.Data
{
    [CreateAssetMenu(menuName = "Scenario/MapShape", fileName = "MapShape", order = 2)]
    [System.Serializable]
    public class MapShape : ScriptableObject 
	{
        public HexTile startTile;
        public int startingCountryside;
        public HexVector[] listOfTilePositions = new HexVector[] { };
	}
}