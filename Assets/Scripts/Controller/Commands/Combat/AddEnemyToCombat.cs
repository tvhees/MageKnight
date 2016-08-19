using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
    public class AddEnemyToCombat : Command
    {
        GameObject enemy;


        public AddEnemyToCombat(GameObject enemy)
        {
            Model.Turn turn = Main.turn;
            this.enemy = enemy;

            if(turn.currentState != turn.GetCombatState())
                requirements.Add(new ChangeTurnState(Main.turn.GetCombatState()));
        }

        protected override CommandResult ExecuteThisCommand()
        {
            Debug.Log(string.Format("Adding {0} to combat.", enemy.name));
            return CommandResult.success;
        }

        protected override void UndoThisCommand()
        {
            Debug.Log(string.Format("Removing {0} from combat.", enemy.name));
        }
    }
}