using Other.Data;
using Other.Utility;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
    public List<TileId> tilesInPlay = new List<TileId>();
    public List<TileId> tilesInStack = new List<TileId>();
    public HexId portalHex;

    private MapShape mapShape;
    private BoardView boardView;

    public Board(Scenario scenario, GamePlayers players, BoardView boardView)
    {
        this.boardView = boardView;
        var scenarioData = scenario.playerCounts[players.Total - scenario.minPlayers];
        var tileData = LoadRandomisedTileData();
        AddTileIdsToStack(tileData, scenarioData);
        PlaceStartingTiles();
        SetPlayerPositions(players);
    }

    private HexTile[] LoadRandomisedTileData()
    {
        var tileDataArray = TileDatabase.GetAllObjects();
        tileDataArray.Shuffle();
        return tileDataArray;
    }

    private void AddTileIdsToStack(HexTile[] tileData, DataForPlayerCount scenarioData)
    {
        var startTile = new TileId();
        var countrysideTiles = new List<TileId>();
        var coreAndCityTiles = new List<TileId>();

        mapShape = MapShapeDatabase.GetScriptableObject(scenarioData.shape.ToString());

        startTile = new TileId(GetHexIdsFromTile(mapShape.startTile), Vector3.zero);
        portalHex = startTile.hexes[3];
        countrysideTiles.AddRange(CreateTileIds("Countryside", scenarioData.numberOfCountrysideTiles, tileData));
        coreAndCityTiles.AddRange(CreateTileIds("Core", scenarioData.numberOfCoreNonCityTiles, tileData));
        coreAndCityTiles.AddRange(CreateTileIds("City", scenarioData.numberOfCoreCityTiles, tileData));
        coreAndCityTiles.Shuffle();

        tilesInStack.Add(startTile);
        tilesInStack.AddRange(countrysideTiles);
        tilesInStack.AddRange(coreAndCityTiles);
    }

    private List<TileId> CreateTileIds(string type, int numberOfTilesNeeded, HexTile[] tileData)
    {
        var tileIds = new List<TileId>();

        for (int i = 0; i < tileData.Length; i++)
        {
            if (!tileData[i].name.Contains(type))
                continue;

            tileIds.Add(new TileId(GetHexIdsFromTile(tileData[i]), Vector3.zero));
            if (tileIds.Count >= numberOfTilesNeeded)
                break;
        }

        return tileIds;
    }

    private HexId[] GetHexIdsFromTile(HexTile tile)
    {
        var hexIds = new HexId[tile.hexes.Length];

        for (int i = 0; i < hexIds.Length; i++)
            hexIds[i] = new HexId(tile, i);

        return hexIds;
    }

    private void PlaceStartingTiles()
    {
        for (int i = 0; i < mapShape.startingCountryside; i++)
        {
            PlaceNextTile(mapShape.listOfTilePositions[i].worldVector);
        }

        foreach (var tileId in tilesInPlay)
            boardView.RpcCreateTile(tileId);
    }

    private void PlaceNextTile(Vector3 position)
    {
        if (tilesInStack.Count <= 0)
            return;

        var tile = tilesInStack.GetFirst(true);
        tile.SetTilePosition(position);
        tilesInPlay.Add(tile);
    }

    public void SetPlayerPositions(GamePlayers players)
    {
        for (int i = 0; i < players.Total; i++)
        {
            players.List[i].OnHexChanged(portalHex);
        }
    }
}