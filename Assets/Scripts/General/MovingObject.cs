using UnityEngine;
using System.Collections;

namespace BoardGame
{
    public class MovingObject : MonoBehaviour
    {
        // Variables controlling speed of changes for this object
        public float speed { get; private set; }
        public float angularSpeed { get; private set; }

        // Vectors for object position 
        public Vector3 m_homePos { get; private set; }
        public Vector3 m_targetPos { get; private set; }

        // Quaternions for object rotation
        public Quaternion m_homeRot { get; private set; }
        public Quaternion m_targetRot { get; private set; }

        public void SetSpeed(float speed)
        {
            this.speed = speed;
        }

        public void SetAngularSpeed(float angularSpeed)
        {
            this.angularSpeed = angularSpeed;
        }

        public float GetSpeed()
        {
            return speed;
        }

        public float GetAngularSpeed()
        {
            return angularSpeed;
        }

        public IEnumerator SetTargetPos(Vector3 newPos, bool wait = false)
        {
            m_targetPos = newPos;

            if (wait)
            {
                yield return StartCoroutine(Game.ObjectMover.MoveUntilFinished(this));
            }
            else
            {
                Game.ObjectMover.MoveThisObject(this);
            }
        }

        public IEnumerator ReturnHome(bool wait = false)
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

        public IEnumerator MoveHomeTowards(Vector3 target, float maxDistance, bool wait = false)
        {
            yield return SetHomePos(Vector3.MoveTowards(m_homePos, target, maxDistance), wait);
        }

        public IEnumerator SetTargetRot(Quaternion newRot, bool wait = false)
        {
            m_targetRot = newRot;

            if (wait)
            {
                yield return StartCoroutine(Game.ObjectMover.RotateUntilFinished(this));
            }
            else
            {
                Game.ObjectMover.RotateThisObject(this);
            }
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
    }
}