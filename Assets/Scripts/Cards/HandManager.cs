using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class HandManager : MonoBehaviour {

    public GameObject m_cardPrefab;
    public int m_startingHandSize;

    private int m_handSize;
    private List<CardObject> m_cardsInHand = new List<CardObject>();

    // ****************
    // INITIALISATION
    // ****************

    // ID defining which player this hand belongs to
    private int m_playerID;

    // Camera showing this hand
    private Camera m_handCamera;

    /// <summary>
    /// Initialise this player's hand
    /// </summary>
    /// <param name="id"></param>
    public void Init (int id) {
        // Give this hand the player's unique id
        m_playerID = id;

        // Store a reference to the camera
        m_handCamera = GetComponentInChildren<Camera>();

        // Create a new hand
        m_handSize = 0;
        RefillHand();
	}


    // **************
    // CREATING AND DRAWING CARDS
    // **************

    void AddCardToHand()
    {
        // Add card to reference lists and variables
        CardObject newCard = transform.InstantiateChild(m_cardPrefab).GetComponent<CardObject>();
        GameManager.m_cardManager.CreateCard(newCard); // Give the card a number and sprites
        m_cardsInHand.Add(newCard);

        // Initialise the card
        Vector3 spawnPos = transform.position + m_handSize * cardSlotWidth / 2f * Vector3.right;
        newCard.Init(CardObject.Location.hand, spawnPos, m_handCamera);
        shiftCardInHand.AddListener(newCard.ShiftInHand);

        m_handSize++;
    }

    void DrawCard()
    {
    }

    void RefillHand()
    {
        for (int i = m_handSize; i < m_startingHandSize; i++)
        {
            shiftCardInHand.Invoke(100f * Vector3.left, cardSlotWidth/2f);
            AddCardToHand();
        }
    }


    // **************
    // MOVING CARDS
    // **************

    private float cardSlotWidth = 0.75f;

    public ShiftCardEvent shiftCardInHand;
}

[System.Serializable]
public class ShiftCardEvent : UnityEvent<Vector3, float>
{
}