using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Board
{
    public class HexInteraction: MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private float holdTimer = 0f;
        private bool pointerDown = false;

        public void OnPointerDown(PointerEventData eventData)
        {
            pointerDown = true;
            holdTimer = 0.2f;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            pointerDown = false;
            Main.Instance.toolTip.Hide();
            if (holdTimer >= 0)
                Clicked();

        }

        void Update()
        {
            if (pointerDown)
            {
                holdTimer -= Time.deltaTime;
                if (holdTimer < 0)
                    Main.Instance.toolTip.ShowTileInformation(gameObject);
            }
        }

        public void Clicked()
        {
            Main.rules.Interact(new EffectData(gameObjectValue: gameObject));
        }
	}
}