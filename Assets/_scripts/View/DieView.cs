using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DieView : MonoBehaviour
{
    public Image face;
    public Image selectionRing;
    public Button button;
    public Color[] manaColours = { Color.red, Color.blue, Color.white, Color.green, Color.yellow, Color.black };
    public GameConstants.ManaType manaType;
    public bool selected;
    public float rollTimer;

    private void Awake()
    {
        button.onClick.AddListener(UiButtonPressed);
    }

    public void SetColour(GameConstants.ManaType manaType, bool animate = false)
    {
        this.manaType = manaType;
        if (animate)
            StartCoroutine(AnimateDiceRolling());
        else
            face.color = manaColours[(int)manaType];
    }

    public void MoveToNewParent(Transform parent)
    {
        transform.SetParent(parent);
        (transform as RectTransform).Reset();
    }

    private IEnumerator AnimateDiceRolling()
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

    private void UiButtonPressed()
    {
        selected = !selected;
        selectionRing.enabled = selected;
        GameController.singleton.UiDieToggled(new ManaId(transform.GetSiblingIndex(), manaType, selected));
    }

    public void Enable(bool enable)
    {
        button.interactable = enable;
        selectionRing.enabled = enable;
    }
}