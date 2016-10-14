using UnityEngine;
using System;
using Other.Data;

public struct CardId
{
    public string name;
    public int identifier;
    public string cardBackName;

	public CardId(string name, int identifier, string cardBackName = "cardback")
	{
        this.name = name;
        this.identifier = identifier;
        this.cardBackName = cardBackName;
	}
}
