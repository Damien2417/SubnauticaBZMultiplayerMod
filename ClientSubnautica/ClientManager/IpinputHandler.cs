using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClientSubnautica
{
	public class IpinputHandler : MonoBehaviour
	{
        public Button button;
        void Start()
        {
            button.onClick.AddListener(() => ButtonClicked(42));
        }
        void ButtonClicked(int buttonNo)
        {
            //Output this to console when the Button3 is clicked
            ErrorMessage.AddMessage("Button clicked = " + buttonNo);

        }
    }
}
