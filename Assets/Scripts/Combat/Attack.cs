using System.Collections.Generic;
using UnityEngine;
using USComics_Debug;
using USComics_Entity;
using USComics_Dynamic;
using USComics_Movement;
using USComics_Environment;
using USComics_Message_Manager;

namespace USComics_Combat {
    [System.Serializable]
    public class Attack {
        public string name;
        public AttackInfo AttackInfo;
        public string[] animationNames;
        public AudioSource audioSource1;
        public AudioSource audioSource2;
        public AudioClip sound1;
        public AudioClip sound2;
        public ParticleSystem[] particleSystems;
        public Light[] lights;
        public Vector3 ForceVector = Vector3.zero;      // Vector3.zero means no force
        public ForceMode ForceMode = ForceMode.Impulse;
        public bool ForceFromCenter;                    // Set to true for an explosion-like (radial) force, or false for a linear force.
        public AbstractBuffInfo[] targetBuffs;
        public GameObject entity;

        private EntityController EntityControllerScript;
        private GameObject healthPanel;
        private Animator Anim;
        private MessageManager messageManagerScript;
        private GameObject combatPanel;
        private GameObject superBar;
        private CombatPad CombatPadScript;
        private Keyboard KeyboardScript;
        private DynamicObjectManager DynamicObjectManagerScript;
        private DebugConsole debugConsoleScript;
        private bool isSetup;

