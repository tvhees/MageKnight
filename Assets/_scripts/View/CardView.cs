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

        void Awake()
        {
            cardImage = GetComponentInChildren<Image>();
        }

        public void SetCardImages(string cardName)
        {
            gameObject.name = cardName;
            cardFront = GetCardImage(cardName);
            cardBack = GetCardImage("cardback");
        }

        Sprite GetCardImage(string name)
        {
            name = name.ToLower().Replace(" ", "");
            Sprite cardFront = Resources.Load<Sprite>("CardImages/" + name);

            if (cardFront == null)
                Debug.Log(name);

            return cardFront;
        }

        public void MoveToNewParent(Transform parent, bool showFront = true)
        {
            transform.SetParent(parent);
            (transform as RectTransform).Reset();

            if (showFront)
                Show();
            else
                Hide();
        }

        public void Show()
        {
            cardImage.sprite = cardFront;
            AllowZooming(true);
        }

        public void Hide()
        {
            cardImage.sprite = cardBack;
            AllowZooming(false);
        }


        #region Private methods
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