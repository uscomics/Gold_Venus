﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace USComics_Movement
{
    public class SpeedBar : MonoBehaviour
    {
        public Image sneakingIcon;
        public Image walkingIcon;
        public Image runningIcon;
        public Image standingIcon;
        public Slider speedBar;
        public Image knob;
        public Image speedBarFill;

        private float maxSpeed = MovementSpeed.RUN_SPEED;
        private Keyboard KeyboardScript;
        private Animator Anim;

        // Use this for initialization
        void Start()
        {
            GameObject playerCharacter = GameObject.FindWithTag("PlayerCharacter") as GameObject;
            if (null != playerCharacter) Anim = playerCharacter.GetComponent<Animator>();
            GameObject movementPad = GameObject.FindWithTag("MovementPad") as GameObject;
            if (null != movementPad) KeyboardScript = movementPad.GetComponent<Keyboard>();

            if (null == speedBar) { Debug.LogError("SpeedBar.Start: speedBar is null."); }
            if (null == Anim) { Debug.LogError("SpeedBar.Start: Anim is null."); }
            if (null == KeyboardScript) { Debug.LogError("SpeedBar.Start: KeyboardScript is null."); }

            if (null == speedBar) { return; }
            if (null == Anim) { return; }
            if (null == KeyboardScript) { return; }

            SetKnobImage(standingIcon, Color.white);
            speedBar.value = 0.0f;
        }

        // Update is called once per frame
        void Update()
        {
            float speed = GetSpeed();
            if (MovementSpeed.SNEAK_SPEED > speed) SetKnobImage(standingIcon, Color.white);
            else if (MovementSpeed.WALK_SPEED > speed) SetKnobImage(sneakingIcon, Color.yellow);
            else if (MovementSpeed.RUN_SPEED > speed) SetKnobImage(walkingIcon, Color.green);
            else SetKnobImage(runningIcon, Color.red);
        }

        public float GetSpeed()
        {
            MovementType movement = KeyboardScript.GetMovementType();
            if (MovementType.None != movement) SetSpeed(MovementSpeed.GetSpeed(movement));
            return speedBar.value * maxSpeed;
        }
        public void SetSpeed(float speed)
        {
            speedBar.value = speed / maxSpeed;
        }

        public void SetKnobImage(Image image, Color color)
        {
            knob.sprite = image.sprite;
            speedBarFill.color = color;
        }
    }
}
