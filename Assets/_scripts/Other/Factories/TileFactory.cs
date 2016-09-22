using UnityEngine;
using UnityEngine.Networking;
using Other.Data;

namespace Other.Factory
{
    [RequireComponent(typeof(HexFactory))]
    public class TileFactory : NetworkBehaviour
	{
        public GameObject holderPrefab;
        private HexFactory hexFactory;

        void OnEnable()
        {
            hexFactory = GetComponent<HexFactory>();
        }

        public GameObject CreateSceneObject(ScriptableObject data)
        {
            GameObject hexTile = Instantiate(holderPrefab);
            hexTile.name = data.name;
            NetworkServer.Spawn(hexTile);

            var tileData = data as HexTile;
            for(var i = 0; i < tileData.hexes.Length; i++)
            {
                GameObject hex = hexFactory.CreateSceneObject(tileData.hexes[i], tileData.features[i]);
                //hexTile.transform.ServerSetChild(hex.transform);
                hex.transform.ServerSetParent(hexTile.transform);
                hex.transform.ServerSetLocalPosition(tileData.localCoordinates[i].worldVector);
            }

            return hexTile;
        }
	}
}