using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Other.Data
{
    [CreateAssetMenu(menuName = "Cards/Deck", fileName = "Deck", order = 1)]
    [System.Serializable]
    public class Deck : ScriptableObject
	{
        public Card[] cards;
        public int[] extraCopies;
        public string cardBackName;
	}
}