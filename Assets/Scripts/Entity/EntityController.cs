﻿using System.Collections.Generic;
using UnityEngine;
using USComics;
using USComics_Combat;
using USComics_Dynamic;
using USComics_Movement;
using USComics_Pickups;

namespace USComics_Entity {
    public class EntityController : MonoBehaviour {
        public string Name;
        public GameObject Entity;
        public List<AbstractBuff> Buffs = new List<AbstractBuff>();
        public Health HealthScript;
        public Attack DeathAttack;
        public GameObject DeathSpawn;
        public bool Dead;
        public AttackBonus AttackBonus;
        public Attack[] Attacks;
        public Vision Vision = new Vision();
        public int CombatEmoteChance;
        public AudioClip[] CombatEmoteSounds;
        public AudioSource CombatEmoteSource;
        public EntityController CurrentEnemy { get; set; }

        // Player game objects
        protected Rigidbody EntityRigidBody;
        protected Vector3 InitialHelthPanelRotation;
        protected bool InitialUpdate = true;

        // State variables
        protected bool ClimbableInRange = false;

        // Scripts
        protected MovementTransitionManager MovementTransitionManagerScript;
        protected SimpleMovementModule SimpleMovementScript;
        protected ClimbMovementModule ClimbMovementScript;

        void Start() { Setup(); }
        void Update() { UpdateBuffs(); }
        protected virtual void OnTriggerEnter(Collider other) {
            GameObject go = GameObjectUtilities.GetGameObject(other);
            if (go.CompareTag("Pickup")) {
                AbstractPickup pickup = go.GetComponent<AbstractPickup>();
                if (null == pickup) return;
                pickup.ExecutePickup(this);
                Destroy(go);
            }
        }
        public virtual bool IsPlayer() { return false; }
        public void AddHealth(float amount) { if (null != HealthScript) HealthScript.AddHealth(amount); }
        public void AddLife() { if (null != HealthScript) HealthScript.AddLife(); }
        public void AddBuff(AbstractBuff buff) { Buffs.Add(buff); }
        public void RemoveBuff(AbstractBuff buff) { Buffs.Remove(buff); }
        public void RemoveExpiredBuffs() { for (int loop = Buffs.Count - 1; loop >= 0; loop--) { if (Buffs[loop].Expired) RemoveBuff(Buffs[loop]); }}
        public float GetMaxAttackRange() {
            float rangeRadius = 0.0f;
            for (int loop = 0; loop < Attacks.Length; loop++) {
                Attack attackInfo = Attacks[loop];
                if (attackInfo.AttackInfo.Range > rangeRadius) rangeRadius = attackInfo.AttackInfo.Range;
            }
            return rangeRadius;
        }
        public bool IsInRange(EntityController entity) {
            float range = GetMaxAttackRange();
            float distance = Direction.GetDistance(transform, entity.transform);
            return distance <= range;
        }
        public void Targetted(EntityController targettedBy) { }
        public void Attacked(EntityController attackedBy, Attack attack) {
            if (Dead) return;
            HealthScript.HealthPoints -= attack.AttackInfo.Damage.DamagePoints;
            if (0 >= HealthScript.HealthPoints) DoDeath(attackedBy);
        }
        public void ClearAttackTimers() {
            for (int loop = 0; loop < Attacks.Length; loop++) {
                Attack attackInfo = Attacks[loop];
                attackInfo.AttackInfo.LastUsed = 0;
            }
        }
        public void UpdateBuffs() {
            RemoveExpiredBuffs();
            foreach (AbstractBuff buff in Buffs) {  buff.Buff(this); }
        }
        public void DoDeath(EntityController whoKilledMe) {
            Dead = true;
            HealthScript.HealthPoints = 0;
            HealthScript.HideHealth();
            if (null != DeathAttack) {
                DeathAttack.Attacker = Entity;
                DeathAttack.DoAttack(whoKilledMe);
            }
            if (null != DeathSpawn) DynamicObjectManager.INSTANCE.Clone(DeathSpawn, DeathSpawn.transform.position, 0.0f, 0.0f, 0.0f);
            if (Entity == whoKilledMe.CurrentEnemy) whoKilledMe.CurrentEnemy = null;
        }
#if UNITY_EDITOR
        public void EditorGizmo(Transform transform) {
            Color c = new Color(0, 0, 0.7f, 0.4f);
            Color c2 = new Color(1.0f, 0, 0f, 0.4f);

            UnityEditor.Handles.color = c;
            Vector3 rotatedForward = Quaternion.Euler(0, -Vision.DetectionAngle * 0.5f, 0) * transform.forward;
            UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, rotatedForward, Vision.DetectionAngle, Vision.DetectionRadius);

            UnityEditor.Handles.color = c2;
            rotatedForward = Quaternion.Euler(0, -Vision.DetectionAngle * 0.5f, 0) * transform.forward;
            UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, rotatedForward, Vision.DetectionAngle, GetMaxAttackRange());

            Gizmos.color = new Color(1.0f, 1.0f, 0.0f, 1.0f);
            Gizmos.DrawWireSphere(transform.position + Vector3.up * Vision.HeightOffset, 0.2f);
        }
#endif
        
        protected virtual bool Setup() {            
            if (null != Entity) EntityRigidBody = Entity.GetComponent<Rigidbody>();
            if (null != Entity) HealthScript = Entity.GetComponent<Health>();

            if (null == EntityRigidBody) { Debug.LogError("EntityController.Setup: " + Name + ": EntityRigidBody is null."); }

            if (null == EntityRigidBody) { return false; }

            if (null != HealthScript) InitialHelthPanelRotation = HealthScript.HealthPanel.transform.eulerAngles;
            CurrentEnemy = null;
            Dead = false;
            return true;
        }
    }
}



