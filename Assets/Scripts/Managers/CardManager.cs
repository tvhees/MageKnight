using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;

public class CardManager : MonoBehaviour {

    public void Init()
    {
        LoadCards();
        ShuffleDecks();
    }

    public Sprite[] m_cardImages;

    public List<int> cardsToAssign = new List<int>();

    public void CreateCard(CardObject newCard)
    {
        int cardNumber = cardsToAssign[0];
        cardsToAssign.RemoveAt(0);

        newCard.SetID(cardNumber);

        newCard.SetSprites(GetCardFront(cardNumber), m_cardImages[0]);
    }

    private Sprite GetCardFront(int cardNumber)
    {        
        // Return the appropriate sprite
        return m_cardImages[cardNumber];
    }

    /// <summary>
    /// Construct randomised decks of cards for a new game
    /// </summary>
    public void ShuffleDecks()
    {
        cardsToAssign.AddIntRange(1, m_cardImages.Length - 1);
        cardsToAssign.Randomise(false);
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

        // Run through each node and extract card information
        foreach(XmlNode card in cardList)
        {
            XmlNodeList cardInfo = card.ChildNodes; // Get child nodes for current card
            m_obj = new Dictionary<string, string>(); // Create a object(Dictionary) to collect the card info and put the card in the cards array.
            string cardName = "march"; // Prepare to store the card's name
            int cardNumber = 0; // Prepare to store the card's number

            foreach (XmlNode cardElement in cardInfo)
            {
                if (cardElement.Name == "number")
                    cardNumber = int.Parse(cardElement.InnerText);

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
        }
    }
}