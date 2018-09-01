using System.Collections.Generic;
using UnityEngine;
using USComics;
using USComics_Debug;
using USComics_Combat;
using USComics_Dynamic;
using USComics_Movement;
using USComics_Environment;
using USComics_Message_Manager;
using USComics_Pickups;

namespace USComics_Entity {
    public class EntityController : MonoBehaviour {
        public string Name;
        public GameObject Entity;
        public GameObject HealthPanel;
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
        protected MessageManager MessageManagerScript;
        protected DebugConsole DebugConsoleScript;
        protected DynamicObjectManager DynamicObjectManagerScript;

        void Start() { Setup(); }
        void Update() { UpdateBuffs(); }
        private void OnTriggerEnter(Collider other) {
            GameObject go = GameObjectUtilities.GetGameObject(other);
            if (go.CompareTag("Pickup")) {
                AbstractPickup pickup = go.GetComponent<AbstractPickup>();
                if (null == pickup) return;
                pickup.ExecutePickup(this);
                Destroy(go);
            }
        }
        public virtual bool IsPlayer() { return false; }
        public void ShowHealth() {
            Renderer[] childComponents = HealthPanel.GetComponentsInChildren<Renderer>();
            for (int loop = 0; loop < childComponents.Length; loop++) { childComponents[loop].enabled = true; }
        }
        public void HideHealth() {
            Renderer[] childComponents = HealthPanel.GetComponentsInChildren<Renderer>();
            for (int loop = 0; loop < childComponents.Length; loop++) { childComponents[loop].enabled = false; }
        }
        public void AddHealth(float amount) {
            HealthScript.AddHealth(amount);
        }
        public void AddLife() {
            HealthScript.AddLife();
        }
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
        public Collider[] GetEnemiesInSight(bool useHeightDifference = true) {
            Collider[] enemies = Environment.GetEnemiesInSight(Entity.transform, Vision.DetectionRadius, Vision.DetectionAngle, Vision.HeightOffset, Vision.MaxHeightDifference, useHeightDifference);
            return enemies;
        }

        public Collider[] GetEnemiesInRange(bool useHeightDifference = true) {
            Collider[] enemies = Environment.GetEnemiesInFront(Entity.transform, GetMaxAttackRange(), Vision.DetectionAngle, Vision.HeightOffset, Vision.MaxHeightDifference, useHeightDifference);
            return enemies;
        }
        public GameObject NearestInRange(bool useHeightDifference = true) {
            Collider[] enemies = GetEnemiesInRange(useHeightDifference);
            if (0 == enemies.Length) return null;
            GameObject[] enemiesGO = GameObjectUtilities.GetGameObjects(enemies);
            return Environment.GetNearestObject(transform.position, enemiesGO);
        }

        public void Targetted(EntityController targettedBy) { }
        public void Attacked(EntityController attackedBy, Attack attack) {
            if (Dead) return;
            HealthScript.health -= attack.AttackInfo.Damage.DamagePoints;
            if (0 >= HealthScript.health) DoDeath(attackedBy);
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
            HealthScript.health = 0;
            HideHealth();
            if (null != DeathAttack) {
                DeathAttack.Attacker = Entity;
                DeathAttack.DoAttack(whoKilledMe);
            }
            if (null != DeathSpawn) DynamicObjectManagerScript.Clone(DeathSpawn, DeathSpawn.transform.position, 0.0f, 0.0f, 0.0f);
            if (Entity == whoKilledMe.CurrentEnemy) whoKilledMe.CurrentEnemy = null;
        }

#if UNITY_EDITOR

        public void EditorGizmo(Transform transform)
        {
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

        protected virtual bool Setup()
        {            
            if (null != Entity) EntityRigidBody = Entity.GetComponent<Rigidbody>();
            GameObject messageCanvas = GameObject.FindWithTag("MessageCanvas") as GameObject;
            if (null != messageCanvas) MessageManagerScript = messageCanvas.GetComponent<MessageManager>();
            GameObject debugConsole = GameObject.FindWithTag("DebugConsole") as GameObject;
            if (null != debugConsole) DebugConsoleScript = debugConsole.GetComponent<DebugConsole>();
            if (null != Entity) HealthScript = Entity.GetComponent<Health>();
            GameObject dynamicObjects = GameObject.FindWithTag("DynamicObjects") as GameObject;
            if (null != dynamicObjects) DynamicObjectManagerScript = dynamicObjects.GetComponent<DynamicObjectManager>();

            if (null == EntityRigidBody) { Debug.LogError("EntityController.SetupEntity: " + Name + ": entityRigidBody is null."); }
            if (null == MessageManagerScript) { Debug.LogError("EntityController.SetupEntity: " + Name + ": messageManager is null."); }
            if (null == DebugConsoleScript) { Debug.LogError("EntityController.SetupEntity: " + Name + ": debugConsoleScript is null."); }
            if (null == HealthScript) { Debug.LogError("EntityController.SetupEntity: " + Name + ": healthScript is null."); }
            if (null == DynamicObjectManagerScript) { Debug.LogError("EntityController.SetupEntity: " + Name + ": DynamicObjectManagerScript is null."); }

            if (null == EntityRigidBody) { return false; }
            if (null == MessageManagerScript) { return false; }
            if (null == DebugConsoleScript) { return false; }
            if (null == HealthScript) { return false; }
            if (null == DynamicObjectManagerScript) { return false; }

            InitialHelthPanelRotation = HealthPanel.transform.eulerAngles;
            CurrentEnemy = null;
            Dead = false;
            return true;
        }
#endif
    }
}



