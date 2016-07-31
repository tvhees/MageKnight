using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
    public class MouseEnter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            SendMessage("MouseEntered");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            SendMessage("MouseExited");
        }
    }
}