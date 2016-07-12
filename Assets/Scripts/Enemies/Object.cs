using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoardGame
{
	namespace Enemy
    {
		public class Object : MonoBehaviour 
		{
            public bool debugMode;

            private Enemy m_attributes;
            private MovingObject movingObject;
            public Canvas enemyInformation;

            private float flipHeight;
            private bool showing;

            
            public void OnMouseUpAsButton()
            {
                if (debugMode)
                {
                    StartCoroutine(Flip());
                }
            }

            public void OnMouseOver()
            {
                if (showing)
                    enemyInformation.enabled = true;
            }

            public void OnMouseExit()
            {
                enemyInformation.enabled = false;
            }

            public void Awake()
            {

            }

            public void SetAttributes(Enemy input, Canvas enemyCanvas)
            {
                m_attributes = input;

                flipHeight = 10f;
                enemyInformation = enemyCanvas;
                enemyCanvas.transform.SetParent(transform);
                enemyCanvas.transform.localPosition = (2f * Vector3.down);
                enemyCanvas.transform.localRotation = Quaternion.Euler(300f, 150f, 180f);

                movingObject = GetComponent<MovingObject>();
                movingObject.SetHomeRot(Quaternion.identity);
                showing = false;
            }

            public void CheckRampaging()
            {
                if (GetComponent<Rampaging>() != null)
                {
                    StartCoroutine(Flip());
                }
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

            public Sprite GetImage()
            {
                return m_attributes.image;
            }

            // HACKY CODE because tokens are offset
            public IEnumerator Flip()
            {
                float finalAngle;

                if (showing)
                    finalAngle = 0f;
                else
                    finalAngle = 180f;

                StartCoroutine(movingObject.SetTargetPos(movingObject.GetHomePos() + flipHeight * Vector3.up));
                yield return StartCoroutine(movingObject.SetTargetRot(Quaternion.Euler(90f, 0f, 0f), true));
                StartCoroutine(movingObject.SetTargetRot(Quaternion.Euler(finalAngle, 0f, 0f)));
                yield return new WaitForSeconds(0.1f);
                StartCoroutine(movingObject.SetHomePos());

                showing = !showing;
            }
        }
	}
}