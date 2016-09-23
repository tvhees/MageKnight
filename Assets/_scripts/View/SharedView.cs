﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using System.Collections;
using Other.Data;

namespace View
{
    public class SharedView : NetworkBehaviour
    {
        public TurnOrderDisplay[] turnOrderDisplays;
        public GameObject currentPlayerIndicator;
        public Text currentPhase;
        public Button[] buttons;
        public Color highlightColour;

        #region Initialise
        void Awake()
        {
            EventManager.stateChanged.AddListener(OnStateChanged);
        }

        void OnStateChanged(GameObject newState)
        {
            currentPhase.text = newState.name;
        }
        #endregion

        #region UiMethods
        public void UiSelectCharacter()
        {
            string name = EventSystem.current.currentSelectedGameObject.name;
            EventManager.characterSelected.Invoke(name);
        }

        public void UiSelectTactic()
        {
            string name = EventSystem.current.currentSelectedGameObject.name;
            EventManager.tacticSelected.Invoke(name);
        }

        public void UiSelectView(TurnOrderDisplay selectedDisplay)
        {
            foreach (var display in turnOrderDisplays)
            {
                if (display != selectedDisplay)
                    if(display.gameObject.activeSelf == true)
                        display.Select(false);
            }

            selectedDisplay.Select(true);
        }

        public void UiEndTurn()
        {
            EventManager.endTurn.Invoke();
        }
        #endregion

        public TurnOrderDisplay GetTurnOrderDisplay(int playerId)
        {
            return turnOrderDisplays[playerId];
        }

        [ClientRpc]
        public void RpcDisableButton(string name)
        {
            foreach (Button button in buttons)
            {
                if (button.name == name)
                    button.interactable = false;
            }
        }

        [ClientRpc]
        public void RpcStopHighlightingPlayer(int playerId)
        {
            var display = GetTurnOrderDisplay(playerId);
            display.SetHighlights(Color.black);
        }

        [ClientRpc]
        public void RpcHighlightPlayer(int playerId)
        {
            var display = GetTurnOrderDisplay(playerId);
            display.SetHighlights(highlightColour, currentPlayerIndicator);
        }
    }
}
