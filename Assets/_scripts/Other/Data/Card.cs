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

        public Command strongEffect;

        public Command GetAutomaticEffect()
        {
            switch (type)
            {
                case Type.Action:
                case Type.Spell:
                    return Instantiate(strongEffect);
                case Type.Artifact:
                    return Instantiate(strongEffect.alternate);
            }
            return null;
        }
	}
}