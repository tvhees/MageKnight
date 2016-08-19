using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Cards
{
    public class DeckDatabase : ScriptableObjectDatabase<Deck>
    {
        void Awake()
        {
            path = "Decks";
        }
    }
}