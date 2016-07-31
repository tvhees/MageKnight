using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    public class FrontEndMenu : MenuImpl
    {
        void Awake()
        {
            Selectable[] selectables = GetComponentsInChildren<Selectable>();

            foreach (Selectable input in selectables)
            {
                ListenToInput(input);
            }
        }
    }
}