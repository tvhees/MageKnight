using UnityEngine;
using System.Collections;

namespace BoardGame
{
    public class MovingObject : MonoBehaviour
    {

        // Vectors for card position 
        private Vector3 m_homePos;
        private Vector3 m_targetPos;

        public void SetTargetPos(Vector3 newPos)
        {
            Game.ObjectMover.Instance.MoveThisObject(this);
            m_targetPos = newPos;
        }

        public Vector3 GetTargetPos()
        {
            return m_targetPos;
        }

        public void SetHomePos()
        {
            SetTargetPos(m_homePos);
        }

        public void SetHomePos(Vector3 newHomePos)
        {
            m_homePos = newHomePos;
            SetTargetPos(m_homePos);
        }

        public Vector3 GetHomePos()
        {
            return m_homePos;
        }
    }
}