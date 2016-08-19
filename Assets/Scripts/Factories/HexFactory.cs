using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Board
{
    public class HexFactory: MonoBehaviour 
	{
        public GameObject[] hexPrefabs;
        public GameObject[] featurePrefabs;

        public GameObject CreateSceneObject(HexTile.Type type, HexTile.FeatureType feature)
        {
            GameObject hex = Instantiate(hexPrefabs[(int)type]);
            hex.name = hex.name.Replace("(Clone)", "");
            if(feature != HexTile.FeatureType.Empty)
                hex.transform.InstantiateChild(featurePrefabs[(int)feature]);
            return hex;
        }        
	}
}