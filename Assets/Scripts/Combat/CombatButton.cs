using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace USComics_Combat
{
    public class CombatButton : MonoBehaviour
    {
        public Image image;
        public Image imageEnabled;
        public Image imageDisabled;
        public Image imageClicked;
        public AttackType attackType;

        private ButtonState buttonState;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetButtonState(ButtonState inButtonState)
        {
            image.sprite = GetButtonImage(inButtonState).sprite;
            buttonState = inButtonState;
        }
        public ButtonState GetButtonState()
        {
            return buttonState;
        }
        public AttackType GetAttackType()
        {
            if (ButtonState.Disabled == GetButtonState()) return AttackType.None;
            return attackType;
        }

        private Image GetButtonImage(ButtonState inButtonState)
        {
            if (ButtonState.Enabled == inButtonState) return imageEnabled;
            if (ButtonState.Disabled == inButtonState) return imageDisabled;
            if (ButtonState.Clicked == inButtonState) return imageClicked;
            return null;
        }
    }

    [System.Serializable]
    public enum ButtonState { Enabled, Disabled, Clicked };
}
