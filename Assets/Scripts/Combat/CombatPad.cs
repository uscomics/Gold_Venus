﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using USComics_Movement;
using USComics_Debug;

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
        private DebugConsole debugConsoleScript;
        private Keyboard KeyboardScript;
        private Color imageColor = Color.white;
        private Color imageClickedColor = new Color(1.0f, 0.3f, 0.3f, 1.0f);

        // Use this for initialization
        void Start()
        {
            GameObject debugConsole = GameObject.FindWithTag("DebugConsole") as GameObject;
            if (null != debugConsole) debugConsoleScript = debugConsole.GetComponent<DebugConsole>();
            GameObject movementPad = GameObject.FindWithTag("MovementPad") as GameObject;
            if (null != movementPad) KeyboardScript = movementPad.GetComponent<Keyboard>();

            if (null == debugConsoleScript) { Debug.LogError("CombatPad.Start: debugConsoleScript is null."); }
            if (null == KeyboardScript) { Debug.LogError("CombatPad.Start: KeyboardScript is null."); }

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
        }

        // Update is called once per frame
        void Update()
        {
            AttackType attack = GetAttack();
            UpdateButtonAppearances();
            SetButtonAppearance(attack);
        }

        private AttackType GetAttack()
        {
            AttackType attack = KeyboardScript.GetAttack();
            if (AttackType.None != attack) return attack;
            if (!Input.GetMouseButtonDown(0)) { return AttackType.None; }
            Vector2 mousePosition = Input.mousePosition;
            if (!padRect.Contains(mousePosition)) return AttackType.None;
            attack = AttackType.None;
            if (punchRect.Contains(mousePosition)) { attack = AttackType.Punch; }
            else if (kickRect.Contains(mousePosition)) { attack = AttackType.Kick; }
            else if (blockRect.Contains(mousePosition)) { attack = AttackType.Block; }
            else if (jumpkickRect.Contains(mousePosition)) { attack = AttackType.Jumpkick; }
            // TODO: Super attack
            return attack;
        }

        private void UpdateButtonAppearances()
        {
            float delta = Time.deltaTime * 3;
            if (imageColor != punchImage.color) punchImage.color = Color.Lerp(punchImage.color, imageColor, delta);
            if (imageColor != kickImage.color) kickImage.color = Color.Lerp(kickImage.color, imageColor, delta);
            if (imageColor != blockImage.color) blockImage.color = Color.Lerp(blockImage.color, imageColor, delta);
            if (imageColor != jumpkickImage.color) jumpkickImage.color = Color.Lerp(jumpkickImage.color, imageColor, delta);
        }

        private void SetButtonAppearance(AttackType attack)
        {
            if (AttackType.None == attack) return;
            else if (AttackType.Punch == attack) punchImage.color = imageClickedColor;
            else if (AttackType.Kick == attack) kickImage.color = imageClickedColor;
            else if (AttackType.Block == attack) blockImage.color = imageClickedColor;
            else if (AttackType.Jumpkick == attack) jumpkickImage.color = imageClickedColor;
        }

    }

    [System.Serializable]
    public enum AttackType { Punch, Kick, Block, Jumpkick, Super, None };
}
