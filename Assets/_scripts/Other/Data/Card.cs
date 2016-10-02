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

        public Command[] commands;

        public Command ChooseEffect(Player playerModel)
        {
            Command effect = null;
            switch (type)
            {
                case Type.Action:
                    if (playerModel.HasMana(colour) || playerModel.HasGold)
                    {
                        effect = Instantiate(commands[1]);
                        effect.requirements.Add(GetColourCost());
                    }
                    else
                        effect = Instantiate(commands[0]);
                    break;
                case Type.Spell:
                    if (playerModel.HasMana(colour) || playerModel.HasGold)
                    {
                        if (playerModel.HasBlack)
                        {
                            effect = Instantiate(commands[1]);
                            var blackCost = CommandDatabase.GetScriptableObject("PayBlack");
                            effect.requirements.Add(Instantiate(blackCost));
                        }
                        else
                            effect = Instantiate(commands[0]);

                        effect.requirements.Add(GetColourCost());
                    }
                    break;
                case Type.Artifact:
                    effect = Instantiate(commands[0]);
                    break;
            }

            return effect;
        }

        public Command GetColourCost()
        {
            var costName = "Pay" + colour.ToString();
            var cost = CommandDatabase.GetScriptableObject(costName);
            return Instantiate(cost);
        }
	}
}