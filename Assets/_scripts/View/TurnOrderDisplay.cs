using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TurnOrderDisplay : MonoBehaviour {

    public PlayerControl playerControl;
    public Toggle toggle;
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

    public void OnToggleChanged(bool on)
    {
        if (on)
            playerControl.Show();
        else
            playerControl.Hide();
    }
}
