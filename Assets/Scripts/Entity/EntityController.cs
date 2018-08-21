using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USComics_Debug;
using USComics_Combat;
using USComics_Movement;
using USComics_Environment;
using USComics_Message_Manager;

namespace USComics_Entity
{
    public class EntityController : MonoBehaviour
    {
        public GameObject entity;
        public string entityName = "";
        public GameObject healthPanel;
        public List<AbstractBuff> buffs = new List<AbstractBuff>();
        public List<AbstractDebuff> debuffs = new List<AbstractDebuff>();
        public Attack death;
        public bool dead;
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
        protected Health healthScript;
        protected SimpleMovementModule simpleMovementScript;
        protected ClimbMovementModule climbMovementScript;
        protected MessageManager messageManagerScript;
        protected DebugConsole debugConsoleScript;

        // Use this for initialization
        void Start()
        {
            SetupEntity();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public virtual bool IsPlayer() { return false; }

        public void ShowHealth()
        {
            Renderer[] childComponents = healthPanel.GetComponentsInChildren<Renderer>();
            for (int loop = 0; loop < childComponents.Length; loop++)
            {
                childComponents[loop].enabled = true;
            }
        }

        public void HideHealth()
        {
            Renderer[] childComponents = healthPanel.GetComponentsInChildren<Renderer>();
            for (int loop = 0; loop < childComponents.Length; loop++)
            {
                childComponents[loop].enabled = false;
            }
        }

        public float GetMaxAttackRange()
        {
            float rangeRadius = 0.0f;
            for (int loop = 0; loop < attacks.Length; loop++)
            {
                Attack attackInfo = attacks[loop];
                if (attackInfo.range > rangeRadius) rangeRadius = attackInfo.range;
            }
            return rangeRadius;
        }

        public Collider[] GetEnemiesInSight(bool useHeightDifference = true)
        {
            Collider[] enemies = Environment.GetEnemiesInSight(entity.transform, vision.detectionRadius, vision.detectionAngle, vision.heightOffset, vision.maxHeightDifference, useHeightDifference);
            return enemies;
        }

        public Collider[] GetEnemiesInRange(bool useHeightDifference = true)
        {
            Debug.Log("entity.transform=" + entity.transform.ToString());
            Debug.Log("GetMaxAttackRange()=" + GetMaxAttackRange());
            Debug.Log("vision.detectionAngle=" + vision.detectionAngle);
            Debug.Log("vision.heightOffset=" + vision.heightOffset);
            Debug.Log("vision.maxHeightDifference=" + vision.maxHeightDifference);
            Collider[] enemies = Environment.GetEnemiesInRange(entity.transform, GetMaxAttackRange(), vision.detectionAngle, vision.heightOffset, vision.maxHeightDifference, useHeightDifference);
            Debug.Log("enemies.Length=" + enemies.Length);
            return enemies;
        }
        public GameObject NearestInRange(bool useHeightDifference = true)
        {
            Collider[] enemies = GetEnemiesInRange(useHeightDifference);
            if (0 == enemies.Length) return null;
            GameObject[] enemiesGO = DirectionUtilities.GetGameObjects(enemies);
            return DirectionUtilities.GetNearestObject(transform.position, enemiesGO);
        }

        public void Targetted(EntityController targettedBy)
        {
        }
        public void Attacked(EntityController attackedBy, Attack attack)
        {
            if (dead) return;
            healthScript.health -= attack.damage;
            if (0 >= healthScript.health) DoDeath(attackedBy);
        }

        public void ClearAttackTimers()
        {
            for (int loop = 0; loop < attacks.Length; loop++)
            {
                Attack attackInfo = attacks[loop];
                attackInfo.lastUsed = 0;
            }
        }

        public void DoDeath(EntityController whoKilledMe)
        {
            dead = true;
            healthScript.health = 0;
            HideHealth();
            if (null == death) return;
            death.DoAttack(whoKilledMe);
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
            messageManagerScript = GetComponent<MessageManager>();
            GameObject debugConsole = GameObject.FindWithTag("DebugConsole") as GameObject;
            if (null != debugConsole) debugConsoleScript = debugConsole.GetComponent<DebugConsole>();
            if (null != entity) healthScript = entity.GetComponent<Health>();

            if (null == entityRigidBody) { Debug.LogError("EntityController.SetupEntity: entityRigidBody is null."); }
            if (null == messageManagerScript) { Debug.LogError("EntityController.SetupEntity: messageManager is null."); }
            if (null == debugConsoleScript) { Debug.LogError("EntityController.SetupEntity: debugConsoleScript is null."); }
            if (null == healthScript) { Debug.LogError("EntityController.SetupEntity: healthScript is null."); }

            if (null == entityRigidBody) { return false; }
            if (null == messageManagerScript) { return false; }
            if (null == debugConsoleScript) { return false; }
            if (null == healthScript) { return false; }

            initialHelthPanelRotation = healthPanel.transform.eulerAngles;
            CurrentEnemy = null;
            dead = false;
            messageManagerScript.ShowMessage("Hello World!");
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



