using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Cards
{
    public class Offer : MonoBehaviour 
	{
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
	}
}