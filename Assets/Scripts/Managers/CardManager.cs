using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class CardManager : MonoBehaviour {

    //************
    // MOVING CARDS
    //************

    List<CardObject> cardsToMove = new List<CardObject>(); // Store all cards that should be moved during update here

    float m_zoomSpeed = 100f;

    public void MoveThisCard(CardObject card)
    {
        cardsToMove.Add(card);
    }

    void Update()
    {
        for (int i = cardsToMove.Count -1; i >= 0; i--) // Count backwards to allow removing cards as we go
        {
            CardObject card = cardsToMove[i];
            Vector3 pos = card.transform.position;
            Vector3 tar = card.GetTargetPos();
            card.transform.position = pos = Vector3.MoveTowards(pos, tar, m_zoomSpeed * Time.deltaTime);

            // Stop tracking this card's movement if it reaches its destination
            if ((tar - pos).sqrMagnitude < Mathf.Epsilon)
                cardsToMove.Remove(card);
        }
    }



    //************
    // CARD EFFECTS
    //************

    private int m_effectValue;

    public IEnumerator ProcessEffect(int cardID, bool weakEffect)
    {
        // Get the dictionary entry for this card
        Dictionary<string, string> cardInfo = GameManager.m_cardManager.GetCard(cardID);
        string effectName;
        string effectValue;

        // Find the type and value of the effect in the dictionary - search term depends on if we
        // are using the strong or weak effect of the card
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

        // Convert the effect type to int and send it to the named coroutine
        int.TryParse(effectValue, out m_effectValue);
        yield return StartCoroutine(effectName, m_effectValue);
    }

    public void UsedEffect()
    {

    }

    public void FailedEffect()
    {
    }

    //************
    // CREATING DECKS
    //************
    public enum DeckType
    {
        player,
        action,
        spell,
        artifact
    }

    public GameObject m_cardPrefab;

    public List<CardObject> CreateDeck(GameObject deck, Camera camera, DeckType type)
    {
        int[] cardNumbers = new DeckList(type).cards;
        cardNumbers.Randomise(false);

        List<CardObject> deckList = new List<CardObject>();

        for (int i = 0; i < cardNumbers.Length; i++)
        {
            CardObject newCard = deck.transform.InstantiateChild(m_cardPrefab).GetComponent<CardObject>();
            CreateCard(newCard, cardNumbers[i]);
            deckList.Add(newCard);
        }

        return deckList;
    }

    //************
    // CARD FACTORY
    //************
    public Sprite[] m_cardImages;

    public void CreateCard(CardObject newCard, int cardNumber)
    {
        newCard.SetID(cardNumber);

        newCard.SetSprites(GetCardFront(cardNumber), m_cardImages[0]);
    }

    private Sprite GetCardFront(int cardNumber)
    {        
        // Return the appropriate sprite
        return m_cardImages[cardNumber];
    }

    public Dictionary<string, string> GetCard(int id)
    {
        return m_cards[id];
    }

    // XML file reference
    public TextAsset m_cardXML;

    // Dictionary objects to store all card data
    List<Dictionary<string, string>> m_cards = new List<Dictionary<string, string>>();
    Dictionary<string, string> m_obj;

    // Dictionary objects to store link between name and card number
    Dictionary<string, int> m_nameRef = new Dictionary<string, int>();

    public void LoadCards()
    {
        XmlDocument cardDB = new XmlDocument(); // Create XML container
        cardDB.LoadXml(m_cardXML.text); // Load card information stored in XML
        XmlNodeList cardList = cardDB.GetElementsByTagName("card"); // Create array of nodes, one for each card
   
        int cardNumber = 0;

        // Run through each node and extract card information
        foreach(XmlNode card in cardList)
        {
            XmlNodeList cardInfo = card.ChildNodes; // Get child nodes for current card
            m_obj = new Dictionary<string, string>(); // Create a object(Dictionary) to collect the card info and put the card in the cards array.
            string cardName = "march"; // Prepare to store the card's name

            foreach (XmlNode cardElement in cardInfo)
            {
                if(cardElement.Name == "string")
                    switch (cardElement.Attributes["name"].Value)
                    {
                        case "name":
                            cardName = cardElement.InnerText;
                            m_obj.Add("name", cardElement.InnerText);
                            break;
                        case "type":
                            m_obj.Add("type", cardElement.InnerText);
                            break;
                        case "colour":
                            m_obj.Add("colour", cardElement.InnerText);
                            break;
                    }

                if(cardElement.Name == "effect")
                    switch (cardElement.Attributes["type"].Value)
                    {
                        case "weak":
                            foreach (XmlNode effectElement in cardElement)
                                if (effectElement.Name == "string")
                                    m_obj.Add("effect_w", effectElement.InnerText);
                                else if (effectElement.Name == "int")
                                    m_obj.Add("value_w", effectElement.InnerText);
                            break;
                        case "strong":
                            foreach (XmlNode effectElement in cardElement)
                                if (effectElement.Name == "string")
                                    m_obj.Add("effect_s", effectElement.InnerText);
                                else if (effectElement.Name == "int")
                                    m_obj.Add("value_s", effectElement.InnerText);
                            break;
                    }
            }

            m_nameRef.Add(cardName, cardNumber);
            m_cards.Add(m_obj);
            cardNumber++;
        }
    }

    //***********
    // CARD EFFECT METHODS
    //***********

    IEnumerator Movement(int effectValue)
    {
        if (true)
        {
            GameManager.m_movement.AddMovement(effectValue);
            UsedEffect();
        }
        else
        {
            FailedEffect();
        }

        yield return null;
    }

    IEnumerator Attack(int effectValue)
    {
        yield return null;
    }

    IEnumerator Ranged(int effectValue)
    {
        yield return null;
    }

    IEnumerator Influence(int effectValue)
    {
        yield return null;
    }

    IEnumerator Tranquility(int effectValue)
    {
        yield return null;
    }

    IEnumerator Threaten(int effectValue)
    {
        Influence(effectValue);
        // Lose reputation = effectValue

        yield return null;
    }

    IEnumerator Crystallize(int effectValue)
    {
        // Get a mana crystal
        // cost = effectValue

        yield return null;
    }

    IEnumerator ManaDraw(int effectValue)
    {
        // 1 = use additional mana die
        // 2 = set mana die + get two tokens

        yield return null;
    }

    IEnumerator Concentration(int effectiveValue)
    {
        // 1 = gain red/blue/white token
        // 2 = use strong effect with +effectiveValue if move, influence, block, attack

        yield return null;
    }

    IEnumerator Improvisation(int effectiveValue)
    {
        // Discard card for 3/5 of miba

        yield return null;
    }
}

[System.Serializable]
public struct DeckList
{
    public int[] cards;

    public DeckList(CardManager.DeckType type) {
        cards = new int[0];
        switch (type)
        {
            case CardManager.DeckType.player:
                cards = new int[16] { 1, 1, 2, 3, 3, 4, 4, 5, 5, 6, 7, 8, 9, 10, 11, 12 };
                break;
            case CardManager.DeckType.action:
                break;
            case CardManager.DeckType.spell:
                break;
            case CardManager.DeckType.artifact:
                break;
        }
    }
}
