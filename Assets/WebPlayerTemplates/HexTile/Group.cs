﻿using UnityEngine;
using System.Collections;

// Constructs a group of objects with 6 hexes surrounding a central hex
namespace Boardgame
{
    namespace HexTile
    {
        public class Group : MonoBehaviour
        {

            public GameObject[] m_hexPrefabs;
            public GameObject[] m_featurePrefabs;

            public void Init(GroupData groupData)
            {
                HexCoordinates groupCoOrds = GetComponent<Hex>().GetCoordinates();

                for (int i = 0; i < groupData.tileCoordinates.Length; i++)
                {
                    // Get terrain and world coordinate references for this hex
                    Rulesets.Components.Terrain terrain = groupData.terrainTypes[i];
                    Rulesets.Components.Feature feature = groupData.featureTypes[i];
                    HexCoordinates coOrds = groupCoOrds + groupData.tileCoordinates[i];

                    // Instantiate a hex of the correct terrain type
                    GameObject hex = transform.InstantiateChild(m_hexPrefabs[(int)terrain]);

                    // Set appropriate movement costs
                    hex.GetComponent<Manager>().Init(terrain);

                    // Place the hex at the required co-ordinates
                    hex.GetComponent<Hex>().SetCoordinates(coOrds);

                    // HACKY CODE!!
                    string feat = feature.ToString();

                    // Place required features on the tile
                    switch (feat)
                    {
                        case "orc":
                        case "draconum":
                            Enemy.Object newEnemy = Enemy.Manager.Instance.GetEnemy(feat, hex.transform);
                            newEnemy.CheckRampaging();
                            break;
                        case "keep":
                        case "tower":
                            Building.Garrison newGarrison = hex.transform.InstantiateChild(
                                    m_featurePrefabs[(int)feature]).GetComponent<Building.Garrison>();
                            newGarrison.Init();
                            break;
                        default:
                            if (m_featurePrefabs[(int)feature] != null)
                            {
                                hex.transform.InstantiateChild(m_featurePrefabs[(int)feature]);
                            }
                            break;
                    }
                }
            }

        }
    }
}
