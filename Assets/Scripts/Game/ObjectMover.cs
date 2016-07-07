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

            public List<MovingObject> m_objectsToMove = new List<MovingObject>(); // Store all cards that should be moved during update
            public List<MovingObject> m_objectsToRotate = new List<MovingObject>(); // Store all cards that should be rotated during update

            public bool debugMode;

            public void MoveThisObject(MovingObject obj)
            {
                if(obj.GetSpeed() > 0f)
                    m_objectsToMove.Add(obj);
            }

            public void RotateThisObject(MovingObject obj)
            {
                if(obj.GetAngularSpeed() > 0f)
                    m_objectsToRotate.Add(obj);
            }

            public IEnumerator MoveUntilFinished(MovingObject obj)
            {
                m_objectsToMove.Add(obj);

                while (m_objectsToMove.Contains(obj))
                {
                    yield return null;
                }
            }

            public IEnumerator RotateUntilFinished(MovingObject obj)
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
                    Vector3 tar = obj.GetTargetPos();
                    obj.transform.position = pos = Vector3.MoveTowards(pos, tar, obj.GetSpeed() * Time.deltaTime);

                    // Stop tracking this card's movement if it reaches its destination
                    if ((tar - pos).sqrMagnitude < Mathf.Epsilon)
                    {
                        m_objectsToMove.Remove(obj);
                        if(debugMode)
                            Debug.Log("Objects to move: " + m_objectsToMove.Count + " - " + obj.name);
                    }
                }

                for (int i = m_objectsToRotate.Count - 1; i >= 0; i--) // Count backwards to allow removing cards as we go
                {
                    // Get the card's current position and move it towards the card's new position
                    MovingObject obj = m_objectsToRotate[i];
                    Quaternion rot = obj.transform.localRotation;
                    Quaternion tar = obj.GetTargetRot();
                    obj.transform.localRotation = rot = Quaternion.RotateTowards(rot, tar, obj.GetAngularSpeed() * Time.deltaTime);

                    // Stop tracking this card's movement if it reaches its destination
                    if (Quaternion.Angle(tar, rot) < Mathf.Epsilon)
                    {
                        Debug.Log(tar.eulerAngles + " " + rot.eulerAngles);
                        m_objectsToRotate.Remove(obj);
                        if(debugMode)
                            Debug.Log("Objects to Rotate: " + m_objectsToRotate.Count + " - " + obj.name);
                    }
                }
            }
        }
    }
}