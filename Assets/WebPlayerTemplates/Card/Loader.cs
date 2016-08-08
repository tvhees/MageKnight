using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace Boardgame
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

                    int effectNumber = 0;
                    foreach (XmlNode element in cardInfo)
                    {
                        if (element.Name == "string")
                            AddToDictionary(obj, element.Attributes["name"].Value, element.InnerText);

                        // Read the card effects
                        if (element.Name == "effect")
                        {
                            if(element.Attributes["type"].Value.Contains("effect")) // Single effect entries
                            {
                                SaveEffectInformation(obj, element, effectNumber.ToString());
                            }

                            if(element.Attributes["type"].Value.Contains("choice") || element.Attributes["type"].Value.Contains("combine"))
                            {
                                SaveMultipleEffectInformation(obj, element, effectNumber.ToString());
                            }
                            effectNumber++;
                        }
                    }
                    cardList.Add(obj);
                    cardNumber++;
                }

                return cardList;
            }

            static void SaveEffectInformation(Dictionary<string, string> obj, XmlNode element, string identifier)
            {
                string effectType = element.Attributes["type"].Value + "_" + identifier;
                string valueType = effectType.Replace("effect", "value");

                foreach (XmlNode effectElement in element)
                    if (effectElement.Name == "string")
                        AddToDictionary(obj, effectType, effectElement.InnerText);
                    else if (effectElement.Name == "int")
                        AddToDictionary(obj, valueType, effectElement.InnerText);
            }

            static void SaveMultipleEffectInformation(Dictionary<string, string> obj, XmlNode element, string identifier)
            {
                string multipleEffectType = element.Attributes["type"].Value + "_" + identifier;
                AddToDictionary(obj, multipleEffectType, "Multiple");

                char[] subChar = new char[10] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j' };
                int i = 0;

                foreach (XmlNode subEffect in element)
                {
                    string subIdentifier = identifier + subChar[i];
                    SaveEffectInformation(obj, subEffect, subIdentifier);
                    i++;
                    
                }
            }

            static void AddToDictionary(Dictionary<string, string> dictionary, string key, string value)
            {
                try
                {
                    dictionary.Add(key, value);
                }
                catch (System.Exception e)
                {
                    Debug.Log(string.Format("Error adding to dictionary: key = {0} value = {1}", key, value));
                }
            }
        }
    }
}