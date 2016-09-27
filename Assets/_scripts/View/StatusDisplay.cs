using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace View
{
    public class StatusDisplay : MonoBehaviour
    {
        public Image panelImage;
        public Text number;

        public void SetNumber(int newNumber)
        {
            number.text = newNumber.ToString();
        }

        public void PanelFlash()
        {

        }
    }
}