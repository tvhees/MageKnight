using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    public class toolTipPanel: MonoBehaviour 
	{
        public Text mainText;
        public LayoutElement layoutElement;

        public void Show(DisplayableFeature featureToShow)
        {
            BuildPanel(featureToShow);
            gameObject.SetActive(true);
        }

        void BuildPanel(DisplayableFeature objectToShow)
        {
            string infoText = objectToShow.displayName;
            infoText += "\n" + objectToShow.description;

            mainText.text = infoText;

            layoutElement.preferredHeight = objectToShow.layoutHeight;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
	}
}