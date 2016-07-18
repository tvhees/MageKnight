using UnityEngine;
using System.Collections;

namespace BoardGame
{
    namespace Cards
    {
        public class EffectButton : MonoBehaviour
        {

            public Manager.EffectType effectType;

            private Object m_card;
            private int m_playerID;
            private Collider collider;
            private MeshRenderer m_meshRenderer;

            public void Init(int playerID)
            {
                m_card = GetComponentInParent<Object>();
                m_playerID = playerID;
                collider = GetComponent<Collider>();
                m_meshRenderer = GetComponent<MeshRenderer>();
                Deactivate(); // We don't want to fully activate buttons until the card is zoomed on
            }

            public void Activate()
            {
                collider.enabled = true;
            }

            public void Deactivate()
            {
                collider.enabled = false;
                m_meshRenderer.enabled = false;
            }

            public void OnMouseEnter()
            {
                m_meshRenderer.enabled = true;
            }

            public void OnMouseExit()
            {
                m_meshRenderer.enabled = false;
            }

            public void OnMouseUpAsButton()
            {
                Manager.Instance.ProcessEffect(m_playerID, m_card, effectType);
            }
        }
    }
}