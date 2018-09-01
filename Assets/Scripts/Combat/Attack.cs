using System.Collections.Generic;
using UnityEngine;
using USComics;
using USComics_FX;
using USComics_Debug;
using USComics_Entity;
using USComics_Dynamic;
using USComics_Movement;
using USComics_Environment;
using USComics_Message_Manager;

namespace USComics_Combat {
    [System.Serializable]
    public class Attack {
        public string Name;
        public GameObject Attacker;
        public AttackInfo AttackInfo;
        public AttackFXInfo FXInfo = new AttackFXInfo();
        public AbstractBuffInfo[] TargetBuffs;

        private EntityController _entityControllerScript;
        private GameObject _healthPanel;
        private MessageManager _messageManagerScript;
        private GameObject _combatPanel;
        private GameObject _superBar;
        private CombatPad _combatPadScript;
        private Keyboard _keyboardScript;
        private DynamicObjectManager _dynamicObjectManagerScript;
        private DebugConsole _debugConsoleScript;
        private FX _fx;
        private bool _isSetup;

        public Attack() { }
        public Attack(Attack from) {
            Name = from.Name;
            AttackInfo = new AttackInfo(from.AttackInfo);
            TargetBuffs = from.TargetBuffs;
            Attacker = from.Attacker;
            _fx = from._fx;
        }
        public bool IsUseable(GameObject[] objs)  { return IsCharged() && AnyInRange(objs); }
        public bool IsCharged() {
            if (0 == AttackInfo.LastUsed) return true;
            return AttackInfo.LastUsed + AttackInfo.Recharge <= Time.time;
        }
        public bool InRange(GameObject obj) {
            if (!_isSetup) SetupAttack();
            return Direction.GetDistance(Attacker.transform, obj.transform) <= AttackInfo.Range;
        }
        public bool AnyInRange(GameObject[] objs) {
            for (int loop = 0; loop < objs.Length; loop++) {
                if (InRange(objs[loop])) return true;
            }
            return false;
        }
        public GameObject[] AllInRange(GameObject[] objs) {
            List<GameObject> result = new List<GameObject>();
            for (int loop = 0; loop < objs.Length; loop++) {
                if (InRange(objs[loop])) result.Add(objs[loop]);
            }
            return result.ToArray();
        }
        public void DoAttack(EntityController target) {
            if (!_isSetup) SetupAttack();
            Attack clone = new Attack(this);
            clone = ApplyBuffsToAttack(clone);
            AttackInfo.LastUsed = Time.time;
            if (!AttackInfo.Damage.IsAoE) {
                target.Attacked(_entityControllerScript, clone);
                PlayAll(clone, target);
            } else {
                Collider[] targets;
                if (!target.IsPlayer()) { targets = Environment.GetEnemiesInSight(Attacker.transform, clone.AttackInfo.Damage.RadiusAoE, 360.0f, 0.0f, 4.0f, false); }
                else { targets = Environment.GetPlayersInSight(Attacker.transform, clone.AttackInfo.Damage.RadiusAoE, 360.0f, 0.0f, 4.0f, false); }
                for (int loop = 0; loop < targets.Length; loop++) {
                    EntityController targetEntityController = targets[loop].GetComponent<EntityController>();
                    if (null != targetEntityController) targetEntityController.Attacked(_entityControllerScript, clone);
                    if (0 == loop) PlayAll(clone, targetEntityController);
                }
            }
        }

