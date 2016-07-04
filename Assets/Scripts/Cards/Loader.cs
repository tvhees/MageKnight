using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace BoardGame
{
    namespace Cards
    {
        public static class Loader
        {
            public static List<Dictionary<string, string>> LoadCards(TextAsset XML)
            {
                // Dictionary objects to store all card data
                List<Dictionary<string, string>> cardList = new List<Dictionary<string, string>>();
                Dictionary<string, string> m_obj;

                XmlDocument cardDB = new XmlDocument(); // Create XML container
                cardDB.LoadXml(XML.text); // Load card information stored in XML
                XmlNodeList nodeList = cardDB.GetElementsByTagName("card"); // Create array of nodes, one for each card

                int cardNumber = 0;

                // Run through each node and extract card information
                foreach (XmlNode node in nodeList)
                {
                    XmlNodeList cardInfo = node.ChildNodes; // Get child nodes for current card
                    m_obj = new Dictionary<string, string>(); // Create a object(Dictionary) to collect the card info and put the card in the cards array.

                    foreach (XmlNode element in cardInfo)
                    {
                        // Examples of XML tags to process
                        if (element.Name == "string")
                            m_obj.Add(element.Attributes["name"].Value, element.InnerText);

                        if (element.Name == "effect")
                            switch (element.Attributes["type"].Value)
                            {
                                case "effect_1":
                                    foreach (XmlNode effectElement in element)
                                        if (effectElement.Name == "string")
                                            m_obj.Add("effect_1", effectElement.InnerText);
                                        else if (effectElement.Name == "int")
                                            m_obj.Add("value_1", effectElement.InnerText);
                                    break;
                                case "effect_2":
                                    foreach (XmlNode effectElement in element)
                                        if (effectElement.Name == "string")
                                            m_obj.Add("effect_2", effectElement.InnerText);
                                        else if (effectElement.Name == "int")
                                            m_obj.Add("value_2", effectElement.InnerText);
                                    break;
                            }
                    }
                    cardList.Add(m_obj);
                    cardNumber++;
                }

                return cardList;
            }
        }
    }
}