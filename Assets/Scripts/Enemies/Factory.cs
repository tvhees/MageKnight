using UnityEngine;
using UnityEngine.UI;
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
            public GameObject enemyCanvasPrefab; // Canvas for showing enemy information within the scene

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

                Canvas enemyCanvas = CreateCanvas(output);

                enemy.SetAttributes(output, enemyCanvas);
            }

            private Canvas CreateCanvas(Enemy enemy)
            {
                Canvas enemyCanvas = (Instantiate(enemyCanvasPrefab) as GameObject)
                                    .GetComponent<Canvas>();

                enemyCanvas.GetComponentInChildren<Image>().sprite = enemy.image;

                enemyCanvas.enabled = false;

                return enemyCanvas;
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

                // Define the enemy names to include in each type of stack
                // The same enemy can be included multiple times
                // HACKY CODE - this could be read from the data file
                switch (type)
                {
                    case Factory.EnemyType.orc:
                        names = new string[6] { "Prowlers", "Diggers", "CursedHags", "WolfRiders", "Ironclads", "OrcSummoners" };
                        numbers = new int[6] { 2, 2, 2, 2, 2, 2 };
                        break;
                    case Factory.EnemyType.keep:
                        names = new string[4] { "Crossbowmen", "Guardsmen", "Swordsmen", "Golems" };
                        numbers = new int[4] { 3, 3, 2, 2 };
                        break;
                    case Factory.EnemyType.tower:
                        names = new string[6] { "Monks", "IceMages", "FireMages", "Illusionists", "IceGolems", "FireGolems" };
                        numbers = new int[6] { 2, 2, 2, 2, 1, 1 };
                        break;
                    case Factory.EnemyType.dungeon:
                        names = new string[5] { "Minotaur", "Gargoyle", "Medusa", "CryptWorm", "Werewolf" };
                        numbers = new int[5] { 2, 2, 2, 2, 2 };
                        break;
                    case Factory.EnemyType.draconum:
                        names = new string[4] { "SwampDragon", "FireDragon", "IceDragon", "HighDragon" };
                        numbers = new int[4] { 2, 2, 2, 2 };
                        break;
                    case Factory.EnemyType.city:
                        names = new string[4] { "Freezers", "Gunners", "AltemGuardsmen", "AltemMages" };
                        numbers = new int[4] { 3, 3, 2, 2 };
                        break;
                }
            }
        }
    }
}