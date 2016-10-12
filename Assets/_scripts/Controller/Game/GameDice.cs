using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using View;
using System.Collections.Generic;

public class GameDice : NetworkBehaviour
{
    public List<ManaId> usedDice = new List<ManaId>();
    ManaPool mana;
    SharedView sharedView;

    public void Enable(ManaPool mana, SharedView sharedView)
    {
        this.mana = mana;
        this.sharedView = sharedView;
        sharedView.RpcEnableDice(mana.DiceTotal);
    }

    [Server]
    public void RollAll()
    {
        for (int i = 0; i < mana.DiceTotal; i++)
        {
            RollDie(i);
        }

        if (!mana.HasEnoughBasicMana())
            RollAll();
    }

    [Server]
    public void DeselectAll()
    {
        for (int i = 0; i < mana.DiceTotal; i++)
            mana.dice[i].selected = false;

        sharedView.RpcDeselectAllDice();
    }

    [Server]
    public void RollDie(int index)
    {
        var manaId = mana.RollDie(index);
        mana.dice[manaId.index] = manaId;
        sharedView.RpcRollDiceColour(manaId);
    }

    [Server]
    public void SetDieValue(ManaId manaId)
    {
        mana.dice[manaId.index] = manaId;
        sharedView.RpcSetDiceColour(manaId);
    }

    [Server]
    public ManaId GetSelected(GameConstants.ManaType colour)
    {
        return mana.GetSelectedDie(colour);
    }
}
