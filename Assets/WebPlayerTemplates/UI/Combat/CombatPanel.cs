using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
	namespace GUI
    {
		public class CombatPanel : MonoBehaviour 
		{
            public PhaseIndicator combatPhases;
            public EnemyArea m_enemyArea { get; private set; }
            public PlayerArea m_playerArea { get; private set; }

            public void Awake()
            {
                m_enemyArea = GetComponentInChildren<EnemyArea>();
                m_playerArea = GetComponentInChildren<PlayerArea>();
            }

            public void StartCombat(List<Enemy.Object> enemies)
            {
                gameObject.SetActive(true);
                combatPhases.StartCombat();
                m_enemyArea.Init(enemies);
            }

            public void NextPhase()
            {
                combatPhases.NextPhase();
                m_enemyArea.ResetAll();
                m_enemyArea.ShowAttackOrDefense();
                m_enemyArea.SelectNext();
                m_playerArea.ResetStrength();
            }

            public void EndCombat()
            {
                m_enemyArea.CleanUp();
                gameObject.SetActive(false);
            }

		}
	}
}