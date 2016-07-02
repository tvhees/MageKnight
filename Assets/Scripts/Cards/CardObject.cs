using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CardObject : MonoBehaviour {

    // ****************
    // INITIALISATION
    // ****************

    private EffectButton[] effectButtons;

    public void Init(PlayerManager.Location startLoc, Vector3 homePos, Camera camera)
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_spriteRenderer.sprite = m_faceSprite;
        SetLocation(startLoc);
        m_Camera = camera;
        m_targetPos = m_homePos = transform.parent.position + homePos;

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

    private SpriteRenderer m_spriteRenderer;
    private Sprite m_faceSprite;
    private Sprite m_backSprite;

    public void SetSprites(Sprite fSprite, Sprite bSprite)
    {
        m_faceSprite = fSprite;
        m_backSprite = bSprite;
    }

    public void ChooseSprite()
    {
        switch (location)
        {
            case PlayerManager.Location.deck:
                m_spriteRenderer.sprite = m_backSprite;
                break;
            case PlayerManager.Location.hand:
            case PlayerManager.Location.play:
            case PlayerManager.Location.discard:
                m_spriteRenderer.sprite = m_faceSprite;
                break;
        }
    }

    // Listener method for any events that want to highlight this card.
    public void Highlight()
    {
        if(location == PlayerManager.Location.hand)
            Debug.Log("Highlighting Card " + cardID);
    }

    // ****************
    // MOUSE INTERACTION
    // ****************

    private bool m_focused;

    void OnMouseOver()
    {
        if (location == PlayerManager.Location.hand)
        {
            if (!m_focused)
            {
                SetTargetPos(m_homePos + GetZoomVector());
            }
        }
    }

    void OnMouseExit()
    {
        if (location == PlayerManager.Location.hand)
        {
            if (!m_focused)
            {
                SetTargetPos(m_homePos);
            }
        }
    }

    /// <summary>
    /// Zoom and center a card if clicked to select, return it to the home position if clicked to deselect
    /// </summary>
    void OnMouseUpAsButton()
    {
        GameManager.m_cardManager.MoveThisCard(this);

        if (!m_focused)
        {
            ZoomAndFocus();
        }
        else
        {
            SendToHome();
        }
            
    }

    // ****************
    // CARD MOVEMENT
    // ****************

    // Vectors for card position 
    private Vector3 m_homePos;
    private Vector3 m_targetPos;

    // Variables for zooming towards camera on hover and click
    private Camera m_Camera;
    private Vector3 m_clickDepth = new Vector3(0f, 0f, 3f);
    private float m_zoomDistance = 2f;

    /// <summary>
    /// Returns a vector of length zoomDistance in the direction of the camera showing this card
    /// </summary>
    /// <returns></returns>
    private Vector3 GetZoomVector()
    {
        Vector3 directionToCamera = m_zoomDistance * Vector3.Normalize(m_Camera.transform.position - m_homePos);
        return directionToCamera;
    }

    public void SetTargetPos(Vector3 newPos)
    {
        GameManager.m_cardManager.MoveThisCard(this);
        m_targetPos = newPos;
    }

    public Vector3 GetTargetPos()
    {
        return m_targetPos;
    }

    public void SetHomePos(Vector3 newHomePos)
    {
        m_homePos = newHomePos;
        SetTargetPos(m_homePos);
    }

    public Vector3 GetHomePos()
    {
        return m_homePos;
    }

    /// <summary>
    /// Zoom card to large size and center on screen
    /// </summary>
    void ZoomAndFocus()
    {
        m_focused = true;
        SetTargetPos(m_Camera.transform.position + m_clickDepth); // Move to center of screen

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

    // ****************
    // CARD LOCATION
    // ****************
    private PlayerManager.Location location;

    public void SetLocation(PlayerManager.Location newLocation)
    {
        location = newLocation;
        ChooseSprite();
    }

    public PlayerManager.Location GetLocation()
    {
        return location;
    }
}
