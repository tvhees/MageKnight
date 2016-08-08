using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Board
{
    public class HexFactory: MonoBehaviour 
	{
        public GameObject[] hexPrefabs;

        public GameObject CreateSceneObject(HexTile.Type data)
        {
            return Instantiate(hexPrefabs[(int)data]);
        }        
	}
}