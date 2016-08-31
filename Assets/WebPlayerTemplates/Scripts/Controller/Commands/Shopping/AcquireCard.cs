using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
    public class AcquireCard : Command
    {
        protected Cards.MovementAndDisplay card;
        protected Cards.Offer offerPanel;
        protected int siblingIndex;

        protected void SetParameters(Cards.Acquirable acquirable, Command acquisitionCost)
        {
            card = acquirable.startParent;

            offerPanel = card.transform.GetComponentInParent<Cards.Offer>();

            siblingIndex = card.transform.GetSiblingIndex();

            if (acquisitionCost != null)
                requirements.Add(acquisitionCost);
        }

        protected override CommandResult ExecuteThisCommand()
        {
            switch (offerPanel.type)
            {
                case Cards.Offer.Type.Action:
                case Cards.Offer.Type.Spell:
                    Main.cardShop.FillCardOffers();
                    break;
                case Cards.Offer.Type.Unit:
                    return CommandResult.success;
            }

            return CommandResult.permanent;
        }

        protected override void UndoThisCommand()
        {
            card.MoveToNewParent(offerPanel.transform);
            card.transform.SetSiblingIndex(siblingIndex);
            card.ShowFront();
        }
    }
}