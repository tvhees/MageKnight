using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace BoardGame
{
    namespace HexGrid
    {
        public class Manager : MonoBehaviour
        {

            // ****************
            // INITIALISATION
            // ****************

            // Component Reference
            private Rules.TerrainInfo m_terrain;

            public Rules.TerrainInfo GetTerrain()
            {
                return m_terrain;
            }

            // Toggles
            private bool selected;

            // Use this for initialization
            public void Init(Rules.Components.Terrain input)
            {
                m_terrain = new Rules.TerrainInfo(input);
                selected = false;
            }

            // ****************
            // CLICK BEHAVIOUR
            // ****************

            void MouseClicked()
            {
                Enemy.Rampaging rampagingEnemy = GetComponentInChildren<Enemy.Rampaging>();

                if (rampagingEnemy != null)
                    rampagingEnemy.Provoke(); // There's a rampaging enemy on this hex - we can't move there until it's gone
                else
                    // Set the tile to selected if successfully add it to movement path
                    Rules.Movement.Instance.AddTileToPath(this);
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