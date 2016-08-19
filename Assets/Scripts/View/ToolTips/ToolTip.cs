using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    public class ToolTip: MonoBehaviour 
	{
        public Canvas canvas;
        public Camera mainCamera;
        public RectTransform panelTransform;
        public toolTipPanel[] toolTipPanels;

        void Display(Vector3 position)
        {
            canvas.enabled = true;
        }

        public void Hide()
        {
            canvas.enabled = false;
        }

        public void ShowTileInformation(GameObject tile)
        {
            DisplayableFeature[] features = tile.GetComponentsInChildren<DisplayableFeature>();

            for (int i = 0; i < toolTipPanels.Length; i++)
            {
                if (i < features.Length)
                    toolTipPanels[i].Show(features[i]);
                else
                    toolTipPanels[i].Hide();
            }

            Display(tile.transform.position);
        }
	}
}