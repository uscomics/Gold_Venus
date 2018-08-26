using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        public GameObject entity;
        public GameObject healthPanel;
        public List<AbstractBuff> buffs = new List<AbstractBuff>();
        public Health healthScript;
        public Attack death;
        public GameObject deathSpawn;
        public bool dead;
        public AttackBonus AttackBonus;
        public Attack[] attacks;
        public Vision vision = new Vision();
        public int CombatEmoteChance;
        public AudioClip[] CombatEmoteSounds;
        public AudioSource CombatEmoteSource;
        public EntityController CurrentEnemy { get; set; }

        // Player game objects
        protected Rigidbody entityRigidBody;
        protected Vector3 initialHelthPanelRotation;
        protected bool initialUpdate = true;

        // State variables
        protected bool climbableInRange = false;

        // Scripts
        protected MovementTransitionManager movementTransitionManagerScript;
        protected SimpleMovementModule simpleMovementScript;
        protected ClimbMovementModule climbMovementScript;
        protected MessageManager messageManagerScript;
        protected DebugConsole debugConsoleScript;
        protected DynamicObjectManager DynamicObjectManagerScript;

        void Start() { SetupEntity(); }
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
            Renderer[] childComponents = healthPanel.GetComponentsInChildren<Renderer>();
            for (int loop = 0; loop < childComponents.Length; loop++) { childComponents[loop].enabled = true; }
        }
        public void HideHealth() {
            Renderer[] childComponents = healthPanel.GetComponentsInChildren<Renderer>();
            for (int loop = 0; loop < childComponents.Length; loop++) { childComponents[loop].enabled = false; }
        }
        public void AddHealth(float amount) {
            healthScript.AddHealth(amount);
        }
        public void AddLife() {
            healthScript.AddLife();
        }
        public void AddBuff(AbstractBuff buff) { buffs.Add(buff); }
        public void RemoveBuff(AbstractBuff buff) { buffs.Remove(buff); }
        public void RemoveExpiredBuffs() { for (int loop = buffs.Count - 1; loop >= 0; loop--) { if (buffs[loop].Expired) RemoveBuff(buffs[loop]); }}
        public float GetMaxAttackRange() {
            float rangeRadius = 0.0f;
            for (int loop = 0; loop < attacks.Length; loop++) {
                Attack attackInfo = attacks[loop];
                if (attackInfo.AttackInfo.Range > rangeRadius) rangeRadius = attackInfo.AttackInfo.Range;
            }
            return rangeRadius;
        }
        public Collider[] GetEnemiesInSight(bool useHeightDifference = true) {
            Collider[] enemies = Environment.GetEnemiesInSight(entity.transform, vision.detectionRadius, vision.detectionAngle, vision.heightOffset, vision.maxHeightDifference, useHeightDifference);
            return enemies;
        }

        public Collider[] GetEnemiesInRange(bool useHeightDifference = true) {
            Collider[] enemies = Environment.GetEnemiesInFront(entity.transform, GetMaxAttackRange(), vision.detectionAngle, vision.heightOffset, vision.maxHeightDifference, useHeightDifference);
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
            if (dead) return;
            healthScript.health -= attack.AttackInfo.Damage.DamagePoints;
            if (0 >= healthScript.health) DoDeath(attackedBy);
        }

        public void ClearAttackTimers() {
            for (int loop = 0; loop < attacks.Length; loop++) {
                Attack attackInfo = attacks[loop];
                attackInfo.AttackInfo.LastUsed = 0;
            }
        }

        public void UpdateBuffs() {
            RemoveExpiredBuffs();
            foreach (AbstractBuff buff in buffs) {  buff.Buff(this); }
        }
        public void DoDeath(EntityController whoKilledMe) {
            dead = true;
            healthScript.health = 0;
            HideHealth();
            if (null != death) {
                death.entity = entity;
                death.DoAttack(whoKilledMe);
            }
            if (null != deathSpawn) DynamicObjectManagerScript.Clone(deathSpawn, deathSpawn.transform.position, 0.0f, 0.0f, 0.0f);
            if (entity == whoKilledMe.CurrentEnemy) whoKilledMe.CurrentEnemy = null;
        }

#if UNITY_EDITOR

        public void EditorGizmo(Transform transform)
        {
            Color c = new Color(0, 0, 0.7f, 0.4f);
            Color c2 = new Color(1.0f, 0, 0f, 0.4f);

            UnityEditor.Handles.color = c;
            Vector3 rotatedForward = Quaternion.Euler(0, -vision.detectionAngle * 0.5f, 0) * transform.forward;
            UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, rotatedForward, vision.detectionAngle, vision.detectionRadius);

            UnityEditor.Handles.color = c2;
            rotatedForward = Quaternion.Euler(0, -vision.detectionAngle * 0.5f, 0) * transform.forward;
            UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, rotatedForward, vision.detectionAngle, GetMaxAttackRange());

            Gizmos.color = new Color(1.0f, 1.0f, 0.0f, 1.0f);
            Gizmos.DrawWireSphere(transform.position + Vector3.up * vision.heightOffset, 0.2f);
        }

        protected virtual bool SetupEntity()
        {            
            if (null != entity) entityRigidBody = entity.GetComponent<Rigidbody>();
            GameObject messageCanvas = GameObject.FindWithTag("MessageCanvas") as GameObject;
            if (null != messageCanvas) messageManagerScript = messageCanvas.GetComponent<MessageManager>();
            GameObject debugConsole = GameObject.FindWithTag("DebugConsole") as GameObject;
            if (null != debugConsole) debugConsoleScript = debugConsole.GetComponent<DebugConsole>();
            if (null != entity) healthScript = entity.GetComponent<Health>();
            GameObject dynamicObjects = GameObject.FindWithTag("DynamicObjects") as GameObject;
            if (null != dynamicObjects) DynamicObjectManagerScript = dynamicObjects.GetComponent<DynamicObjectManager>();

            if (null == entityRigidBody) { Debug.LogError("EntityController.SetupEntity: " + Name + ": entityRigidBody is null."); }
            if (null == messageManagerScript) { Debug.LogError("EntityController.SetupEntity: " + Name + ": messageManager is null."); }
            if (null == debugConsoleScript) { Debug.LogError("EntityController.SetupEntity: " + Name + ": debugConsoleScript is null."); }
            if (null == healthScript) { Debug.LogError("EntityController.SetupEntity: " + Name + ": healthScript is null."); }
            if (null == DynamicObjectManagerScript) { Debug.LogError("EntityController.SetupEntity: " + Name + ": DynamicObjectManagerScript is null."); }

            if (null == entityRigidBody) { return false; }
            if (null == messageManagerScript) { return false; }
            if (null == debugConsoleScript) { return false; }
            if (null == healthScript) { return false; }
            if (null == DynamicObjectManagerScript) { return false; }

            initialHelthPanelRotation = healthPanel.transform.eulerAngles;
            CurrentEnemy = null;
            dead = false;
            return true;
        }
#endif
    }

    [System.Serializable]
    public class Vision
    {
        public float heightOffset = 0.0f;
        public float detectionRadius = 10;
        [Range(0.0f, 360.0f)]
        public float detectionAngle = 270;
        public float maxHeightDifference = 4.0f;
    }
}



