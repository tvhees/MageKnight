using UnityEngine;
using UnityEngine.Networking;
using Other.Data;

namespace Other.Factory
{
    public class HexFactory : NetworkBehaviour 
	{
        public PrefabArray hexPrefabs;
        public PrefabArray featurePrefabs;

        public GameObject CreateSceneObject(HexId hexId)
        {
            return CreateSceneObject(hexId.terrain, hexId.feature);
        }

        public GameObject CreateSceneObject(HexTile.Type type, HexTile.FeatureType feature)
        {
            GameObject hex = Instantiate(hexPrefabs.prefabs[(int)type]);
            hex.name = hex.name.Replace("(Clone)", "");
            if (feature != HexTile.FeatureType.Empty)
            {
                var feat = Instantiate(featurePrefabs.prefabs[(int)feature]);
                feat.transform.SetParent(hex.transform);
            }

            return hex;
        }        
	}
}