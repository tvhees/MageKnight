using UnityEngine;
using System;
using System.Collections.Generic;
using Other.Data;

public class Player
{
    public List<CardId> hand;
    public List<CardId> deck;
    public List<CardId> discard;
    public List<CardId> units;

    public GameObject currentTile;

    public int movement;

    public Player(Character character, Cards cards)
    {
        deck = cards.CreateDeck(character.deck);
        hand = new List<CardId>();
        discard = new List<CardId>();
        units = new List<CardId>();
    }

    public void DrawCards(int numberToDraw)
    {
        for (int i = 0; i < numberToDraw; i++)
        {
            if (deck.Count <= 0)
                break;

            hand.Add(deck.GetFirst(remove: true));
        }
    }
}
