using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Cards
{
    public class MovementAndDisplay: MonoBehaviour
	{
        public SpriteRenderer cardImage;
        public Sprite cardFront;
        public Sprite cardBack;

        public void MoveToNewParent(GameObject parent)
        {
            transform.SetParent(parent.transform);
            ResetTransform();
        }

        void ResetTransform()
        {
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one * Main.scalingFactor;
        }

        public void ShowFront()
        {
            cardImage.sprite = cardFront;
            AllowZooming(true);
        }

        public void HideFront()
        {
            cardImage.sprite = cardBack;
            AllowZooming(false);
        }

        void AllowZooming(bool allow)
        {
            Moveable zoomScript = GetComponentInChildren<Moveable>();
            if (zoomScript != null)
                zoomScript.enabled = allow;
        }
	}
}