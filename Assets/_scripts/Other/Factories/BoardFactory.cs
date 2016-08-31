using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Other.Utility;
using Other.Data;

namespace Other.Factory
{
    [RequireComponent(typeof(TileFactory))]
    public class BoardFactory : MonoBehaviour
	{
        private TileFactory tileFactory;
        private HexTile[] allTiles;

        void OnEnable()
        {
            tileFactory = GetComponent<TileFactory>();
            LoadTileData();
        }

        public void LoadTileData()
        {
            allTiles = HexTileDatabase.GetAllObjects();
            allTiles.Randomise();
        }

        public List<GameObject> CreateCountrysideStack(DataForPlayerCount data)
        {
            return CreateTileStack("Countryside", data.numberOfCountrysideTiles);
        }

        public List<GameObject> CreateCoreAndCityStack(DataForPlayerCount data)
        {
            List<GameObject> coreTiles = CreateTileStack("Core", data.numberOfCoreNonCityTiles);
            coreTiles.AddRange(CreateTileStack("City", data.numberOfCoreCityTiles));
            coreTiles.Randomise();
            return coreTiles;
        }

        List<GameObject> CreateTileStack(string type, int numberOfTiles)
        {
            List<GameObject> tileStack = new List<GameObject>();

            for(int i = 0; i < allTiles.Length; i++)
            {
                if (allTiles[i].name.Contains(type))
                {
                    tileStack.Add(tileFactory.CreateSceneObject(allTiles[i]));
                    if (tileStack.Count >= numberOfTiles)
                        break;
                }
            }

            return tileStack;
        }

        public GameObject CreateStartTile(MapShape mapShape)
        {
            return tileFactory.CreateSceneObject(mapShape.startTile);
        }
	}
}