using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardEffects : MonoBehaviour {

    public static void ProcessEffect(int cardID, bool weakEffect)
    {
        Dictionary<string, string> cardInfo = GameManager.m_cardManager.GetCard(cardID);

        string effectName;
        string effectValue;

        if (weakEffect)
        {
            cardInfo.TryGetValue("effect_w", out effectName);
            cardInfo.TryGetValue("value_w", out effectValue);
        }
        else
        {
            cardInfo.TryGetValue("effect_s", out effectName);
            cardInfo.TryGetValue("value_s", out effectValue);
        }

        Debug.Log(effectName + ": " + effectValue);
    }

    void Movement(int effectValue)
    {
        GameManager.m_movement.AddMovement(effectValue);
    }
}
