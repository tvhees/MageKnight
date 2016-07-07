using UnityEngine;
using System.Collections;

namespace BoardGame
{
    public class MovingObject : MonoBehaviour
    {
        // Variables controlling speed of changes for this object
        [SerializeField]
        private float m_speed;
        [SerializeField]
        private float m_angularSpeed;

        // Vectors for object position 
        private Vector3 m_homePos;
        private Vector3 m_targetPos;

        // Quaternions for object rotation
        private Quaternion m_homeRot;
        private Quaternion m_targetRot;

        public float GetSpeed()
        {
            return m_speed;
        }

        public float GetAngularSpeed()
        {
            return m_angularSpeed;
        }

        public IEnumerator SetTargetPos(Vector3 newPos, bool wait = false)
        {
            m_targetPos = newPos;

            if (wait)
            {
                yield return StartCoroutine(Game.ObjectMover.Instance.MoveUntilFinished(this));
            }
            else
            {
                Game.ObjectMover.Instance.MoveThisObject(this);
            }
        }

        public Vector3 GetTargetPos()
        {
            return m_targetPos;
        }

        public IEnumerator SetHomePos(bool wait = false)
        {
            if (wait)
                yield return StartCoroutine(SetTargetPos(m_homePos, true));
            else
                StartCoroutine(SetTargetPos(m_homePos));
        }

        public IEnumerator SetHomePos(Vector3 newHomePos, bool wait = false)
        {
            m_homePos = newHomePos;
            yield return StartCoroutine(SetTargetPos(m_homePos, wait));
        }

        public Vector3 GetHomePos()
        {
            return m_homePos;
        }

        public IEnumerator SetTargetRot(Quaternion newRot, bool wait = false)
        {
            m_targetRot = newRot;

            if (wait)
            {
                yield return StartCoroutine(Game.ObjectMover.Instance.RotateUntilFinished(this));
            }
            else
            {
                Game.ObjectMover.Instance.RotateThisObject(this);
            }
        }

        public Quaternion GetTargetRot()
        {
            return m_targetRot;
        }

        public IEnumerator SetHomeRot(bool wait = false)
        {
            if(wait)
                yield return StartCoroutine(SetTargetRot(m_homeRot, true));
            else
                StartCoroutine(SetTargetRot(m_homeRot, true));
        }

        public IEnumerator SetHomeRot(Quaternion newHomeRot, bool wait = false)
        {
            m_homeRot = newHomeRot;
            yield return StartCoroutine(SetTargetRot(m_homeRot, wait));
        }

        public Quaternion GetHomeRot()
        {
            return m_homeRot;
        }
    }
}