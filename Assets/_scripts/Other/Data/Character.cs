using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Other.Data
{
    [CreateAssetMenu(menuName = "Character", fileName = "Character", order = 1)]
    public class Character : ScriptableObject 
	{            
        public string description;
        public Color colour;
        public Deck deck;
	}
}