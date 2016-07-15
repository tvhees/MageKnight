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

            public GameObject tapButtonPrefab;
            public GameObject effect1ButtonPrefab;
            public GameObject effect2ButtonPrefab;

            private EffectButton[] m_effectButtons;
            private int m_playerID;
            public MovingObject movingObject { get; private set; }

            // Initialisation method for cards owned by players
            public void InitialiseForPlayer(int playerID, Players.Player.Location startLoc, Camera camera, bool buttonsOn = true)
            {
                m_playerID = playerID;

                Initialise(camera);

                SetLocation(startLoc);

                // Initialise effect buttons
                Vector3 buttonScale = new Vector3(1.3f, 1.3f, 0.01f);
                transform.InstantiateChild(tapButtonPrefab, localPosition: new Vector3(0f, -0.15f, -0.05f)).transform.localScale = buttonScale;
                transform.InstantiateChild(effect1ButtonPrefab, localPosition: new Vector3(0f, -0.15f, -0.05f)).transform.localScale = buttonScale;
                transform.InstantiateChild(effect2ButtonPrefab, localPosition: new Vector3(0f, -0.15f, -0.05f)).transform.localScale = buttonScale;

                m_effectButtons = GetComponentsInChildren<EffectButton>();

                for (int i = 0; i < m_effectButtons.Length; i++)
                {
                    if (buttonsOn)
                    {
                        m_effectButtons[i].gameObject.SetActive(true);
                        m_effectButtons[i].Init(m_playerID);
                    }
                    else
                        m_effectButtons[i].gameObject.SetActive(false);
                }
            }

            // Initialisation method for cards owned by shared decks
            public void Initialise(Camera camera)
            {
                m_spriteRenderer = GetComponent<SpriteRenderer>();
                m_spriteRenderer.sprite = m_faceSprite;

                movingObject = GetComponent<MovingObject>();
                movingObject.SetSpeed(40);

                m_Camera = camera;
                StartCoroutine(movingObject.SetHomePos(transform.parent.position + movingObject.homePos));
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
                switch (location)
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

            private bool focused;

            void OnMouseOver()
            {
                if (location == Players.Player.Location.hand)
                {
                    if (!focused)
                    {
                        StartCoroutine(movingObject.SetTargetPos(movingObject.homePos + GetZoomVector()));
                    }
                }
            }

            void OnMouseExit()
            {
                if (location == Players.Player.Location.hand)
                {
                    if (!focused)
                    {
                        StartCoroutine(movingObject.ReturnHome());
                    }
                }
            }

            /// <summary>
            /// Zoom and center a card if clicked to select, return it to the home position if clicked to deselect
            /// </summary>
            void OnMouseUpAsButton()
            {
                if (!focused)
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
                Vector3 directionToCamera = m_zoomDistance * Vector3.Normalize(m_Camera.transform.position - movingObject.homePos);
                return directionToCamera;
            }

            /// <summary>
            /// Zoom card to large size and center on screen
            /// </summary>
            void ZoomAndFocus()
            {
                focused = true;
                StartCoroutine(movingObject.SetTargetPos(m_Camera.transform.position + m_clickDepth)); // Move to center of screen

                for (int i = 0; i < m_effectButtons.Length; i++)
                    m_effectButtons[i].Enable();
            }

            /// <summary>
            /// Unzoom card and return to home position
            /// </summary>
            void SendToHome()
            {
                focused = false;
                StartCoroutine(movingObject.ReturnHome()); // Move back to wherever it came from

                for (int i = 0; i < m_effectButtons.Length; i++)
                    m_effectButtons[i].Disable();
            }

            // ****************
            // CARD LOCATION
            // ****************
            public Players.Player.Location location { get; private set; }

            public void SetLocation(Players.Player.Location newLocation)
            {
                location = newLocation;
                ChooseSprite();
            }
        }
    }
}