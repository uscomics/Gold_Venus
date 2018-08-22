using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using USComics_Debug;

namespace USComics_Movement
{
    public class MovementPad : MonoBehaviour
    {
        public DirectionType CurrentDirection { get; set; }
        public Image movementPadStop;
        public Image movementPadNorth;
        public Image movementPadSouth;
        public Image movementPadEast;
        public Image movementPadWest;
        public Image movementPadNE;
        public Image movementPadNW;
        public Image movementPadSE;
        public Image movementPadSW;

        private Rect padRect;
        private Rect centerRect;
        private Rect northRect;
        private Rect southRect;
        private Rect eastRect;
        private Rect westRect;
        private Image movementPadImage;
        private DebugConsole debugConsoleScript;
        private Keyboard KeyboardScript;
        private SpeedBar SpeedBarScript;
        private CanvasGroup MovementPadCanvasGroupScript;
        private CanvasGroup SpeedBarCanvasGroupScript;

        // Use this for initialization
        void Start()
        {
            GameObject debugConsole = GameObject.FindWithTag("DebugConsole") as GameObject;
            if (null != debugConsole) debugConsoleScript = debugConsole.GetComponent<DebugConsole>();
            GameObject movementPad = GameObject.FindWithTag("MovementPad") as GameObject;
            if (null != movementPad) MovementPadCanvasGroupScript = movementPad.GetComponent<CanvasGroup>();
            if (null != movementPad) KeyboardScript = movementPad.GetComponent<Keyboard>();
            if (null != movementPad) movementPadImage = movementPad.GetComponent<Image>();
            GameObject speedBarKnob = GameObject.FindWithTag("SpeedBarKnob") as GameObject;
            if (null != speedBarKnob) SpeedBarScript = speedBarKnob.GetComponent<SpeedBar>();
            GameObject speedBarPanel = GameObject.FindWithTag("SpeedBarPanel") as GameObject;
            if (null != speedBarPanel) SpeedBarCanvasGroupScript = speedBarPanel.GetComponent<CanvasGroup>();

            if (null == debugConsoleScript) { Debug.LogError("MovementPad.Start: debugConsoleScript is null."); }
            if (null == MovementPadCanvasGroupScript) { Debug.LogError("MovementPad.Start: MovementPadCanvasGroupScript is null."); }
            if (null == KeyboardScript) { Debug.LogError("MovementPad.Start: KeyboardScript is null."); }
            if (null == SpeedBarScript) { Debug.LogError("MovementPad.Start: SpeedBarScript is null."); }
            if (null == SpeedBarCanvasGroupScript) { Debug.LogError("MovementPad.Start: SpeedBarCanvasGroupScript is null."); }

            if (null == debugConsoleScript) { return; }
            if (null == MovementPadCanvasGroupScript) { return; }
            if (null == KeyboardScript) { return; }
            if (null == SpeedBarScript) { return; }
            if (null == SpeedBarCanvasGroupScript) { return; }

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
            float northLeft = padLeft + 95.0f;
            float northBottom = padBottom + 185.0f;
            float northWidth = 64.0f;
            float northHeight = 55.0f;
            northRect = new Rect(northLeft, northBottom, northWidth, northHeight);
            float southLeft = northLeft;
            float southBottom = padBottom + 10.0f;
            float southWidth = northWidth;
            float southHeight = northHeight;
            southRect = new Rect(southLeft, southBottom, southWidth, southHeight);
            float eastLeft = padLeft + 180.0f;
            float eastBottom = padBottom + 101.0f;
            float eastWidth = 58.0f;
            float eastHeight = 55.0f;
            eastRect = new Rect(eastLeft, eastBottom, eastWidth, eastHeight);
            float westLeft = padLeft + 20.0f;
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
                CurrentDirection = direction;
                if (DirectionType.Stop == direction) SpeedBarScript.SetSpeed(MovementSpeed.GetSpeed(MovementType.Standing));
                SetMovementPadImage(direction);
            }
        }

        public void ShowMovementUI() {
            MovementPadCanvasGroupScript.alpha = 1.0f;
            MovementPadCanvasGroupScript.interactable = true;
            MovementPadCanvasGroupScript.blocksRaycasts = true;
            SpeedBarCanvasGroupScript.alpha = 1.0f;
            SpeedBarCanvasGroupScript.interactable = true;
            SpeedBarCanvasGroupScript.blocksRaycasts = true;
        }

        public void HideMovementUI() {
            MovementPadCanvasGroupScript.alpha = 0.0f;
            MovementPadCanvasGroupScript.interactable = false;
            MovementPadCanvasGroupScript.blocksRaycasts = false;
            SpeedBarCanvasGroupScript.alpha = 0.0f;
            SpeedBarCanvasGroupScript.interactable = false;
            SpeedBarCanvasGroupScript.blocksRaycasts = false;
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

        private void SetMovementPadImage(DirectionType direction)
        {
            if (DirectionType.North == direction) movementPadImage.sprite = movementPadNorth.sprite;
            else if (DirectionType.South == direction) movementPadImage.sprite = movementPadSouth.sprite;
            else if (DirectionType.East == direction) movementPadImage.sprite = movementPadEast.sprite;
            else if (DirectionType.West == direction) movementPadImage.sprite = movementPadWest.sprite;
            else if (DirectionType.NE == direction) movementPadImage.sprite = movementPadNE.sprite;
            else if (DirectionType.NW == direction) movementPadImage.sprite = movementPadNW.sprite;
            else if (DirectionType.SE == direction) movementPadImage.sprite = movementPadSE.sprite;
            else if (DirectionType.SW == direction) movementPadImage.sprite = movementPadSW.sprite;
            else if (DirectionType.Stop == direction) movementPadImage.sprite = movementPadStop.sprite;
        }

    }
}
