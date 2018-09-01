using UnityEngine;
using USComics;
using USComics_Debug;
using USComics_Entity;
using USComics_Movement;
using ProgressBar;

namespace USComics_Combat
{
    public class CombatPad : MonoBehaviour
    {
        public AttackType CurrentAttack { get; set; }
        public CombatButton punchButton;
        public CombatButton kickButton;
        public CombatButton blockButton;
        public CombatButton jumpkickButton;

        private Rect padRect;
        private Rect punchRect;
        private Rect kickRect;
        private Rect blockRect;
        private Rect jumpkickRect;
        private Rect superRect;
        private GameObject combatPanel;
        private GameObject superBar;
        private ProgressBarBehaviour ProgressBarBehaviourScript;
        private DebugConsole debugConsoleScript;
        private Keyboard KeyboardScript;
        private PlayerController PlayerControllerScript;
        private CanvasGroup CombatPanelCanvasGroupScript;
        private CanvasGroup SuperBarCanvasGroupScript;

        // Use this for initialization
        void Start()
        {
            combatPanel = GameObject.FindWithTag("CombatPanel") as GameObject;
            if (null != combatPanel) CombatPanelCanvasGroupScript = combatPanel.GetComponent<CanvasGroup>();
            superBar = GameObject.FindWithTag("SuperBar") as GameObject;
            if (null != superBar) ProgressBarBehaviourScript = superBar.GetComponent<ProgressBarBehaviour>();
            if (null != superBar) SuperBarCanvasGroupScript = superBar.GetComponent<CanvasGroup>();
            GameObject debugConsole = GameObject.FindWithTag("DebugConsole") as GameObject;
            if (null != debugConsole) debugConsoleScript = debugConsole.GetComponent<DebugConsole>();
            GameObject movementPad = GameObject.FindWithTag("MovementPad") as GameObject;
            if (null != movementPad) KeyboardScript = movementPad.GetComponent<Keyboard>();
            GameObject playerCharacter = GameObject.FindWithTag("PlayerCharacter") as GameObject;
            if (null != playerCharacter) PlayerControllerScript = playerCharacter.GetComponent<PlayerController>();

            if (null == combatPanel) { Debug.LogError("CombatPad.Start: combatPanel is null."); }
            if (null == CombatPanelCanvasGroupScript) { Debug.LogError("CombatPad.Start: CombatPanelCanvasGroupScript is null."); }
            if (null == superBar) { Debug.LogError("CombatPad.Start: superBar is null."); }
            if (null == ProgressBarBehaviourScript) { Debug.LogError("CombatPad.Start: ProgressBarBehaviourScript is null."); }
            if (null == SuperBarCanvasGroupScript) { Debug.LogError("CombatPad.Start: SuperBarCanvasGroupScript is null."); }
            if (null == debugConsoleScript) { Debug.LogError("CombatPad.Start: debugConsoleScript is null."); }
            if (null == KeyboardScript) { Debug.LogError("CombatPad.Start: KeyboardScript is null."); }
            if (null == PlayerControllerScript) { Debug.LogError("CombatPad.Start: PlayerControllerScript is null."); }

            if (null == combatPanel) { return; }
            if (null == CombatPanelCanvasGroupScript) { return; }
            if (null == superBar) { return; }
            if (null == ProgressBarBehaviourScript) { return; }
            if (null == SuperBarCanvasGroupScript) { return; }
            if (null == debugConsoleScript) { return; }
            if (null == KeyboardScript) { return; }
            if (null == PlayerControllerScript) { return; }

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
            CurrentAttack = AttackType.None;
        }

        // Update is called once per frame
        void Update()
        {
            Collider[] enemies = PlayerControllerScript.GetEnemiesInSight();
            if (0 == enemies.Length) HideCombatUI();
            else
            {
                GameObject[] enemiesGO= GameObjectUtilities.GetGameObjects(enemies);
                ShowCombatUI();
                Attack punch = PlayerControllerScript.Attacks[(int)PlayerAttackIndex.Punch];
                Attack kick = PlayerControllerScript.Attacks[(int)PlayerAttackIndex.Kick];
                Attack block = PlayerControllerScript.Attacks[(int)PlayerAttackIndex.Block];
                Attack jumpkick = PlayerControllerScript.Attacks[(int)PlayerAttackIndex.Jumpkick];
                if (punch.IsUseable(enemiesGO)) punchButton.SetButtonStateOn();
                else punchButton.SetButtonState(ButtonState.Disabled);
                if (kick.IsUseable(enemiesGO)) kickButton.SetButtonStateOn();
                else kickButton.SetButtonState(ButtonState.Disabled);
                if (block.IsUseable(enemiesGO)) blockButton.SetButtonStateOn();
                else blockButton.SetButtonState(ButtonState.Disabled);
                if (jumpkick.IsUseable(enemiesGO)) jumpkickButton.SetButtonStateOn();
                else jumpkickButton.SetButtonState(ButtonState.Disabled);
            }
            AttackType attack = GetAttack();
            Attack(attack);
            CurrentAttack = attack;
        }

