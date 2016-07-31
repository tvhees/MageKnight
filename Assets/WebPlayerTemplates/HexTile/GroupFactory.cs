using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    namespace HexTile
    {
        public class GroupFactory : MonoBehaviour
        {
            private List<Dictionary<string, string>> listOfHexGroupDefinitions = new List<Dictionary<string, string>>();

            public void LoadData(string scenarioName)
            {
                var hexLoader = new XmlLoaderImp();
                listOfHexGroupDefinitions = hexLoader.GetListOfDefinitions(scenarioName);
            }

            //************
            // CREATING GROUPED TILES
            //************
            public GameObject m_hexGroupPrefab;

            private List<int> countrysideTiles = new List<int>();
            private List<int> coreTiles = new List<int>();

            public void CreateRandomStacks()
            {
                // Add 11 countryside tiles and 8 core tiles.
                // Tile 0 is always the starting tile.
                countrysideTiles.AddIntRange(1, 11);
                coreTiles.AddIntRange(12, 19);

                countrysideTiles.Randomise();
                coreTiles.Randomise();
            }

            public GameObject GetGroupOfHexTiles(int groupNumber, HexCoordinates groupCoordinates)
            {
                GameObject groupOfHexTiles = Instantiate(m_hexGroupPrefab);
                groupOfHexTiles.GetComponent<Hex>().SetCoordinates(groupCoordinates);

                Dictionary<string, string> groupInfo = listOfHexGroupDefinitions[groupNumber];
                var groupData = new GroupData();
                for (int i = 0; i < 7; i++)
                {
                    string t;
                    string f;
                    groupInfo.TryGetValue("terrain" + i, out t);
                    groupInfo.TryGetValue("feature" + i, out f);
                    groupData.terrainTypes[i] = Rulesets.Components.TerrainFromString(t);
                    groupData.featureTypes[i] = Rulesets.Components.FeatureFromString(f);
                }

                groupOfHexTiles.GetComponent<Group>().Init(groupData);
                
                return groupOfHexTiles;
            }

            /// <summary>
            /// Places the first HexGroup in the specific stack on to the board.
            /// </summary>
            public GameObject GetHexGroup(bool core, HexCoordinates groupCoordinates)
            {

                List<int> tileType;
                if (core)
                    tileType = coreTiles;
                else
                    tileType = countrysideTiles;

                if (tileType.Count > 0)
                {
                    int i = tileType[0];
                    tileType.RemoveAt(0);
                    return GetGroupOfHexTiles(i, groupCoordinates);
                }
                else
                {
                    Debug.Log(tileType.ToString() + " stack is empty!");
                    return null;
                }
            }
        }

        public class GroupData
        {
            public HexCoordinates[] tileCoordinates;
            public Rulesets.Components.Terrain[] terrainTypes;
            public Rulesets.Components.Feature[] featureTypes;

            public GroupData()
            {
                terrainTypes = new Rulesets.Components.Terrain[7];
                featureTypes = new Rulesets.Components.Feature[7];
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
            }
        }
    }
}