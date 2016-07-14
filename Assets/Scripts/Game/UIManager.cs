using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoardGame
{
    namespace Board
    {
        public class UIManager : Singleton<UIManager>
        {
            public GameObject m_arrowPrefab;
            public GameObject m_signPrefab;
            private List<GameObject> m_pathArrows = new List<GameObject>();
            private List<GameObject> m_pathSigns = new List<GameObject>();


            // Draw a path of arrows across relevant hexes
            public void DrawPath(Vector3 start, Vector3 end, string sign)
            {
                Vector3 midPoint = (end + start) * 0.5f; // Place arrows and numbers at midpoint between tiles
                Quaternion rotation = Quaternion.LookRotation(Vector3.Cross((end - start), Vector3.up)); // arrow prefab is already rotated 90 degrees so we need to make it 'look' at the orthogonal direction
                GameObject arrow = Instantiate(m_arrowPrefab, midPoint, rotation) as GameObject;
                m_pathArrows.Add(arrow);

                // Show total movement cost on number above array
                GameObject number = Instantiate(m_signPrefab, midPoint, Quaternion.Euler(60f, 30f, 0f)) as GameObject;
                number.GetComponent<TextMesh>().text = sign;
                m_pathSigns.Add(number);
            }

            public void ColourNode(int i, Color colour)
            {
                if (i < m_pathSigns.Count)
                    m_pathSigns[i].GetComponent<TextMesh>().color = colour;
            }

            public void DeleteLast()
            {
                // Destroy associated arrow
                GameObject arrow = m_pathArrows.GetLast();
                Destroy(arrow);
                m_pathArrows.RemoveLast();

                // Destroy associated number
                GameObject number = m_pathSigns.GetLast();
                Destroy(number);
                m_pathSigns.RemoveLast();
            }
        }
    }
}