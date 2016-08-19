using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
    public class CardHolder : MonoBehaviour, IDropHandler
    {
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
            Debug.Log("OnDrop");

            if (eventData.pointerDrag.GetComponent<Cards.Acquirable>() != null)
                Debug.Log(string.Format("{0} was dropped on to {1}", eventData.pointerDrag.name, gameObject.name));
        }
    }
}