using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
    public class InfluenceLocals : Command
    {
        GameObject shop;
        PlayerImpl player;

        public InfluenceLocals(GameObject shop)
        {
            Model.Turn turn = Main.turn;
            player = Main.players.currentPlayer;
            this.shop = shop;

            if (turn.currentState != turn.GetInfluenceState())
                requirements.Add(new ChangeTurnState(turn.GetInfluenceState()));
        }

        protected override CommandResult ExecuteThisCommand()
        {
            Debug.Log("Interacting With Locals");
            return CommandResult.success;
        }

        protected override void UndoThisCommand()
        {
            Debug.Log("Undoing Interaction With Locals");
        }
    }
}