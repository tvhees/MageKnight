using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Other.Data
{
    [CreateAssetMenu(menuName = "Cards/Card", fileName = "Card", order = 1)]
    [System.Serializable]
    public class Card : ScriptableObject
	{
        public enum Type
        {
            Action,
            Spell,
            Artifact,
            CommonUnit,
            EliteUnit,
            Wound,
            Tactic
        }

        public Type type;

        public enum Colour
        {
            Red,
            Blue,
            White,
            Green,
            None
        }

        public Colour colour;

        public int number;
	}
}