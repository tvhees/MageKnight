using UnityEngine;
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
        public GameObject manaPanel;
        public DieView[] manaDice;

        #region Initialise
        void Awake()
        {
            EventManager.stateChanged.AddListener(OnStateChanged);
        }

        void OnStateChanged(GameState newState)
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

        public void TogglePlayerHighlight(int playerId, bool on)
        {
            var display = GetTurnOrderDisplay(playerId);
            if(on)
                display.SetHighlights(highlightColour, currentPlayerIndicator);
            else
                display.SetHighlights(Color.black);
        }

        [ClientRpc]
        public void RpcEnableDice(int numberOfDice)
        {
            for (int i = 0; i < numberOfDice; i++)
            {
                manaDice[i].gameObject.SetActive(true);
            }
        }

        [ClientRpc]
        public void RpcSetDiceColour(ManaId id)
        {
            manaDice[id.index].SetColour(id.colour);
        }

        [ClientRpc]
        public void RpcRollDiceColour(ManaId id)
        {
            manaDice[id.index].SetColour(id.colour, animate: true);
        }

        public void ToggleDice(bool interactible)
        {
            foreach (var die in manaDice)
            {
                if (!die.selected)
                    die.button.interactable = interactible;
            }
        }

        [ClientRpc]
        public void RpcMoveDieToPlay(ManaId manaId)
        {
            var die = manaDice[manaId.index];
            die.Enable(false);

            die.MoveToNewParent(PlayerControl.current.view.play.transform);
        }

        [ClientRpc]
        public void RpcMoveDieToPool(ManaId manaId)
        {
            var die = manaDice[manaId.index];
            die.Enable(true);
            die.MoveToNewParent(manaPanel.transform);
            die.transform.SetSiblingIndex(manaId.index);
        }
    }
}
