using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace BoardGame
{
    namespace Card
    {
        public class Object : MonoBehaviour
        {

            // ****************
            // INITIALISATION
            // ****************

            public GameObject tapButtonPrefab;
            public GameObject effect1ButtonPrefab;
            public GameObject effect2ButtonPrefab;

            public Canvas buttonCanvas;
            private Effect.CardButton[] effectButtons;
            public Players.Player owningPlayer { get; private set; }
            public MovingObject movingObject { get; private set; }

            // Initialisation method for cards owned by shared decks
            public void Awake()
            {
                // Set the card's movement properties
                movingObject = GetComponent<MovingObject>();
                movingObject.SetSpeed(40);

                // Add no effects by default
                effectButtons = new Effect.CardButton[0];
            }

            public void AddEffectButtons(Players.Player player, Dictionary<string, string> cardInfo)
            {
                owningPlayer = player;

                buttonCanvas.worldCamera = player.playerCamera; // Set it so that the buttons will register clicks through this player's camera

                GameObject[] buttonPrefabs = new GameObject[3] { tapButtonPrefab, effect1ButtonPrefab, effect2ButtonPrefab };

                for (int i = 0; i < buttonPrefabs.Length; i++)
                {
                    string effectKey = "effect_" + i.ToString();
                    string choiceKey = "choice_" + i.ToString();
                    if (cardInfo.ContainsKey(effectKey) || cardInfo.ContainsKey(choiceKey))
                    {
                        Effect.CardButton effectButton = buttonCanvas.transform.InstantiateChild(buttonPrefabs[i]).GetComponent<Effect.CardButton>();
                        effectButton.GetComponent<RectTransform>().Reset();

                        effectButton.AddUnityActionsForButton(cardInfo, i);
                        effectButton.AddEffectCostByType(cardType, i, cardColour);
                        effectButton.AddCleanupMethod();
                    }

                }

                effectButtons = GetComponentsInChildren<Effect.CardButton>();
                for (int i = 0; i < effectButtons.Length; i++)
                {
                    effectButtons[i].Init(player);
                }
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
                offer,
                throwAway
            }

            // Card type variables
            private string stringName;
            public string cardName
                {
                    get { return stringName; }
                    set { stringName = value; }
                }
            private string stringType;
            public string cardType
            {
                get { return stringType; }
                set { stringType = value; }
            }
            private string stringColour;
            public string cardColour
            {
                get { return stringColour; }
                set { stringColour = value; }
            }

            // Sprites and renderer
            private SpriteRenderer spriteRenderer;
            private Sprite faceSprite;
            private Sprite backSprite;

            public void AddSprites(Sprite faceSprite, Sprite backSprite)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
                this.faceSprite = faceSprite;
                this.backSprite = backSprite;
            }

            public void ChooseSprite()
            {
                switch (location)
                {
                    case Location.deck:
                        spriteRenderer.sprite = backSprite;
                        break;
                    case Location.hand:
                    case Location.play:
                    case Location.discard:
                    case Location.offer:
                        spriteRenderer.sprite = faceSprite;
                        break;
                }
            }

            // ****************
            // MOUSE INTERACTION
            // ****************

            private bool focused;

            void MouseEntered()
            {
                if (location == Location.hand)
                {
                    if (!focused)
                    {
                        StartCoroutine(movingObject.SetTargetPos(movingObject.homePos + GetZoomVector()));
                    }
                }
            }

            void MouseExited()
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
            void MouseClicked()
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
            private Camera camera;
            private Vector3 m_clickDepth = new Vector3(0f, 0f, 3f);
            private float m_zoomDistance = 2f;

            public void AddCamera(Camera camera)
            {
                this.camera = camera;
            }

            /// <summary>
            /// Returns a vector of length zoomDistance in the direction of the camera showing this card
            /// </summary>
            /// <returns></returns>
            private Vector3 GetZoomVector()
            {
                Vector3 directionToCamera = m_zoomDistance * Vector3.Normalize(camera.transform.position - movingObject.homePos);
                return directionToCamera;
            }

            /// <summary>
            /// Zoom card to large size and center on screen
            /// </summary>
            void ZoomAndFocus()
            {
                if (location != Location.deck)
                {
                    focused = true;
                    StartCoroutine(movingObject.SetTargetPos(camera.transform.position + m_clickDepth)); // Move to center of screen

                    if (location == Location.hand)
                        EnableEffectButtons();
                }
            }

            /// <summary>
            /// Unzoom card and return to home position
            /// </summary>
            void SendToHome()
            {
                focused = false;
                DisableEffectButtons();
                StartCoroutine(movingObject.ReturnHome()); // Move back to wherever it came from
            }

            // ****************
            // CARD LOCATION
            // ****************
            public Location location { get; private set; }

            public void SetLocation(Location newLocation)
            {
                location = newLocation;
                ChooseSprite();
                DisableEffectButtons();
            }
        }
    }
}