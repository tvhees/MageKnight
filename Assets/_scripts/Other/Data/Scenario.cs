using UnityEngine;

namespace Other.Data
{
    [System.Serializable]
    public enum Shape { Wedge, Open }

    [System.Serializable]
    public enum CompetitiveMode { Competitive, Cooperative }

    [CreateAssetMenu(menuName = "Scenario/Scenario", fileName = "Scenario", order = 1)]
    public class Scenario : ScriptableObject
    {
        public string description;
        public CompetitiveMode competitiveMode;
        public int days;
        public int minPlayers;
        public int maxPlayers;
        public DataForPlayerCount[] playerCounts = { };
    }

    [System.Serializable]
    public class DataForPlayerCount
    {
        public Shape shape;
        public int numberOfCountrysideTiles;
        public int numberOfCoreCityTiles;
        public int numberOfCoreNonCityTiles;
    }
}