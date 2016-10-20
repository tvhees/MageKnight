using UnityEngine;
using System;
using System.Collections.Generic;
using Other.Data;
using Commands;

public class Player
{

    #region Card variables

    public List<CardId> hand;
    public List<CardId> deck;
    public List<CardId> discard;
    public List<CardId> units;
    public List<CardId> play;
    public int handSize;
    public CardId tacticId;
    public bool tacticIsActive;

    public int movement;
    public int influence;

    #endregion Card variables

    #region Mana variables

    public int diceAllowed;
    public int[] mana;
    public int[] crystals;

    public bool HasGold { get { return mana[4] > 0; } }
    public bool HasBlack { get { return mana[5] > 0; } }

    #endregion Mana variables

    #region Properties

    public bool CanUseDice { get { return diceAllowed > 0; } }

    #endregion Properties

    #region Constructor

    public Player(Character character, Cards cards, int handSize = 5)
    {
        deck = cards.CreateDeck(character.deck);
        hand = new List<CardId>();
        discard = new List<CardId>();
        units = new List<CardId>();
        play = new List<CardId>();
        this.handSize = handSize;
        ResetMana();
        ResetVariables();
    }

    #endregion

    #region Mana management

    public bool HasMana(GameConstants.ManaType colour)
    {
        return mana[(int)colour] > 0;
    }

    public void DieToggled(ManaId manaId)
    {
        if (manaId.selected)
        {
            diceAllowed--;
            AddMana(manaId.colour);
        }
        else
        {
            diceAllowed++;
            AddMana(manaId.colour, subtract: true);
        }
    }

    public void AddMana(GameConstants.ManaType colour, bool subtract = false)
    {
        if (subtract)
            mana[(int)colour]--;
        else
            mana[(int)colour]++;
    }

    #endregion Mana management

    #region Card management

    bool ListContainsCard(CardId card, List<CardId> list, bool remove = false)
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

        if (ListContainsCard(card, play, remove: true))
            return;

        if (ListContainsCard(card, deck, remove: true))
            return;

        if (ListContainsCard(card, discard, remove: true))
            return;

        if (ListContainsCard(card, units, remove: true))
            return;
    }

    public void MoveCardToLimbo(CardId card)
    {
        RemoveCardFromLists(card);
    }

    public void MoveCardToHand(CardId card)
    {
        RemoveCardFromLists(card);
        hand.Add(card);
    }

    public void MoveCardToPlay(CardId card)
    {
        RemoveCardFromLists(card);
        play.Add(card);
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

    #endregion Card management

    #region Reset methods

    public void ServerEndTurn(PlayerControl player)
    {
        DiscardPlayedCards(player);
        ResetMana();
        ResetVariables();
    }

    void DiscardPlayedCards(PlayerControl player)
    {
        var cardsInPlay = play.Count;
        for (int i = 0; i < cardsInPlay; i++)
        {
            player.ServerMoveCard(play[0], GameConstants.Location.Discard);
        }
    }

    public void ResetMana()
    {
        mana = new int[] { 0, 0, 0, 0, 0, 0 };
        diceAllowed = 1;
    }

    void ResetVariables()
    {
        movement = 0;
        influence = 0;
    }

#endregion Reset methods
}
