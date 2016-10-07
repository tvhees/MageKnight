using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using View;
using Other.Data;
using Other.Utility;

public class Tactic : NetworkBehaviour {

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
