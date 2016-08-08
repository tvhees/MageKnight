using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
	namespace Board
    {
		public class HexTileTester : ScriptableObjectTester<HexTile>
        {
            protected override void LogDescription(HexTile obj)
            {
                Debug.Log(string.Format("Name: {0}, Tile 0: {1}, {2}", obj.name, obj.hexes[0], obj.features[0]));
            }
        }
	}
}