using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Board
{
    [RequireComponent(typeof(MapShapeDatabase))]
    public class Board: MonoBehaviour 
	{
        public BoardFactory boardFactory;

        private List<HexVector> tilePositions;
        private List<GameObject> countrysideTiles;
        private List<GameObject> coreTiles;

        private GameObject tileStack;

        private GameObject boardHolder;

        void Awake()
        {
            Main.gameSetup.AddListener(CreateBoard);
        }

        public void CreateBoard(Scenario scenario, int numberOfPlayers)
        {
            PlayerCount playerCountData = scenario.playerCounts[numberOfPlayers - scenario.minPlayers];
            CreateTilesForScenario(playerCountData);
            CreateStartingBoard(playerCountData);
        }

        void CreateTilesForScenario(PlayerCount playerCountData)
        {
            boardFactory.LoadTileData(playerCountData);
            tileStack = new GameObject("TileStack");

            countrysideTiles = boardFactory.CreateCountrysideStack();
            coreTiles = boardFactory.CreateCoreAndCityStack();

            AddTilesToStack(countrysideTiles);
            AddTilesToStack(coreTiles);

            tileStack.transform.position = new Vector3(0f, 30f, 0f);
        }

        void AddTilesToStack(List<GameObject> tileList)
        {
            foreach (GameObject tile in tileList)
                tile.transform.SetParent(tileStack.transform);
        }

        void CreateStartingBoard(PlayerCount playerCountData)
        {
            boardHolder = new GameObject("BoardHolder");
            MapShape mapShape = GetComponent<MapShapeDatabase>().GetScriptableObject(playerCountData.shape.ToString());
            tilePositions = new List<HexVector>(mapShape.listOfTilePositions);

            AddPortalTileToBoard(mapShape);
            AddStartingCountryside(mapShape);
        }

        void AddPortalTileToBoard(MapShape mapShape)
        {
            GameObject portalTile = boardFactory.CreateStartTile(mapShape);
            portalTile.transform.SetParent(boardHolder.transform);
            tilePositions.RemoveAt(0);

            portalTile.transform.GetChild(3).tag = "PortalTile";
        }

        void AddStartingCountryside(MapShape mapShape)
        {
            for (int i = 1; i < mapShape.startingCountryside; i++)
            {
                PlaceNewTile();
            }
        }

        void PlaceNewTile()
        {
            GameObject tile = countrysideTiles[0];
            tile.transform.SetParent(boardHolder.transform);
            tile.transform.localPosition = tilePositions[0].worldVector;
            tile.GetComponent<TileEventManager>().tilePlacedEvent.Invoke();

            countrysideTiles.RemoveAt(0);
            tilePositions.RemoveAt(0);
        }
    }
}