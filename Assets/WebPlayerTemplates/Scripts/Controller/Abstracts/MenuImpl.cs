using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Boardgame
{
    [System.Serializable]
    public class MenuEvent : UnityEvent<string, object[]> { }

    public abstract class MenuImpl : MonoBehaviour, Menu
	{
        private MenuEvent menuEvent = new MenuEvent();

        protected void ListenToInput(Selectable input)
        {
            if (input is Button)
            {
                Button i = input as Button;
                i.onClick.AddListener(delegate { InvokeEvent(i.name, null); });
            }

            if (input is InputField)
            {
                InputField i = input as InputField;
                i.onValueChanged.AddListener(delegate { InvokeEvent(i.name, i.text); });
            }

            if (input is Toggle)
            {
                Toggle i = input as Toggle;
                i.onValueChanged.AddListener(delegate { InvokeEvent(i.name, i.isOn); });
            }

            if (input is Slider)
            {
                Slider i = input as Slider;
                i.onValueChanged.AddListener(delegate { InvokeEvent(i.name, i.value); });
            }

            if (input is Dropdown)
            {
                Dropdown i = input as Dropdown;
                if (i.name.EndsWith("Int"))
                {
                    string trimmedName = i.name.Remove(i.name.Length - 3);
                    i.onValueChanged.AddListener(delegate { InvokeEvent(trimmedName, i.value); });
                }
                else
                {
                    i.onValueChanged.AddListener(delegate { InvokeEvent(i.name, i.captionText.text); });
                }
            }

        }

        public void InvokeEvent(string name, params object[] parameters)
        {
            menuEvent.Invoke(name, parameters);
        }

        public void AddListener(UnityAction<string, object[]> listener)
        {
            menuEvent.AddListener(listener);
        }
    }
}