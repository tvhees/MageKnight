using UnityEngine;
using System.Collections;

public class HexGroup : MonoBehaviour {

    public GameObject[] m_hexPrefabs;
    public GameObject[] m_featurePrefabs;

    public void Init(int groupNumber)
    {
        HexGroupData groupData = new HexGroupData(groupNumber);
        HexCoordinates groupCoords = GetComponent<Hex>().GetCoordinates();

        for (int i = 0; i < groupData.terrainTypes.Length; i++)
        {
            // Get terrain and world coordinate references for this hex
            Components.Terrain terrain = groupData.terrainTypes[i];
            Components.Feature feature = groupData.featureTypes[i];
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
