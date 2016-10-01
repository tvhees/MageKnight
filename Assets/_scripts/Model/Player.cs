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

    public int movement;
    public int influence;

    #region Mana
    public int diceAllowed;
    public int[] mana;
    public int[] crystals;
    #endregion

    public Player(Character character, Cards cards)
    {
        deck = cards.CreateDeck(character.deck);
        hand = new List<CardId>();
        discard = new List<CardId>();
        units = new List<CardId>();
    }

    public void ResetMana()
    {
        mana = new int[6] { 0, 0, 0, 0, 0, 0 };
        diceAllowed = 1;
    }

    public void DrawCards(int numberToDraw)
    {
        for (int i = 0; i < numberToDraw; i++)
        {
            if (deck.Count <= 0)
                break;

            MoveCardToHand(deck.GetFirst());
        }
    }

    public bool ListContainsCard(CardId card, List<CardId> list, bool remove = false)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].identifier == card.identifier)
            {
                if (remove)
                    list.RemoveAt(i);

                return true;
            }
        }

        return false;
    }

    public void RemoveCardFromLists(CardId card)
    {
        if (ListContainsCard(card, hand, remove: true))
            return;

        if (ListContainsCard(card, deck, remove: true))
            return;

        if (ListContainsCard(card, discard, remove: true))
            return;

        if (ListContainsCard(card, units, remove: true))
            return;
    }

    public void MoveCardToHand(CardId card)
    {
        RemoveCardFromLists(card);
        hand.Add(card);
    }

    public void MoveCardToDiscard(CardId card)
    {
        RemoveCardFromLists(card);
        discard.Add(card);
    }

    public void MoveCardToDeck(CardId card)
    {
        RemoveCardFromLists(card);
        deck.Add(card);
    }

    public void MoveCardToUnits(CardId card)
    {
        RemoveCardFromLists(card);
        units.Add(card);
    }
}
