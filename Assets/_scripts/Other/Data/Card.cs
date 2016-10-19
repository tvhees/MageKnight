using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Commands;
using Other.Utility;

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
        public GameConstants.ManaType colour;
        public int number;

        [SerializeField] Command[] effects;
        [SerializeField] int automaticIndex;
        [SerializeField] bool isRepeatable;

        public int AutomaticIndex { get { return automaticIndex; } }
        public bool IsRepeatable { get { return isRepeatable; } }

        public bool HasEffect(int index)
        {
            return index < effects.Length;
        }

        public Command GetEffect(int index)
        {
            return Instantiate(effects[index]);
        }
    }
}