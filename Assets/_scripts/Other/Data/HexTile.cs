using Other.Utility;
using UnityEngine;

namespace Other.Data
{
    [CreateAssetMenu(menuName = "Scenario/HexTile", fileName = "HexTile", order = 3)]
    [System.Serializable]
    public class HexTile : ScriptableObject
    {
        public GameConstants.TerrainType[] hexes = new GameConstants.TerrainType[7];
        public GameConstants.FeatureType[] features = new GameConstants.FeatureType[7];

        public HexVector[] localCoordinates
        {
            get
            {
                return new HexVector[]{
                    new HexVector(1, 1, 0),
                    new HexVector(1, 0, 0),
                    new HexVector(0, 1, 0),
                    new HexVector(0, 0, 0),
                    new HexVector(1, 0, 1),
                    new HexVector(0, 1, 1),
                    new HexVector(0, 0, 1)};
            }
        }
    }
}