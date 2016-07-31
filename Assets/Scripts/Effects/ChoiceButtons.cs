using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;

namespace BoardGame
{
	namespace Effect
    {
		public class ChoiceButtons : BaseEffect 
		{
            public bool optionChosen;

            private GameObject choiceCanvasPrefab;
            private GameObject choiceOptionPrefab;

            private Canvas choiceCanvas;
            private Button[] choiceOptions;

            public void AddButtons(List<UnityAction> choices, List<string> descriptions)
            {
                choiceCanvasPrefab = Resources.Load("ChoiceCanvas") as GameObject;
                choiceOptionPrefab = Resources.Load("ChoiceOption") as GameObject;

                choiceCanvas = transform.InstantiateChild(choiceCanvasPrefab).GetComponent<Canvas>();
                choiceCanvas.worldCamera = GetComponentInParent<Card.Object>().owningPlayer.playerCamera;
                choiceOptions = new Button[choices.Count];

                for (int i = 0; i < choices.Count; i++)
                {
                    GameObject button = choiceCanvas.transform.InstantiateChild(choiceOptionPrefab);
                    button.transform.GetComponentInChildren<Text>().text = descriptions[i];
                    choiceOptions[i] = button.GetComponent<Button>(); // Get the regular Unity Button class
                    choiceOptions[i].onClick.AddListener(choices[i]); // Tell it to run Method i
                    choiceOptions[i].onClick.AddListener(ButtonCallback); // Add callback method to end the choice coroutine
                }
                
                HideChoices();
            }

            public override void UseEffect()
            {
                StartCoroutine(WaitForChoice());
            }

            public IEnumerator WaitForChoice()
            {
                optionChosen = false;

                ShowChoices();

                while (!optionChosen)
                    yield return null;

                HideChoices();
            }

            public void ButtonCallback()
            {
                optionChosen = true;
            }

            public void ShowChoices()
            {
                choiceCanvas.enabled = true;
            }

            public void HideChoices()
            {
                choiceCanvas.enabled = false;
            }
        }
	}
}