using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    public interface Menu
	{
        void InvokeEvent(string name, params object[] parameters);

        void AddListener(UnityAction<string, object[]> listenerMethod);
    }
}