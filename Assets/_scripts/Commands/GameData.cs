using UnityEngine;
using System.Collections;

public class GameData
{
    public PlayerControl player;
    public CardId cardId;
    public HexId hexId;

    public GameData(PlayerControl player = null, CardId cardId = default(CardId), HexId hexId = default(HexId))
    {
        this.player = player;
        this.cardId = cardId;
        this.hexId = hexId;
    }
}
