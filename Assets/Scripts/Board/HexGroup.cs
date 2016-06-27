using UnityEngine;
using System.Collections;

public class HexGroup : MonoBehaviour {

    public GameObject[] m_hexPrefabs;

    private HexComponents m_components;

    public void Init(int groupNumber)
    {
        m_components = new HexComponents(groupNumber);

        for (int i = 0; i < m_components.terrainTypes.Length; i++)
        {
            GameObject hex = transform.InstantiateChild(m_hexPrefabs[(int)m_components.terrainTypes[i]]);
            hex.GetComponent<Hex>().SetCoordinates(m_components.tileCoordinates[i]);
        }
    }
	
}

public struct HexComponents
{
    public BoardManager.Terrain[] terrainTypes;
    public HexCoordinates[] tileCoordinates;

    public HexComponents(int groupNumber)
    {
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

        terrainTypes = new BoardManager.Terrain[7];
        switch (groupNumber)
        {
            case 0:
                terrainTypes = new BoardManager.Terrain[7]
                { BoardManager.Terrain.plains,
                BoardManager.Terrain.forest,
                BoardManager.Terrain.plains,
                BoardManager.Terrain.mountain,
                BoardManager.Terrain.lake,
                BoardManager.Terrain.mountain,
                BoardManager.Terrain.plains };
                break;
            case 1:
                terrainTypes = new BoardManager.Terrain[7]
                { BoardManager.Terrain.forest,
                BoardManager.Terrain.lake,
                BoardManager.Terrain.plains,
                BoardManager.Terrain.plains,
                BoardManager.Terrain.plains,
                BoardManager.Terrain.forest,
                BoardManager.Terrain.forest };
                break;
            case 2:
                terrainTypes = new BoardManager.Terrain[7]
                { BoardManager.Terrain.hill,
                BoardManager.Terrain.forest,
                BoardManager.Terrain.plains,
                BoardManager.Terrain.plains,
                BoardManager.Terrain.hill,
                BoardManager.Terrain.plains,
                BoardManager.Terrain.hill };
                break;
        }
    }
}
