using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using System.Collections;

public class NetworkHeirarchyTransform : NetworkBehaviour
{
    [Server]
    public void ServerSyncParent(Transform parent)
    {
        RpcSyncParent(parent.GetComponent<NetworkIdentity>().netId);
    }

    [ClientRpc]
    void RpcSyncParent(NetworkInstanceId parentId)
    {
        GameObject parent = ClientScene.FindLocalObject(parentId);

        if (transform.parent != null)
            if (transform.parent == parent.transform)
                return;

        transform.SetParent(parent.transform);
    }

    [Server]
    public void ServerSyncChild(Transform child)
    {
        RpcSyncChild(child.GetComponent<NetworkIdentity>().netId);
    }

    [ClientRpc]
    void RpcSyncChild(NetworkInstanceId childId)
    {
        GameObject child = ClientScene.FindLocalObject(childId);
        if (child.transform.parent != transform)
            child.transform.SetParent(transform);
    }
}
