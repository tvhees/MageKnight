using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
	namespace Enemy
    {
		public class Object : MonoBehaviour 
		{
            public Canvas enemyInformation;
            private Component m_attributes;
            public Factory.EnemyType type { get; private set; }
            public MovingObject movingObject { get; private set; }

            private float normalSpeed = 40f;
            private float flipHeight;
            private bool showing;

            public void MouseEntered()
            {
                if (showing)
                    enemyInformation.enabled = true;
            }

            public void MouseExited()
            {
                enemyInformation.enabled = false;
            }

            public void SetAttributes(Component input, Canvas enemyCanvas, Factory.EnemyType type)
            {
                m_attributes = input;
                this.type = type;

                flipHeight = 10f;

                SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
                spriteRenderers[0].sprite = input.image;
                spriteRenderers[1].sprite = input.backImage;

                enemyInformation = enemyCanvas;
                enemyCanvas.transform.SetParent(transform);
                enemyCanvas.transform.localPosition = (2f * Vector3.down);
                enemyCanvas.transform.localRotation = Quaternion.Euler(300f, 150f, 180f);

                movingObject = GetComponent<MovingObject>();
                movingObject.SetSpeed(normalSpeed);
                movingObject.SetAngularSpeed(400f);
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

                movingObject.SetSpeed(2f);

                StartCoroutine(movingObject.SetTargetPos(movingObject.homePos + flipHeight * Vector3.up));
                yield return StartCoroutine(movingObject.SetTargetRot(Quaternion.Euler(90f, 0f, 0f), true));
                StartCoroutine(movingObject.SetTargetRot(Quaternion.Euler(finalAngle, 0f, 0f)));
                yield return new WaitForSeconds(0.1f);
                yield return StartCoroutine(movingObject.ReturnHome(true));

                showing = !showing;

                movingObject.SetSpeed(normalSpeed);
            }
        }
	}
}