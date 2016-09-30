using UnityEngine;
using System.Collections;

public class ManaPool {

    public GameConstants.ManaType[] dice;

    public ManaPool(int numberOfPlayers)
    {
        int numberOfDice = numberOfPlayers + 2;
        dice = new GameConstants.ManaType[numberOfDice];
    }
}
