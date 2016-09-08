using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Cards
{
    public class CardDragEvent : UnityEvent<Card.Type, bool> { }

    public class MovementAndDisplay: MonoBehaviour
	{
        public Image cardImage;
        public Sprite cardFront;
        public Sprite cardBack;
        public Card.Type type;
        public CardDragEvent cardDrag = new CardDragEvent();

        private static float scalingFactor = 1.0f;

        void Awake()
        {
            cardImage = GetComponentInChildren<Image>();
        }

        public void OnDrag()
        {
            cardDrag.Invoke(type, true);
        }

        public void EndDrag()
        {
            cardDrag.Invoke(type, false);
        }

        public void MoveToNewParent(Transform parent)
        {
            transform.SetParent(parent.transform);
            ResetTransform();
        }

        void ResetTransform()
        {
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one * scalingFactor;
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