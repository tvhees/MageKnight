using Commands;
using UnityEngine;

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

        public bool isRepeatable;

        public Command strongEffect;

        public Command GetAutomaticEffect()
        {
            switch (type)
            {
                case Type.Action:
                case Type.Spell:
                case Type.Tactic:
                    return Instantiate(strongEffect);

                case Type.Artifact:
                    return Instantiate(strongEffect.alternate);
            }
            return null;
        }
    }
}