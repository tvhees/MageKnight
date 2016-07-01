using UnityEngine;
using System.Collections;

public class Movecost : MonoBehaviour {

    private int m_dayCost;
    private int m_nightCost;

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
        bool dayTime = GameManager.IsDayTime();
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
