using UnityEngine;
using UnityEngine.UI;

public class DebugPanel : MonoBehaviour
{
    public GameObject messagePrefab;

    private void Awake()
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
}