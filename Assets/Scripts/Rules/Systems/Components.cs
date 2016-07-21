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
            private int dayCost;
            private int nightCost;

            public TerrainInfo(Components.Terrain input)
            {
                m_terrain = input;
                dayCost = 0;
                nightCost = 0;
                SetCost(m_terrain);
            }

            // Set the movement cost for day and night given a terrain type
            // Impassable terrain is set to int.MaxValue
            public void SetCost(Components.Terrain type)
            {
                switch (type)
                {
                    case Components.Terrain.plains:
                        dayCost = nightCost = 2;
                        break;
                    case Components.Terrain.forest:
                        dayCost = 3;
                        nightCost = 5;
                        break;
                    case Components.Terrain.hill:
                        dayCost = nightCost = 3;
                        break;
                    case Components.Terrain.wasteland:
                        dayCost = nightCost = 4;
                        break;
                    case Components.Terrain.desert:
                        dayCost = 5;
                        nightCost = 3;
                        break;
                    case Components.Terrain.swamp:
                        dayCost = nightCost = 5;
                        break;
                    case Components.Terrain.lake:
                        dayCost = nightCost = int.MaxValue;
                        break;
                    case Components.Terrain.mountain:
                        dayCost = nightCost = int.MaxValue;
                        break;
                }
            }

            // Get the current movement cost given the time of day
            public int cost
            {
                get
                {
                    if (Board.State.IsDayTime())
                    {
                        return dayCost;
                    }
                    else
                    {
                        return nightCost;
                    }
                }
            }
        }
    }
}
