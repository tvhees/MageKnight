using UnityEngine;
using System.Collections;

public struct ManaId {
    public int index;
    public GameConstants.ManaType colour;
    public bool selected;

    public ManaId(int index, GameConstants.ManaType colour = GameConstants.ManaType.Red, bool selected = false)
    {
        this.index = index;
        this.colour = colour;
        this.selected = selected;
    }
}
