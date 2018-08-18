using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USComics_Debug;

namespace USComics_Movement
{
    public class MovementPad : MonoBehaviour
    {
        public DirectionType CurrentDirection { get; set; }

        private Rect padRect;
        private Rect centerRect;
        private Rect northRect;
        private Rect southRect;
        private Rect eastRect;
        private Rect westRect;
        private DebugConsole debugConsoleScript;

        private GameObject movementPadIndicator;
        private Vector2 movementPadIndicatorOriginalPosition;
        private Keyboard KeyboardScript;
        private SpeedBar SpeedBarScript;

        // Use this for initialization
        void Start()
        {
            movementPadIndicator = GameObject.FindWithTag("MovementPadIndicator") as GameObject;
            GameObject debugConsole = GameObject.FindWithTag("DebugConsole") as GameObject;
            if (null != debugConsole) debugConsoleScript = debugConsole.GetComponent<DebugConsole>();
            GameObject movementPad = GameObject.FindWithTag("MovementPad") as GameObject;
            if (null != movementPad) KeyboardScript = movementPad.GetComponent<Keyboard>();
            GameObject speedBarKnob = GameObject.FindWithTag("SpeedBarKnob") as GameObject;
            if (null != speedBarKnob) SpeedBarScript = speedBarKnob.GetComponent<SpeedBar>();

            if (null == movementPadIndicator) { Debug.LogError("MovementPad.Start: movementPadIndicator is null."); }
            if (null == debugConsoleScript) { Debug.LogError("MovementPad.Start: debugConsoleScript is null."); }
            if (null == KeyboardScript) { Debug.LogError("MovementPad.Start: KeyboardScript is null."); }
            if (null == SpeedBarScript) { Debug.LogError("MovementPad.Start: SpeedBarScript is null."); }

            if (null == movementPadIndicator) { return; }
            if (null == debugConsoleScript) { return; }
            if (null == KeyboardScript) { return; }
            if (null == SpeedBarScript) { return; }

            movementPadIndicatorOriginalPosition = movementPadIndicator.transform.position;
            CurrentDirection = DirectionType.None;

            float padLeft = 500.0f;
            float padBottom = 60.0f;
            float padWidth = 255.0f;
            float padHeight = 255.0f;
            padRect = new Rect(padLeft, padBottom, padWidth, padHeight);
            float centerLeft = padLeft + 73.0f;
            float centerBottom = padBottom + 73.0f;
            float centerWidth = 110.0f;
            float centerHeight = 110.0f;
            centerRect = new Rect(centerLeft, centerBottom, centerWidth, centerHeight);
            float northLeft = padLeft + 85.0f;
            float northBottom = padBottom + 185.0f;
            float northWidth = 85.0f;
            float northHeight = 65.0f;
            northRect = new Rect(northLeft, northBottom, northWidth, northHeight);
            float southLeft = northLeft;
            float southBottom = padBottom + 10.0f;
            float southWidth = northWidth;
            float southHeight = northHeight;
            southRect = new Rect(southLeft, southBottom, southWidth, southHeight);
            float eastLeft = padLeft + 187.0f;
            float eastBottom = padBottom + 98.0f;
            float eastWidth = 60.0f;
            float eastHeight = 65.0f;
            eastRect = new Rect(eastLeft, eastBottom, eastWidth, eastHeight);
            float westLeft = padLeft + 3;
            float westBottom = eastBottom;
            float westWidth = eastWidth;
            float westHeight = eastHeight;
            westRect = new Rect(westLeft, westBottom, westWidth, westHeight);
    }

    // Update is called once per frame
    void Update()
        {
            DirectionType direction = GetDirection();
            if (DirectionType.None != direction) {
                Vector3 indicatiorPosition = GetIndicatorPosition(direction, movementPadIndicatorOriginalPosition);
                CurrentDirection = direction;
                movementPadIndicator.transform.position = indicatiorPosition;
                if (DirectionType.Stop == direction) SpeedBarScript.SetSpeed(MovementSpeed.GetSpeed(MovementType.Standing));

            }
        }

        public void SetDirection(DirectionType direction) {
            if (DirectionType.None == direction) return;
            CurrentDirection = direction;
            SetIndicatorPosition(direction);
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
            else if (northRect.Contains(mousePosition)) { direction = DirectionType.North; }
            else if (southRect.Contains(mousePosition)) { direction = DirectionType.South; }
            else if (eastRect.Contains(mousePosition)) { direction = DirectionType.East; }
            else if (westRect.Contains(mousePosition)) { direction = DirectionType.West; }
            else
            {
                float diameterX = padRect.width / 2;
                float diameterY = padRect.height / 2;
                if (mousePosition.x < padRect.x + diameterX)
                {
                    if (mousePosition.y < padRect.y + diameterY) { direction = DirectionType.SW; }
                    else { direction = DirectionType.NW; }
                }
                else
                {
                    if (mousePosition.y < padRect.y + diameterY) { direction = DirectionType.SE; }
                    else { direction = DirectionType.NE; }
                }
            }
            return direction;
        }

        public void SetIndicatorPosition(DirectionType direction)
        {
            Vector3 indicatiorPosition = GetIndicatorPosition(direction, movementPadIndicatorOriginalPosition);
            movementPadIndicator.transform.position = indicatiorPosition;
        }

        private Vector3 GetIndicatorPosition(DirectionType inDirection, Vector2 inMovementPadIndicatorOriginalPosition)
        {
            Vector2 position = inMovementPadIndicatorOriginalPosition;
            if (DirectionType.North == inDirection) { position.y += 83; }
            else if (DirectionType.South == inDirection) { position.y -= 83; }
            else if (DirectionType.East == inDirection) { position.x += 83; }
            else if (DirectionType.West == inDirection) { position.x -= 83; }
            else if (DirectionType.NE == inDirection) { position.x += 60; position.y += 60; }
            else if (DirectionType.NW == inDirection) { position.x -= 60; position.y += 60; }
            else if (DirectionType.SE == inDirection) { position.x += 60; position.y -= 60; }
            else if (DirectionType.SW == inDirection) { position.x -= 60; position.y -= 60; }
            return new Vector3(position.x, position.y, 0);
        }
    }
}
