using UnityEngine;
using UnityEngine.Networking;
using Other.Data;

namespace Other.Factory
{
    public class HexFactory: NetworkBehaviour 
	{
        public PrefabArray hexPrefabs;
        public PrefabArray featurePrefabs;

        public GameObject CreateSceneObject(HexTile.Type type, HexTile.FeatureType feature)
        {
            GameObject hex = Instantiate(hexPrefabs.prefabs[(int)type]);
            NetworkServer.Spawn(hex);
            hex.name = hex.name.Replace("(Clone)", "");
            if (feature != HexTile.FeatureType.Empty)
            {
                var feat = hex.transform.InstantiateChild(featurePrefabs.prefabs[(int)feature]);
                NetworkServer.Spawn(feat);
                hex.GetComponent<NetworkHeirarchySync>().ServerSyncChild(feat);
            }

            return hex;
        }        
	}
}