        public Attack() { }
        public Attack(Attack @from) {
            name = @from.name;
            AttackInfo = new AttackInfo(@from.AttackInfo);
            animationNames = @from.animationNames;
            audioSource1 = @from.audioSource1;
            audioSource2 = @from.audioSource2;
            sound1 = @from.sound1;
            sound2 = @from.sound2;
            particleSystems = @from.particleSystems;
            lights = @from.lights;
            ForceVector = @from.ForceVector;
            ForceMode = @from.ForceMode;
            ForceFromCenter = @from.ForceFromCenter;
            targetBuffs = @from.targetBuffs;
            entity = @from.entity;
        }
        public bool IsUseable(GameObject[] objs)  { return IsCharged() && AnyInRange(objs); }
        public bool IsCharged() {
            if (0 == AttackInfo.LastUsed) return true;
            return AttackInfo.LastUsed + AttackInfo.Recharge <= Time.time;
        }
        public bool InRange(GameObject obj) {
            if (!isSetup) SetupAttack();
            return Direction.GetDistance(entity.transform, obj.transform) <= AttackInfo.Range;
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
            if (!isSetup) SetupAttack();
            Attack clone = new Attack(this);
            clone = ApplyBuffsToAttack(clone);
            AttackInfo.LastUsed = Time.time;
            if (!AttackInfo.Damage.IsAoE) {
                target.Attacked(EntityControllerScript, clone);
                PlayAll(clone, target);
            } else {
                Collider[] targets;
                if (!target.IsPlayer()) { targets = Environment.GetEnemiesInSight(entity.transform, clone.AttackInfo.Damage.RadiusAoE, 360.0f, 0.0f, 4.0f, false); }
                else { targets = Environment.GetPlayersInSight(entity.transform, clone.AttackInfo.Damage.RadiusAoE, 360.0f, 0.0f, 4.0f, false); }
                for (int loop = 0; loop < targets.Length; loop++) {
                    EntityController entityController = targets[loop].GetComponent<EntityController>();
                    if (null != entityController) entityController.Attacked(EntityControllerScript, clone);
                    if (0 == loop) PlayAll(clone, entityController);
                }
            }
        }
        public void StopAll() {
            StopSounds();
            StopParticleSystems();
            StopLights();
        }
        private Attack ApplyBuffsToAttack(Attack attack) {
            if (null == EntityControllerScript || null == EntityControllerScript.buffs) return attack;
            for (int loop = 0; loop < EntityControllerScript.buffs.Count; loop++) {
                attack = EntityControllerScript.buffs[loop].Buff(attack);
            }
            return attack;
        }
        private void ApplyBuffsToTarget(EntityController target) {
            if (null == targetBuffs) return;
            for (int loop = 0; loop < targetBuffs.Length; loop++) {
                AbstractBuffInfo buffInfo = targetBuffs[loop];
                if (null == buffInfo) continue;
                AbstractBuff buff = buffInfo.GetBuff();
                buff.Target = target;
                buff.Attacker = EntityControllerScript;
                target.AddBuff(buff);
            }
        }
        private void ApplyDoTToTarget(Attack attack, EntityController target) {
            if (!attack.AttackInfo.Damage.IsDoT) return;
            DamageDoTEntity dot = new DamageDoTEntity();
            dot.FromAttack(this, EntityControllerScript, target);
            target.AddBuff(dot);
        }
        private void PlayAll(Attack attack, EntityController target) {
            if (!isSetup) SetupAttack();
            PlaySounds();
            PlayAnimation();
            PlayEmote();
            PlayParticleSystems();
            PlayLights();
            SpawnPoints(target);
            EntityControllerScript.AttackBonus.CalculateBonus(attack, EntityControllerScript, target);
            ApplyDoTToTarget(attack, target);
            ApplyBuffsToTarget(target);
            ApplyForce(EntityControllerScript.transform, AttackInfo.Damage.RadiusAoE);
        }
        private void PlaySounds() {
            if (null == audioSource1) return;
            if (null == sound1) return;
            audioSource1.PlayOneShot(sound1);
            if (null == sound2) return;
            if (null == audioSource2) {
                Debug.LogError("Need two audio sources to play two audio clips.");
                return;
            }
            audioSource2.PlayOneShot(sound2);
        }
        private void StopSounds() {
            if (null != audioSource1 && audioSource1.isPlaying) audioSource1.Stop();
            if (null != audioSource2 && audioSource2.isPlaying) audioSource2.Stop();
        }
        private void PlayEmote() {
            if (null == EntityControllerScript.CombatEmoteSource) return;
            if (0 == EntityControllerScript.CombatEmoteSounds.Length) return;
            int chance = Random.Range(1, 101);
            if (chance > EntityControllerScript.CombatEmoteChance) return;
            int randomEmote = Random.Range(0, EntityControllerScript.CombatEmoteSounds.Length);
            EntityControllerScript.CombatEmoteSource.PlayOneShot(EntityControllerScript.CombatEmoteSounds[randomEmote]);
        }
        private void PlayAnimation() {
            if (null == Anim) return;
            if (0 == animationNames.Length) return;
            int randomAniation = Random.Range(0, animationNames.Length);
            Anim.Play(animationNames[randomAniation]);
        }
        private void PlayParticleSystems() { foreach (var particleSystem in particleSystems) { if (null != particleSystem) particleSystem.Play(); }}
        private void StopParticleSystems() { foreach (var particleSystem in particleSystems) { if (null != particleSystem) particleSystem.Stop(); }}
        private void PlayLights() { foreach (var light in lights) { if (null != light) light.enabled = true; }}
        private void StopLights() { foreach (var light in lights) { if (null != light) light.enabled = false; }}
        private void SpawnPoints(EntityController target) {
            if (null == AttackInfo.Damage.PointsObject) return;
            DynamicObjectManagerScript.Clone(AttackInfo.Damage.PointsObject, target.transform.position, 0.0f, 180.0f, 0.0f);
        }
        private void ApplyForce(Transform transform, float radius) {
            if (ForceVector == Vector3.zero) return;
            Collider[] targets = Environment.GetObjectsInRadius(transform.position, radius, LayerMaskValues.PLAYER | LayerMaskValues.ENEMY);
            ApplyForce(transform, targets);
        }
        private void ApplyForce(Transform transform, Collider[] targets) {
            if (ForceVector == Vector3.zero) return; 
            ApplyForce(transform, GameObjectUtilities.GetGameObjects(targets));
        }
        private void ApplyForce(Transform transform, GameObject[] targets) {
            if (ForceVector == Vector3.zero) return; 
            ApplyForce(transform, GameObjectUtilities.GetRigidbodies(targets));
        }
        private void ApplyForce(Transform transform, Rigidbody[] targets) {
            if (ForceVector == Vector3.zero) return;
            foreach (var target in targets) {
                if (ForceFromCenter) target.AddForceAtPosition(ForceVector, transform.position, ForceMode);
                else target.AddForce(ForceVector, ForceMode);
            }
        }
        private bool SetupAttack() {
            if (null != entity) EntityControllerScript = entity.GetComponent<EntityController>();
            if (null != entity) Anim = entity.GetComponent<Animator>();
            GameObject messageCanvas = GameObject.FindWithTag("MessageCanvas") as GameObject;
            if (null != messageCanvas) messageManagerScript = messageCanvas.GetComponent<MessageManager>();
            healthPanel = GameObject.FindWithTag("HealthGameObject") as GameObject;
            GameObject debugConsole = GameObject.FindWithTag("DebugConsole") as GameObject;
            if (null != debugConsole) debugConsoleScript = debugConsole.GetComponent<DebugConsole>();
            GameObject dynamicObjects = GameObject.FindWithTag("DynamicObjects") as GameObject;
            if (null != dynamicObjects) DynamicObjectManagerScript = dynamicObjects.GetComponent<DynamicObjectManager>();
            combatPanel = GameObject.FindWithTag("CombatPanel") as GameObject;
            if (null != combatPanel) CombatPadScript = combatPanel.GetComponent<CombatPad>();
            superBar = GameObject.FindWithTag("SuperBar") as GameObject;
            GameObject movementPad = GameObject.FindWithTag("MovementPad") as GameObject;
            if (null != movementPad) KeyboardScript = movementPad.GetComponent<Keyboard>();

            if (null == entity) { Debug.LogError("Attack.SetupAttack: entity is null."); }
            if (null == EntityControllerScript) { Debug.LogError("Attack.SetupAttack: EntityControllerScript is null."); }
            if (null == Anim) { Debug.LogWarning("Attack.SetupAttack: Anim is null."); }
            if (null == messageManagerScript) { Debug.LogError("Attack.SetupAttack: messageManagerScript is null."); }
            if (null == healthPanel) { Debug.LogError("Attack.SetupAttack: healthPanel is null."); }
            if (null == debugConsoleScript) { Debug.LogError("Attack.SetupAttack: debugConsoleScript is null."); }
            if (null == DynamicObjectManagerScript) { Debug.LogError("Attack.SetupAttack: DynamicObjectManagerScript is null."); }
            if (null == combatPanel) { Debug.LogError("Attack.SetupAttack: combatPanel is null."); }
            if (null == superBar) { Debug.LogError("Attack.SetupAttack: superBar is null."); }
            if (null == CombatPadScript) { Debug.LogError("Attack.SetupAttack: CombatPadScript is null."); }
            if (null == KeyboardScript) { Debug.LogError("Attack.SetupAttack: KeyboardScript is null."); }

            if (null == entity) { return false; }
            if (null == messageManagerScript) { return false; }
            if (null == healthPanel) { return false; }
            if (null == debugConsoleScript) { return false; }
            if (null == DynamicObjectManagerScript) { return false; }
            if (null == combatPanel) { return false; }
            if (null == superBar) { return false; }
            if (null == CombatPadScript) { return false; }
            if (null == KeyboardScript) { return false; }

            isSetup = true;
            return true;
        }
    }
}