using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace BoardGame
{
	namespace Players
    {
		public class Fame : MonoBehaviour 
		{
            public float m_sliderSpeed;

            private Slider m_slider;
            private FameTrack m_track;
            private int m_total;
            private int m_level;

            public void Init(int playerLevel = 1)
            {
                m_slider = GetComponent<Slider>();

                m_level = playerLevel;
                m_track = new FameTrack(m_level);
                m_total = 0;
                m_slider.value = 0;
            }

            void CreateFameTrack(int level)
            {
                m_track = new FameTrack(level);
                m_slider.minValue = m_track.min;
                m_slider.maxValue = m_track.max;
            }

            public void AddFame(int value)
            {
                m_total += value;
            }

            void Update()
            {
                m_slider.value = Mathf.MoveTowards(m_slider.value, m_total, m_sliderSpeed * Time.deltaTime);

                if (m_slider.value > m_track.max)
                {
                    m_level++;
                    CreateFameTrack(m_level);
                    m_slider.value = m_track.min;
                }
            }

		}

        public struct FameTrack
        {
            public int min { get; private set; }
            public int max { get; private set; }

            public FameTrack(int playerLevel)
            {
                max = -1;
                min = 0;
                for (int i = 1; i <= playerLevel; i++)
                {
                    min = max + 1;
                    max = min + 2 * (i);
                }
            }
        }
	}
}