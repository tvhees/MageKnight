using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using System.Collections;

public class NetworkHeirarchySync : NetworkBehaviour {
    [Server]
    public void ServerSyncChildren()
    {
        for(int i = 0; i < transform.childCount; i++)
         RpcSyncChild(transform.GetChild(i).GetComponent<NetworkIdentity>().netId);
    }

    [Server]
    public void ServerSyncChild(GameObject child)
    {
        RpcSyncChild(child.GetComponent<NetworkIdentity>().netId);
    }

    [ClientRpc]
    void RpcSyncChild(NetworkInstanceId childId)
    {
        GameObject child = ClientScene.FindLocalObject(childId);
        if(child.transform.parent != transform)
            child.transform.SetParent(transform);
    }
}
