using UnityEngine;
using System;

public class GameConstants
{
    #region Numbers
    public const int hexesPerTile = 7;
    public const float tileDistance = 2.5f;
    #endregion

    #region Types
    public enum CardType
    { Action, Spell, Artifact, Unit, Wound, Tactic }

    public enum TerrainType
    {
        Plains, Hill, Forest, Wasteland, Desert, Swamp, Lake, Mountain, City, Sea
    }

    public enum FeatureType
    {
        Portal, RampagingOrc, RampagingDraconum, Keep, MageTower, City, Village, MagicalGlade, RedMine, BlueMine,
        WhiteMine, GreenMine, Monastery, MonsterDen, Dungeon, SpawningGround, Tomb, Ruins, Empty
    }
    #endregion

    #region Costs
    public struct TerrainCosts
    {
        public int[] costs;

        public TerrainCosts(bool dayTime)
        {
            if(dayTime)
                costs = new int[] { 2, 3, 3, 4, 5, 5, 20, 20, 2, 20 };
            else
                costs = new int[] { 2, 3, 5, 4, 3, 5, 20, 20, 2, 20 };
        }

        public TerrainCosts(int[] input)
        {
            costs = input;
        }

        public int MoveCost(TerrainType type)
        {
            return costs[(int)type];
        }

        public bool IsTraversable(TerrainType type)
        {
            return MoveCost(type) <= 10;
        }

        public static TerrainCosts operator +(TerrainCosts costOne, TerrainCosts costTwo)
        {
            var newCosts = new int[costOne.costs.Length];
            for (int i = 0; i < costOne.costs.Length; i++)
            {
                newCosts[i] = costOne.costs[i] + costTwo.costs[i];
            }
            return new TerrainCosts(newCosts);
        }
    }
    #endregion
}
