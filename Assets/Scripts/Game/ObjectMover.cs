using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoardGame
{
    namespace Game
    {
        public class ObjectMover : Singleton<ObjectMover>
        {
            //************
            // MOVING IN SCENE
            //************

            List<MovingObject> m_objectsToMove = new List<MovingObject>(); // Store all cards that should be moved during update here

            [SerializeField]
            private float m_speed = 100f;

            public void MoveThisObject(MovingObject obj)
            {
                m_objectsToMove.Add(obj);
            }

            public IEnumerator MoveUntilFinished(MovingObject obj)
            {
                m_objectsToMove.Add(obj);

                while (m_objectsToMove.Contains(obj))
                {
                    yield return null;
                }
            }

            void FixedUpdate()
            {
                for (int i = m_objectsToMove.Count - 1; i >= 0; i--) // Count backwards to allow removing cards as we go
                {
                    // Get the card's current position and move it towards the card's new position
                    MovingObject obj = m_objectsToMove[i];
                    Vector3 pos = obj.transform.position;
                    Vector3 tar = obj.GetTargetPos();
                    obj.transform.position = pos = Vector3.MoveTowards(pos, tar, m_speed * Time.deltaTime);

                    // Stop tracking this card's movement if it reaches its destination
                    if ((tar - pos).sqrMagnitude < Mathf.Epsilon)
                        m_objectsToMove.Remove(obj);
                }
            }
        }
    }
}