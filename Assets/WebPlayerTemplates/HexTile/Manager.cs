using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Boardgame
{
    namespace HexTile
    {
        public class Manager : MonoBehaviour
        {

            // ****************
            // INITIALISATION
            // ****************

            // Component Reference
            private Rulesets.TerrainInfo m_terrain;

            public Rulesets.TerrainInfo GetTerrain()
            {
                return m_terrain;
            }

            // Toggles
            private bool selected;

            // Use this for initialization
            public void Init(Rulesets.Components.Terrain input)
            {
                m_terrain = new Rulesets.TerrainInfo(input);
                selected = false;
            }

            public bool isSelected
            {
                get { return selected; }
            }

            public void Select()
            {
                selected = true;
            }

            public void Deselect()
            {
                selected = false;
            }
        }
    }
}