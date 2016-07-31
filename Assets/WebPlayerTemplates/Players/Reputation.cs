using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
	namespace Players
    {
		public class Reputation : MonoBehaviour 
		{
            public float m_sliderSpeed;

            private Slider m_slider;
            private int[] m_track;
            private int m_total;

            public void Init(int rep = 7)
            {
                m_slider = GetComponent<Slider>();

                m_track = new int[15] { -int.MaxValue, -5, -3, -2, -1, -1, 0, 0, 0, 1, 1, 2, 2, 3, 5 };
                m_total = rep;
                m_slider.value = m_total + 0.5f;
            }

            public void AddReputation(int value)
            {
                m_total += value;
                m_total = Mathf.Clamp(m_total, 0, 14);
            }

            void Update()
            {
                m_slider.value = Mathf.MoveTowards(m_slider.value, m_total + 0.5f, m_sliderSpeed * Time.deltaTime);
            }

            public int GetReputationBonus()
            {
                return m_track[m_total];
            }


        }
    }
}