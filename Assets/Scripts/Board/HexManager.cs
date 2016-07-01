using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class HexManager : MonoBehaviour {

    // ****************
    // INITIALISATION
    // ****************

    // Component References
    private Movecost m_moveCost;
    private Hex m_hex;

    // Toggles
    private bool m_isSelected;

    // Use this for initialization
    void Awake () {
        m_moveCost = GetComponent<Movecost>();
        m_hex = GetComponent<Hex>();
        m_isSelected = false;
	}

    // ****************
    // CLICK BEHAVIOUR
    // ****************

    // Declare click event to assign in editor
    public HexClickEvent clicked;

    void OnMouseUpAsButton()
    {
        // Send tile information to all recievers assigned in editor
        //clicked.Invoke(m_isSelected, m_moveCost);

        // Set the tile to selected if successfully add it to movement path
        m_isSelected = ProcessMovementCost();
    }

    bool ProcessMovementCost()
    {
        // When clicking a deselected tile: sends false, returns true if the tile could be added to movement path
        return GameManager.m_movement.ChangeCost(m_isSelected, m_moveCost);
    }

    public void Deselect()
    {
        m_isSelected = false;
    }
	
}

// Event class that sends information to all the game rule managers
[System.Serializable]
public class HexClickEvent : UnityEvent<bool, Movecost>
{
}