        public void StopAll() {
            if (null != _fx) _fx.Stop(); 
            
        }
        private Attack ApplyBuffsToAttack(Attack attack) {
            if (null == _entityControllerScript || null == _entityControllerScript.Buffs) return attack;
            for (int loop = 0; loop < _entityControllerScript.Buffs.Count; loop++) {
                attack = _entityControllerScript.Buffs[loop].Buff(attack);
            }
            return attack;
        }
        private void ApplyBuffsToTarget(EntityController target) {
            if (null == TargetBuffs) return;
            for (int loop = 0; loop < TargetBuffs.Length; loop++) {
                AbstractBuffInfo buffInfo = TargetBuffs[loop];
                if (null == buffInfo) continue;
                AbstractBuff buff = buffInfo.GetBuff();
                buff.Target = target;
                buff.Attacker = _entityControllerScript;
                target.AddBuff(buff);
            }
        }
        private void ApplyDoTToTarget(Attack attack, EntityController target) {
            if (!attack.AttackInfo.Damage.IsDoT) return;
            DamageDoTEntity dot = new DamageDoTEntity();
            dot.FromAttack(this, _entityControllerScript, target);
            target.AddBuff(dot);
        }
        private void PlayAll(Attack attack, EntityController target) {
            if (!_isSetup) SetupAttack();
            if (null != _fx) {
                _fx.OptionalTarget = target.Entity;
                _fx.PlayCoroutine();
            }
            PlayEmote();
            if (null != attack.AttackInfo.Damage.PointsObject) {
                _dynamicObjectManagerScript.Clone(attack.AttackInfo.Damage.PointsObject, target.Entity.transform.position + (target.Entity.transform.up * 1.2f), 0.0f, 180.0f, 0.0f);
            }
            _entityControllerScript.AttackBonus.CalculateBonus(attack, _entityControllerScript, target);
            ApplyDoTToTarget(attack, target);
            ApplyBuffsToTarget(target);
        }
        private void PlayEmote() {
            if (null == _entityControllerScript.CombatEmoteSource) return;
            if (0 == _entityControllerScript.CombatEmoteSounds.Length) return;
            int chance = Random.Range(1, 101);
            if (chance > _entityControllerScript.CombatEmoteChance) return;
            int randomEmote = Random.Range(0, _entityControllerScript.CombatEmoteSounds.Length);
            _entityControllerScript.CombatEmoteSource.PlayOneShot(_entityControllerScript.CombatEmoteSounds[randomEmote]);
        }
        private bool SetupAttack() {
            if (null != Attacker) _entityControllerScript = Attacker.GetComponent<EntityController>();
            if (null != Attacker) _fx = Attacker.AddComponent<FX>();
            if (null != _fx) {
                FXInfo.CopyTo(_fx);
                _fx.InitFX();
            }
            GameObject messageCanvas = GameObject.FindWithTag("MessageCanvas") as GameObject;
            if (null != messageCanvas) _messageManagerScript = messageCanvas.GetComponent<MessageManager>();
            _healthPanel = GameObject.FindWithTag("HealthGameObject") as GameObject;
            GameObject debugConsole = GameObject.FindWithTag("DebugConsole") as GameObject;
            if (null != debugConsole) _debugConsoleScript = debugConsole.GetComponent<DebugConsole>();
            GameObject dynamicObjects = GameObject.FindWithTag("DynamicObjects") as GameObject;
            if (null != dynamicObjects) _dynamicObjectManagerScript = dynamicObjects.GetComponent<DynamicObjectManager>();
            _combatPanel = GameObject.FindWithTag("CombatPanel") as GameObject;
            if (null != _combatPanel) _combatPadScript = _combatPanel.GetComponent<CombatPad>();
            _superBar = GameObject.FindWithTag("SuperBar") as GameObject;
            GameObject movementPad = GameObject.FindWithTag("MovementPad") as GameObject;
            if (null != movementPad) _keyboardScript = movementPad.GetComponent<Keyboard>();

            if (null == Attacker) { Debug.LogError("Attack.SetupAttack: Attacker is null."); }
            if (null == _entityControllerScript) { Debug.LogError("Attack.SetupAttack: _entityControllerScript is null."); }
            if (null == _messageManagerScript) { Debug.LogError("Attack.SetupAttack: _messageManagerScript is null."); }
            if (null == _healthPanel) { Debug.LogError("Attack.SetupAttack: _healthPanel is null."); }
            if (null == _debugConsoleScript) { Debug.LogError("Attack.SetupAttack: _debugConsoleScript is null."); }
            if (null == _dynamicObjectManagerScript) { Debug.LogError("Attack.SetupAttack: _dynamicObjectManagerScript is null."); }
            if (null == _combatPanel) { Debug.LogError("Attack.SetupAttack: _combatPanel is null."); }
            if (null == _superBar) { Debug.LogError("Attack.SetupAttack: _superBar is null."); }
            if (null == _combatPadScript) { Debug.LogError("Attack.SetupAttack: _combatPadScript is null."); }
            if (null == _keyboardScript) { Debug.LogError("Attack.SetupAttack: _keyboardScript is null."); }

            if (null == Attacker) { return false; }
            if (null == _messageManagerScript) { return false; }
            if (null == _healthPanel) { return false; }
            if (null == _debugConsoleScript) { return false; }
            if (null == _dynamicObjectManagerScript) { return false; }
            if (null == _combatPanel) { return false; }
            if (null == _superBar) { return false; }
            if (null == _combatPadScript) { return false; }
            if (null == _keyboardScript) { return false; }

            _isSetup = true;
            return true;
        }
    }
}