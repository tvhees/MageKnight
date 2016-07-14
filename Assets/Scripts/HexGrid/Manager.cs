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
            private bool m_isSelected;

            // Use this for initialization
            public void Init(Rules.Components.Terrain input)
            {
                m_terrain = new Rules.TerrainInfo(input);
                m_isSelected = false;
            }

            // ****************
            // CLICK BEHAVIOUR
            // ****************

            void OnMouseUpAsButton()
            {
                Enemy.Rampaging rampagingEnemy = GetComponentInChildren<Enemy.Rampaging>();

                if (rampagingEnemy != null)
                    rampagingEnemy.Provoke(); // There's a rampaging enemy on this hex - we can't move there until it's gone
                else
                    // Set the tile to selected if successfully add it to movement path
                    m_isSelected = Rules.Movement.Instance.ChangeCost(m_isSelected, this);
            }

            public void Deselect()
            {
                m_isSelected = false;
            }
        }
    }
}