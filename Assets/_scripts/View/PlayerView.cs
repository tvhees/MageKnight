using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using System.Collections;
using Other.Data;

namespace View
{
    public class PlayerView : NetworkBehaviour
    {
        public TurnOrderDisplay[] turnOrderDisplays;
        public Text currentPhase;
        public Button[] buttons;

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

        public void UiEndTurn()
        {
            EventManager.endTurn.Invoke();
        }
        #endregion

        public TurnOrderDisplay GetTurnOrderDisplay(int playerId)
        {
            return turnOrderDisplays[playerId];
        }

        public void SetPlayerName(int playerId, string playerName)
        {
            turnOrderDisplays[playerId].GetComponentInChildren<Text>().text = playerName;
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
    }
}
