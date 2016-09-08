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
        public GameObject characterSelect;
        public Button[] characterButtons;

        public void UiSelectCharacter()
        {
            string name = EventSystem.current.currentSelectedGameObject.name;
            EventManager.characterSelected.Invoke(name);
        }

        [ClientRpc]
        public void RpcDisableCharacterButton(string name)
        {
            foreach (Button button in characterButtons)
            {
                if (button.name == name)
                    button.interactable = false;
            }
        }

        public void UiEndTurn()
        {
            EventManager.endTurn.Invoke();
        }
    }
}
