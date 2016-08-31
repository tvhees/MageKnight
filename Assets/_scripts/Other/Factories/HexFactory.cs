using UnityEngine;
using UnityEngine.Networking;
using Other.Data;

namespace Other.Factory
{
    [RequireComponent(typeof(TileFactory))]
    public class HexFactory: NetworkBehaviour 
	{
        public GameObject[] hexPrefabs;
        public GameObject[] featurePrefabs;

        public GameObject CreateSceneObject(HexTile.Type type, HexTile.FeatureType feature)
        {
            GameObject hex = Instantiate(hexPrefabs[(int)type]);
            hex.name = hex.name.Replace("(Clone)", "");
            NetworkServer.Spawn(hex);
            if (feature != HexTile.FeatureType.Empty)
            {
                var feat = hex.transform.InstantiateChild(featurePrefabs[(int)feature]);
                NetworkServer.Spawn(feat);
            }

            return hex;
        }        
	}
}