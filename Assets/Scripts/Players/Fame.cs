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
            public float maxTileWidth;
            public float maxWidth;
            public float m_sliderSpeed;
            public GameObject m_fameTilePrefab;
            public GameObject m_backgroundPrefab;
            public Sprite m_levelShield;
            public Sprite m_levelCardAbility;
            public Image m_rewardImg;

            private Slider m_slider;
            private FameTrack m_track;
            private GameObject m_background;
            private int m_total;
            private int m_level;

            public void Init(int playerLevel = 1)
            {
                m_slider = GetComponent<Slider>();

                m_level = playerLevel;
                CreateFameTrack(playerLevel);
                m_total = 0;
                m_slider.value = 0;
            }

            void CreateFameTrack(int level)
            {
                m_track = new FameTrack(level); // Create a new fame struct with max and minimum numbers for this player level
                m_slider.minValue = m_track.min;
                m_slider.maxValue = m_track.max;

                CreateFameBackground(level);
                GetRewardImage(level);
            }

            void CreateFameBackground(int level)
            {
                int numSprites = level * 2 + 1;
                float xWidth = 1.0f / numSprites;

                SetWidth(numSprites);

                if (m_background != null)
                    Destroy(m_background);

                m_background = transform.InstantiateChild(m_backgroundPrefab);
                m_background.GetComponent<RectTransform>().Reset();
                m_background.transform.SetSiblingIndex(0);

                for (int i = 0; i < numSprites; i++)
                {
                    GameObject tile = m_background.transform.InstantiateChild(m_fameTilePrefab);
                    RectTransform rTrans = tile.GetComponent<RectTransform>();
                    rTrans.anchorMin = new Vector2(xWidth * i, 0.25f);
                    rTrans.anchorMax = new Vector2(xWidth * (i + 1), 0.75f);
                    rTrans.Reset();
                    tile.GetComponentInChildren<Text>().text = (m_track.min + i).ToString();
                }
            }

            void SetWidth(int numTiles)
            {
                float width = maxTileWidth * numTiles;
                width = Mathf.Min(width, maxWidth);
                RectTransform rTrans = GetComponent<RectTransform>();
                rTrans.anchorMin = new Vector2(0.5f - width / 2f, 0.9f);
                rTrans.anchorMax = new Vector2(0.5f + width / 2f, 1f);
                rTrans.Reset();
            }

            void GetRewardImage(int level)
            {
                if (level % 2 == 1)
                    m_rewardImg.sprite = m_levelCardAbility;
                else
                    m_rewardImg.sprite = m_levelShield;
            }

            public void AddFame(int value)
            {
                m_total += value;
            }

            void Update()
            {
                m_slider.value = Mathf.MoveTowards(m_slider.value, m_total + 0.5f, m_sliderSpeed * Time.deltaTime);

                if (m_slider.value >= m_track.max)
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
                max = 0;
                min = 0;
                for (int i = 1; i <= playerLevel; i++)
                {
                    min = max;
                    max = min + 1 + 2 * i;
                }
            }
        }
	}
}