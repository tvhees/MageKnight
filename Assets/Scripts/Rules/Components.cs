using UnityEngine;
using System.Collections;

namespace BoardGame
{
    namespace Rules
    {
        public static class Components
        {
            public static Terrain TerrainFromString(string name)
            {
                return (Terrain)System.Enum.Parse(typeof(Terrain), name);
            }

            public static Feature FeatureFromString(string name)
            {
                return (Feature)System.Enum.Parse(typeof(Feature), name);
            }

            public enum Terrain
            {
                plains,
                forest,
                hill,
                wasteland,
                desert,
                swamp,
                lake,
                mountain,
                coast
            }

            public enum Feature
            {
                none,
                portal,
                orc,
                keep,
                tower,
                den,
                dungeon,
                draconum,
                ruins,
                mine,
                glade,
                village,
                monastery,
                spawning,
                tomb,
                city
            }
        }

        public struct TerrainInfo
        {
            private Components.Terrain m_terrain;
            private int m_dayCost;
            private int m_nightCost;

            public TerrainInfo(Components.Terrain input)
            {
                m_terrain = input;
                m_dayCost = 0;
                m_nightCost = 0;
                SetCost(m_terrain);
            }

            // Set the movement cost for day and night given a terrain type
            // Impassable terrain is set to int.MaxValue
            public void SetCost(Components.Terrain type)
            {
                switch (type)
                {
                    case Components.Terrain.plains:
                        m_dayCost = m_nightCost = 2;
                        break;
                    case Components.Terrain.forest:
                        m_dayCost = 3;
                        m_nightCost = 5;
                        break;
                    case Components.Terrain.hill:
                        m_dayCost = m_nightCost = 3;
                        break;
                    case Components.Terrain.wasteland:
                        m_dayCost = m_nightCost = 4;
                        break;
                    case Components.Terrain.desert:
                        m_dayCost = 5;
                        m_nightCost = 3;
                        break;
                    case Components.Terrain.swamp:
                        m_dayCost = m_nightCost = 5;
                        break;
                    case Components.Terrain.lake:
                        m_dayCost = m_nightCost = int.MaxValue;
                        break;
                    case Components.Terrain.mountain:
                        m_dayCost = m_nightCost = int.MaxValue;
                        break;
                }
            }

            // Get the current movement cost given the time of day
            public int GetCost()
            {
                if (Board.State.IsDayTime())
                {
                    return m_dayCost;
                }
                else
                {
                    return m_nightCost;
                }
            }
        }
    }
}
