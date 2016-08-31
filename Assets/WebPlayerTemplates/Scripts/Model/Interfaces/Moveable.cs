﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
    public class Moveable : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        static GameObject displayPanel;
        static Moveable currentObject;
        public CanvasGroup canvasGroup;
        public Cards.MovementAndDisplay startParent;
        public float minorZoomLevel;
        public float majorZoomLevel;

        private bool zoomedIn;
        private RectTransform rectTransform { get { return transform as RectTransform; } }

        void Awake()
        {
            if (displayPanel != null)
                displayPanel = GameObject.Find("MajorZoom");
        }

        #region dragging
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!zoomedIn)
            {
                canvasGroup.blocksRaycasts = false;
                currentObject = this;
                startParent.OnDrag();
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!zoomedIn)
            {
                Vector3 worldPos;
                RectTransformUtility.ScreenPointToWorldPointInRectangle(transform as RectTransform, eventData.position, eventData.pressEventCamera, out worldPos);
                transform.position = worldPos;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!zoomedIn)
            {
                startParent.EndDrag();
                canvasGroup.blocksRaycasts = true;
                currentObject = null;
                ZoomOut();
            }
        }
        #endregion

        #region zooming
        public void OnPointerClick(PointerEventData eventData)
        {
            if (currentObject == null)
            {
                MajorZoom(eventData);
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
            if(currentObject == null)
                MinorZoom(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (currentObject == null)
                ZoomOut();
        }

        public void MajorZoom(PointerEventData eventData)
        {
            zoomedIn = true;
            transform.SetParent(displayPanel.transform);
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.localScale = majorZoomLevel * Vector3.one;
        }

        public Vector3 PositionAlongCentrelineOfCamera(Camera camera)
        {
            Vector3 cameraCenter = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f));
            return new Vector3(cameraCenter.x, cameraCenter.y, transform.position.z);
        }

        public void MinorZoom(PointerEventData eventData)
        {
            Vector2 screenPos = eventData.enterEventCamera.WorldToScreenPoint(transform.position);
            Vector2 displayPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(displayPanel.transform as RectTransform, screenPos, null, out displayPos);
            transform.SetParent(displayPanel.transform);
            rectTransform.anchoredPosition = new Vector2(displayPos.x, displayPos.y);
            rectTransform.localScale = minorZoomLevel * Vector3.one;
        }

        void ZoomOut()
        {
            transform.SetParent(startParent.transform);
            rectTransform.Reset();
            transform.localPosition = Vector3.zero;
            zoomedIn = false;
        }
        #endregion
    }
}