using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace BoardGame
{
    namespace Cards
    {
        public class Object : MonoBehaviour
        {

            // ****************
            // INITIALISATION
            // ****************

            private EffectButton[] m_effectButtons;
            private int m_playerID;
            public MovingObject m_movingObject { get; private set; }

            // Initialisation method for cards owned by players
            public void Init(int playerID, Players.Player.Location startLoc, Camera camera)
            {
                m_playerID = playerID;

                m_spriteRenderer = GetComponent<SpriteRenderer>();
                m_spriteRenderer.sprite = m_faceSprite;

                m_movingObject = GetComponent<MovingObject>();

                SetLocation(startLoc);
                m_Camera = camera;
                StartCoroutine(m_movingObject.SetHomePos(transform.parent.position + m_movingObject.m_homePos));

                // Initialise effect buttons
                m_effectButtons = GetComponentsInChildren<EffectButton>();
                for (int i = 0; i < m_effectButtons.Length; i++)
                    m_effectButtons[i].Init(m_playerID);
            }

            // Initialisation method for cards owned by shared decks
            public void Init(Vector3 homePos, Camera camera)
            {
                m_spriteRenderer = GetComponent<SpriteRenderer>();
                m_spriteRenderer.sprite = m_faceSprite;

                m_movingObject = GetComponent<MovingObject>();

                m_Camera = camera;
                StartCoroutine(m_movingObject.SetHomePos(transform.parent.position + m_movingObject.m_homePos));
            }

            // ****************
            // CARD ID, ART AND HIGHLIGHTING
            // ****************

            // Card type variables
            public int m_ID { get; private set; }

            public void SetID(int newID)
            {
                m_ID = newID;
            }

            // Sprites and renderer
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
                switch (m_location)
                {
                    case Players.Player.Location.deck:
                        m_spriteRenderer.sprite = m_backSprite;
                        break;
                    case Players.Player.Location.hand:
                    case Players.Player.Location.play:
                    case Players.Player.Location.discard:
                        m_spriteRenderer.sprite = m_faceSprite;
                        break;
                }
            }

            // ****************
            // MOUSE INTERACTION
            // ****************

            private bool m_focused;

            void OnMouseOver()
            {
                if (m_location == Players.Player.Location.hand)
                {
                    if (!m_focused)
                    {
                        StartCoroutine(m_movingObject.SetTargetPos(m_movingObject.m_homePos + GetZoomVector()));
                    }
                }
            }

            void OnMouseExit()
            {
                if (m_location == Players.Player.Location.hand)
                {
                    if (!m_focused)
                    {
                        StartCoroutine(m_movingObject.ReturnHome());
                    }
                }
            }

            /// <summary>
            /// Zoom and center a card if clicked to select, return it to the home position if clicked to deselect
            /// </summary>
            void OnMouseUpAsButton()
            {
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
                Vector3 directionToCamera = m_zoomDistance * Vector3.Normalize(m_Camera.transform.position - m_movingObject.m_homePos);
                return directionToCamera;
            }

            /// <summary>
            /// Zoom card to large size and center on screen
            /// </summary>
            void ZoomAndFocus()
            {
                m_focused = true;
                StartCoroutine(m_movingObject.SetTargetPos(m_Camera.transform.position + m_clickDepth)); // Move to center of screen

                for (int i = 0; i < m_effectButtons.Length; i++)
                    m_effectButtons[i].Enable();
            }

            /// <summary>
            /// Unzoom card and return to home position
            /// </summary>
            void SendToHome()
            {
                m_focused = false;
                StartCoroutine(m_movingObject.ReturnHome()); // Move back to wherever it came from

                for (int i = 0; i < m_effectButtons.Length; i++)
                    m_effectButtons[i].Disable();
            }

            // ****************
            // CARD LOCATION
            // ****************
            public Players.Player.Location m_location { get; private set; }

            public void SetLocation(Players.Player.Location newLocation)
            {
                m_location = newLocation;
                ChooseSprite();
            }
        }
    }
}