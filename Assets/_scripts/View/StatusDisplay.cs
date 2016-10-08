using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class StatusDisplay : MonoBehaviour
    {
        public Image panelImage;
        public Text number;
        public Animator animator;

        public void SetNumber(int newNumber)
        {
            number.text = newNumber.ToString();
            PanelFlash();
        }

        public void PanelFlash()
        {
            animator.SetTrigger("Flash");
        }
    }
}