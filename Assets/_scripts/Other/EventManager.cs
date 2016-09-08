using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using Other.Data;

public class StringEvent : UnityEvent<string> { }

public static class EventManager {

    public static UnityEvent endTurn = new UnityEvent();

    public static StringEvent characterSelected = new StringEvent();
    public static StringEvent debugMessage = new StringEvent();
}
