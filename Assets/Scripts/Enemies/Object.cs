using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoardGame
{
	namespace Enemy
    {
		public class Object : MonoBehaviour 
		{
            private Enemy m_attributes;
            private MovingObject movingObject;

            private float flipHeight;
            private bool showing;

            public void OnMouseUpAsButton()
            {
                if (showing)
                    StartCoroutine(Flip(0f));
                else
                    StartCoroutine(Flip(180f));

                showing = !showing;
            }

            public void Awake()
            {
                flipHeight = 10f;
                movingObject = GetComponent<MovingObject>();
                movingObject.SetHomeRot(Quaternion.identity);
                showing = false;
            }

            public void SetAttributes(Enemy input)
            {
                m_attributes = input;
            }

            public Attack GetAttack()
            {
                return m_attributes.attack;
            }

            public Defense GetDefense()
            {
                return m_attributes.defense;
            }

            public Reward GetReward()
            {
                return m_attributes.reward;
            }

            // HACKY CODE because tokens are offset
            public IEnumerator Flip(float finalAngle)
            {
                StartCoroutine(movingObject.SetTargetPos(movingObject.GetHomePos() + flipHeight * Vector3.up));
                yield return StartCoroutine(movingObject.SetTargetRot(Quaternion.Euler(90f, 0f, 0f), true));
                StartCoroutine(movingObject.SetTargetRot(Quaternion.Euler(finalAngle, 0f, 0f)));
                yield return new WaitForSeconds(0.1f);
                StartCoroutine(movingObject.SetHomePos());
            }
        }
	}
}