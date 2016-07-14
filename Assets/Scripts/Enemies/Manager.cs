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

            private Dictionary<string, List<Object>> enemyStacks = new Dictionary<string, List<Object>>();
            private Dictionary<string, List<Object>> enemyDiscardStacks = new Dictionary<string, List<Object>>();

            private Dictionary<string, GameObject> stackHolders = new Dictionary<string, GameObject>();
            private Dictionary<string, GameObject> discardHolders = new Dictionary<string, GameObject>();

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
                    GameObject discardHolder = transform.InstantiateChild(m_stackHolderPrefab);
                    stackHolder.transform.localPosition = (((float)Factory.EnemyType.numberOfTypes/2f + m_stackCenter) - i) * m_stackSpacing * Vector3.up;
                    discardHolder.transform.localPosition = stackHolder.transform.localPosition + 1.5f * Vector3.left;

                    // Ask the factory to create a stack of this type of enemy
                    Factory.EnemyType type = (Factory.EnemyType)i; // Cast back to enum
                    stackHolder.name = type.ToString() + " stack";
                    discardHolder.name = type.ToString() + " discard";

                    List<Object> newStack = Factory.Instance.CreateStack(stackHolder, m_stackCamera, type);

                    // Randomise the order of the enemies in this stack and add it to the dictionary
                    newStack.Randomise(false);
                    enemyStacks.Add(type.ToString(), newStack);
                    stackHolders.Add(type.ToString(), stackHolder);

                    // Create an empty discard stack for defeated enemies
                    List<Object> discardStack = new List<Object>();
                    enemyDiscardStacks.Add(type.ToString(), discardStack);
                    discardHolders.Add(type.ToString(), discardHolder);
                }
            }

            public Object GetEnemy(string type, Transform parentObj)
            {
                List<Object> stack;
                enemyStacks.TryGetValue(type, out stack);
                GameObject stackHolder;
                stackHolders.TryGetValue(type, out stackHolder);

                if (stack.Count == 0)
                    RefillStack(stack, stackHolder, type);

                // Take the top enemy off the stack
                Object topOfStack = stack.GetLast();
                stack.RemoveLast();

                // Position enemy in heirarchy and world space
                Vector3 newHome = parentObj.position + 0.1f * Vector3.up;
                topOfStack.transform.position = newHome;
                StartCoroutine(topOfStack.movingObject.SetHomePos(newHome));
                topOfStack.transform.SetParent(parentObj);

                return topOfStack;
            }

            public void DiscardEnemy(Object enemy)
            {
                // Add enemy to list of discarded enemies of this type
                List<Object> discardList;
                enemyDiscardStacks.TryGetValue(enemy.type.ToString(), out discardList);
                discardList.Add(enemy);

                // Move token to the discard location
                GameObject discardHolder;
                discardHolders.TryGetValue(enemy.type.ToString(), out discardHolder);
                enemy.transform.SetParent(discardHolder.transform);
                StartCoroutine(enemy.movingObject.SetHomePos(discardHolder.transform.position));
            }

            public void RefillStack(List<Object> stackList, GameObject stackHolder, string type)
            {
                List<Object> discardList;
                enemyDiscardStacks.TryGetValue(type, out discardList);

                foreach (Object enemy in discardList)
                {
                    stackList.Add(enemy);
                    enemy.transform.SetParent(stackHolder.transform);
                    enemy.movingObject.SetHomePos(stackHolder.transform.position);
                }

                stackList.Randomise(false);
            }
		}
	}
}