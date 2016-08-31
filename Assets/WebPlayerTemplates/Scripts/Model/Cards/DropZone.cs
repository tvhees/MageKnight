using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
    public class DropZone : MonoBehaviour, IDropHandler
    {
        public enum Target
        {
            Deck,
            Discard,
            Hand,
            Units
        }

        public Target target;

        public Image dropZone;

        public void Show(bool show)
        {
            dropZone.enabled = show;
        }

        public void OnDrop(PointerEventData eventData)
        {
            Cards.Acquirable acquirable = eventData.pointerDrag.GetComponent<Cards.Acquirable>();

            if (acquirable != null)
            {
                Command cost = null;
                Command acquireCommand = null;
                switch (target)
                {
                    case Target.Deck:
                        acquireCommand = new AcquireCardToDeck(acquirable, cost);
                        break;
                    case Target.Discard:
                        acquireCommand = new AcquireCardToDiscard(acquirable, cost);
                        break;
                    case Target.Hand:
                        acquireCommand = new AcquireCardToHand(acquirable, cost);
                        break;
                    case Target.Units:
                        acquireCommand = new AcquireUnit(acquirable, cost);
                        break; 
                }

                Main.commandStack.RunCommand(acquireCommand);
            }
        }
    }
}