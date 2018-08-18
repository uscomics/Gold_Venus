using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using USComics_Movement;
using USComics_Debug;
using ProgressBar;

namespace USComics_Combat
{
    public class CombatPad : MonoBehaviour
    {
        public AttackType CurrentAttack { get; set; }
        public Image punchImage;
        public Image kickImage;
        public Image blockImage;
        public Image jumpkickImage;

        private Rect padRect;
        private Rect punchRect;
        private Rect kickRect;
        private Rect blockRect;
        private Rect jumpkickRect;
        private Rect superRect;
        private float deltaPunch = 13;
        private float deltaBlock = 13;
        private float deltaKick = 1.0f;
        private float deltaJumpkick = 0.5f;
        private GameObject combatPanel;
        private GameObject superBar;
        private ProgressBarBehaviour ProgressBarBehaviourScript;
        private DebugConsole debugConsoleScript;
        private Keyboard KeyboardScript;
        private Color imageColor = Color.white;
        private Color imageClickedColor = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        private Color punchColor = Color.white;
        private Color kickColor = Color.white;
        private Color blockColor = Color.white;
        private Color jumpkickColor = Color.white;

        // Use this for initialization
        void Start()
        {
            combatPanel = GameObject.FindWithTag("CombatPanel") as GameObject;
            superBar = GameObject.FindWithTag("SuperBar") as GameObject;
            if (null != superBar) ProgressBarBehaviourScript = superBar.GetComponent<ProgressBarBehaviour>();
            GameObject debugConsole = GameObject.FindWithTag("DebugConsole") as GameObject;
            if (null != debugConsole) debugConsoleScript = debugConsole.GetComponent<DebugConsole>();
            GameObject movementPad = GameObject.FindWithTag("MovementPad") as GameObject;
            if (null != movementPad) KeyboardScript = movementPad.GetComponent<Keyboard>();

            if (null == combatPanel) { Debug.LogError("CombatPad.Start: combatPanel is null."); }
            if (null == superBar) { Debug.LogError("CombatPad.Start: superBar is null."); }
            if (null == ProgressBarBehaviourScript) { Debug.LogError("CombatPad.Start: ProgressBarBehaviourScript is null."); }
            if (null == debugConsoleScript) { Debug.LogError("CombatPad.Start: debugConsoleScript is null."); }
            if (null == KeyboardScript) { Debug.LogError("CombatPad.Start: KeyboardScript is null."); }

            if (null == combatPanel) { return; }
            if (null == superBar) { return; }
            if (null == ProgressBarBehaviourScript) { return; }
            if (null == debugConsoleScript) { return; }
            if (null == KeyboardScript) { return; }

            float padLeft = 20.0f;
            float padBottom = 70.0f;
            float padWidth = 256.0f;
            float padHeight = 256.0f;
            padRect = new Rect(padLeft, padBottom, padWidth, padHeight);
            float punchLeft = padLeft;
            float punchBottom = padBottom + 128.0f;
            float punchWidth = 128.0f;
            float punchHeight = 128.0f;
            punchRect = new Rect(punchLeft, punchBottom, punchWidth, punchHeight);
            float kickLeft = padLeft + 128.0f;
            float kickBottom = padBottom + 128.0f;
            float kickWidth = 128.0f;
            float kickHeight = 128.0f;
            kickRect = new Rect(kickLeft, kickBottom, kickWidth, kickHeight);
            float blockLeft = padLeft;
            float blockBottom = padBottom;
            float blockWidth = 128.0f;
            float blockHeight = 128.0f;
            blockRect = new Rect(blockLeft, blockBottom, blockWidth, blockHeight);
            float jumpkickLeft = padLeft + 128.0f;
            float jumpkickBottom = padBottom;
            float jumpkickWidth = 128.0f;
            float jumpkickHeight = 128.0f;
            jumpkickRect = new Rect(jumpkickLeft, jumpkickBottom, jumpkickWidth, jumpkickHeight);
            float superLeft = 30.0f;
            float superBottom = 20.0f;
            float superWidth = 225.0f;
            float superHeight = 30.0f;
            superRect = new Rect(superLeft, superBottom, superWidth, superHeight);
        }

