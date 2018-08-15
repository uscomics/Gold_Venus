using System.Collections;
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
        public Image climbingIcon;
        public Image fallingIcon;
        public Image standingIcon;
        public Slider speedBar;
        public Image knob;

        private float maxSpeed = MovementSpeed.RUN_SPEED;
        private Animator Anim;

        // Use this for initialization
        void Start()
        {
            GameObject playerCharacter = GameObject.FindWithTag("PlayerCharacter") as GameObject;
            if (null != playerCharacter) Anim = playerCharacter.GetComponent<Animator>();

            if (null == speedBar) { Debug.LogError("SpeedBar.Start: speedBar is null."); }
            if (null == Anim) { Debug.LogError("SpeedBar.Start: Anim is null."); }

            if (null == speedBar) { return; }
            if (null == Anim) { return; }

            SetKnobImage(standingIcon);
            speedBar.value = 0.0f;
        }

        // Update is called once per frame
        void Update()
        {
            float speed = GetSpeed();
            if (MovementSpeed.SNEAK_SPEED > speed) SetKnobImage(standingIcon);
            else if (MovementSpeed.WALK_SPEED > speed) SetKnobImage(sneakingIcon);
            else if (MovementSpeed.RUN_SPEED > speed) SetKnobImage(walkingIcon);
            else SetKnobImage(runningIcon);
        }

        public float GetSpeed()
        {
            return speedBar.value * maxSpeed;
        }
        public void SetSpeed(float speed)
        {
            speedBar.value = speed / maxSpeed;
        }

        public void SetKnobImage(Image image)
        {
            knob.sprite = image.sprite;
        }
    }
}
