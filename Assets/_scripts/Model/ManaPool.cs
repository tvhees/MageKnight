using UnityEngine;
using System.Collections;

public class ManaPool {

    public ManaId[] dice;

    public int diceAvailable;

    public ManaPool(GamePlayers players)
    {
        int numberOfDice = players.Total + 2;
        dice = new ManaId[numberOfDice];
        for (int i = 0; i < numberOfDice; i++)
        {
            dice[i] = new ManaId(i);
        }
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
        for (int i = 0; i < dice.Length; i++)
        {
            if (dice[i].selected)
                if (dice[i].colour == colour)
                    return dice[i];
        }

        return new ManaId(-1);
    }
}
