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
                    GameObject stackHolder = transform.InstantiateChild(m_stackHolderPrefab);
                    // Arrange stacks with regular spacing from top to bottom
                    stackHolder.transform.localPosition = (((float)Factory.EnemyType.numberOfTypes/2f + m_stackCenter) - i) * m_stackSpacing * Vector3.up;
                    Factory.EnemyType type = (Factory.EnemyType)i; // Cast back to enum
                    List<Object> newStack = Factory.Instance.CreateStack(stackHolder, m_stackCamera, type);
                    m_enemyStacks.Add(type.ToString(), newStack);
                }
            }

		}
	}
}