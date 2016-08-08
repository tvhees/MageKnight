using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    namespace Effect
    {
        public class CardButton : MonoBehaviour
        {
            private Button button;
            private Image image;
            private Object card;
            public Players.Player player { get; private set; }

            // Effect component
            BaseEffect effect;

            public void Init(Players.Player player)
            {
                image = GetComponent<Image>();
                card = GetComponentInParent<Card.Object>();
                this.player = player;
            }

            public void AddUnityActionsForButton(Dictionary<string, string> cardInfo, int effectNumber)
            {
                button = GetComponent<Button>();
                UnityAction<Rulesets.Ruleset> action = AddEffectActionByName(cardInfo, effectNumber);
                button.onClick.AddListener(delegate{ action(Game.GetRules()); });
            }

            public UnityAction<Rulesets.Ruleset> AddEffectActionByName(Dictionary<string, string> cardInfo, int effectNumber, string subChar = "")
            {
                // Attach a method to the button by name
                string effectKey = "effect_" + effectNumber.ToString() + subChar; // The key is of the form effect_0, effect_1, etc.
                string choiceKey = "choice_" + effectNumber.ToString() + subChar; // The key is of the form choice_0, choice_1, etc.
                string combineKey = "combine_" + effectNumber.ToString() + subChar; // The key is of the form combine_0, combine_1, etc.
                string effectName;

                if (cardInfo.TryGetValue(effectKey, out effectName)) // Get the name of the Class to add from the card's dictionary
                {
                    System.Type effectMethod = System.Type.GetType("BoardGame.Effect." + effectName); // Convert the name to a Class type
                    effect = (BaseEffect)gameObject.AddComponent(effectMethod); // Add the Class component to the button
                    AddEffectValue(cardInfo, effect, effectNumber, subChar); // Effects can have a value in the dictionary too
                }
                else if (cardInfo.ContainsKey(choiceKey)) // If we're adding a multiple method choice there's more classes to set up
                {
                    ChoiceButtons choiceScript = gameObject.AddComponent<ChoiceButtons>(); // This button will call the class written to generate choices
                    List<UnityAction<Rulesets.Ruleset>> choices = AddMultipleEffects(cardInfo, effectNumber); // Get a reference list for the effects to choose between
                    List<string> descriptions = AddEffectDescriptions(cardInfo, effectNumber);
                    choiceScript.AddButtons(choices, descriptions); // Set up the buttons and listeners for each effect choice, these will be added with their "effect_" keys

                    effect = choiceScript; // After adding the child effects, make sure we set the choice class as the one to call on click
                }
                else if (cardInfo.ContainsKey(combineKey)) // If we're getting a combination of effects
                {
                    CombineEffects combineScript = gameObject.AddComponent<CombineEffects>(); // This button will call the class written to fire multiple actions
                    List<UnityAction<Rulesets.Ruleset>> combinedEffects = AddMultipleEffects(cardInfo, effectNumber); // Grab all the effects to combine
                    combineScript.AddEffects(combinedEffects); // Give the combinedScript the list of actions to execute when called

                    effect = combineScript; // After adding the child effects, make sure we set the combine class as the one to call on click
                }
                else return null; // There's no effect or choice of this type in the dictionary to get

                return effect.UseEffect;
            }

            List<UnityAction<Rulesets.Ruleset>> AddMultipleEffects(Dictionary<string, string> cardInfo, int effectNumber) // Recursive method - reuses the AddEffectByName method to add all choices for a single effect
            {
                List<UnityAction<Rulesets.Ruleset>> effects = new List<UnityAction<Rulesets.Ruleset>>();
                foreach (string i in new string[4] { "a", "b", "c", "d" })
                {
                    UnityAction<Rulesets.Ruleset> effectAction = AddEffectActionByName(cardInfo, effectNumber, i);
                    if (effectAction != null) effects.Add(effectAction);
                }

                return effects;
            }

            List<string> AddEffectDescriptions(Dictionary<string, string> cardInfo, int effectNumber)
            {
                List<string> descriptions = new List<string>();
                foreach (string i in new string[4] { "a", "b", "c", "d" })
                {
                    string descriptionKey = "effect_" + effectNumber.ToString() + i;
                    string effectDescription;
                    cardInfo.TryGetValue(descriptionKey, out effectDescription);
                    if (effectDescription != null) descriptions.Add(effectDescription);
                }

                return descriptions;
            }

            void AddEffectValue(Dictionary<string, string> cardInfo, BaseEffect effect, int effectNumber, string choiceChar = "")
            {
                string valueKey = "value_" + effectNumber.ToString() + choiceChar;
                string effectValue;
                if (cardInfo.TryGetValue(valueKey, out effectValue))
                {
                    int value;
                    if (int.TryParse(effectValue, out value)) effect.intValue = value;
                }
            }

            public void AddEffectCostByType(string cardType, int effectNumber, string cardColour)
            {
                if (cardType == "basic" || cardType == "advanced")
                    if (effectNumber == 2) AddEffectCost(new string[1] { cardColour });

                if (cardType == "spell")
                {
                    if (effectNumber == 1) AddEffectCost(new string[1] { cardColour });
                    else if (effectNumber == 2) AddEffectCost(new string[2] { cardColour, "black" });
                }
            }

            void AddEffectCost(string[] costColours)
            {
                Cost effectCost = gameObject.AddComponent<Cost>();
                effectCost.costColours = costColours;
            }

            public void AddCleanupMethod()
            {
                gameObject.AddComponent<CleanupMethod>();
            }

            public void Activate()
            {
                image.enabled = true;
                button.enabled = true;
            }

            public void Deactivate()
            {
                image.enabled = false;
                button.enabled = false;
            }
        }
    }
}