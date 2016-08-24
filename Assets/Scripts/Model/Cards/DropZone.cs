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

        public void ShowDropZone()
        {
            dropZone.enabled = true;
        }

        public void HideDropZone()
        {
            dropZone.enabled = false;
        }

        public void OnDrop(PointerEventData eventData)
        {
            Cards.Acquirable acquirable = eventData.pointerDrag.GetComponent<Cards.Acquirable>();
            if (acquirable != null)
            {
                Command acquireCommand = null;
                Debug.Log(string.Format("{0} was dropped on to {1}", eventData.pointerDrag.name, gameObject.name));
                switch (target)
                {
                    case Target.Deck:
                        acquireCommand = new AcquireCardToDeck(acquirable);
                        break;
                    case Target.Discard:
                        acquireCommand = new AcquireCardToDiscard(acquirable);
                        break;
                    case Target.Hand:
                        acquireCommand = new AcquireCardToHand(acquirable);
                        break;
                }

                Main.commandStack.AddCommand(acquireCommand);
                

            }
        }
    }
}