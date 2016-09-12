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
        public GameObject[] playerNames;
        public Text currentPhase;
        public Button[] buttons;        

        void Awake()
        {
            EventManager.stateChanged.AddListener(OnStateChanged);
        }

        void OnStateChanged(GameObject newState)
        {
            currentPhase.text = newState.name;
        }

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

        public void SetPlayerName(int playerId, string playerName)
        {
            playerNames[playerId].GetComponentInChildren<Text>().text = playerName;
        }

        public void SetPlayerColour(int playerId, Color colour)
        {
            var img = playerNames[playerId].GetComponent<Image>();
            float alpha = img.color.a;
            img.color = new Color(colour.r, colour.g, colour.b, alpha);
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
