using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    namespace HexGrid
    {
        public class Factory : Singleton<Factory>
        {

            //************
            // HEX GROUP FACTORY
            //************

            // Image and text data sources
            public TextAsset m_hexXML; // XML file reference

            // List of card dictionaries
            private List<Dictionary<string, string>> m_hexList = new List<Dictionary<string, string>>();

            public void LoadXML() // Load text data in to dictionaries
            {
                m_hexList = Loader.LoadHexes(m_hexXML);
            }

            public List<Dictionary<string, string>> GetHexList()
            {
                return m_hexList;
            }

            //************
            // CREATING GROUPED TILES
            //************
            public GameObject m_hexGroupPrefab;

            private List<int> m_countrysideTiles = new List<int>();
            private List<int> m_coreTiles = new List<int>();

            /// <summary>
            /// Creates board tile index stacks appropriate to scenario and shuffles them
            /// </summary>
            public void CreateRandomStacks()
            {
                // Add 11 countryside tiles and 8 core tiles.
                // Tile 0 is always the starting tile.
                m_countrysideTiles.AddIntRange(1, 5);
                m_coreTiles.AddIntRange(12, 19);

                // Directly randomise the lists
                m_countrysideTiles.Randomise(false);
                m_coreTiles.Randomise(false);
            }

            /// <summary>
            /// Places a specifically numbered HexGroup on to the board.
            /// </summary>
            /// <param name="groupNumber"></param>
            public GameObject GetHexGroup(int groupNumber, HexCoordinates groupCoordinates)
            {
                GameObject group = Instantiate(m_hexGroupPrefab);
                group.GetComponent<Hex>().SetCoordinates(groupCoordinates);
                group.GetComponent<Group>().Init(new GroupData(groupNumber));
                return group;
            }

            /// <summary>
            /// Places the first HexGroup in the specific stack on to the board.
            /// </summary>
            public GameObject GetHexGroup(bool core, HexCoordinates groupCoordinates)
            {

                List<int> tileType;
                if (core)
                    tileType = m_coreTiles;
                else
                    tileType = m_countrysideTiles;

                if (tileType.Count > 0)
                {
                    int i = tileType[0];
                    tileType.RemoveAt(0);
                    return GetHexGroup(i, groupCoordinates);
                }
                else
                {
                    Debug.Log(tileType.ToString() + " stack is empty!");
                    return null;
                }
            }
        }

        public struct GroupData
        {
            public HexCoordinates[] m_tileCoordinates;
            public Rules.Components.Terrain[] m_terrainTypes;
            public Rules.Components.Feature[] m_featureTypes;

            public GroupData(int groupNumber)
            {
                m_terrainTypes = new Rules.Components.Terrain[7];
                m_featureTypes = new Rules.Components.Feature[7];
                // Each group has 7 hexes. Hex coordinates and features are defined with center hex first
                // then outer hexes in a clockwise fashion starting at (1,0,0)
                m_tileCoordinates = new HexCoordinates[7]
                    {
                new HexCoordinates(0,0,0),
                new HexCoordinates(1,0,0),
                new HexCoordinates(1,0,1),
                new HexCoordinates(0,0,1),
                new HexCoordinates(-1,0,0),
                new HexCoordinates(0,1,0),
                new HexCoordinates(1,1,0)
                    };

                Dictionary<string, string> groupInfo = Factory.Instance.GetHexList()[groupNumber];
                for (int i = 0; i < 7; i++)
                {
                    string t;
                    string f;
                    groupInfo.TryGetValue("terrain" + i, out t);
                    groupInfo.TryGetValue("feature" + i, out f);
                    m_terrainTypes[i] = Rules.Components.TerrainFromString(t);
                    m_featureTypes[i] = Rules.Components.FeatureFromString(f);
                }
            }
        }
    }
}