        // Update is called once per frame
        void Update()
        {
            AttackType attack = AdjustAttack(GetAttack());
            UpdateButtonAppearances();
            SetButtonAppearance(attack);
            UpdateSuperBar(attack);
            CurrentAttack = attack;
        }

        public void IncrementSuperBar(float value)
        {
            ProgressBarBehaviourScript.IncrementValue(value);
        }

        public void ClearKickTimer()
        {
            kickImage.color = imageColor;
        }

        public void ClearJumpkickTimer()
        {
            jumpkickImage.color = imageColor;
        }

        private AttackType GetAttack()
        {
            AttackType attack = KeyboardScript.GetAttack();
            if (AttackType.None != attack) return attack;
            if (!Input.GetMouseButtonDown(0)) { return AttackType.None; }
            Vector2 mousePosition = Input.mousePosition;
            if (!padRect.Contains(mousePosition) && !superRect.Contains(mousePosition)) return AttackType.None;
            attack = AttackType.None;
            if (punchRect.Contains(mousePosition)) { attack = AttackType.Punch; }
            else if (kickRect.Contains(mousePosition) && imageColor == kickImage.color) { attack = AttackType.Kick; }
            else if (blockRect.Contains(mousePosition)) { attack = AttackType.Block; }
            else if (jumpkickRect.Contains(mousePosition) && imageColor == jumpkickImage.color) { attack = AttackType.Jumpkick; }
            else if (superRect.Contains(mousePosition) && 100 == ProgressBarBehaviourScript.Value) {
                attack = AttackType.Super;
                ProgressBarBehaviourScript.Value = 0;
            }
            return attack;
        }

        private AttackType AdjustAttack(AttackType attack)
        {
            if (AttackType.None == attack) return attack;
            else if ((AttackType.Punch == attack) && (imageColor != punchImage.color)) return AttackType.None;
            else if ((AttackType.Kick == attack) && (imageColor != kickImage.color)) return AttackType.None;
            else if ((AttackType.Block == attack) && (imageColor != blockImage.color)) return AttackType.None;
            else if ((AttackType.Jumpkick == attack) && (imageColor != jumpkickImage.color)) return AttackType.None;
            return attack;
        }

        private void UpdateButtonAppearances()
        {
            float delta = (Time.deltaTime * 10);
            if (imageColor != punchImage.color) punchColor = Color.Lerp(punchColor, imageColor, delta * deltaPunch);
            if (imageColor != kickImage.color) kickColor = Color.Lerp(kickColor, imageColor, delta * deltaKick);
            if (imageColor != blockImage.color) blockColor = Color.Lerp(blockColor, imageColor, delta * deltaBlock);
            if (imageColor != jumpkickImage.color) jumpkickColor = Color.Lerp(jumpkickColor, imageColor, delta * deltaJumpkick);
            if (imageColor == punchColor) punchImage.color = imageColor;
            if (imageColor == kickColor) kickImage.color = imageColor;
            if (imageColor == blockColor) blockImage.color = imageColor;
            if (imageColor == jumpkickColor) jumpkickImage.color = imageColor;
        }

        private void SetButtonAppearance(AttackType attack)
        {
            if (AttackType.None == attack) return;
            else if (AttackType.Punch == attack) punchColor = punchImage.color = imageClickedColor;
            else if (AttackType.Kick == attack) kickColor = kickImage.color = imageClickedColor;
            else if (AttackType.Block == attack) blockColor = blockImage.color = imageClickedColor;
            else if (AttackType.Jumpkick == attack) jumpkickColor = jumpkickImage.color = imageClickedColor;
        }

        private void UpdateSuperBar(AttackType attack)
        {
            if (AttackType.None == attack) return;
            else if (AttackType.Punch == attack) IncrementSuperBar(3f);
            else if (AttackType.Kick == attack) IncrementSuperBar(4f);
            else if (AttackType.Block == attack) IncrementSuperBar(0f);
            else if (AttackType.Jumpkick == attack) IncrementSuperBar(5f);
        }

    }
}
