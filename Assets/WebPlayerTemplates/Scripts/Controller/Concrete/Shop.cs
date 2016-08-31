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

        void Awake()
        {
            Main.cardShop = this;
            Main.shopCamera = transform.root.GetComponent<Camera>();
            Main.turnStart.AddListener(FillUnitOffer);
        }

        #region GenericShopMethods
        public void OpenShop(Type type = Type.View)
        {
            hideOffers.Invoke();
            ShowOffersBasedOnType(type);

            Main.shopCamera.gameObject.SetActive(true);

            FillCardOffers();
        }

        public void CloseShop()
        {
            Main.shopCamera.gameObject.SetActive(false);
        }

        public void FillCardOffers()
        {
            FillActionOffer();
            FillSpellOffer();
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
                    AddToOffer(cardController, actionOffer);
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
                    AddToOffer(cardController, spellOffer);
                }
            }
        }

        void AddToOffer(MovementAndDisplay cardController, Offer offer)
        {
            cardController.MoveToNewParent(offer.transform);
            cardController.ShowFront();
            Main.commandStack.ClearCommandList();
        }

        public void FillUnitOffer(PlayerImpl player)
        {
            int unitsRequired = Network.connections.Length + 3;
            for (int i = 0; i < unitsRequired; i++)
            {
                GameObject card = cards.GetCommonUnit();
                if (card != null)
                {
                    var cardController = card.GetComponent<MovementAndDisplay>();
                    AddToOffer(cardController, unitOffer);
                }
            }

            for (int i = 0; i < Board.Monastery.standingMonasteries; i++)
            {
                AddMonasteryAction();
            }
        }
        #endregion

        #region TypeSpecificMethods
        void ShowOffersBasedOnType(Type type)
        {
            switch (type)
            {
                case Type.View:
                    actionOffer.Show();
                    spellOffer.Show();
                    unitOffer.Show();
                    break;
                case Type.Village:
                    unitOffer.Show();
                    villageOffer.Show();
                    break;
                case Type.Keep:
                    unitOffer.Show();
                    break;
                case Type.MageTower:
                    spellOffer.Show();
                    unitOffer.Show();
                    break;
                case Type.Monastery:
                    unitOffer.Show();
                    monasteryOffer.Show();
                    break;
            }
        }

        public void BuyHealing(int cost)
        {
            Main.rules.AddHealing(new EffectData(), cost);
        }

        public void PlunderVillage()
        {
            Main.rules.PlunderVillage(new EffectData());
        }

        public void AddMonasteryAction()
        {
            Debug.Log("Adding Monastery Action");

            GameObject card = cards.GetAdvancedAction();
            if (card != null)
            {
                var cardController = card.GetComponent<MovementAndDisplay>();
                AddToOffer(cardController, unitOffer);
            }
        }
        #endregion
    }
}