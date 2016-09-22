using UnityEngine;
using System;

public class GameConstants
{
    #region Numbers
    public const int hexesPerTile = 7;
    #endregion

    #region Types
    public enum CardType
    { Action, Spell, Artifact, Unit, Wound, Tactic }
    #endregion
}
