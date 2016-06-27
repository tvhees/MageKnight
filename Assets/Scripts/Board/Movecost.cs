using UnityEngine;
using System.Collections;

public class Movecost : MonoBehaviour {

    [SerializeField]
    private int m_dayCost;
    [SerializeField]
    private int m_nightCost;

    // Set the movement cost for day and night given a terrain type
    // Impassable terrain is set to int.MaxValue
    public void SetCost(BoardManager.Terrain type)
    {
        switch (type)
        {
            case BoardManager.Terrain.plains:
                m_dayCost = m_nightCost = 2;
                break;
            case BoardManager.Terrain.forest:
                m_dayCost = 3;
                m_nightCost = 5;
                break;
            case BoardManager.Terrain.hill:
                m_dayCost = m_nightCost = 3;
                break;
            case BoardManager.Terrain.wasteland:
                m_dayCost = m_nightCost = 4;
                break;
            case BoardManager.Terrain.desert:
                m_dayCost = 5;
                m_nightCost = 3;
                break;
            case BoardManager.Terrain.swamp:
                m_dayCost = m_nightCost = 5;
                break;
            case BoardManager.Terrain.lake:
                m_dayCost = m_nightCost = int.MaxValue;
                break;
            case BoardManager.Terrain.mountain:
                m_dayCost = m_nightCost = int.MaxValue;
                break;
        }
    }

    // Get the current movement cost given the time of day
    public int GetCost(bool dayTime)
    {
        if (dayTime)
        {
            return m_dayCost;
        }
        else
        {
            return m_nightCost;
        }
    }
}
