using UnityEngine;
using UnityEngine.Networking;
using Other.Data;

namespace Other.Factory
{
    [RequireComponent(typeof(HexFactory))]
    public class TileFactory : NetworkBehaviour
	{
        private HexFactory hexFactory;

        void OnEnable()
        {
            hexFactory = GetComponent<HexFactory>();
        }

        public GameObject CreateSceneObject(ScriptableObject data)
        {
            GameObject hexTile = new GameObject(data.name);
            
            var tileData = data as HexTile;

            for(var i = 0; i < tileData.hexes.Length; i++)
            {
                GameObject hex = hexFactory.CreateSceneObject(tileData.hexes[i], tileData.features[i]);
                hex.transform.SetParent(hexTile.transform);
                hex.transform.localPosition = tileData.localCoordinates[i].worldVector;
            }

            return hexTile;
        }
	}
}