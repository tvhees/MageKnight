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

    public static void ServerSetLocalPosition(this Transform param, Vector3 newLocalPosition)
    {
        param.GetComponent<NetworkHeirarchyTransform>().SetLocalPosition(newLocalPosition);
    }

    public static void ServerSetLocalScale(this Transform param, Vector3 newLocalScale)
    {
        param.GetComponent<NetworkHeirarchyTransform>().SetLocalScale(newLocalScale);
    }
}
