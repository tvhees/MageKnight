using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    namespace Game
    {
        public class ObjectMover : Singleton<ObjectMover>
        {
            //************
            // MOVING IN SCENE
            //************

            public static List<MovingObject> m_objectsToMove = new List<MovingObject>(); // Store all cards that should be moved during update
            public static List<MovingObject> m_objectsToRotate = new List<MovingObject>(); // Store all cards that should be rotated during update


            public static void MoveThisObject(MovingObject obj)
            {
                if(obj.GetSpeed() > 0f)
                    m_objectsToMove.Add(obj);
            }

            public static void RotateThisObject(MovingObject obj)
            {
                if(obj.GetAngularSpeed() > 0f)
                    m_objectsToRotate.Add(obj);
            }

            public static IEnumerator MoveUntilFinished(MovingObject obj)
            {
                m_objectsToMove.Add(obj);

                while (m_objectsToMove.Contains(obj))
                {
                    yield return null;
                }
            }

            public static IEnumerator RotateUntilFinished(MovingObject obj)
            {
                m_objectsToRotate.Add(obj);

                while (m_objectsToRotate.Contains(obj))
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
                    Vector3 tar = obj.m_targetPos;
                    obj.transform.position = pos = Vector3.MoveTowards(pos, tar, obj.GetSpeed() * Time.deltaTime);

                    // Stop tracking this card's movement if it reaches its destination
                    if ((tar - pos).sqrMagnitude < Mathf.Epsilon)
                    {
                        m_objectsToMove.Remove(obj);
                    }
                }

                for (int i = m_objectsToRotate.Count - 1; i >= 0; i--) // Count backwards to allow removing cards as we go
                {
                    // Get the card's current position and move it towards the card's new position
                    MovingObject obj = m_objectsToRotate[i];
                    Quaternion rot = obj.transform.localRotation;
                    Quaternion tar = obj.m_targetRot;
                    obj.transform.localRotation = rot = Quaternion.RotateTowards(rot, tar, obj.GetAngularSpeed() * Time.deltaTime);

                    // Stop tracking this card's movement if it reaches its destination
                    if (Quaternion.Angle(tar, rot) < Mathf.Epsilon)
                    {
                        m_objectsToRotate.Remove(obj);
                    }
                }
            }
        }
    }
}