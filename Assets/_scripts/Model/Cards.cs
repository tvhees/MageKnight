using UnityEngine;
using System;
using System.Collections.Generic;
using Other.Data;
using Other.Utility;

public class Cards
{
    public List<CardId> actions;
    public List<CardId> spells;
    public List<CardId> artifacts;
    public List<CardId> wounds;
    public List<CardId> commonUnits;
    public List<CardId> eliteUnits;
    public List<CardId> dayTactics;
    public List<CardId> nightTactics;

    public Cards(GamePlayers players)
    {
        CreateCommonDecks();
        CreatePlayerDecks(players);
    }

    void CreateCommonDecks()
    {
        actions = CreateDeck(DeckDatabase.GetScriptableObject("AdvancedActions"));
        spells = CreateDeck(DeckDatabase.GetScriptableObject("Spells"));
        artifacts = CreateDeck(DeckDatabase.GetScriptableObject("Artifacts"));
        wounds = CreateDeck(DeckDatabase.GetScriptableObject("Wounds"));
        commonUnits = CreateDeck(DeckDatabase.GetScriptableObject("CommonUnits"));
        eliteUnits = CreateDeck(DeckDatabase.GetScriptableObject("EliteUnits"));
        dayTactics = CreateDeck(DeckDatabase.GetScriptableObject("DayTactics"), false);
        nightTactics = CreateDeck(DeckDatabase.GetScriptableObject("NightTactics"), false);
    }

    public List<CardId> CreateDeck(Deck deckData, bool shuffle = true)
    {
        var deck = new List<CardId>();

        for (int i = 0; i < deckData.cards.Length; i++)
            for (int j = 0; j <= deckData.extraCopies[i]; j++)
                deck.Add(new CardId(deckData.cards[i].name, GameConstants.CardNumbers.GetLast(true), deckData.cardBackName));

        if(shuffle)
            deck.Shuffle();
        return deck;
    }

    void CreatePlayerDecks(GamePlayers players)
    {
        for (int i = 0; i < players.Connected; i++)
        {
            players.List[i].ServerCreateModel(this);
            players.List[i].ServerRefillHand();
        }
    }

    public CardId GetTacticId(int tacticNumber)
    {
        return dayTactics[tacticNumber];
    }
}
