using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace View
{
    public class CardView : NetworkBehaviour
	{
        #region Variables

        public CardId cardId;
        public Sprite cardFront;
        public Sprite cardBack;
        public bool draggable;

        #endregion New Region

        #region References

        Image cardImage;

        #endregion References

        #region Creation

        void Awake()
        {
            cardImage = GetComponentInChildren<Image>();
        }

        public void SetCardImages()
        {
            gameObject.name = cardId.name;
            cardFront = LoadCardImage(cardId.name);
            cardBack = LoadCardImage(cardId.cardBackName);
            Show();
        }

        Sprite LoadCardImage(string imageName)
        {
            imageName = imageName.ToLower().Replace(" ", "");
            var img = Resources.Load<Sprite>("CardImages/" + imageName);

            Assert.IsNotNull(img, imageName);

            return img;
        }

        #endregion Creation

        #region Display methods

        public void MoveToNewParent(Transform parent, bool showFront = true, bool draggable = false)
        {
            transform.SetParent(parent);
            (transform as RectTransform).Reset();
            this.draggable = draggable;
            if (showFront)
                Show();
            else
                Hide();
        }

        void Show()
        {
            cardImage.sprite = cardFront;
            AllowZooming(true);
        }

        void Hide()
        {
            cardImage.sprite = cardBack;
            AllowZooming(false);
        }

        void AllowZooming(bool allow)
        {
            var zoomScript = GetComponentInChildren<Moveable>();
            if (zoomScript != null)
            {
                zoomScript.enabled = allow;
            }
        }

        #endregion Display methods
    }
}