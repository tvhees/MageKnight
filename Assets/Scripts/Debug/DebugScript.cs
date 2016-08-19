using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    public class DebugScript: MonoBehaviour 
	{
        public void AddMovement(int amount)
        {
            Main.rules.AddMovement(new EffectData(intValue: amount));
        }

        public void AddInfluence(int amount)
        {
            Main.rules.AddInfluence(new EffectData(intValue: amount));
        }

        public void AddAttack(int amount)
        {
            Main.rules.AddAttack(new EffectData(intValue: amount));
        }

        public void AddBlock(int amount)
        {
            Main.rules.AddBlock(new EffectData(intValue: amount));
        }

        public void DrawCard()
        {
            Main.players.currentPlayer.DrawCard();
        }

        public void SetTarget(int value)
        {
            CardHolder target = null;
            switch (value)
            {
                case 1:
                    target = Main.players.currentPlayer.belongings.deckPanel;
                    break;
                case 2:
                    target = Main.players.currentPlayer.belongings.discardPanel;
                    break;
                case 3:
                    target = Main.players.currentPlayer.belongings.handPanel;
                    break;
            }

            Main.players.currentPlayer.belongings.SetCardAcquisitionTarget(target);
        }
    }
}