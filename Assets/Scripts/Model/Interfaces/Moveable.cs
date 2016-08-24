using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
    public class Moveable : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        static Moveable currentObject;
        public CanvasGroup canvasGroup;
        public Cards.MovementAndDisplay startParent;

        bool zoomedIn;

        #region dragging
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!zoomedIn)
            {
                canvasGroup.blocksRaycasts = false;
                currentObject = this;
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
            transform.SetParent(Main.Instance.displayPanel.transform);
            (transform as RectTransform).Reset();
        }

        public Vector3 PositionAlongCentrelineOfCamera(Camera camera)
        {
            Vector3 cameraCenter = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f));
            return new Vector3(cameraCenter.x, cameraCenter.y, transform.position.z);
        }

        public void MinorZoom(PointerEventData eventData)
        {
            Camera eventCamera = eventData.enterEventCamera;
            transform.position = Vector3.Lerp(transform.position, eventCamera.transform.position, 0.2f);
        }

        void ZoomOut()
        {
            if (zoomedIn)
            {
                transform.SetParent(startParent.transform);
                (transform as RectTransform).Reset();
            }
            transform.localPosition = Vector3.zero;
            zoomedIn = false;
        }
        #endregion
    }
}