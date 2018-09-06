using UnityEngine;
using UnityEngine.UI;

namespace USComics_Movement {
    public class ClimbPad : MonoBehaviour {
        public DirectionType CurrentDirection { get; set; }
        public Image ClimbPadStop;
        public Image ClimbPadUp;
        public Image ClimbPadDown;

        private Rect _padRect;
        private Rect _centerRect;
        private Rect _upRect;
        private Rect _downRect;
        private Image _climbPadImage;
        private Keyboard _keyboardScript;
        private CanvasGroup _climbPadCanvasGroup;
        private CanvasGroup _climbingPanelCanvasGroup;

        // Use this for initialization
        void Start() {
            GameObject climbPad = GameObject.FindWithTag("ClimbPad") as GameObject;
            if (null != climbPad) _climbPadCanvasGroup = climbPad.GetComponent<CanvasGroup>();
            if (null != climbPad) _keyboardScript = climbPad.GetComponent<Keyboard>();
            if (null != climbPad) _climbPadImage = climbPad.GetComponent<Image>();
            GameObject climbingPanel = GameObject.FindWithTag("ClimbingPanel") as GameObject;
            if (null != climbingPanel) _climbingPanelCanvasGroup = climbingPanel.GetComponent<CanvasGroup>();

            if (null == _climbPadCanvasGroup) { Debug.LogError("ClimbPad.Start: _climbPadCanvasGroup is null."); }
            if (null == _keyboardScript) { Debug.LogError("ClimbPad.Start: _keyboardScript is null."); }
            if (null == _climbingPanelCanvasGroup) { Debug.LogError("ClimbPad.Start: _climbingPanelCanvasGroup is null."); }

            if (null == _climbPadCanvasGroup) { return; }
            if (null == _keyboardScript) { return; }
            if (null == _climbingPanelCanvasGroup) { return; }

            CurrentDirection = DirectionType.None;

            float padLeft = 500.0f;
            float padBottom = 60.0f;
            float padWidth = 255.0f;
            float padHeight = 255.0f;
            _padRect = new Rect(padLeft, padBottom, padWidth, padHeight);
            float centerLeft = padLeft + 75.0f;
            float centerBottom = padBottom + 84.0f;
            float centerWidth = 108.0f;
            float centerHeight = 90.0f;
            _centerRect = new Rect(centerLeft, centerBottom, centerWidth, centerHeight);
            float upLeft = padLeft + 95.0f;
            float upBottom = padBottom + 185.0f;
            float upWidth = 64.0f;
            float upHeight = 55.0f;
            _upRect = new Rect(upLeft, upBottom, upWidth, upHeight);
            float downLeft = upLeft;
            float downBottom = padBottom + 10.0f;
            float downWidth = upWidth;
            float downHeight = upHeight;
            _downRect = new Rect(downLeft, downBottom, downWidth, downHeight);
        }

        // Update is called once per frame
        void Update() {
            DirectionType direction = GetDirection();
            if (DirectionType.None != direction) {
                CurrentDirection = direction;
                // if (DirectionType.Stop == direction) SpeedBarScript.SetSpeed(MovementSpeed.GetSpeed(MovementType.Standing));
                SetMovementPadImage(direction);
            }
        }

        public void ShowClimbingUI() {
            _climbPadCanvasGroup.alpha = 1.0f;
            _climbPadCanvasGroup.interactable = true;
            _climbPadCanvasGroup.blocksRaycasts = true;
            _climbingPanelCanvasGroup.alpha = 1.0f;
            _climbingPanelCanvasGroup.interactable = true;
            _climbingPanelCanvasGroup.blocksRaycasts = true;
        }
        public void HideClimbingUI() {
            _climbPadCanvasGroup.alpha = 0.0f;
            _climbPadCanvasGroup.interactable = false;
            _climbPadCanvasGroup.blocksRaycasts = false;
            _climbingPanelCanvasGroup.alpha = 0.0f;
            _climbingPanelCanvasGroup.interactable = false;
            _climbingPanelCanvasGroup.blocksRaycasts = false;
        }
        public void SetDirection(DirectionType direction) {
            if (DirectionType.None == direction) return;
            CurrentDirection = direction;
            SetMovementPadImage(direction);
        }

        private DirectionType GetDirection() {
            DirectionType direction = _keyboardScript.GetDirection();
            if (DirectionType.None != direction) return direction;
            if (!Input.GetMouseButtonDown(0)) { return DirectionType.None; }
            Vector2 mousePosition = Input.mousePosition;
            if (!_padRect.Contains(mousePosition)) return DirectionType.None;
            direction = DirectionType.None;
            if (_centerRect.Contains(mousePosition)) { direction = DirectionType.Stop; }
            else if (_upRect.Contains(mousePosition)) { direction = DirectionType.North; }
            else if (_downRect.Contains(mousePosition)) { direction = DirectionType.South; }
            return direction;
        }
        private void SetMovementPadImage(DirectionType direction) {
            if (DirectionType.North == direction) _climbPadImage.sprite = ClimbPadUp.sprite;
            else if (DirectionType.South == direction) _climbPadImage.sprite = ClimbPadDown.sprite;
            else if (DirectionType.Stop == direction) _climbPadImage.sprite = ClimbPadStop.sprite;
        }

    }
}
