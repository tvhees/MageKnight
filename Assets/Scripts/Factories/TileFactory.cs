using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Board
{
    public class TileFactory: MonoBehaviour 
	{
        public HexFactory hexFactory;

        public GameObject CreateSceneObject(ScriptableObject data)
        {
            GameObject hexTile = new GameObject();
            var tileData = data as HexTile;
            for(var i = 0; i < tileData.hexes.Length; i++)
            {
                GameObject hex = hexFactory.CreateSceneObject(tileData.hexes[i], tileData.features[i]);
                hex.transform.SetParent(hexTile.transform);
                hex.transform.localPosition = tileData.localCoordinates[i].worldVector;
            }

            hexTile.name = data.name;

            return hexTile;
        }
	}
}