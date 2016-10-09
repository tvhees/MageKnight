using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Prototype.NetworkLobby;
using View;
using Other.Data;

public class GamePlayers : NetworkBehaviour
{
    private int nextIndex;
    private int expected;

    public PlayerControl local { get; private set; }
    public PlayerControl current { get; private set; }
    private List<PlayerControl> list = new List<PlayerControl>();
    private PlayerControl[] nextOrder;
    

    #region properties
    public List<PlayerControl> List { get { return list; } }

    public int Expected {
        get {
            if (expected <= 0)
                expected = CalculateExpectedPlayers();
            return expected; }
    }

    public int Total {
        get { return list.Count; }
    }

    public bool AllConnected {
        get { return Total >= Expected; }
    }

    public bool OnLastForRound {
        get { return nextIndex >= Total; }
    }
    #endregion

    #region Connection management
    [Server]
    int CalculateExpectedPlayers()
    {
        int i = 0;
        foreach (NetworkLobbyPlayer player in LobbyManager.s_Singleton.lobbySlots)
        {
            if (player != null)
                i++;
        }
        return i;
    }

    [Server]
    public void Add(PlayerControl player)
    {
        list.Add(player);
    }

    [Client]
    public void SetLocal(PlayerControl player)
    {
        local = player;
    }
    #endregion

    #region Turn Order
    public void ClearNextOrder()
    {
        nextOrder = new PlayerControl[6];
    }

    public void AddCurrentToNextOrder(int index)
    {
        nextOrder[index] = current;

        if (OnLastForRound)
            SetNewOrder();
            
        MoveToNext();
    }

    [Server]
    public void RandomiseOrder()
    {
        nextOrder = list.ToArray();
        nextOrder.Shuffle();
        SetNewOrder();
        MoveToNext();
    }
    
    [Server]
    public void SetNewOrder()
    {
        list.Clear();
        for (int i = 0; i < nextOrder.Length; i++)
        {
            if (nextOrder[i] == null)
                continue;

            nextOrder[i].RpcMoveToIndexInTurnOrder(list.Count);
            list.Add(nextOrder[i]);
        }

        nextIndex = 0;
    }

    [Server]
    public void MoveToNext()
    {
        if (current != null)
        {
            current.RpcYourTurn(false);
            current.view.RpcEnableEndTurn(false);
        }

        // Wrap index to 0 because Mathf.Repeat doesn't take integers
        if (nextIndex >= list.Count)
            nextIndex = 0;
        current = list[nextIndex];
        current.RpcYourTurn(true);
        current.view.RpcEnableEndTurn(true);

        nextIndex++;
    }

    [Client]
    public void SetCurrent(PlayerControl player)
    {
        current = player;
    }

    [Server]
    public void EndTurn()
    {
        current.ServerRefillHand();
        current.model.EndTurn(current);
        MoveToNext();
    }
    #endregion

    #region Characters and Tactics
    [Server]
    public void AssignCharacter()
    {
        // The first tactic selection proceeds in reverse order of character selection
        AddCurrentToNextOrder(list.Count - nextIndex);
    }

    [Server]
    public void AssignTactic(Cards cards, Card tactic)
    {
        current.AssignChosenTactic(cards, tactic);
        AddCurrentToNextOrder(tactic.number);
    }
    #endregion
}
