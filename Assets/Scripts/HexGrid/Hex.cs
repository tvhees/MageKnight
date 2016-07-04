using UnityEngine;
using System.Collections;

namespace BoardGame
{
    namespace HexGrid
    {
        public class Hex : MonoBehaviour
        {

            private HexCoordinates m_hexCoordinates;

            /// <summary>
            /// Turn a hexagonal coordinate system position in to cubic coordinates
            /// </summary>
            /// <param name="newCoordinates"></param>
            public void SetCoordinates(HexCoordinates newCoordinates)
            {
                // Store the hexagonal coordinates to retrieve later
                // This avoids having to convert backwards
                m_hexCoordinates = newCoordinates;

                transform.position = m_hexCoordinates.WorldCoordinates();;
            }

            /// <summary>
            /// Retrieve hexagonal coordinates
            /// </summary>
            /// <returns></returns>
            public HexCoordinates GetCoordinates()
            {
                return m_hexCoordinates;
            }

#if UNITY_EDITOR
            // Editor code to allow changing coordinates at run time
            void FixedUpdate()
            {
                SetCoordinates(m_hexCoordinates);
            }
#endif
        }

        // Store the dimensions of each hex here
        public static class HexMetrics
        {
            public const float outerRadius = 1f;

            public const float innerRadius = outerRadius * 0.866025404f;
        }

        // Struct that converts hex coordinates in to cubic coordinates
        [System.Serializable]
        public struct HexCoordinates
        {
            public int m_i;
            public int m_j;
            public int m_k;

            /// <summary>
            /// Constructor for HexCoordinates
            /// </summary>
            /// <param name="i"></param>
            /// <param name="j"></param>
            /// <param name="k"></param>
            public HexCoordinates(int i, int j, int k)
            {
                m_i = i;
                m_j = j;
                m_k = k;
            }

            /// <summary>
            /// Defines addition of two HexCoordinates
            /// </summary>
            /// <param name="c1"></param>
            /// <param name="c2"></param>
            /// <returns></returns>
            public static HexCoordinates operator +(HexCoordinates c1, HexCoordinates c2)
            {
                return new HexCoordinates(c1.m_i + c2.m_i, c1.m_j + c2.m_j, c1.m_k + c2.m_k);
            }

            /// <summary>
            /// Returns the world position in cubic coordinates
            /// </summary>
            /// <returns></returns>
            public Vector3 WorldCoordinates()
            {
                return new Vector3((m_i - m_j) * 1.5f * HexMetrics.outerRadius, 0f, (m_i + m_j - 2 * m_k) * HexMetrics.innerRadius);
            }
        }
    }
}