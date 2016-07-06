using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoardGame
{
    namespace Enemy
    {
        public class Factory : Singleton<Factory>
        {

            //************
            // ENEMY FACTORY
            //************

            // Image and text data sources
            public TextAsset m_enemyXML; // XML file reference

            // Enemy dictionary
            Dictionary<string, Enemy> m_enemyDictionary = new Dictionary<string, Enemy>();

            public void LoadXML(EnemyType type) // Load text data in to dictionaries
            {
                m_enemyDictionary = Loader.LoadEnemies(m_enemyXML, type);
            }

            private void GiveEnemyIdentity(Object enemy, string name) // Set a card to a specific number and matching sprites 
            {
                Enemy output;
                m_enemyDictionary.TryGetValue(name, out output);
                enemy.SetAttributes(output);
            }

            //************
            // CREATING STACKS
            //************
            public enum EnemyType
            {
                orc,
                keep,
                tower,
                dungeon,
                draconum,
                city,
                numberOfTypes
            }

            public GameObject[] m_enemyPrefabs;

            public List<Object> CreateStack(GameObject stackHolder, Camera camera, EnemyType type)
            {
                LoadXML(type);
                EnemyStack stack = new EnemyStack(type);
                string[] enemyNames = stack.names;
                int[] enemyNumbers = stack.numbers;

                List<Object> stackList = new List<Object>();

                for (int i = 0; i < enemyNames.Length; i++)
                {
                    for (int j = 0; j < enemyNumbers[i]; j++)
                    {
                        Object newEnemy = stackHolder.transform.InstantiateChild(m_enemyPrefabs[(int)type]).GetComponent<Object>();
                        GiveEnemyIdentity(newEnemy, enemyNames[i]);
                        stackList.Add(newEnemy);
                    }
                }

                stackList.Randomise(false);

                return stackList;
            }
        }

        // This struct contains information on which cards to include in each deck of cards to be created
        [System.Serializable]
        public struct EnemyStack
        {
            // Cardlists are stored as an array of integers representing card IDs
            public string[] names;
            public int[] numbers;

            // Constructor for making a new decklist
            public EnemyStack(Factory.EnemyType type)
            {
                names = new string[0];
                numbers = new int[0];

                // Define the card numbers to include in each type of deck here
                // The same card can be included multiple times
                switch (type)
                {
                    case Factory.EnemyType.orc:
                        names = new string[6] { "Prowlers", "Diggers", "Cursed Hags", "Wolf Riders", "Ironclads", "Orc Summoners" };
                        numbers = new int[6] { 2, 2, 2, 2, 2, 2 };
                        break;
                    case Factory.EnemyType.keep:
                        names = new string[4] { "Crossbowmen", "Guardsmen", "Swordsmen", "Golems" };
                        numbers = new int[4] { 3, 3, 2, 2 };
                        break;
                    case Factory.EnemyType.tower:
                        names = new string[6] { "Monks", "Ice Mages", "Fire Mages", "Illusionists", "Ice Golems", "Fire Golems" };
                        numbers = new int[6] { 2, 2, 2, 2, 1, 1 };
                        break;
                    case Factory.EnemyType.dungeon:
                        names = new string[5] { "Minotaur", "Gargoyle", "Medusa", "Crypt Worm", "Werewolf" };
                        numbers = new int[5] { 2, 2, 2, 2, 2 };
                        break;
                    case Factory.EnemyType.draconum:
                        names = new string[4] { "Swamp Dragon", "Fire Dragon", "Ice Dragon", "High Dragon" };
                        numbers = new int[4] { 2, 2, 2, 2 };
                        break;
                    case Factory.EnemyType.city:
                        names = new string[4] { "Freezers", "Gunners", "Altem Guardsmen", "Altem Mages" };
                        numbers = new int[4] { 3, 3, 2, 2 };
                        break;
                }
            }
        }
    }
}