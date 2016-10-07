using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using Other.Data;

public class StringEvent : UnityEvent<string> { }
public class GameObjectEvent : UnityEvent<GameObject> { }
public class GameStateEvent : UnityEvent<GameState> { }

public static class EventManager {

    public static UnityEvent endTurn = new UnityEvent();

    public static GameStateEvent stateChanged = new GameStateEvent();
    public static StringEvent characterSelected = new StringEvent();
    public static StringEvent tacticSelected = new StringEvent();
    public static StringEvent debugMessage = new StringEvent();
}
