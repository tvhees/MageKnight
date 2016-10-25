using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

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
        RectTransform rectTransform { get { return transform as RectTransform; } }

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
            this.draggable = draggable;
            transform.SetParent(parent);
            rectTransform.Reset();
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