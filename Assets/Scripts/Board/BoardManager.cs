using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour {

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

    [SerializeField]
    private GameObject m_hexGroup;

    [SerializeField]
    private List<int> m_countrysideTiles = new List<int>();
    private List<int> m_coreTiles = new List<int>();

    /// <summary>
    /// Places a specifically numbered HexGroup on to the board.
    /// </summary>
    /// <param name="groupNumber"></param>
    void PlaceHexGroup(int groupNumber, HexCoordinates groupCoordinates)
    {
        GameObject group = Instantiate(m_hexGroup);
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

    void Start()
    {
        shuffleTiles();

        // Place starting tile
        PlaceHexGroup(0, new HexCoordinates(0, 0, 0));

        // Place first two countryside tiles
        PlaceHexGroup(m_countrysideTiles, new HexCoordinates(3, 2, 0));
        PlaceHexGroup(m_countrysideTiles, new HexCoordinates(3, 0, 1));
    }

    /// <summary>
    /// Creates board tile index stacks appropriate to scenario and shuffles them
    /// </summary>
    void shuffleTiles()
    {
        // Add 11 countryside tiles and 8 core tiles.
        // Tile 0 is always the starting tile.
        m_countrysideTiles.AddIntRange(1, 2);
        m_coreTiles.AddIntRange(12, 19);

        // Directly randomise the lists
        m_countrysideTiles.Randomise(false);
        m_coreTiles.Randomise(false);
    }
}
