using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace BoardGame
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
                Debug.Log("get image component");
                image = GetComponent<Image>();
                card = GetComponentInParent<Card.Object>();
                this.player = player;
            }

            public void AddUnityActionsForButton(Dictionary<string, string> cardInfo, int effectNumber)
            {
                button = GetComponent<Button>();
                UnityAction action = AddEffectActionByName(cardInfo, effectNumber);
                button.onClick.AddListener(action);
            }

            public UnityAction AddEffectActionByName(Dictionary<string, string> cardInfo, int effectNumber, string choiceChar = "")
            {
                // Attach a method to the button by name
                string effectKey = "effect_" + effectNumber.ToString() + choiceChar; // The key is of the form effect_0, effect_1, etc.
                string choiceKey = "choice_" + effectNumber.ToString() + choiceChar; // The key is of the form choice_0, choice_1, etc.
                string effectName;

                if (cardInfo.TryGetValue(effectKey, out effectName)) // Get the name of the Class to add from the card's dictionary
                {
                    System.Type effectMethod = System.Type.GetType("BoardGame.Effect." + effectName); // Convert the name to a Class type
                    effect = (BaseEffect)gameObject.AddComponent(effectMethod); // Add the Class component to the button
                    AddEffectValue(cardInfo, effect, effectNumber, choiceChar); // Effects can have a value in the dictionary too
                }
                else if (cardInfo.ContainsKey(choiceKey)) // If we're adding a multiple method choice there's more classes to set up
                {
                    ChoiceButtons choiceScript = gameObject.AddComponent<ChoiceButtons>(); // Convert the effect reference to ChoiceButtons class specifically
                    List<UnityAction> choices = AddChoiceEffects(cardInfo, effectNumber); // Get a reference list for the effects to choose between
                    List<string> descriptions = AddEffectDescriptions(cardInfo, effectNumber);
                    choiceScript.AddButtons(choices, descriptions); // Set up the buttons and listeners for each effect choice, these will be added with their "effect_" keys

                    // After adding the child effects, make sure we set the choice script as the one to call on click
                    effect = choiceScript;
                }
                else return null; // There's no effect or choice of this type in the dictionary to get

                return effect.UseEffect;
            }

            List<UnityAction> AddChoiceEffects(Dictionary<string, string> cardInfo, int effectNumber) // Recursive method - reuses the AddEffectByName method to add all choices for a single effect
            {
                List<UnityAction> choices = new List<UnityAction>();
                foreach (string i in new string[4] { "a", "b", "c", "d" })
                {
                    UnityAction choiceAction = AddEffectActionByName(cardInfo, effectNumber, i);
                    if (choiceAction != null) choices.Add(choiceAction);
                }

                return choices;
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