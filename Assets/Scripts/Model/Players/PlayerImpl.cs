using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
    public class PlayerImpl: MonoBehaviour
	{
        public GameObject playerObject { get; set; }
        public GameObject tile;

        public Player.Variables variables { get; private set; }
        public Player.MovementPath movementPath { get; private set; }

        public Vector3 position { get { return playerObject.transform.position; } }

        Vector3 center { get { return position + new Vector3(0f, transform.localScale.y * 0.5f, 0f); } }

        public void NewVariables()
        {
            variables = new Player.Variables();
        }

        public void NewMovementPath()
        {
            movementPath = new Player.MovementPath(this);
        }

        public void AddInfluence(int value)
        {
            variables.influence += value;
        }

        public void AddMovement(int value)
        {
            variables.movement += value;
        }
    }
}