using UnityEngine;
using System;
using Other.Data;

public struct CardId
{
    public string name;
    public int identifier;
    public string cardBackName;
    public GameConstants.Location location;

	public CardId(string name, int identifier, string cardBackName = "cardback", GameConstants.Location location = GameConstants.Location.Deck)
	{
        this.name = name;
        this.identifier = identifier;
        this.cardBackName = cardBackName;
        this.location = location;
	}
}
