using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Commands;

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

        public GameConstants.ManaType colour;

        public int number;

        public Command[] commands;

        public Command ChooseEffect(Player player)
        {
            switch (type)
            {
                case Type.Action:
                    if (player.HasMana(colour))
                        return commands[1];
                    else
                        return commands[0];
                case Type.Spell:
                    if (player.HasMana(colour))
                    {
                        if (player.HasBlack)
                            return commands[1];
                        else
                            return commands[0];
                    }
                    break;
                case Type.Artifact:
                    return commands[0];
            }

            return null;
        }
	}
}