﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace BoardGame
{
	namespace GUI
    {
		public class EnemyArea : MonoBehaviour 
		{
            public GameObject enemyHolderPrefab;
            public ToggleGroup m_enemyToggles;

            private List<EnemyHolder> enemyHolders;

            public void Init(List<Enemy.Object> enemies)
            {
                enemyHolders = new List<EnemyHolder>();

                for (int i = 0; i < enemies.Count; i++)
                {
                    EnemyHolder instance = transform.InstantiateChild(enemyHolderPrefab, Vector3.zero, Quaternion.identity).GetComponent<EnemyHolder>();
                    enemyHolders.Add(instance);
                    instance.Init(enemies[i], i);
                }

                SelectNext();
            }

            public void SelectNext()
            {
                for (int i = 0; i < enemyHolders.Count; i++)
                {
                    if (enemyHolders[i].IsSelectable())
                    {
                        enemyHolders[i].Select();
                        return;
                    }
                }
            }

            public void DefeatEnemy(List<EnemyHolder> defeatedEnemies)
            {
                for (int i = 0; i < defeatedEnemies.Count; i++)
                {
                    enemyHolders.Remove(defeatedEnemies[i]);
                    Enemy.Manager.Instance.DiscardEnemy(defeatedEnemies[i].enemy);
                }
            }

            public bool IsEmpty()
            {
                return enemyHolders.Count == 0;
            }

            public void ResetAll()
            {
                foreach (EnemyHolder holder in enemyHolders)
                    holder.Reset();
            }

            public void ToggleGroup(bool on)
            {
                if(on)
                    foreach (EnemyHolder holder in enemyHolders)
                        GetComponentInChildren<Toggle>().group = m_enemyToggles;
                else
                    foreach (EnemyHolder holder in enemyHolders)
                        GetComponentInChildren<Toggle>().group = null;
            }

            public void ShowAttackOrDefense()
            {
                if (Rules.Combat.Instance.phase == Rules.Combat.Phase.block)
                {
                    foreach (EnemyHolder obj in enemyHolders)
                        obj.ShowAttack();
                }
                else
                {
                    foreach (EnemyHolder obj in enemyHolders)
                        obj.ShowDefense();
                }
            }

            public void CleanUp()
            {
                for (int i = 0; i < enemyHolders.Count; i++)
                {
                    Destroy(enemyHolders[i].gameObject);
                }
            }

		}
	}
}