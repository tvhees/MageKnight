using Other.Data;
using Prototype.NetworkLobby;
using System.Collections.Generic;
using UnityEngine.Networking;

public class GamePlayers
{
    int nextIndex;
    int expected;

    List<PlayerControl> currentOrder = new List<PlayerControl>();
    PlayerControl[] nextOrder;

    #region properties

    public List<PlayerControl> List
    {
        get { return currentOrder; }
    }

    public bool OnLastForRound
    {
        get { return nextIndex >= Connected; }
    }

    public int Connected
    {
        get { return currentOrder.Count; }
    }

    public bool AllConnected
    {
        get { return Connected >= Expected; }
    }

    int Expected
    {
        get
        {
            if (expected <= 0) expected = ServerCalculateExpectedPlayers();
            return expected;
        }
    }
    #endregion properties

    #region Connection management

    public void ServerAdd(PlayerControl player)
    {
        currentOrder.Add(player);
    }

    int ServerCalculateExpectedPlayers()
    {
        int i = 0;
        foreach (NetworkLobbyPlayer player in LobbyManager.s_Singleton.lobbySlots)
        {
            if (player != null)
                i++;
        }
        return i;
    }
    #endregion Connection management

    #region Turn Order
    public void ServerRandomiseOrder()
    {
        nextOrder = currentOrder.ToArray();
        nextOrder.Shuffle();
        ServerSetNewOrder();
        ServerMoveToNext();
    }

    void ServerSetNewOrder()
    {
        currentOrder.Clear();
        for (int i = 0; i < nextOrder.Length; i++)
        {
            if (nextOrder[i] == null)
                continue;

            nextOrder[i].RpcMoveToIndexInTurnOrder(currentOrder.Count);
            currentOrder.Add(nextOrder[i]);
        }

        nextIndex = 0;
    }

    void ServerMoveToNext()
    {
        if (PlayerControl.current != null)
        {
            PlayerControl.current.RpcNewTurn(false);
            PlayerControl.current.view.RpcEnableEndTurn(false);
            PlayerControl.current.view.RpcEnableUndo(false);
        }

        // Wrap index to 0 because Mathf.Repeat doesn't take integers
        if (nextIndex >= currentOrder.Count)
            nextIndex = 0;
        PlayerControl.current = currentOrder[nextIndex];
        PlayerControl.current.RpcNewTurn(true);
        PlayerControl.current.view.RpcEnableEndTurn(true);

        nextIndex++;
    }

    public void ServerEndTurn()
    {
        PlayerControl.current.ServerRefillHand();
        PlayerControl.current.ServerEndTurn();
        ServerMoveToNext();
    }

    public void ServerClearNextOrder()
    {
        nextOrder = new PlayerControl[6];
    }
    #endregion Turn Order

    #region Characters and Tactics

    public void ServerAssignCharacter()
    {
        // The first tactic selection proceeds in reverse order of character selection
        ServerAddCurrentToNextOrder(currentOrder.Count - nextIndex);
    }

    public void ServerAssignTactic(Cards cards, Card tactic)
    {
        PlayerControl.current.AssignChosenTactic(cards, tactic);
        ServerAddCurrentToNextOrder(tactic.number);
    }

    void ServerAddCurrentToNextOrder(int index)
    {
        nextOrder[index] = PlayerControl.current;

        if (OnLastForRound)
            ServerSetNewOrder();

        ServerMoveToNext();
    }

    #endregion Characters and Tactics
}