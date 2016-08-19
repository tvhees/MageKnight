using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
    public class PointerDownAndUp : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            gameObject.SendMessage("PointerDown");
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            gameObject.SendMessage("PointerUp");
        }
    }
}