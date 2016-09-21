using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using System.Collections;

public class NetworkHeirarchyTransform : NetworkBehaviour
{
    [SyncVar(hook = "SetParentId")]
    public NetworkInstanceId parentId;
    [SyncVar(hook = "SetLocalScale")]
    public Vector3 localScale;
    [SyncVar(hook = "SetLocalPosition")]
    public Vector3 localPosition;

    public override void OnStartClient()
    {
        base.OnStartClient();
        SetParentId(parentId);
        SetLocalScale(localScale);
        SetLocalPosition(localPosition);
    }

    public void SetLocalPosition(Vector3 newPosition)
    {
        localPosition = newPosition;
        var rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.offsetMax = newPosition;
            rectTransform.offsetMin = newPosition;
            rectTransform.Reset();
        }
        else
            transform.localPosition = localPosition;
    }

    public void SetLocalScale(Vector3 newScale)
    {
        localScale = newScale;
        var rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
            rectTransform.localScale = localScale;
        else
            transform.localScale = localScale;
    }

    void SetParentId(NetworkInstanceId newParentId)
    {
        parentId = newParentId;
        GameObject parentObject = ClientScene.FindLocalObject(parentId);
        if(parentObject != null)
            transform.SetParent(parentObject.transform);
    }

    [Server]
    public void ServerSyncParent(Transform parent)
    {
        parentId = parent.GetComponent<NetworkIdentity>().netId;
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
