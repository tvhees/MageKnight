using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Cards
{
    [System.Serializable]
    public class OfferEvent : UnityEvent { }

    public class Shop : MonoBehaviour 
	{
        public OfferEvent hideOffers;
        private OfferEvent showOffers = new OfferEvent();

        public enum Type
        {
            View,
            Village,
            Keep,
            MageTower,
            Monastery
        }

        public Cards cards;

        public Offer actionOffer;
        public Offer spellOffer;
        public Offer unitOffer;
        public Offer villageOffer;
        public Offer monasteryOffer;

        public DropZone deckZone;
        public DropZone discardZone;
        public DropZone handZone;

        private DropZone target;

        #region GenericShopMethods
        public void OpenShop(Type type = Type.View)
        {
            hideOffers.Invoke();
            ShowOffersBasedOnType(type);

            Main.Instance.shopCamera.gameObject.SetActive(true);

            FillActionOffer();
            FillSpellOffer();
        }

        public void CloseShop()
        {
            TurnOffAcquisitionTarget();
            Main.Instance.shopCamera.gameObject.SetActive(false);
        }

        void FillActionOffer()
        {
            int cardsRequired = 3 - actionOffer.transform.childCount;
            for (int i = 0; i < cardsRequired; i++)
            {
                GameObject card = cards.GetAdvancedAction();
                if (card != null)
                {
                    var cardController = card.GetComponent<MovementAndDisplay>();
                    cardController.MoveToNewParent(actionOffer.transform);
                    cardController.ShowFront();
                }
            }
        }

        void FillSpellOffer()
        {
            int cardsRequired = 3 - spellOffer.transform.childCount;
            for (int i = 0; i < cardsRequired; i++)
            {
                GameObject card = cards.GetSpell();
                if (card != null)
                {
                    var cardController = card.GetComponent<MovementAndDisplay>();
                    cardController.MoveToNewParent(spellOffer.transform);
                    cardController.ShowFront();
                }
            }
        }

        public void FillUnitOffer()
        {
            int unitsRequired = Main.Instance.NumberOfPlayers + 2;
            for (int i = 0; i < unitsRequired; i++)
            {
                GameObject card = cards.GetCommonUnit();
                if (card != null)
                {
                    var cardController = card.GetComponent<MovementAndDisplay>();
                    cardController.MoveToNewParent(unitOffer.transform);
                    cardController.ShowFront();
                }
            }
        }
        #endregion

        #region CardAcquisitionTargets
        public void SetDeckAsTarget()
        {
            SetCardAcquisitionTarget(deckZone);
        }

        public void SetHandAsTarget()
        {
            SetCardAcquisitionTarget(handZone);
        }

        public void SetDiscardAsTarget()
        {
            SetCardAcquisitionTarget(discardZone);
        }

        void SetCardAcquisitionTarget(DropZone target)
        {
            this.target = target;
            target.ShowDropZone();
        }

        public void TurnOffAcquisitionTarget()
        {
            if (target != null)
            {
                target.HideDropZone();
                target = null;
            }
        }
        #endregion

        #region TypeSpecificMethods
        void ShowOffersBasedOnType(Type type)
        {
            showOffers.RemoveAllListeners();
            switch (type)
            {
                case Type.View:
                    showOffers.AddListener(actionOffer.Show);
                    showOffers.AddListener(spellOffer.Show);
                    showOffers.AddListener(unitOffer.Show);
                    break;
                case Type.Village:
                    showOffers.AddListener(unitOffer.Show);
                    showOffers.AddListener(villageOffer.Show);
                    break;
                case Type.Keep:
                    showOffers.AddListener(unitOffer.Show);
                    break;
                case Type.MageTower:
                    showOffers.AddListener(spellOffer.Show);
                    showOffers.AddListener(unitOffer.Show);
                    break;
                case Type.Monastery:
                    showOffers.AddListener(unitOffer.Show);
                    showOffers.AddListener(monasteryOffer.Show);
                    break;
            }

            showOffers.Invoke();
        }

        public void BuyHealing(int cost)
        {
            Main.rules.AddHealing(1, cost);
        }
        #endregion
    }
}