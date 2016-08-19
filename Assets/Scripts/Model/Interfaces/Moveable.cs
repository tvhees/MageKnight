using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
    public class Moveable : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
    {
        static GameObject currentObject;

        #region dragging
        float zPosition;

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (currentObject == null)
            {
                zPosition = transform.position.z;
                currentObject = gameObject;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (currentObject == gameObject)
            {
                Vector3 pointerWorldPos = eventData.pointerCurrentRaycast.worldPosition;
                transform.position = new Vector3(pointerWorldPos.x, pointerWorldPos.y, zPosition);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
        }

        public void OnDrop(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region zooming
        public void OnPointerClick(PointerEventData eventData)
        {
            if (currentObject == null)
            {
                MajorZoom(eventData);
                currentObject = gameObject;
            }
            else if (currentObject == gameObject)
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
            Camera eventCamera = eventData.pressEventCamera;
            Vector3 target = Vector3.Lerp(PositionAlongCentrelineOfCamera(eventCamera), eventCamera.transform.position, 0.6f);
            transform.position = target;
            Debug.Log("MajorZoom");
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
            transform.localPosition = Vector3.zero;
        }
        #endregion
    }
}