using Other.Data;
using UnityEngine;
using UnityEngine.Networking;

namespace Other.Factory
{
    public class HexFactory : NetworkBehaviour
    {
        public PrefabArray hexPrefabs;
        public PrefabArray featurePrefabs;

        public GameObject CreateSceneObject(HexId hexId)
        {
            var hex = CreateSceneObject(hexId.terrain, hexId.feature);
            hex.GetComponent<HexInteraction>().id = hexId;
            return hex;
        }

        public GameObject CreateSceneObject(GameConstants.TerrainType type, GameConstants.FeatureType feature)
        {
            var hex = Instantiate(hexPrefabs.prefabs[(int)type]);
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