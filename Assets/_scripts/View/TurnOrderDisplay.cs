using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TurnOrderDisplay : MonoBehaviour {

    public PlayerControl playerControl;
    public Button button;
    public GameObject currentlyViewingIndicator;
    public Text nameText;
    public Text characterText;
    public Image img;

    public void AssignToPlayer(PlayerControl playerControl)
    {
        this.playerControl = playerControl;
        gameObject.SetActive(true); 
    }

    public void SetPlayerName(string name)
    {
        nameText.text = name;
    }

    public void SetCharacterName(string name)
    {
        characterText.text = name;
    }

    public void SetPlayerColour(Color colour)
    {
        float alpha = img.color.a;
        img.color = new Color(colour.r, colour.g, colour.b, alpha);
    }

    public void Select(bool isOn)
    {
        currentlyViewingIndicator.SetActive(isOn);

        if (isOn)
        {
            playerControl.Show();
            button.interactable = false;
        }
        else
        {
            playerControl.Hide();
            button.interactable = true;
        }
    }

    public void SetHighlights(Color highlightColour, GameObject highlightIndicator = null)
    {
        SetTextColour(highlightColour);
        if (highlightIndicator != null)
        {
            highlightIndicator.transform.SetParent(transform);
            (highlightIndicator.transform as RectTransform).Reset();
        }
    }

    public void SetTextColour(Color colour)
    {
        nameText.color = colour;
        characterText.color = colour;
    }
}
