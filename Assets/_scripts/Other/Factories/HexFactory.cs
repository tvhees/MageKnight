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
            GameObject hex = CreateSceneObject(hexId.terrain, hexId.feature);
            hex.GetComponent<Hex>().Id = hexId;
            return hex;
        }

        public GameObject CreateSceneObject(GameConstants.TerrainType type, GameConstants.FeatureType feature)
        {
            GameObject hex = Instantiate(hexPrefabs.prefabs[(int)type]);
            hex.name = hex.name.Replace("(Clone)", "");
            if (feature != GameConstants.FeatureType.Empty)
            {
                var feat = Instantiate(featurePrefabs.prefabs[(int)feature]);
                feat.transform.SetParent(hex.transform);
            }

            return hex;
        }        
	}
}