using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Rulesets
{
    public enum TerrainType
    {
        Plains,
        Hill,
        Forest,
        Wasteland,
        Desert,
        Swamp,
        City,
        Lake,
        Mountain,
        Sea
    }

    public static class MovementCosts
	{
        static int[] costs;

        public static void Reset(bool day)
        {
            if(day)
                costs = new int[10] { 2, 3, 3, 4, 5, 5, 2, 0, 5, 0 };
            else
                costs = new int[10] { 2, 3, 5, 4, 3, 5, 2, 0, 5, 0 };
        }

        public static bool IsTraversable(TerrainType type)
        {
            if (type == TerrainType.Sea)
                return false;

            if (type == TerrainType.Mountain)
                return false;

            if (type == TerrainType.Lake)
                return false;

            return true;
        }

        public static int GetCost(TerrainType type)
        {
            return costs[(int)type];
        }
	}
}