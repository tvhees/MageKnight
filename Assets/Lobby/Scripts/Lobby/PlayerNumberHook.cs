using UnityEngine;
using UnityEngine.Networking;

namespace Prototype.NetworkLobby
{
    public class PlayerNumberHook : LobbyHook
    {
        public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
        {
            var player = gamePlayer.GetComponent<PlayerControl>();
            var lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
            player.playerId = lobby.playerId;
            player.playerName = lobby.playerName;
        }
    }
}