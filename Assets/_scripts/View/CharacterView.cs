using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

namespace View
{
    public class CharacterView : MonoBehaviour 
	{
        public MeshRenderer characterRenderer;

        public void SetMaterialColour(Color colour)
        {
            characterRenderer.material.SetColor("_EmissionColor", colour);
        }

        public void SetMaterialAlpha(float alpha)
        {
            Color c = characterRenderer.material.color;
            characterRenderer.material.color = new Color(c.r, c.g, c.b, alpha);
        }

        public void MoveToHex(HexId targetHex)
        {
            Vector3 newPosition = targetHex.position;
            gameObject.transform.DOMove(newPosition, GameConstants.characterMovementDuration);
        }
	}
}