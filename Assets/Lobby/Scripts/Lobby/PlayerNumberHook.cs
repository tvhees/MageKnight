using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace Prototype.NetworkLobby
{
    public class PlayerNumberHook : LobbyHook
    {
        public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
        {
            gamePlayer.GetComponent<PlayerControl>().playerId = lobbyPlayer.GetComponent<LobbyPlayer>().playerId;
        }
    }
}