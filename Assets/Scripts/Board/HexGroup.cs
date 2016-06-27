using UnityEngine;
using System.Collections;

public class HexGroup : MonoBehaviour {

    public GameObject[] m_hexPrefabs;
    public GameObject[] m_featurePrefabs;

    public void Init(int groupNumber)
    {
        HexComponents groupData = new HexComponents(groupNumber);
        HexCoordinates groupCoords = GetComponent<Hex>().GetCoordinates();

        for (int i = 0; i < groupData.terrainTypes.Length; i++)
        {
            // Get terrain and world coordinate references for this hex
            BoardManager.Terrain terrain = groupData.terrainTypes[i];
            BoardManager.Feature feature = groupData.featureTypes[i];
            HexCoordinates coords = groupCoords + groupData.tileCoordinates[i];

            // Instantiate a hex of the correct terrain type
            GameObject hex = transform.InstantiateChild(m_hexPrefabs[(int)terrain]);

            // Set appropriate movement costs
            hex.GetComponent<Movecost>().SetCost(terrain);

            // Place the hex at the required co-ordinates
            hex.GetComponent<Hex>().SetCoordinates(coords);
            
            // Place required features on the tile
            if(m_featurePrefabs[(int)feature] != null)
                hex.transform.InstantiateChild(m_featurePrefabs[(int)feature]);
        }
    }
	
}

public struct HexComponents
{
    public HexCoordinates[] tileCoordinates;
    public BoardManager.Terrain[] terrainTypes;
    public BoardManager.Feature[] featureTypes;

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
        featureTypes = new BoardManager.Feature[7];
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

                featureTypes = new BoardManager.Feature[7]
                    { BoardManager.Feature.portal,
                    BoardManager.Feature.none,
                    BoardManager.Feature.none,
                    BoardManager.Feature.none,
                    BoardManager.Feature.none,
                    BoardManager.Feature.none,
                    BoardManager.Feature.none };
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

                featureTypes = new BoardManager.Feature[7]
                    { BoardManager.Feature.glade,
                    BoardManager.Feature.none,
                    BoardManager.Feature.village,
                    BoardManager.Feature.none,
                    BoardManager.Feature.none,
                    BoardManager.Feature.none,
                    BoardManager.Feature.orc };
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

                featureTypes = new BoardManager.Feature[7]
                    { BoardManager.Feature.none,
                    BoardManager.Feature.glade,
                    BoardManager.Feature.village,
                    BoardManager.Feature.none,
                    BoardManager.Feature.mine,
                    BoardManager.Feature.none,
                    BoardManager.Feature.orc };
                break;
        }
    }
}
