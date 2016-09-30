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

    public Cards(Scenario scenario, PlayerControl[] players)
    {
        GameConstants.GenerateCardNumbers();
        CreateCommonDecks(scenario);
        CreatePlayerDecks(players);
    }

    void CreateCommonDecks(Scenario scenario)
    {
        actions = CreateDeck(DeckDatabase.GetScriptableObject("AdvancedActions"));
        spells = CreateDeck(DeckDatabase.GetScriptableObject("Spells"));
        artifacts = CreateDeck(DeckDatabase.GetScriptableObject("Artifacts"));
        wounds = CreateDeck(DeckDatabase.GetScriptableObject("Wounds"));
        commonUnits = CreateDeck(DeckDatabase.GetScriptableObject("CommonUnits"));
        eliteUnits = CreateDeck(DeckDatabase.GetScriptableObject("EliteUnits"));
        dayTactics = CreateDeck(DeckDatabase.GetScriptableObject("DayTactics"), shuffle: false);
        nightTactics = CreateDeck(DeckDatabase.GetScriptableObject("NightTactics"), shuffle: false);
    }

    public List<CardId> CreateDeck(Deck deckData, bool shuffle = true)
    {
        List<CardId> deck = new List<CardId>();

        for (int i = 0; i < deckData.cards.Length; i++)
        {
            for (int j = 0; j <= deckData.extraCopies[i]; j++)
            {
                deck.Add(new CardId(deckData.cards[i].name, GameConstants.cardNumbers.GetLast(remove: true)));
            }
        }

        if(shuffle)
            deck.Shuffle();
        return deck;
    }

    void CreatePlayerDecks(PlayerControl[] players)
    {
        foreach (var playerControl in players)
        {
            playerControl.CreateModel(this);
            playerControl.DrawCards(5);
        }
    }

    public CardId GetTacticId(int tacticNumber)
    {
        return dayTactics[tacticNumber];
    }
}
