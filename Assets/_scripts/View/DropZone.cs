using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;
using Commands;

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

    public void Show(bool show)
    {
        dropZone.enabled = show;
    }

    public void OnDrop(PointerEventData eventData)
    {
        var droppedObject = eventData.pointerDrag.GetComponent<Moveable>();

        if (droppedObject == null)
            return;

        Command cost = null;
        Command acquireCommand = null;
        switch (target)
        {
            case Target.Play:
                GameController.singleton.UiPlayEffect("MarchWeak");
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

    public void PlayEffect()
    {

    }

    public void AcquireObject()
    {

    }
}