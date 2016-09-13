using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TurnOrderDisplay : MonoBehaviour {

    public Text nameText;
    public Text characterText;
    public Image img;

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
}
