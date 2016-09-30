using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ManaView : MonoBehaviour {

    public Image face;

    public Color[] manaColours = new Color[] { Color.red, Color.blue, Color.white, Color.green, Color.yellow, Color.black };

    public GameConstants.ManaType manaType;

    public bool selected;

    public void SetColour(GameConstants.ManaType manaType)
    {
        this.manaType = manaType;
        face.color = manaColours[(int)manaType];
    }

    public void UiButtonPressed()
    {
        selected = !selected;

        UiManaToggled(selected);
    }

    public void UiManaToggled(bool selected)
    {
        GameController.singleton.UiManaToggled(selected, manaType);
    }

}
