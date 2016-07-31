using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace Boardgame
{
    namespace Card
    {
        public class XmlLoaderImp : XmlLoader<string, string>
        {
            private List<Dictionary<string, string>> listOfCardDefinitions;
            private Dictionary<string, string> cardDefinition;
            // Effects require an additional identifier to ensure their key is unique within a card's definition
            private int effectCount;

            public List<Dictionary<string, string>> GetListOfDefinitions(string sourceName)
            {
                listOfCardDefinitions = new List<Dictionary<string, string>>();

                TextAsset xmlFile = GetXmlFile(sourceName);
                XmlNodeList xmlNodeList = GetXmlNodeList(xmlFile, "card");
                foreach (XmlNode cardNode in xmlNodeList)
                {
                    cardDefinition = GetCardDefinitionFromNode(cardNode);
                    listOfCardDefinitions.Add(cardDefinition);
                }

                return listOfCardDefinitions;
            }

            TextAsset GetXmlFile(string sourceName)
            {
                return Board.BoardSetupDatabase.GetBoardSetup(sourceName).cardData;
            }

            XmlNodeList GetXmlNodeList(TextAsset xmlFile, string tagName)
            {
                var xmlDocument = GetXmlDocument(xmlFile);
                return xmlDocument.GetElementsByTagName(tagName);
            }

            XmlDocument GetXmlDocument(TextAsset xmlFile)
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlFile.text);
                return xmlDocument;
            }

            Dictionary<string, string> GetCardDefinitionFromNode(XmlNode cardNode)
            {
                XmlNodeList listOfCardAttributes = cardNode.ChildNodes;
                cardDefinition = new Dictionary<string, string>();
                effectCount = 0;

                foreach (XmlNode attribute in listOfCardAttributes)
                {
                    AddAttributeToCardDefinition(attribute);
                }

                return cardDefinition;
            }

            void AddAttributeToCardDefinition(XmlNode element)
            {
                if (element.Name == "string")
                {
                    AddToDictionary(element.Attributes["name"].Value, element.InnerText);
                }

                if (element.Name == "effect")
                {
                    AddEffect(element);
                    effectCount++;
                }
            }

            void AddToDictionary(string key, string value)
            {
                try
                {
                    cardDefinition.Add(key, value);
                }
                catch (Exception e)
                {
                    Debug.Log(string.Format("Error adding to dictionary: key = {0} value = {1}, error: {2}", key, value, e));
                }
            }

            void AddEffect(XmlNode element)
            {
                // We must add 'i' to the string e.g. "effect0", "effect1"
                // This is to create unique keys for the dictionary
                if (element.Attributes["type"].Value.Contains("effect"))
                {
                    AddSingleEffect(element, effectCount.ToString());
                }
                else if (element.Attributes["type"].Value.Contains("choice") || element.Attributes["type"].Value.Contains("combine"))
                {
                    AddMultipleEffects(element, effectCount.ToString());
                }
            }

            void AddSingleEffect(XmlNode element, string identifier)
            {
                string effectType = element.Attributes["type"].Value + "_" + identifier;
                string valueType = effectType.Replace("effect", "value");

                foreach (XmlNode effectElement in element)
                    if (effectElement.Name == "string")
                        AddToDictionary(effectType, effectElement.InnerText);
                    else if (effectElement.Name == "int")
                        AddToDictionary(valueType, effectElement.InnerText);
            }

            void AddMultipleEffects(XmlNode element, string identifier)
            {
                string multipleEffectType = element.Attributes["type"].Value + "_" + identifier;
                AddToDictionary(multipleEffectType, "Multiple");

                // Subeffects that are part of a combined or choice-based effect require an additional identifier.
                // We use characters to ensure their dictionary key is unique
                char[] subChar = new char[10] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j' };
                int subEffectCount = 0;

                foreach (XmlNode subEffect in element)
                {
                    string subIdentifier = identifier + subChar[subEffectCount];
                    AddSingleEffect(subEffect, subIdentifier);
                    subEffectCount++;
                }
            }
        }
    }
}