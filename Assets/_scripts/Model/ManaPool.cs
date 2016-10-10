using UnityEngine;
using System.Collections;

public class ManaPool {

    public ManaId[] dice;

    #region Properties
    public int DiceTotal {
        get { return dice.Length; }
    }
    #endregion

    #region Constructor
    public ManaPool(GamePlayers players)
    {
        int numberOfDice = players.Connected + 2;
        dice = new ManaId[numberOfDice];
        for (int i = 0; i < numberOfDice; i++)
        {
            dice[i] = new ManaId(i);
        }
    }
    #endregion

    public ManaId RollDie(int i)
    {
        var manaColour = GameConstants.manaColours[Random.Range(0, GameConstants.manaColours.Length)];
        dice[i].colour = manaColour;

        return new ManaId(i, manaColour);
    }

    public bool HasEnoughBasicMana()
    {
        int basicMana = 0;
        foreach (var die in dice)
        {
            if (!(die.colour == GameConstants.ManaType.Black || die.colour == GameConstants.ManaType.Gold))
                basicMana++;
        }

        return basicMana >= Mathf.CeilToInt(dice.Length/2f);
    }

    public ManaId GetSelectedDie(GameConstants.ManaType colour)
    {
        for (int i = 0; i < DiceTotal; i++)
        {
            if (dice[i].selected)
                if (dice[i].colour == colour)
                    return dice[i];
        }
        return new ManaId(-1);
    }
}
