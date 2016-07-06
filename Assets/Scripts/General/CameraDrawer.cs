using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoardGame
{
	public class CameraDrawer : MonoBehaviour 
	{
        public Rect m_minimumSize;
        public Rect m_maximumSize;
        public float m_speed = 1f;
        private Rect m_targetSize;
        private Camera m_camera;
        private bool m_visible;

        void Update()
        {
            m_camera.rect = m_camera.rect.MoveTowards(m_targetSize, m_speed * Time.deltaTime);
        }

        void Awake()
        {
            m_camera = GetComponent<Camera>();
            m_camera.rect = m_targetSize = m_minimumSize;
            Disable();
        }

        void Enable()
        {
            m_camera.enabled = true;
            m_visible = true;
        }

        void Disable()
        {
            m_camera.enabled = false;
            m_visible = false;
        }

        public void Slide()
        {
            if (!m_visible)
            {
                Enable();
                m_targetSize = m_maximumSize;
            }
            else
            {
                m_targetSize = m_minimumSize;
                Disable();
            }
        }
	}
}