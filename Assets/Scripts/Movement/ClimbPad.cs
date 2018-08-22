using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using USComics_Debug;

namespace USComics_Movement
{
    public class ClimbPad : MonoBehaviour
    {
        public DirectionType CurrentDirection { get; set; }
        public Image climbPadStop;
        public Image climbPadUp;
        public Image climbPadDown;

        private Rect padRect;
        private Rect centerRect;
        private Rect upRect;
        private Rect downRect;
        private Image climbPadImage;
        private DebugConsole debugConsoleScript;
        private Keyboard KeyboardScript;

        // Use this for initialization
        void Start()
        {
            GameObject debugConsole = GameObject.FindWithTag("DebugConsole") as GameObject;
            if (null != debugConsole) debugConsoleScript = debugConsole.GetComponent<DebugConsole>();
            GameObject movementPad = GameObject.FindWithTag("ClimbPad") as GameObject;
            if (null != movementPad) KeyboardScript = movementPad.GetComponent<Keyboard>();
            if (null != movementPad) climbPadImage = movementPad.GetComponent<Image>();

            if (null == debugConsoleScript) { Debug.LogError("MovementPad.Start: debugConsoleScript is null."); }
            if (null == KeyboardScript) { Debug.LogError("MovementPad.Start: KeyboardScript is null."); }

            if (null == debugConsoleScript) { return; }
            if (null == KeyboardScript) { return; }

            CurrentDirection = DirectionType.None;

            float padLeft = 500.0f;
            float padBottom = 60.0f;
            float padWidth = 255.0f;
            float padHeight = 255.0f;
            padRect = new Rect(padLeft, padBottom, padWidth, padHeight);
            float centerLeft = padLeft + 75.0f;
            float centerBottom = padBottom + 84.0f;
            float centerWidth = 108.0f;
            float centerHeight = 90.0f;
            centerRect = new Rect(centerLeft, centerBottom, centerWidth, centerHeight);
            float upLeft = padLeft + 95.0f;
            float upBottom = padBottom + 185.0f;
            float upWidth = 64.0f;
            float upHeight = 55.0f;
            upRect = new Rect(upLeft, upBottom, upWidth, upHeight);
            float downLeft = upLeft;
            float downBottom = padBottom + 10.0f;
            float downWidth = upWidth;
            float downHeight = upHeight;
            downRect = new Rect(downLeft, downBottom, downWidth, downHeight);
    }

    // Update is called once per frame
    void Update()
        {
            DirectionType direction = GetDirection();
            if (DirectionType.None != direction) {
                CurrentDirection = direction;
                // if (DirectionType.Stop == direction) SpeedBarScript.SetSpeed(MovementSpeed.GetSpeed(MovementType.Standing));
                SetMovementPadImage(direction);
            }
        }

        public void SetDirection(DirectionType direction) {
            if (DirectionType.None == direction) return;
            CurrentDirection = direction;
            SetMovementPadImage(direction);
        }

        private DirectionType GetDirection()
        {
            DirectionType direction = KeyboardScript.GetDirection();
            if (DirectionType.None != direction) return direction;
            if (!Input.GetMouseButtonDown(0)) { return DirectionType.None; }
            Vector2 mousePosition = Input.mousePosition;
            if (!padRect.Contains(mousePosition)) return DirectionType.None;
            direction = DirectionType.None;
            if (centerRect.Contains(mousePosition)) { direction = DirectionType.Stop; }
            else if (upRect.Contains(mousePosition)) { direction = DirectionType.North; }
            else if (downRect.Contains(mousePosition)) { direction = DirectionType.South; }
            return direction;
        }

        private void SetMovementPadImage(DirectionType direction)
        {
            if (DirectionType.North == direction) climbPadImage.sprite = climbPadUp.sprite;
            else if (DirectionType.South == direction) climbPadImage.sprite = climbPadDown.sprite;
            else if (DirectionType.Stop == direction) climbPadImage.sprite = climbPadStop.sprite;
        }

    }
}
