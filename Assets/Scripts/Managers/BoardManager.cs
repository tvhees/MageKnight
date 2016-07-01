using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour {

    // ****************
    // INITIALISATION
    // ****************
    private GameObject boardHolder;

    void Start()
    {
        // Create tile stacks and a folder object for the board
        shuffleTiles();
        boardHolder = new GameObject("BoardHolder");

        // Place starting tile
        PlaceHexGroup(0, new HexCoordinates(0, 0, 0));

        // Place first two countryside tiles
        PlaceHexGroup(m_countrysideTiles, new HexCoordinates(3, 2, 0));
        PlaceHexGroup(m_countrysideTiles, new HexCoordinates(3, 0, 1));
    }

    // ****************
    // DESTRUCTION
    // ****************
    public void RemoveBoard()
    {
        Destroy(boardHolder);
        Destroy(gameObject);
    }


    public GameObject m_hexGroup;
    private List<int> m_countrysideTiles = new List<int>();
    private List<int> m_coreTiles = new List<int>();

    /// <summary>
    /// Places a specifically numbered HexGroup on to the board.
    /// </summary>
    /// <param name="groupNumber"></param>
    void PlaceHexGroup(int groupNumber, HexCoordinates groupCoordinates)
    {
        GameObject group = boardHolder.transform.InstantiateChild(m_hexGroup);
        group.GetComponent<Hex>().SetCoordinates(groupCoordinates);
        group.GetComponent<HexGroup>().Init(groupNumber);
    }

    /// <summary>
    /// Places the first HexGroup in the specific stack on to the board.
    /// </summary>
    /// <param name="tileType"></param>
    void PlaceHexGroup(List<int> tileType, HexCoordinates groupCoordinates)
    {
        if (tileType.Count > 0)
        {
            PlaceHexGroup(tileType[0], groupCoordinates);
            tileType.RemoveAt(0);
        }
        else
        {
            Debug.Log(tileType.ToString() + " stack is empty!");
        }
    }

    /// <summary>
    /// Creates board tile index stacks appropriate to scenario and shuffles them
    /// </summary>
    void shuffleTiles()
    {
        // Add 11 countryside tiles and 8 core tiles.
        // Tile 0 is always the starting tile.
        m_countrysideTiles.AddIntRange(1,5);
        m_coreTiles.AddIntRange(12, 19);

        // Directly randomise the lists
        m_countrysideTiles.Randomise(false);
        m_coreTiles.Randomise(false);
    }
}

public struct Components
{
    public enum Terrain
    {
        plains,
        forest,
        hill,
        wasteland,
        desert,
        swamp,
        lake,
        mountain,
        coast
    }

    public enum Feature
    {
        none,
        portal,
        orc,
        keep,
        tower,
        den,
        dungeon,
        draconum,
        ruins,
        mine,
        glade,
        village,
        monastery,
        spawning,
        tomb,
        city
    }
}

public struct HexGroupData
{
    public HexCoordinates[] tileCoordinates;
    public Components.Terrain[] terrainTypes;
    public Components.Feature[] featureTypes;

    public HexGroupData(int groupNumber)
    {
        // Each group has 7 hexes. Hex coordinates and features are defined with center hex first
        // then outer hexes in a clockwise fashion starting at (1,0,0)
        tileCoordinates = new HexCoordinates[7]
            {
                new HexCoordinates(0,0,0),
                new HexCoordinates(1,0,0),
                new HexCoordinates(1,0,1),
                new HexCoordinates(0,0,1),
                new HexCoordinates(-1,0,0),
                new HexCoordinates(0,1,0),
                new HexCoordinates(1,1,0)
            };

        terrainTypes = new Components.Terrain[7];
        featureTypes = new Components.Feature[7];
        switch (groupNumber)
        {
            case 0:
                terrainTypes = new Components.Terrain[7]
                    { Components.Terrain.plains,
                    Components.Terrain.forest,
                    Components.Terrain.plains,
                    Components.Terrain.mountain,
                    Components.Terrain.lake,
                    Components.Terrain.mountain,
                    Components.Terrain.plains };

                featureTypes = new Components.Feature[7]
                    { Components.Feature.portal,
                    Components.Feature.none,
                    Components.Feature.none,
                    Components.Feature.none,
                    Components.Feature.none,
                    Components.Feature.none,
                    Components.Feature.none };
                break;
            case 1:
                terrainTypes = new Components.Terrain[7]
                    { Components.Terrain.forest,
                    Components.Terrain.lake,
                    Components.Terrain.plains,
                    Components.Terrain.plains,
                    Components.Terrain.plains,
                    Components.Terrain.forest,
                    Components.Terrain.forest };

                featureTypes = new Components.Feature[7]
                    { Components.Feature.glade,
                    Components.Feature.none,
                    Components.Feature.village,
                    Components.Feature.none,
                    Components.Feature.none,
                    Components.Feature.none,
                    Components.Feature.orc };
                break;
            case 2:
                terrainTypes = new Components.Terrain[7]
                    { Components.Terrain.hill,
                    Components.Terrain.forest,
                    Components.Terrain.plains,
                    Components.Terrain.plains,
                    Components.Terrain.hill,
                    Components.Terrain.plains,
                    Components.Terrain.hill };

                featureTypes = new Components.Feature[7]
                    { Components.Feature.none,
                    Components.Feature.glade,
                    Components.Feature.village,
                    Components.Feature.none,
                    Components.Feature.mine,
                    Components.Feature.none,
                    Components.Feature.orc };
                break;
            case 3:
                terrainTypes = new Components.Terrain[7]
                    { Components.Terrain.forest,
                    Components.Terrain.hill,
                    Components.Terrain.hill,
                    Components.Terrain.hill,
                    Components.Terrain.plains,
                    Components.Terrain.plains,
                    Components.Terrain.plains };

                featureTypes = new Components.Feature[7]
                    { Components.Feature.none,
                    Components.Feature.keep,
                    Components.Feature.none,
                    Components.Feature.mine,
                    Components.Feature.village,
                    Components.Feature.none,
                    Components.Feature.none };
                break;
            case 4:
                terrainTypes = new Components.Terrain[7]
                    { Components.Terrain.desert,
                    Components.Terrain.desert,
                    Components.Terrain.mountain,
                    Components.Terrain.plains,
                    Components.Terrain.plains,
                    Components.Terrain.hill,
                    Components.Terrain.desert };

                featureTypes = new Components.Feature[7]
                    { Components.Feature.tower,
                    Components.Feature.none,
                    Components.Feature.none,
                    Components.Feature.village,
                    Components.Feature.none,
                    Components.Feature.orc,
                    Components.Feature.none };
                break;
            case 5:
                terrainTypes = new Components.Terrain[7]
                    { Components.Terrain.lake,
                    Components.Terrain.plains,
                    Components.Terrain.plains,
                    Components.Terrain.hill,
                    Components.Terrain.forest,
                    Components.Terrain.forest,
                    Components.Terrain.forest };

                featureTypes = new Components.Feature[7]
                    { Components.Feature.none,
                    Components.Feature.monastery,
                    Components.Feature.orc,
                    Components.Feature.mine,
                    Components.Feature.none,
                    Components.Feature.glade,
                    Components.Feature.none };
                break;
        }
    }
}
