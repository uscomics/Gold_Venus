using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USComics_Debug;
using USComics_Combat;
using USComics_Movement;
using USComics_Environment;
using USComics_Message_Manager;

namespace USComics
{
    public class EntityController : MonoBehaviour
    {
        public string entityName = "";
        public List<AbstractBuff> buffs = new List<AbstractBuff>();
        public List<AbstractDebuff> debuffs = new List<AbstractDebuff>();
        public Attack[] attacks;
        public Vision vision = new Vision();
        public int CombatEmoteChance;
        public AudioClip[] CombatEmoteSounds;
        public AudioSource CombatEmoteSource;

        // Player game objects
        protected GameObject entity;
        protected Rigidbody entityRigidBody;
        protected GameObject healthPanel;
        protected Vector3 initialHelthPanelRotation;
        protected bool initialUpdate = true;
        protected GameObject CurrentEnemy { get; set; }
        protected bool CurrentEnemyInRange { get; set; }

        // State variables
        protected bool climbableInRange = false;

        // Scripts
        protected MovementTransitionManager movementTransitionManagerScript;
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
            Collider[] enemies = Environment.GetEnemiesInRange(entity.transform, vision.rangeRadius, vision.detectionAngle, vision.heightOffset, vision.maxHeightDifference, useHeightDifference);
            return enemies;
        }

        public void ClearAttackTimers()
        {
            for (int loop = 0; loop < attacks.Length; loop++)
            {
                Attack attackInfo = attacks[loop];
                attackInfo.lastUsed = 0;
            }
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
            healthPanel = GameObject.FindWithTag("HealthGameObject") as GameObject;
            messageManagerScript = GetComponent<MessageManager>();
            GameObject debugConsole = GameObject.FindWithTag("DebugConsole") as GameObject;
            if (null != debugConsole) debugConsoleScript = debugConsole.GetComponent<DebugConsole>();

            if (null == healthPanel) { Debug.LogError("EntityController.SetupEntity: healthPanel is null."); }
            if (null == messageManagerScript) { Debug.LogError("EntityController.SetupEntity: messageManager is null."); }
            if (null == debugConsoleScript) { Debug.LogError("EntityController.SetupEntity: debugConsoleScript is null."); }

            if (null == healthPanel) { return false; }
            if (null == messageManagerScript) { return false; }
            if (null == debugConsoleScript) { return false; }

            initialHelthPanelRotation = healthPanel.transform.eulerAngles;
            CurrentEnemy = null;
            CurrentEnemyInRange = false;
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
        public float rangeRadius = 1.5f;
        [Range(0.0f, 360.0f)]
        public float detectionAngle = 270;
        public float maxHeightDifference = 4.0f;
    }
}



