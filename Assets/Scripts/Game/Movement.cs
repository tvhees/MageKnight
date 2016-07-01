using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class Movement : MonoBehaviour {

    // Game Rules
    private float maxDistance = 4f;

    // Tracking variables
    private int m_totalCost;
    private int m_totalPaid;
    private List<Movecost> m_hexPath = new List<Movecost>(); // List for reference to hextile component
    private List<GameObject> m_pathArrows = new List<GameObject>(); // List for UI arrows showing path
    private List<int> m_pathCosts = new List<int>(); // Keep track of running total movecost for payment UI
    private List<GameObject> m_pathNumbers = new List<GameObject>(); // List for UI numbers showing move cost

    // UI prefabs
    public GameObject m_arrowPrefab;
    public GameObject m_numberPrefab;


    private UnityEvent m_highlightMoveCards;

    public bool ChangeCost(bool removeHex, Movecost hex)
    {
        // Need to know if it's day or night
        bool timeOfDay = GameManager.IsDayTime();

        // If we're adding new hexes to movement path they go at the end of the list
        if (!removeHex)
        {
            return AddNewNode(hex); // Tell the tile that it is selected iff we manage to add it as a movement node 
        }
        // If we're removing a hex we need to work backwards down the list and remove any later movement nodes
        else
        {
            int i = m_hexPath.IndexOf(hex);

            while (m_hexPath.Count > i)
            {
                DeleteLastNode();
            }

            return false; // Confirm that the tile is deselected
        }
    }

    void DrawMovePath(Vector3 start, Vector3 end)
    {
        Vector3 midPoint = (end + start) * 0.5f; // Place arrows and numbers at midpoint between tiles
        Quaternion rotation = Quaternion.LookRotation(Vector3.Cross((end - start), Vector3.up)); // arrow prefab is already rotated 90 degrees so we need to make it 'look' at the orthogonal direction
        GameObject arrow = Instantiate(m_arrowPrefab, midPoint, rotation) as GameObject;
        m_pathArrows.Add(arrow);

        // Show total movement cost on number above array
        GameObject number = Instantiate(m_numberPrefab, midPoint, Quaternion.Euler(60f, 30f, 0f)) as GameObject;
        number.GetComponent<TextMesh>().text = m_totalCost.ToString();
        m_pathNumbers.Add(number);
    }

    bool AddNewNode(Movecost hex)
    {
        // Store positions of previous and current movement nodes
        Vector3 posA;
        if (m_hexPath.Count < 1)
            posA = GameManager.Instance.GetCurrentPlayer().transform.position; // THIS SHOULD BE THE PLAYER POSITION
        else
            posA = m_hexPath.GetLast().transform.position; // Draw from the last tile added to movement path
        Vector3 posB = hex.transform.position; // Draw to the new tile

        Vector3 direction = posB - posA;
        if (direction.sqrMagnitude < maxDistance) // We can usually only move one tile away
        {
            // Add new hex to the end of the list and show the total movement cost above the new arrow
            m_hexPath.Add(hex);
            m_totalCost += hex.GetCost();
            m_pathCosts.Add(m_totalCost);

            DrawMovePath(posA, posB);

            return true;
        }
        else
            return false;
    }

    void DeleteLastNode()
    {
        // Remove hex and cost attributes
        Movecost lastHex = m_hexPath.GetLast();
        lastHex.GetComponent<HexManager>().Deselect();
        m_hexPath.RemoveLast();
        m_totalCost -= lastHex.GetCost();
        m_pathCosts.RemoveLast();

        // Destroy associated arrow
        GameObject arrow = m_pathArrows.GetLast();
        Destroy(arrow);
        m_pathArrows.RemoveLast();

        // Destroy associated number
        GameObject number = m_pathNumbers.GetLast();
        Destroy(number);
        m_pathNumbers.RemoveLast();
    }

    public void AddMovement(int value)
    {
        m_totalPaid += value;

        ColourPath();
    }

    void ColourPath()
    {
        for (int i = 0; i < m_pathCosts.Count; i++)
        {
            if (m_pathCosts[i] <= m_totalPaid)
            {
                m_pathNumbers[i].GetComponent<TextMesh>().color = Color.red;
            }
            else
            {
                m_pathNumbers[i].GetComponent<TextMesh>().color = Color.white;
            }
        }
    }
}
