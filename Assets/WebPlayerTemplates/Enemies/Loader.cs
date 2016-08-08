using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace Boardgame
{
    namespace Enemy
    {
        public static class Loader
        {
            public static Dictionary<string, Component> LoadEnemies(TextAsset XML, Factory.EnemyType type)
            {
                // Dictionary objects to store all enemy data
                Dictionary<string, Component> enemyDictionary = new Dictionary<string, Component>();
                XmlDocument cardDB = new XmlDocument(); // Create XML container
                cardDB.LoadXml(XML.text); // Load card information stored in XML
                XmlNodeList nodeList = cardDB.GetElementsByTagName(type.ToString()); // Create array of nodes, one for each enemy of the requested type

                // Run through each node and extract enemy information
                foreach (XmlNode node in nodeList)
                {
                    ComponentFactory enemyFactory = new EnemyFactory("Orc");
                    Orc enemy = new Orc("WolfRiders", enemyFactory);
                    XmlNodeList enemyInfo = node.ChildNodes; // Get child nodes for current card

                    foreach (XmlNode element in enemyInfo)
                    {
                        XmlNodeList subInfo = element.ChildNodes;

                        // Examples of XML tags to process
                        if (element.Name == "name")
                        {
                            enemy.name = element.InnerText;
                        }

                        if (element.Name == "attack")
                        {
                            foreach (XmlNode info in subInfo)
                                ProcessAttack(info, enemy.attack);
                        }
                        if (element.Name == "defense")
                        {
                            foreach (XmlNode info in subInfo)
                                ProcessDefense(info, enemy.defense);
                        }
                        if (element.Name == "reward")
                        {
                            foreach (XmlNode info in subInfo)
                                ProcessReward(info, enemy.reward);
                        }
                    }
                    // Get the image for this enemy
                    LoadSprite(enemy, type);

                    enemyDictionary.Add(enemy.name, enemy);
                }

                return enemyDictionary;
            }

            static void LoadSprite(Component enemy, Factory.EnemyType type)
            {
                enemy.image = Resources.Load<Sprite>("EnemyImages/" + enemy.name);
                enemy.backImage = Resources.Load<Sprite>("EnemyImages/" + type.ToString());
            }

            static void ProcessAttack(XmlNode node, Attack input)
            {
                switch (node.Name)
                {
                    case "strength":
                        input.strength = XmlConvert.ToInt32(node.InnerText);
                        break;
                    case "cold":
                        input.cold = true;
                        break;
                    case "fire":
                        input.fire = true;
                        break;
                    case "swiftness":
                        input.swiftness = true;
                        break;
                    case "brutal":
                        input.brutal = true;
                        break;
                    case "poison":
                        input.poison = true;
                        break;
                    case "paralyze":
                        input.paralyze = true;
                        break;
                    case "summoner":
                        input.summoner = true;
                        break;
                }

                Sprite img = Resources.Load<Sprite>("Modifiers/Attack/" + node.Name);
                if (img != null)
                    input.images.Add(img);
            }

            static void ProcessDefense(XmlNode node, Defense input)
            {
                switch (node.Name)
                {
                    case "strength":
                        input.strength = XmlConvert.ToInt32(node.InnerText);
                        break;
                    case "cold":
                        input.cold = true;
                        break;
                    case "fire":
                        input.fire = true;
                        break;
                    case "physical":
                        input.physical = true;
                        break;
                    case "fortified":
                        input.fortified = true;
                        break;
                }

                Sprite img = Resources.Load<Sprite>("Modifiers/Defense/" + node.Name);
                if(img != null)
                    input.images.Add(img);
            }

            static void ProcessReward(XmlNode node, Reward input)
            {
                switch (node.Name)
                {
                    case "fame":
                        input.fame = XmlConvert.ToInt32(node.InnerText);
                        break;
                    case "reputation":
                        input.reputation = XmlConvert.ToInt32(node.InnerText);
                        break;
                }
            }
        }
    }
}