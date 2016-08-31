using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Other.Utility;

namespace Other.Data
{
    [CreateAssetMenu(menuName = "Scenario/HexTile", fileName = "HexTile", order = 3)]
    [System.Serializable]
    public class HexTile : ScriptableObject
    {
        public enum Type
        {
            Plains,
            Hill,
            Forest,
            Wasteland,
            Desert,
            Swamp,
            Lake,
            Mountain,
            City,
            Sea
        }

        public enum FeatureType
        {
            Portal,
            RampagingOrc,
            RampagingDraconum,
            Keep,
            MageTower,
            City,
            Village,
            MagicalGlade,
            RedMine,
            BlueMine,
            WhiteMine,
            GreenMine,
            Monastery,
            MonsterDen,
            Dungeon,
            SpawningGround,
            Tomb,
            Ruins,
            Empty
        }

        public Type[] hexes = new Type[7];
        public FeatureType[] features = new FeatureType[7];

        public HexVector[] localCoordinates {
            get {
                return new HexVector[7]{
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