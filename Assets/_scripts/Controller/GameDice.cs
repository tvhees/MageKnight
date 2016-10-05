using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using View;

public class GameDice : NetworkBehaviour
{
    private ManaPool mana;
    private SharedView sharedView;

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
            var manaId = mana.RollDie(i);
            mana.dice[manaId.index] = manaId;
            sharedView.RpcRollDiceColour(manaId);
        }

        if (!mana.HasEnoughBasicMana())
            RollAll();
    }

    [Command]
    public void CmdSetDieValue(ManaId manaId)
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
