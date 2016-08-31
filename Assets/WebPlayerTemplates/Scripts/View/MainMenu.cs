using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    public class MainMenu: MonoBehaviour 
	{
        public GameObject soloGameButton;
        public GameObject multiGameButton;

        public void PlayerCountSelected(Dropdown dropdown)
        {
            if (dropdown.value > 0)
            {
                soloGameButton.SetActive(false);
                multiGameButton.SetActive(true);
            }
            else
            {
                soloGameButton.SetActive(true);
                multiGameButton.SetActive(false);
            }
        }

        public void ScenarioSelected(Text text)
        {
            Main.Instance.ChooseScenario(text);
        }

        public void HostLobby()
        {
//            Main.Instance.Open();
            NetworkManager.singleton.StartHost();
        }

        public void JoinLobby()
        {
//            Main.Instance.Open();
            NetworkManager.singleton.StartClient();
        }
	}
}