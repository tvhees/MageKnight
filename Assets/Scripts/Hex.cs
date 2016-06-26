using UnityEngine;
using System.Collections;

public class Hex : MonoBehaviour {

    //[SerializeField]
    public HexCoordinates m_hexCoordinates;

    void FixedUpdate() {
        SetCoordinates(m_hexCoordinates);
    }

    public void SetCoordinates(HexCoordinates newCoordinates)
    {
        m_hexCoordinates = newCoordinates;
        
        transform.localPosition = m_hexCoordinates.WorldCoordinates();
    }

    public HexCoordinates GetCoordinates()
    {
        return m_hexCoordinates;
    }
}

public static class HexMetrics
{
    public const float outerRadius = 1f;

    public const float innerRadius = outerRadius * 0.866025404f;
}

[System.Serializable]
public struct HexCoordinates
{
    public int i;
    public int j;
    public int k;

    public Vector3 WorldCoordinates()
    {
        return new Vector3((i - j) * 1.5f * HexMetrics.outerRadius, 0f, (i + j - 2 * k) * HexMetrics.innerRadius);
    }
}