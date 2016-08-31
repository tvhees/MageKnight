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
           // Main.rules.AddMovement(new EffectData(intValue: amount));
        }

        public void AddInfluence(int amount)
        {
            Main.rules.AddInfluence(amount);
        }

        public void AddAttack(int amount)
        {
            //Main.rules.AddAttack(new EffectData(intValue: amount));
        }

        public void AddBlock(int amount)
        {
            //Main.rules.AddBlock(new EffectData(intValue: amount));
        }

        public void DrawCard()
        {
            Main.players.currentPlayer.DrawCards();
        }

        public void OpenShop()
        {
            Main.cardShop.OpenShop();
        }
    }
}