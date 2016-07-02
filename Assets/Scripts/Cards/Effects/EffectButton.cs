using UnityEngine;
using System.Collections;

public class EffectButton : MonoBehaviour {

    public bool weakEffect;

    private Collider coll;
    private MeshRenderer meshRenderer;

    public void Init()
    {
        coll = GetComponent<Collider>();
        meshRenderer = GetComponent<MeshRenderer>();
        Disable();
    }

    public void Enable()
    {
        coll.enabled = true;
    }

    public void Disable()
    {
        coll.enabled = false;
        meshRenderer.enabled = false;
    }

    public void OnMouseEnter()
    {
        meshRenderer.enabled = true;
    }

    public void OnMouseExit()
    {
        meshRenderer.enabled = false;
    }

    public void OnMouseUpAsButton()
    {
        StartCoroutine(GameManager.m_cardManager.ProcessEffect(GetComponentInParent<CardObject>().GetID(), weakEffect));
    }
}
