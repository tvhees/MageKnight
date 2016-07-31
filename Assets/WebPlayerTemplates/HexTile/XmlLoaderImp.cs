using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace Boardgame
{
    namespace HexTile
    {
        public class XmlLoaderImp : XmlLoader<string, string>
        {
            private List<Dictionary<string, string>> listOfHexGroupDefinitions;

            public List<Dictionary<string, string>> GetListOfDefinitions(string sourceName)
            {
                listOfHexGroupDefinitions = new List<Dictionary<string, string>>();

                TextAsset xmlFile = GetXmlFile(sourceName);
                XmlNodeList xmlNodeList = GetXmlNodeList(xmlFile, "group");
                foreach (XmlNode hexGroup in xmlNodeList)
                {
                    AddHexGroupDefinitionToList(hexGroup);
                }

                return listOfHexGroupDefinitions;
            }

            TextAsset GetXmlFile(string sourceName)
            {
                return Board.BoardSetupDatabase.GetBoardSetup(sourceName).hexTileData;
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

            void AddHexGroupDefinitionToList(XmlNode hexGroup)
            {
                Dictionary<string, string> hexGroupDefinition = GetHexGroupDefinition(hexGroup.ChildNodes);
                listOfHexGroupDefinitions.Add(hexGroupDefinition);
            }

            Dictionary<string, string> GetHexGroupDefinition(XmlNodeList listOfHexDefinitions)
            {
                var hexGroupDefinition = new Dictionary<string, string>();

                int i = 0;
                foreach (XmlNode hex in listOfHexDefinitions)
                {
                    AddHexToGroupDefinition(hex, hexGroupDefinition, i);
                    i++;
                }

                return hexGroupDefinition;
            }

            void AddHexToGroupDefinition(XmlNode hex, Dictionary<string, string> hexGroupDefinition, int i)
            {
                XmlNodeList hexDefinition = hex.ChildNodes;

                foreach (XmlNode element in hexDefinition)
                {
                    if (element.Name == "string")
                    {
                        // We must add 'i' to the string e.g. "terrain0", "feature0"
                        // This is to create unique keys for the dictionary
                        hexGroupDefinition.Add(element.Attributes["name"].Value + i, element.InnerText);
                    }
                }
            }
        }
    }
}