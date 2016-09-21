using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ToolTipPanel : MonoBehaviour
{
    public Text mainText;
    public LayoutElement layoutElement;

    public void Show(ObjectDescription description)
    {
        BuildPanel(description);
        gameObject.SetActive(true);
    }

    void BuildPanel(ObjectDescription description)
    {
        string infoText = description.displayName;
        infoText += "\n" + description.description;

        mainText.text = infoText;

        layoutElement.preferredHeight = description.layoutHeight;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}