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
        public List<AbstractBuff> buffs = new List<AbstractBuff>();
        public List<AbstractDebuff> debuffs = new List<AbstractDebuff>();
        public AttackInfo[] attacks;
        public Vision vision = new Vision();

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
        protected CombatModule combatModuleScript;
        protected ClimbMovementModule climbMovementScript;
        protected MessageManager messageManagerScript;
        protected DebugConsole debugConsoleScript;

        // Use this for initialization
        void Start()
        {
            entity = GameObject.FindWithTag("PlayerCharacter") as GameObject;
            if (null != entity) entityRigidBody = entity.GetComponent<Rigidbody>();
            if (null != entity) movementTransitionManagerScript = entity.GetComponent<MovementTransitionManager>();
            if (null != entity) simpleMovementScript = entity.GetComponent<SimpleMovementModule>();
            if (null != entity) combatModuleScript = entity.GetComponent<CombatModule>();
            if (null != entity) climbMovementScript = entity.GetComponent<ClimbMovementModule>();
            healthPanel = GameObject.FindWithTag("HealthGameObject") as GameObject;
            messageManagerScript = GetComponent<MessageManager>();
            GameObject debugConsole = GameObject.FindWithTag("DebugConsole") as GameObject;
            if (null != debugConsole) debugConsoleScript = debugConsole.GetComponent<DebugConsole>();

            if (null == entity) { Debug.LogError("PlayerController.Start: playerCharacter is null."); }
            if (null == entityRigidBody) { Debug.LogError("PlayerController.Start: v is null."); }
            if (null == movementTransitionManagerScript) { Debug.LogError("PlayerController.Start: MovementTransitionManagerScript is null."); }
            if (null == simpleMovementScript) { Debug.LogError("PlayerController.Start: movementManagerScript is null."); }
            if (null == combatModuleScript) { Debug.LogError("PlayerController.Start: combatModuleScript is null."); }
            if (null == climbMovementScript) { Debug.LogError("PlayerController.Start: climbManagerScript is null."); }
            if (null == healthPanel) { Debug.LogError("PlayerController.Start: healthPanel is null."); }
            if (null == messageManagerScript) { Debug.LogError("PlayerController.Start: messageManager is null."); }
            if (null == debugConsoleScript) { Debug.LogError("PlayerController.Start: debugConsoleScript is null."); }

            if (null == entity) { return; }
            if (null == entityRigidBody) { return; }
            if (null == movementTransitionManagerScript) { return; }
            if (null == simpleMovementScript) { return; }
            if (null == combatModuleScript) { return; }
            if (null == climbMovementScript) { return; }
            if (null == healthPanel) { return; }
            if (null == messageManagerScript) { return; }
            if (null == debugConsoleScript) { return; }

            initialHelthPanelRotation = healthPanel.transform.eulerAngles;
            CurrentEnemy = null;
            CurrentEnemyInRange = false;
            messageManagerScript.ShowMessage("Hello World!");

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
                AttackInfo attackInfo = attacks[loop];
                if (attackInfo.range > rangeRadius) rangeRadius = attackInfo.range;
            }
            return rangeRadius;
        }

        public Collider[] GetEnemiesInSight(Transform player, bool useHeightDifference = true)
        {
            Collider[] enemies = Environment.GetEnemiesInSight(player, vision.detectionRadius, vision.detectionAngle, vision.heightOffset, vision.maxHeightDifference, useHeightDifference);
            return enemies;
        }

        public Collider[] GetEnemiesInRange(Transform player, bool useHeightDifference = true)
        {
            Collider[] enemies = Environment.GetEnemiesInRange(player, vision.rangeRadius, vision.detectionAngle, vision.heightOffset, vision.maxHeightDifference, useHeightDifference);
            return enemies;
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

#endif
    }

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



