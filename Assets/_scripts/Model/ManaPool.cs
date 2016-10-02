using UnityEngine;
using System.Collections;

public class ManaPool {

    public GameConstants.ManaType[] dice;

    public int diceAvailable;

    public ManaPool(int numberOfPlayers)
    {
        int numberOfDice = numberOfPlayers + 2;
        dice = new GameConstants.ManaType[numberOfDice];
    }

    public bool HasEnoughBasicMana()
    {
        int basicMana = 0;
        foreach (var die in dice)
        {
            if (!(die == GameConstants.ManaType.Black || die == GameConstants.ManaType.Gold))
                basicMana++;
        }

        return basicMana >= Mathf.CeilToInt(dice.Length/2f);
    }
}
