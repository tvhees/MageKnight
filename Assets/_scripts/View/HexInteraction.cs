using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class HexInteraction : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public HexId id;

    private float holdTimer = 0f;
    private bool pointerDown = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;
        holdTimer = 0.2f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointerDown = false;
        GameController.singleton.toolTip.Hide();
        if (holdTimer >= 0)
            Clicked();

    }

    void Update()
    {
        if (pointerDown)
        {
            holdTimer -= Time.deltaTime;
            if (holdTimer < 0)
                GameController.singleton.toolTip.ShowTileInformation(gameObject);
        }
    }

    public void Clicked()
    {
        GameController.singleton.players.local.CmdMoveToHex(id);
    }
}