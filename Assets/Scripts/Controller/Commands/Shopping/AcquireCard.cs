using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
    public class AcquireCard : Command
    {
        protected Cards.MovementAndDisplay card;
        protected GameObject offerPanel;
        protected int siblingIndex;

        protected void SetParameters(Cards.Acquirable acquirable, Command acquisitionCost)
        {
            card = acquirable.startParent;

            offerPanel = card.transform.parent.gameObject;

            siblingIndex = card.transform.GetSiblingIndex();

            if (acquisitionCost != null)
                requirements.Add(acquisitionCost);
        }

        protected override CommandResult ExecuteThisCommand()
        {
            Main.cardShop.CloseShop();

            return CommandResult.success;
        }

        protected override void UndoThisCommand()
        {
            card.MoveToNewParent(offerPanel.transform);
            card.transform.SetSiblingIndex(siblingIndex);

            Main.cardShop.OpenShop();
        }
    }
}