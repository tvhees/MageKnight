using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Other.Data;
using Other.Utility;

public class Board
{
    public List<TileId> tilesInPlay = new List<TileId>();
    public List<TileId> tilesInStack = new List<TileId>();

    private DataForPlayerCount dataForPlayerCount;
    private MapShape dataForMapShape;
    private HexTile[] tileDataArray;
    private BoardView boardView;

    public Board(Scenario scenario, int numberOfPlayers, BoardView boardView)
    {
        this.boardView = boardView;
        dataForPlayerCount = scenario.playerCounts[numberOfPlayers - scenario.minPlayers];
        LoadRandomisedTileData();
        AddTileIdsToStack();
        PlaceStartingTiles();
    }

    void LoadRandomisedTileData()
    {
        tileDataArray = TileDatabase.GetAllObjects();
        tileDataArray.Shuffle();
    }

    void AddTileIdsToStack()
    {
        var startTile = new TileId();
        var countrysideTiles = new List<TileId>();
        var coreAndCityTiles = new List<TileId>();

        dataForMapShape = MapShapeDatabase.GetScriptableObject(dataForPlayerCount.shape.ToString());

        startTile = new TileId(GetHexIdsFromTile(dataForMapShape.startTile), Vector3.zero);
        countrysideTiles.AddRange(CreateTileIds("Countryside", dataForPlayerCount.numberOfCountrysideTiles));
        coreAndCityTiles.AddRange(CreateTileIds("Core", dataForPlayerCount.numberOfCoreNonCityTiles));
        coreAndCityTiles.AddRange(CreateTileIds("City", dataForPlayerCount.numberOfCoreCityTiles));
        coreAndCityTiles.Shuffle();

        tilesInStack.Add(startTile);
        tilesInStack.AddRange(countrysideTiles);
        tilesInStack.AddRange(coreAndCityTiles);
    }

    List<TileId> CreateTileIds(string type, int numberOfTilesNeeded)
    {
        var tileIds = new List<TileId>();

        for (int i = 0; i < tileDataArray.Length; i++)
        {
            if (!tileDataArray[i].name.Contains(type))
                continue;

            tileIds.Add(new TileId(GetHexIdsFromTile(tileDataArray[i]), Vector3.zero));
            if (tileIds.Count >= numberOfTilesNeeded)
                break;
        }

        return tileIds;
    }

    HexId[] GetHexIdsFromTile(HexTile tile)
    {
        var hexIds = new HexId[tile.hexes.Length];

        for (int i = 0; i < hexIds.Length; i++)
            hexIds[i] = new HexId(tile, i);

        return hexIds;
    }

    void PlaceStartingTiles()
    {
        for (int i = 0; i < dataForMapShape.startingCountryside; i++)
        {
            PlaceNextTile(dataForMapShape.listOfTilePositions[i].worldVector);
        }

        foreach (var tileId in tilesInPlay)
            boardView.RpcCreateTile(tileId);
    }

    void PlaceNextTile(Vector3 position)
    {
        if (tilesInStack.Count <= 0)
            return;

        TileId tile = tilesInStack.GetFirst(remove: true);
        tile.SetTilePosition(position);
        tilesInPlay.Add(tile);
    }
}
