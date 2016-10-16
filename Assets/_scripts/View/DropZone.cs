using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using RSG;

public class DropZone : MonoBehaviour, IDropHandler
{
    public enum Target
    {
        Play,
        Deck,
        Discard,
        Hand,
        Units,
    }

    public Target target;

    public Image dropZone;

    bool allowDrop;

    void Awake()
    {
        EventManager.stateChanged.AddListener(Enable);
    }

    public void Enable(GameState newState)
    {
        allowDrop = newState.AllowEffects;
    }

    public void ShowInUi(bool show)
    {
        if (dropZone == null)
            return;

        dropZone.enabled = show;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!allowDrop)
            return;

        var droppedObject = eventData.pointerDrag.GetComponent<Moveable>();

        if (droppedObject == null || !droppedObject.startParent.draggable)
            return;

        var cardId = droppedObject.startParent.cardId;

        switch (target)
        {
            case Target.Play:
                PlayerControl.local.CmdPlayCard(cardId);
                break;
            case Target.Deck:
                break;
            case Target.Discard:
                break;
            case Target.Hand:
                break;
            case Target.Units:
                break;
        }
    }
}