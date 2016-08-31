using UnityEngine;
using Other.Data;

namespace Other.Factory
{
    [RequireComponent(typeof(TileFactory))]
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