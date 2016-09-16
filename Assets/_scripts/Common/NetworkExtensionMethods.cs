using UnityEngine;
using System.Collections;

public static class NetworkExtensionMethods {

    public static void ServerSetParent(this Transform param, Transform newParent)
    {
        param.SetParent(newParent);
        param.GetComponent<NetworkHeirarchyTransform>().ServerSyncParent(newParent);
    }

    public static void ServerSetChild(this Transform param, Transform newChild)
    {
        newChild.SetParent(param);
        param.GetComponent<NetworkHeirarchyTransform>().ServerSyncChild(newChild);
    }
}
