using UnityEngine;

namespace Other.Utility
{
    // This class transforms a 3-coordinate hexagonal vector in to world coordinates on the X-Z plane
    // One unit of i --> one hex NE (60 degrees from x axis)
    // One unit of j --> one hex W (-'ve x direction)
    // One unit k --> one hex SE (-60 degrees from x axis)
    [System.Serializable]
    public class HexVector
    {
        public int i;
        public int j;
        public int k;

        public HexVector(int i, int j, int k)
        {
            this.i = i;
            this.j = j;
            this.k = k;
        }

        public static HexVector operator +(HexVector one, HexVector two)
        {
            return new HexVector(one.i + two.i, one.j + two.j, one.k + two.k);
        }

        public float x
        {
            get { return (i - j) * 1.5f * HexMetrics.outerRadius; }
        }

        public float y
        {
            get { return 0f; }
        }

        public float z
        {
            get { return (i + j - 2 * k) * HexMetrics.innerRadius; }
        }

        public Vector3 worldVector
        {
            get { return new Vector3(x, y, z); }
        }
    }

    public static class HexMetrics
    {
        public const float outerRadius = 1f;

        public const float innerRadius = outerRadius * 0.866025404f;
    }
}