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
            private Collider m_collider;
            private MeshRenderer m_meshRenderer;

            public void Init(int playerID)
            {
                m_card = GetComponentInParent<Object>();
                m_playerID = playerID;
                m_collider = GetComponent<Collider>();
                m_meshRenderer = GetComponent<MeshRenderer>();
                Disable();
            }

            public void Enable()
            {
                m_collider.enabled = true;
            }

            public void Disable()
            {
                m_collider.enabled = false;
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