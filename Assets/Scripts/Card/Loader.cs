using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace BoardGame
{
    namespace Card
    {
        public static class Loader
        {
            public static List<Dictionary<string, string>> LoadCards(TextAsset XML)
            {
                // Dictionary objects to store all card data
                List<Dictionary<string, string>> cardList = new List<Dictionary<string, string>>();
                Dictionary<string, string> obj;

                XmlDocument cardDB = new XmlDocument(); // Create XML container
                cardDB.LoadXml(XML.text); // Load card information stored in XML
                XmlNodeList nodeList = cardDB.GetElementsByTagName("card"); // Create array of nodes, one for each card

                int cardNumber = 0;

                // Run through each node and extract card information
                foreach (XmlNode node in nodeList)
                {
                    XmlNodeList cardInfo = node.ChildNodes; // Get child nodes for current card
                    obj = new Dictionary<string, string>(); // Create a object(Dictionary) to collect the card info and put the card in the cards array.

                    foreach (XmlNode element in cardInfo)
                    {
                        if (element.Name == "string")
                            obj.Add(element.Attributes["name"].Value, element.InnerText);

                        // Read the card effects
                        if (element.Name == "effect")
                        {
                            if (element.Attributes["type"].Value.Contains("choice"))
                            {
                                SaveChoiceInformation(obj, element);
                            }
                            
                            if(element.Attributes["type"].Value.Contains("effect"))
                            {
                                SaveEffectInformation(obj, element);
                            }
                        }
                    }
                    cardList.Add(obj);
                    cardNumber++;
                }

                return cardList;
            }

            static void SaveEffectInformation(Dictionary<string, string> obj, XmlNode element)
            {
                string effectType = element.Attributes["type"].Value;
                string valueType = effectType.Replace("effect", "value");

                foreach (XmlNode effectElement in element)
                    if (effectElement.Name == "string")
                        obj.Add(effectType, effectElement.InnerText);
                    else if (effectElement.Name == "int")
                        obj.Add(valueType, effectElement.InnerText);
            }

            static void SaveChoiceInformation(Dictionary<string, string> obj, XmlNode element)
            {
                string choiceType = element.Attributes["type"].Value;
                obj.Add(choiceType, "Choice");

                foreach (XmlNode choiceElement in element) SaveEffectInformation(obj, choiceElement);
            }
        }
    }
}