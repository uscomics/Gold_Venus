using UnityEngine;
using UnityEngine.UI;

namespace USComics_Movement {
    public class FallPad : MonoBehaviour {
        private CanvasGroup _fallPadCanvasGroup;

        // Use this for initialization
        void Start() {
            GameObject fallPad = GameObject.FindWithTag("Falling Paanel") as GameObject;
            if (null != fallPad) _fallPadCanvasGroup = fallPad.GetComponent<CanvasGroup>();

            if (null == _fallPadCanvasGroup) { Debug.LogError("ClimbPad.Start: _fallPadCanvasGroup is null."); }

            if (null == _fallPadCanvasGroup) { return; }
        }

        // Update is called once per frame
        void Update() { }

        public void ShowFallingUI() {
            _fallPadCanvasGroup.alpha = 1.0f;
            _fallPadCanvasGroup.interactable = true;
            _fallPadCanvasGroup.blocksRaycasts = true;
        }
        public void HideFallingUI() {
            _fallPadCanvasGroup.alpha = 0.0f;
            _fallPadCanvasGroup.interactable = false;
            _fallPadCanvasGroup.blocksRaycasts = false;
        }
    }
}
