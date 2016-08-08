using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
	namespace GUI
    {
		public class EnemyHolder : MonoBehaviour 
		{
            public Text m_strength;
            public Image[] m_modifiers;
            public Image m_token;
            public Enemy.Object enemy { get; private set; }

            private RectTransform m_rTrans;
            private Toggle m_toggle;

            public void Init(Enemy.Object enemy, int i)
            {
                m_toggle = GetComponentInChildren<Toggle>();

                this.enemy = enemy;
                ShowDefense();
                m_token.sprite = enemy.GetImage();

                
                m_rTrans = GetComponent<RectTransform>();
                m_rTrans.anchorMax = new Vector2(1f, 1f - 0.2f * i);
                m_rTrans.anchorMin = new Vector2(0f, 1f - 0.2f * (i + 1));
                m_rTrans.Reset();
            }

            public bool IsSelectable()
            {
                return m_toggle.IsInteractable();
            }

            public void Reset() // Make an enemy selectable again
            {
                m_toggle.interactable = true;
            }

            public void Select() // Used to force selection of an enemy
            {
                m_toggle.isOn = true;
            }

            public void Disable() // Force deselection of an enemy and make it non-selectable
            {
                m_toggle.isOn = false;
                m_toggle.interactable = false;
            }

            public void ToggleEnemySelection() // Add or remove an enemy from the current instance when their toggle value changes
            {
                Rules.Combat.Instance.band.AddOrRemoveEnemy(this);
            }

            public void ShowDefense()
            {
                Enemy.Defense def = enemy.GetDefense();
                m_strength.text = def.strength.ToString();

                for (int i = 0; i < m_modifiers.Length; i++)
                {
                    if (i < def.images.Count)
                    {
                        m_modifiers[i].gameObject.SetActive(true);
                        m_modifiers[i].sprite = def.images[i];
                    }
                    else
                        m_modifiers[i].gameObject.SetActive(false);
                }

            }

            public void ShowAttack()
            {
                Enemy.Attack atk = enemy.GetAttack();
                m_strength.text = atk.strength.ToString();

                for (int i = 0; i < m_modifiers.Length; i++)
                {
                    if (i < atk.images.Count)
                    {
                        m_modifiers[i].gameObject.SetActive(true);
                        m_modifiers[i].sprite = atk.images[i];
                    }
                    else
                        m_modifiers[i].gameObject.SetActive(false);
                }
            }
		}
	}
}