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

        private Rect _padRect;
        private Rect _punchRect;
        private Rect _kickRect;
        private Rect _blockRect;
        private Rect _jumpkickRect;
        private Rect _superRect;
        private GameObject _combatPanel;
        private GameObject _superBar;
        private ProgressBarBehaviour _progressBarBehaviourScript;
        private Keyboard _keyboardScript;
        private PlayerController _playerControllerScript;
        private CanvasGroup _combatPanelCanvasGroupScript;
        private CanvasGroup _superBarCanvasGroupScript;
        private bool _superInRange;

        // Use this for initialization
        void Start()
        {
            _combatPanel = GameObject.FindWithTag("CombatPanel") as GameObject;
            if (null != _combatPanel) _combatPanelCanvasGroupScript = _combatPanel.GetComponent<CanvasGroup>();
            _superBar = GameObject.FindWithTag("SuperBar") as GameObject;
            if (null != _superBar) _progressBarBehaviourScript = _superBar.GetComponent<ProgressBarBehaviour>();
            if (null != _superBar) _superBarCanvasGroupScript = _superBar.GetComponent<CanvasGroup>();
            GameObject movementPad = GameObject.FindWithTag("MovementPad") as GameObject;
            if (null != movementPad) _keyboardScript = movementPad.GetComponent<Keyboard>();
            GameObject playerCharacter = GameObject.FindWithTag("PlayerCharacter") as GameObject;
            if (null != playerCharacter) _playerControllerScript = playerCharacter.GetComponent<PlayerController>();

            if (null == _combatPanel) { Debug.LogError("CombatPad.Start: combatPanel is null."); }
            if (null == _combatPanelCanvasGroupScript) { Debug.LogError("CombatPad.Start: CombatPanelCanvasGroupScript is null."); }
            if (null == _superBar) { Debug.LogError("CombatPad.Start: superBar is null."); }
            if (null == _progressBarBehaviourScript) { Debug.LogError("CombatPad.Start: ProgressBarBehaviourScript is null."); }
            if (null == _superBarCanvasGroupScript) { Debug.LogError("CombatPad.Start: SuperBarCanvasGroupScript is null."); }
            if (null == _keyboardScript) { Debug.LogError("CombatPad.Start: KeyboardScript is null."); }
            if (null == _playerControllerScript) { Debug.LogError("CombatPad.Start: PlayerControllerScript is null."); }

            if (null == _combatPanel) { return; }
            if (null == _combatPanelCanvasGroupScript) { return; }
            if (null == _superBar) { return; }
            if (null == _progressBarBehaviourScript) { return; }
            if (null == _superBarCanvasGroupScript) { return; }
            if (null == _keyboardScript) { return; }
            if (null == _playerControllerScript) { return; }

            float padLeft = 20.0f;
            float padBottom = 70.0f;
            float padWidth = 256.0f;
            float padHeight = 256.0f;
            _padRect = new Rect(padLeft, padBottom, padWidth, padHeight);
            float punchLeft = padLeft;
            float punchBottom = padBottom + 128.0f;
            float punchWidth = 128.0f;
            float punchHeight = 128.0f;
            _punchRect = new Rect(punchLeft, punchBottom, punchWidth, punchHeight);
            float kickLeft = padLeft + 128.0f;
            float kickBottom = padBottom + 128.0f;
            float kickWidth = 128.0f;
            float kickHeight = 128.0f;
            _kickRect = new Rect(kickLeft, kickBottom, kickWidth, kickHeight);
            float blockLeft = padLeft;
            float blockBottom = padBottom;
            float blockWidth = 128.0f;
            float blockHeight = 128.0f;
            _blockRect = new Rect(blockLeft, blockBottom, blockWidth, blockHeight);
            float jumpkickLeft = padLeft + 128.0f;
            float jumpkickBottom = padBottom;
            float jumpkickWidth = 128.0f;
            float jumpkickHeight = 128.0f;
            _jumpkickRect = new Rect(jumpkickLeft, jumpkickBottom, jumpkickWidth, jumpkickHeight);
            float superLeft = 30.0f;
            float superBottom = 20.0f;
            float superWidth = 225.0f;
            float superHeight = 30.0f;
            _superRect = new Rect(superLeft, superBottom, superWidth, superHeight);
            CurrentAttack = AttackType.None;
        }

        // Update is called once per frame
        void Update()
        {
            Collider[] enemies = _playerControllerScript.GetEnemiesInSight();
            if (0 == enemies.Length) HideCombatUI();
            else
            {
                GameObject[] enemiesGO= GameObjectUtilities.GetGameObjects(enemies);
                ShowCombatUI();
                Attack punch = _playerControllerScript.Attacks[(int)PlayerAttackIndex.Punch];
                Attack kick = _playerControllerScript.Attacks[(int)PlayerAttackIndex.Kick];
                Attack block = _playerControllerScript.Attacks[(int)PlayerAttackIndex.Block];
                Attack jumpkick = _playerControllerScript.Attacks[(int)PlayerAttackIndex.Jumpkick];
                Attack super = _playerControllerScript.Attacks[(int)PlayerAttackIndex.Super];
                if (punch.IsUseable(enemiesGO)) punchButton.SetButtonStateOn();
                else punchButton.SetButtonState(ButtonState.Disabled);
                if (kick.IsUseable(enemiesGO)) kickButton.SetButtonStateOn();
                else kickButton.SetButtonState(ButtonState.Disabled);
                if (block.IsUseable(enemiesGO)) blockButton.SetButtonStateOn();
                else blockButton.SetButtonState(ButtonState.Disabled);
                if (jumpkick.IsUseable(enemiesGO)) jumpkickButton.SetButtonStateOn();
                else jumpkickButton.SetButtonState(ButtonState.Disabled);
                _superInRange = super.IsUseable(enemiesGO);
            }
            AttackType attack = GetAttack();
            Attack(attack);
            CurrentAttack = attack;
        }

        public void ShowCombatUI()
        {
            _combatPanelCanvasGroupScript.alpha = 1.0f;
            _combatPanelCanvasGroupScript.interactable = true;
            _combatPanelCanvasGroupScript.blocksRaycasts = true;
            _superBarCanvasGroupScript.alpha = 1.0f;
            _superBarCanvasGroupScript.interactable = true;
            _superBarCanvasGroupScript.blocksRaycasts = true;
        }

        public void HideCombatUI()
        {
            _combatPanelCanvasGroupScript.alpha = 0.0f;
            _combatPanelCanvasGroupScript.interactable = false;
            _combatPanelCanvasGroupScript.blocksRaycasts = false;
            _superBarCanvasGroupScript.alpha = 0.0f;
            _superBarCanvasGroupScript.interactable = false;
            _superBarCanvasGroupScript.blocksRaycasts = false;
        }

        public bool IsCombatUIVisible()
        {
            return (0.0f < _combatPanelCanvasGroupScript.alpha);
        }

        public void IncrementSuperBar(float value)
        {
            if (100 > _progressBarBehaviourScript.Value) _progressBarBehaviourScript.IncrementValue(value);
        }

        public void ResetSuperBar()
        {
            _progressBarBehaviourScript.Value = 0;
        }

        private AttackType GetAttack()
        {
            AttackType attack = _keyboardScript.GetAttack();
            if (AttackType.None != attack) return attack;
            if (!Input.GetMouseButtonDown(0)) { return AttackType.None; }
            Vector2 mousePosition = Input.mousePosition;
            if (!_padRect.Contains(mousePosition) && !_superRect.Contains(mousePosition)) return AttackType.None;
            attack = AttackType.None;
            if (_punchRect.Contains(mousePosition)) { attack = punchButton.GetAttackType(); }
            else if (_kickRect.Contains(mousePosition)) { attack = kickButton.GetAttackType(); }
            else if (_blockRect.Contains(mousePosition)) { attack = blockButton.GetAttackType(); }
            else if (_jumpkickRect.Contains(mousePosition)) { attack = jumpkickButton.GetAttackType(); }
            else if (_superRect.Contains(mousePosition) && 100 == _progressBarBehaviourScript.Value) {
                attack = AttackType.Super;
            }

            if (AttackType.Super == attack && !_superInRange) attack = AttackType.None;
            return attack;
        }

        private void Attack(AttackType inAttack)
        {
            if (AttackType.None == inAttack) return;
            PlayerAttackIndex index = _playerControllerScript.ConvertAttackTypeToPlayerAttackIndex(inAttack);
            if (PlayerAttackIndex.None == index) return;
            Attack attack = _playerControllerScript.GetAttackAt(index);
            if (AttackType.Super == inAttack) {
                int x = 1;
            }
            if (null == attack) return;
            if (null != _playerControllerScript.CurrentEnemy && _playerControllerScript.CurrentEnemy.Dead) _playerControllerScript.CurrentEnemy = null;
            if (null == _playerControllerScript.CurrentEnemy)
            {
                GameObject target = _playerControllerScript.GetNearestEnemyInRange();
                if (null == target) return;
                _playerControllerScript.CurrentEnemy = target.GetComponent<EnemyController>();
                _playerControllerScript.CurrentEnemy.Targetted(_playerControllerScript);
            }
            attack.DoAttack(_playerControllerScript.CurrentEnemy);
            if (AttackType.Super != inAttack) UpdateSuperBar(inAttack);
            else ResetSuperBar();
        }
        private void UpdateSuperBar(AttackType inAttack)
        {
            if (AttackType.None == inAttack) return;
            PlayerAttackIndex index = _playerControllerScript.ConvertAttackTypeToPlayerAttackIndex(inAttack);
            if (PlayerAttackIndex.None == index) return;
            Attack attack = _playerControllerScript.GetAttackAt(index);
            if (null == attack || 0 == attack.AttackInfo.SuperBarValue) return;
            IncrementSuperBar(attack.AttackInfo.SuperBarValue);
        }
    }
}
