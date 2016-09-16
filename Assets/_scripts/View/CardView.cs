using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace View
{
    public class CardView : NetworkBehaviour
	{
        public Sprite cardFront;
        public Sprite cardBack;

        private Image cardImage;

        private static float scalingFactor = 1.0f;

        void Awake()
        {
            cardImage = GetComponentInChildren<Image>();
        }

        public void MoveToNewParent(Transform parent, bool showFront = true)
        {
            parent.ServerSetChild(transform);
            ResetTransform();

            if (showFront)
                RpcShow();
            else
                RpcHide();
        }

        [ClientRpc]
        public void RpcShow()
        {
            cardImage.sprite = cardFront;
            AllowZooming(true);
        }

        [ClientRpc]
        public void RpcHide()
        {
            cardImage.sprite = cardBack;
            AllowZooming(false);
        }


        #region Private methods
        void ResetTransform()
        {
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one * scalingFactor;
        }

        void AllowZooming(bool allow)
        {
            /*
            Moveable zoomScript = GetComponentInChildren<Moveable>();
            if (zoomScript != null)
                zoomScript.enabled = allow;
                */
        }
        #endregion
    }
}