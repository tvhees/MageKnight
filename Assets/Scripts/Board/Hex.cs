using UnityEngine;
using System.Collections;

public class Hex : MonoBehaviour {

    [SerializeField]
    private HexCoordinates m_hexCoordinates;

    // Default constructor
    public Hex()
    {
    }

    // Constructor with explicit coordinate setting
    public Hex(HexCoordinates newCoordinates)
    {
        SetCoordinates(newCoordinates);
    }

    // Turn a hexagonal coordinate system position in to cubic coordinates
    public void SetCoordinates(HexCoordinates newCoordinates)
    {
        // Store the hexagonal coordinates to retrieve later
        // This avoids having to convert backwards
        m_hexCoordinates = newCoordinates;
        
        transform.localPosition = m_hexCoordinates.WorldCoordinates();
    }

    // Retrieve hexagonal coordinates
    public HexCoordinates GetCoordinates()
    {
        return m_hexCoordinates;
    }

#if UNITY_EDITOR
    // Editor code to allow changing coordinates at run time
    void FixedUpdate()
    {
        SetCoordinates(m_hexCoordinates);
    }
#endif
}

public static class HexMetrics
{
    public const float outerRadius = 1f;

    public const float innerRadius = outerRadius * 0.866025404f;
}

[System.Serializable]
public struct HexCoordinates
{
    public int m_i;
    public int m_j;
    public int m_k;

    public HexCoordinates(int i, int j, int k)
    {
        m_i = i;
        m_j = j;
        m_k = k;
    }

    public Vector3 WorldCoordinates()
    {
        return new Vector3((m_i - m_j) * 1.5f * HexMetrics.outerRadius, 0f, (m_i + m_j - 2 * m_k) * HexMetrics.innerRadius);
    }
}