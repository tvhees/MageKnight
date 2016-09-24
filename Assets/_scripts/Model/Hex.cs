using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hex : MonoBehaviour
{
    public HexId Id;

    public bool isTraversable
    {
        get { return GameController.singleton.movementCosts.IsTraversable(Id.terrain); }
    }

    public int movementCost
    {
        get { return GameController.singleton.movementCosts.MoveCost(Id.terrain); }
    }
}