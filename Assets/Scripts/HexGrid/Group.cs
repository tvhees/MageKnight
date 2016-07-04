using UnityEngine;
using System.Collections;

// Constructs a group of objects with 6 hexes surrounding a central hex
namespace BoardGame
{
    namespace HexGrid
    {
        public class Group : MonoBehaviour
        {

            public GameObject[] m_hexPrefabs;
            public GameObject[] m_featurePrefabs;

            public void Init(GroupData groupData)
            {
                HexCoordinates groupCoOrds = GetComponent<Hex>().GetCoordinates();

                for (int i = 0; i < groupData.m_tileCoordinates.Length; i++)
                {
                    // Get terrain and world coordinate references for this hex
                    Rules.Components.Terrain terrain = groupData.m_terrainTypes[i];
                    Rules.Components.Feature feature = groupData.m_featureTypes[i];
                    HexCoordinates coOrds = groupCoOrds + groupData.m_tileCoordinates[i];

                    // Instantiate a hex of the correct terrain type
                    GameObject hex = transform.InstantiateChild(m_hexPrefabs[(int)terrain]);

                    // Set appropriate movement costs
                    hex.GetComponent<Manager>().Init(terrain);

                    // Place the hex at the required co-ordinates
                    hex.GetComponent<Hex>().SetCoordinates(coOrds);

                    // Place required features on the tile
                    if (m_featurePrefabs[(int)feature] != null)
                        hex.transform.InstantiateChild(m_featurePrefabs[(int)feature]);
                }
            }

        }
    }
}
