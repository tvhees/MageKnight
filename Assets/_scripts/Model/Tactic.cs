using Other.Data;
using Other.Utility;
using UnityEngine.Networking;
using View;

public class Tactic : NetworkBehaviour
{
    public bool active;
    public Card tactic;
    public CardId tacticId;
    public PlayerControl owner;

    public void SetTactic(CardView tacticView)
    {
        tacticId = tacticView.cardId;
        tactic = CardDatabase.GetScriptableObject(tacticId.name);
        active = true;
    }
}