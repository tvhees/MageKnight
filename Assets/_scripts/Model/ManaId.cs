using UnityEngine;
using System.Collections;

public struct ManaId {
    public int index;
    public GameConstants.ManaType colour;

    public ManaId(int index, GameConstants.ManaType colour)
    {
        this.index = index;
        this.colour = colour;
    }
}
