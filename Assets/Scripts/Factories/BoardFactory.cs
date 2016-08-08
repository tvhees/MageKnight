using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Board
{
    public class BoardFactory: MonoBehaviour 
	{
        public TileFactory tileFactory;
        public HexTileDatabase tileDatabase;

        private HexTile[] allTiles;
        private PlayerCount data;

        public void LoadTileData(PlayerCount playerCountData)
        {
            data = playerCountData;

            allTiles = tileDatabase.GetAllObjects();
            allTiles.Randomise();
        }

        public List<GameObject> CreateCountrysideStack()
        {
            return CreateTileStack("Countryside", data.numberOfCountrysideTiles);
        }

        public List<GameObject> CreateCoreAndCityStack()
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