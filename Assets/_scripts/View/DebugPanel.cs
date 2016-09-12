using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class DebugPanel : MonoBehaviour
{
    public GameObject messagePrefab;

    void Awake()
    {
        EventManager.debugMessage.AddListener(LogDebugMessage);
    }

    public void LogDebugMessage(string message)
    {
        Debug.Log(message);

        GameObject msg = transform.InstantiateChild(messagePrefab);
        msg.transform.SetSiblingIndex(0);
        msg.transform.localScale = Vector3.one;
        msg.GetComponentInChildren<Text>().text = message;
    }

    public void DebugLocalPlayer()
    {
        string debugMsg = "";
        if (GameController.singleton == null)
            debugMsg = "No GameController";
        else if (GameController.singleton.localPlayer == null)
            debugMsg = "No Local Player";
        else
            debugMsg = "Local Player: " + GameController.singleton.localPlayer.name;

        LogDebugMessage(debugMsg);
    }
}