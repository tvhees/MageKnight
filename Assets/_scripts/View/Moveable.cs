using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;
using View;
using DG.Tweening;

public class Moveable : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    #region Variables

    [SerializeField] float minorZoomLevel;
    [SerializeField] float majorZoomLevel;
    bool zoomedIn;

    #endregion Variables

    #region References

    static Moveable currentObject;
    public CardView startParent;
    [SerializeField]
    CanvasGroup canvasGroup;
    RectTransform rectTransform { get { return transform as RectTransform; } }

    #endregion References

    #region Dragging
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (zoomedIn || !startParent.draggable) return;
        canvasGroup.blocksRaycasts = false;
        currentObject = this;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (zoomedIn || !startParent.draggable) return;
        Vector3 worldPos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(transform as RectTransform, eventData.position, eventData.pressEventCamera, out worldPos);
        transform.position = worldPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (zoomedIn || !startParent.draggable) return;
        canvasGroup.blocksRaycasts = true;
        currentObject = null;
        ZoomOut();
    }

    #endregion

    #region Zooming

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentObject == null)
        {
            MajorZoom();
            currentObject = this;
        }
        else if (currentObject == this)
        {
            ZoomOut();
            currentObject = null;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentObject == null)
            MinorZoom(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentObject == null)
            ZoomOut();
    }

    void MajorZoom()
    {
        zoomedIn = true;
        transform.SetParent(GameController.singleton.zoomDisplayPanel.transform);
        rectTransform.DOAnchorPos(Vector2.zero, GameConstants.cardMovementDuration);
        rectTransform.DOScale(majorZoomLevel * Vector3.one, GameConstants.cardMovementDuration);
    }

    Vector3 PositionAlongCentrelineOfCamera(Camera cam)
    {
        var cameraCenter = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f));
        return new Vector3(cameraCenter.x, cameraCenter.y, transform.position.z);
    }

    void MinorZoom(PointerEventData eventData)
    {
        Vector2 screenPos = eventData.enterEventCamera.WorldToScreenPoint(transform.position);
        Vector2 displayPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GameController.singleton.zoomDisplayPanel.transform as RectTransform, screenPos, null, out displayPos);
        transform.SetParent(GameController.singleton.zoomDisplayPanel.transform);
        rectTransform.anchoredPosition = new Vector2(displayPos.x, displayPos.y);
        rectTransform.localScale = minorZoomLevel * Vector3.one;
    }

    void ZoomOut()
    {
        transform.SetParent(startParent.transform);
        rectTransform.Reset();
        zoomedIn = false;
    }

    #endregion
}