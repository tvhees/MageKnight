using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoardGame
{
	namespace Enemy
    {
		public class Manager : Singleton<Manager> 
		{
            public Camera m_stackCamera;
            public GameObject m_stackHolderPrefab;

            private float m_stackSpacing = 2f;
            private float m_stackCenter = 0.0f;

            private Dictionary<string, List<Object>> m_enemyStacks = new Dictionary<string, List<Object>>();

            public void Init()
            {
                NewStacks();
            }

            void NewStacks()
            {
                for (int i = 0; i < (int)Factory.EnemyType.numberOfTypes; i++)
                {
                    // Make a new folder object for each enemy type and
                    // arrange stacks with regular spacing from top to bottom
                    GameObject stackHolder = transform.InstantiateChild(m_stackHolderPrefab);
                    stackHolder.transform.localPosition = (((float)Factory.EnemyType.numberOfTypes/2f + m_stackCenter) - i) * m_stackSpacing * Vector3.up;

                    // Ask the factory to create a stack of this type of enemy
                    Factory.EnemyType type = (Factory.EnemyType)i; // Cast back to enum
                    List<Object> newStack = Factory.Instance.CreateStack(stackHolder, m_stackCamera, type);

                    // Randomise the order of the enemies in this stack and add it to the dictionary
                    newStack.Randomise(false);
                    m_enemyStacks.Add(type.ToString(), newStack);
                }
            }

            public Object GetEnemy(string type, Transform parentObj)
            {
                List<Object> stack;
                m_enemyStacks.TryGetValue(type, out stack);

                // Take the top enemy off the stack
                Object topOfStack = stack.GetLast();
                stack.RemoveLast();

                // Position enemy in heirarchy and world space
                Vector3 newHome = parentObj.position + 0.1f * Vector3.up;
                topOfStack.transform.position = newHome;
                StartCoroutine(topOfStack.GetComponent<MovingObject>().SetHomePos(newHome));
                topOfStack.transform.SetParent(parentObj);

                return topOfStack;
            }

		}
	}
}