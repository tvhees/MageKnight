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

            private EffectButton[] effectButtons;
            private int playerID;
            public MovingObject movingObject { get; private set; }

            // Initialisation method for cards owned by players
            public void InitialiseForPlayer(Players.Player player, bool buttonsOn = true)
            {
                playerID = player.id;

                Initialise(player.playerCamera);

                if (buttonsOn)
                {               
                    // Initialise effect buttons
                    Vector3 buttonScale = new Vector3(1.3f, 1.3f, 0.01f);
                    transform.InstantiateChild(tapButtonPrefab, localPosition: new Vector3(0f, -0.15f, -0.05f)).transform.localScale = buttonScale;
                    transform.InstantiateChild(effect1ButtonPrefab, localPosition: new Vector3(0f, -0.15f, -0.05f)).transform.localScale = buttonScale;
                    transform.InstantiateChild(effect2ButtonPrefab, localPosition: new Vector3(0f, -0.15f, -0.05f)).transform.localScale = buttonScale;

                    effectButtons = GetComponentsInChildren<EffectButton>();
                    for (int i = 0; i < effectButtons.Length; i++)
                    {
                        effectButtons[i].Init(playerID);
                    }

                    EnableEffectButtons();
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
                //StartCoroutine(movingObject.SetHomePos(transform.parent.position + movingObject.homePos));

                effectButtons = new EffectButton[0];
            }

            // Activate card effect buttons
            public void EnableEffectButtons()
            {
                for (int i = 0; i < effectButtons.Length; i++)
                {
                    effectButtons[i].Activate();
                }
            }

            public void DisableEffectButtons()
            {
                for (int i = 0; i < effectButtons.Length; i++)
                {
                    effectButtons[i].Deactivate();
                }
            }

            // ****************
            // CARD ID, ART AND HIGHLIGHTING
            // ****************

            public enum Location
            {
                deck,
                hand,
                play,
                discard,
                offer
            }

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
                    case Location.deck:
                        m_spriteRenderer.sprite = m_backSprite;
                        break;
                    case Location.hand:
                    case Location.play:
                    case Location.discard:
                    case Location.offer:
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
                if (location == Location.hand)
                {
                    if (!focused)
                    {
                        StartCoroutine(movingObject.SetTargetPos(movingObject.homePos + GetZoomVector()));
                    }
                }
            }

            void OnMouseExit()
            {
                if (location == Location.hand)
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

                for (int i = 0; i < effectButtons.Length; i++)
                    effectButtons[i].Activate();
            }

            /// <summary>
            /// Unzoom card and return to home position
            /// </summary>
            void SendToHome()
            {
                focused = false;
                StartCoroutine(movingObject.ReturnHome()); // Move back to wherever it came from

                for (int i = 0; i < effectButtons.Length; i++)
                    effectButtons[i].Deactivate();
            }

            // ****************
            // CARD LOCATION
            // ****************
            public Location location { get; private set; }

            public void SetLocation(Location newLocation)
            {
                location = newLocation;
                ChooseSprite();
            }
        }
    }
}