        public void ShowCombatUI()
        {
            CombatPanelCanvasGroupScript.alpha = 1.0f;
            CombatPanelCanvasGroupScript.interactable = true;
            CombatPanelCanvasGroupScript.blocksRaycasts = true;
            SuperBarCanvasGroupScript.alpha = 1.0f;
            SuperBarCanvasGroupScript.interactable = true;
            SuperBarCanvasGroupScript.blocksRaycasts = true;
        }

        public void HideCombatUI()
        {
            CombatPanelCanvasGroupScript.alpha = 0.0f;
            CombatPanelCanvasGroupScript.interactable = false;
            CombatPanelCanvasGroupScript.blocksRaycasts = false;
            SuperBarCanvasGroupScript.alpha = 0.0f;
            SuperBarCanvasGroupScript.interactable = false;
            SuperBarCanvasGroupScript.blocksRaycasts = false;
        }

        public bool IsCombatUIVisible()
        {
            return (0.0f < CombatPanelCanvasGroupScript.alpha);
        }

        public void IncrementSuperBar(float value)
        {
            if (100 > ProgressBarBehaviourScript.Value) ProgressBarBehaviourScript.IncrementValue(value);
        }

        public void ResetSuperBar()
        {
            ProgressBarBehaviourScript.Value = 0;
        }

        private AttackType GetAttack()
        {
            AttackType attack = KeyboardScript.GetAttack();
            if (AttackType.None != attack) return attack;
            if (!Input.GetMouseButtonDown(0)) { return AttackType.None; }
            Vector2 mousePosition = Input.mousePosition;
            if (!padRect.Contains(mousePosition) && !superRect.Contains(mousePosition)) return AttackType.None;
            attack = AttackType.None;
            if (punchRect.Contains(mousePosition)) { attack = punchButton.GetAttackType(); }
            else if (kickRect.Contains(mousePosition)) { attack = kickButton.GetAttackType(); }
            else if (blockRect.Contains(mousePosition)) { attack = blockButton.GetAttackType(); }
            else if (jumpkickRect.Contains(mousePosition)) { attack = jumpkickButton.GetAttackType(); }
            else if (superRect.Contains(mousePosition) && 100 == ProgressBarBehaviourScript.Value) {
                attack = AttackType.Super;
            }
            return attack;
        }

        private void Attack(AttackType inAttack)
        {
            if (AttackType.None == inAttack) return;
            PlayerAttackIndex index = PlayerControllerScript.ConvertAttackTypeToPlayerAttackIndex(inAttack);
            if (PlayerAttackIndex.None == index) return;
            Attack attack = PlayerControllerScript.GetAttackAt(index);
            if (AttackType.Super == inAttack) {
                int x = 1;
            }
            if (null == attack) return;
            if (null != PlayerControllerScript.CurrentEnemy && PlayerControllerScript.CurrentEnemy.Dead) PlayerControllerScript.CurrentEnemy = null;
            if (null == PlayerControllerScript.CurrentEnemy)
            {
                GameObject target = PlayerControllerScript.NearestInRange();
                if (null == target) return;
                PlayerControllerScript.CurrentEnemy = target.GetComponent<EnemyController>();
                PlayerControllerScript.CurrentEnemy.Targetted(PlayerControllerScript);
            }
            attack.DoAttack(PlayerControllerScript.CurrentEnemy);
            if (AttackType.Super != inAttack) UpdateSuperBar(inAttack);
            else ResetSuperBar();
        }
        private void UpdateSuperBar(AttackType inAttack)
        {
            if (AttackType.None == inAttack) return;
            PlayerAttackIndex index = PlayerControllerScript.ConvertAttackTypeToPlayerAttackIndex(inAttack);
            if (PlayerAttackIndex.None == index) return;
            Attack attack = PlayerControllerScript.GetAttackAt(index);
            if (null == attack || 0 == attack.AttackInfo.SuperBarValue) return;
            IncrementSuperBar(attack.AttackInfo.SuperBarValue);
        }
    }
}
