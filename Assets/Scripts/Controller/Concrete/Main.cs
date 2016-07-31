using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

namespace Boardgame
{
    public class Main: MonoBehaviour 
	{
        public MenuImpl frontEndMenu;

        private Dictionary<string, MethodInfo> dictionaryOfMethods = new Dictionary<string, MethodInfo>();

        void Awake()
        {
            SubscribeToMenu(frontEndMenu);
            MakeDictionaryOfMethods();
        }

        void SubscribeToMenu(Menu menu)
        {
            menu.AddListener(eventInput);
        }

        void MakeDictionaryOfMethods()
        {
            MethodInfo[] methodArray = GetArrayOfMethods();
            AddMethodsToDictionary(methodArray);
        }

        MethodInfo[] GetArrayOfMethods()
        {
            var type = GetType();
            return type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
        }

        void AddMethodsToDictionary(MethodInfo[] methodArray)
        {
            foreach (MethodInfo method in methodArray)
            {
                dictionaryOfMethods.Add(method.Name, method);
            }
        }

        void eventInput(string name, object[] parameters)
        {
            MethodInfo method;
            if (dictionaryOfMethods.TryGetValue(name, out method))
                method.Invoke(this, parameters);
        }

        public void NewGame()
        {
            Debug.Log("Create new game");
        }

        public void ChooseScenario(string scenarioName)
        {
            Debug.Log(string.Format("Scenario set to {0}", scenarioName));
        }
	}
}