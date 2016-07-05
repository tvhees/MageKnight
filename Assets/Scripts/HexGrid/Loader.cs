using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace BoardGame
{
    namespace HexGrid // Eventually try to merge with Cards.Loader
    {
        public static class Loader
        {
            public static List<Dictionary<string, string>> LoadHexes(TextAsset XML)
            {
                // Dictionary objects to store all card data
                List<Dictionary<string, string>> hexGroups = new List<Dictionary<string, string>>();
                Dictionary<string, string> m_obj;

                XmlDocument cardDB = new XmlDocument(); // Create XML container
                cardDB.LoadXml(XML.text); // Load card information stored in XML
                XmlNodeList groupList = cardDB.GetElementsByTagName("group"); // Create array of nodes, one for each HexGroup

                // Run through each node and extract card information
                foreach (XmlNode group in groupList)
                {
                    XmlNodeList groupInfo = group.ChildNodes; // Get child nodes for current group

                    m_obj = new Dictionary<string, string>(); // Create a object(Dictionary) to collect the group info and put it in the array.

                    int i = 0; // track which number hex this we are in
                    foreach (XmlNode hex in groupInfo)
                    {
                        XmlNodeList hexInfo = hex.ChildNodes;

                        foreach (XmlNode element in hexInfo)
                        {
                            // E.g. "terrain0", "feature0"
                            if (element.Name == "string")
                            {
                                m_obj.Add(element.Attributes["name"].Value + i, element.InnerText);
                            }
                        }
                        i++;
                    }
                    hexGroups.Add(m_obj);
                }

                return hexGroups;
            }
        }
    }
}