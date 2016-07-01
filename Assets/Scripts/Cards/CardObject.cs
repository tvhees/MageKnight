using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CardObject : MonoBehaviour {

    // ****************
    // INITIALISATION
    // ****************

    private EffectButton[] effectButtons;

    public void Init(Location startLoc, Vector3 homePos, Camera camera)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = faceSprite;
        location = startLoc;
        m_Camera = camera;
        m_targetPos = m_homePos = homePos;

        // Get references to effect colliders and initialise them
        effectButtons = GetComponentsInChildren<EffectButton>();
        effectButtons[0].Init();
        effectButtons[1].Init();
    }

    // ****************
    // CARD ID AND HIGHLIGHTING
    // ****************

    // Card type variables
    private int cardID;

    public void SetID(int newID)
    {
        cardID = newID;
    }

    public int GetID()
    {
        return cardID;
    }

    private SpriteRenderer spriteRenderer;
    private Sprite faceSprite;
    private Sprite backSprite;

    public void SetSprites(Sprite fSprite, Sprite bSprite)
    {
        faceSprite = fSprite;
        backSprite = bSprite;
    }

    // Listener method for any events that want to highlight this card.
    public void Highlight()
    {
        if(location == Location.hand)
            Debug.Log("Highlighting Card " + cardID);
    }

    // ****************
    // MOUSE INTERACTION
    // ****************

    private bool m_focused;

    void OnMouseOver()
    {
        if(!m_focused)
            SetTargetPos(m_homePos + GetZoomVector());
    }

    void OnMouseExit()
    {
        if(!m_focused)
            SetTargetPos(m_homePos);
    }

    /// <summary>
    /// Zoom and center a card if clicked to select, return it to the home position if clicked to deselect
    /// </summary>
    void OnMouseUpAsButton()
    {
        if (!m_focused)
            ZoomAndFocus();
        else
            SendToHome();
            
    }

    // ****************
    // CARD MOVEMENT
    // ****************

    // Vectors for card position 
    private Vector3 m_homePos;
    private Vector3 m_clickPos = new Vector3(0f, 2f, -7f);
    private Vector3 m_targetPos;

    // Variables for zooming towards camera on hover
    private Camera m_Camera;
    private float m_zoomDistance = 2f;
    private float m_zoomSpeed = 100f;

    // Variables for hand positioning
    private float shiftDistance = 0.7f;

    /// <summary>
    /// Returns a vector of length zoomDistance in the direction of the camera showing this card
    /// </summary>
    /// <returns></returns>
    Vector3 GetZoomVector()
    {
        Vector3 directionToCamera = m_zoomDistance * Vector3.Normalize(m_Camera.transform.position - m_homePos);
        return directionToCamera;
    }

    void SetTargetPos(Vector3 newPos)
    {
        m_targetPos = newPos;
    }

    /// <summary>
    /// Move a card within the player's hand to keep the hand neat and visible as cards are added or removed.
    /// </summary>
    public void ShiftInHand(Vector3 pos, float shiftDistance)
    {
        if (location == Location.hand)
        {
            m_targetPos = m_homePos = Vector3.MoveTowards(m_homePos, pos, shiftDistance);
        }
    }

    /// <summary>
    /// Zoom card to large size and center on screen
    /// </summary>
    void ZoomAndFocus()
    {
        m_focused = true;
        SetTargetPos(m_clickPos); // Move to center of screen

        effectButtons[0].Enable();
        effectButtons[1].Enable();
    }

    /// <summary>
    /// Unzoom card and return to home position
    /// </summary>
    void SendToHome()
    {
        m_focused = false;
        SetTargetPos(m_homePos); // Move back to wherever it came from

        effectButtons[0].Disable();
        effectButtons[1].Disable();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, m_targetPos, m_zoomSpeed * Time.deltaTime);
    }

    // ****************
    // CARD LOCATION
    // ****************

    public enum Location
    {
        deck,
        hand,
        discard
    }

    [SerializeField]
    private Location location;

    public void SetLocation(Location newLocation)
    {
        location = newLocation;
    }

    public Location GetLocation()
    {
        return location;
    }
}
