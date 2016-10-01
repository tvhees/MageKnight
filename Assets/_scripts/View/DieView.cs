using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DieView : MonoBehaviour {

    public Image face;
    public Image selectionRing;
    public Button button;
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

        UiDieToggled(selected);
    }

    public void UiDieToggled(bool selected)
    {
        selectionRing.enabled = selected;
        GameController.singleton.UiDieToggled(selected, manaType);
    }

}
