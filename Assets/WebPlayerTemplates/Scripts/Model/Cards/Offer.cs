using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Cards
{
    public class Offer : MonoBehaviour 
	{
        public enum Type { Action, Spell, Artifact, Unit, Village, Monastery }

        public Type type;
        public DropZone[] dropZones;

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void ShowDropZones(bool show)
        {
            foreach (DropZone zone in dropZones)
            {
                zone.gameObject.SetActive(show);
            }
        }
	}
}