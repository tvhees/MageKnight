using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    private bool allowDrop;

    private void Awake()
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

        if (droppedObject == null)
            return;

        switch (target)
        {
            case Target.Play:
                GameController.singleton.UiPlayEffect(droppedObject.startParent.cardId);
                break;

            case Target.Deck:
                //acquireCommand = new AcquireCardToDeck(acquirable, cost);
                break;

            case Target.Discard:
                //acquireCommand = new AcquireCardToDiscard(acquirable, cost);
                break;

            case Target.Hand:
                //acquireCommand = new AcquireCardToHand(acquirable, cost);
                break;

            case Target.Units:
                //acquireCommand = new AcquireUnit(acquirable, cost);
                break;
        }

        //GameController.singleton.commandStack.RunCommand(acquireCommand);
    }
}