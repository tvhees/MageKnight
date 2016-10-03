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
    public float rollTimer;

    public void SetColour(GameConstants.ManaType manaType)
    {
        this.manaType = manaType;
        StartCoroutine(AnimateDiceRolling());
    }

    IEnumerator AnimateDiceRolling()
    {
        float timer = rollTimer;
        float index = Random.Range(0, manaColours.Length);
        while (timer >= 0f)
        {
            index += 6f * Time.deltaTime;
            face.color = manaColours[Mathf.RoundToInt(Mathf.Repeat(index, manaColours.Length - 1))];
            timer -= Time.deltaTime;
            yield return null;
        }

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
        GameController.singleton.UiDieToggled(new ManaId(transform.GetSiblingIndex(), manaType, selected));
    }

}
