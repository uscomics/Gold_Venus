using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using USComics_Debug;
using USComics_Combat;
using USComics_Movement;
using USComics_Environment;
using USComics_Message_Manager;

namespace USComics_Player
{
    public class PlayerController : MonoBehaviour
    {
        public TargetScanner targetScanner;

        // Player game objects
        private GameObject playerCharacter;
        private Rigidbody playerCharacterRigidbody;
        private GameObject healthPanel;
        private Vector3 initialHelthPanelRotation;
        private bool initialUpdate = true;

        // State variables
        private bool climbableInRange = false;

        // Scripts
        private MovementTransitionManager movementTransitionManagerScript;
        private SimpleMovementModule simpleMovementScript;
        private CombatModule combatModuleScript;
        private ClimbMovementModule climbMovementScript;
        private MessageManager messageManagerScript;
        private DebugConsole debugConsoleScript;

        void Start()
        {
            playerCharacter = GameObject.FindWithTag("PlayerCharacter") as GameObject;
            if (null != playerCharacter) playerCharacterRigidbody = playerCharacter.GetComponent<Rigidbody>();
            if (null != playerCharacter) movementTransitionManagerScript = playerCharacter.GetComponent<MovementTransitionManager>();
            if (null != playerCharacter) simpleMovementScript = playerCharacter.GetComponent<SimpleMovementModule>();
            if (null != playerCharacter) combatModuleScript = playerCharacter.GetComponent<CombatModule>();
            if (null != playerCharacter) climbMovementScript = playerCharacter.GetComponent<ClimbMovementModule>();
            healthPanel = GameObject.FindWithTag("HealthGameObject") as GameObject;
            messageManagerScript = GetComponent<MessageManager>();
            GameObject debugConsole = GameObject.FindWithTag("DebugConsole") as GameObject;
            if (null != debugConsole) debugConsoleScript = debugConsole.GetComponent<DebugConsole>();

            if (null == playerCharacter) { Debug.LogError("PlayerController.Start: playerCharacter is null."); }
            if (null == playerCharacterRigidbody) { Debug.LogError("PlayerController.Start: v is null."); }
            if (null == movementTransitionManagerScript) { Debug.LogError("PlayerController.Start: MovementTransitionManagerScript is null."); }
            if (null == simpleMovementScript) { Debug.LogError("PlayerController.Start: movementManagerScript is null."); }
            if (null == combatModuleScript) { Debug.LogError("PlayerController.Start: combatModuleScript is null."); }
            if (null == climbMovementScript) { Debug.LogError("PlayerController.Start: climbManagerScript is null."); }
            if (null == healthPanel) { Debug.LogError("PlayerController.Start: healthPanel is null."); }
            if (null == messageManagerScript) { Debug.LogError("PlayerController.Start: messageManager is null."); }
            if (null == debugConsoleScript) { Debug.LogError("PlayerController.Start: debugConsoleScript is null."); }

            if (null == playerCharacter) { return; }
            if (null == playerCharacterRigidbody) { return; }
            if (null == movementTransitionManagerScript) { return; }
            if (null == simpleMovementScript) { return; }
            if (null == combatModuleScript) { return; }
            if (null == climbMovementScript) { return; }
            if (null == healthPanel) { return; }
            if (null == messageManagerScript) { return; }
            if (null == debugConsoleScript) { return; }

            initialHelthPanelRotation = healthPanel.transform.eulerAngles;
            messageManagerScript.ShowMessage("Hello World!");
        }

        void Update()
        {
            UpdateCombat();
            if (initialUpdate)
            {
                simpleMovementScript.StartModule();
                initialUpdate = false;
            }
            if (simpleMovementScript.IsRunning())
            {
                AttackType currentAttack = combatModuleScript.CurrentAttack;

                debugConsoleScript.SetOther1("currentAttack=" + currentAttack);
                if (AttackType.None == currentAttack)
                {
                    Move currentMove = simpleMovementScript.CurrentMove;
                    Vector3 currentVector = simpleMovementScript.CurrentVector;
                    if (Vector3.zero != currentVector) playerCharacter.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(currentVector), 0.15F);
                    playerCharacter.transform.Translate(currentVector * currentMove.Speed * Time.deltaTime, Space.World);
                    if (DirectionType.Stop != currentMove.Direction) SetHealthPosition(currentMove.Direction);
                    debugConsoleScript.SetCurrentMove(currentMove);
                    debugConsoleScript.SetOther1("currentVector=" + currentVector);
                }

            }
            else if (climbMovementScript.IsRunning())
            {
                ClimbMove currentMove = climbMovementScript.CurrentMove;
                Vector3 currentVector = climbMovementScript.CurrentVector;
                playerCharacter.transform.Translate(currentVector * ClimbSpeed.GetSpeed(currentMove.Climb) * Time.deltaTime, Space.World);
                debugConsoleScript.SetCurrentMove(currentMove);
                debugConsoleScript.SetOther1("currentVector=" + currentVector);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            Move currentMove = simpleMovementScript.CurrentMove;
            if ("Climbable" == collision.gameObject.tag) climbableInRange = true;
            if (climbableInRange)
            {
                if (!climbMovementScript.IsRunning())
                {
                    Collider[] climbables = Environment.GetClimbables(playerCharacter.transform);
                    if (0 != climbables.Length)
                    {
                        movementTransitionManagerScript.StartTransitionFrom(ModuleTypes.Simple, ModuleTypes.Climbing);
                    }
                }
                else
                {
                    climbMovementScript.ForceStop();
                    movementTransitionManagerScript.StartTransitionFrom(ModuleTypes.Climbing, ModuleTypes.Simple);

                }
            } else
            {
                if ((int)LayerValues.TERRAIN != collision.gameObject.layer) simpleMovementScript.ForceStop();
            }
        }
        private void OnCollisionStay(Collision collision)
        {
            // Debug.Log("STILL BANG! tag = " + collision.gameObject.tag);
        }
        private void OnCollisionExit(Collision collision)
        {
            if ("Climbable" == collision.gameObject.tag) climbableInRange = false;
            // Debug.Log("BANG DONE! tag = " + collision.gameObject.tag);
        }
        private bool PlayerCanClimb()
        {
            Collider[] colliders = Environment.GetClimbables(playerCharacter.transform);
            return (0 < colliders.Length);
        }

        private void SetHealthPosition(DirectionType inDirection)
        {
            healthPanel.transform.localRotation = Quaternion.identity;
            if (DirectionType.South == inDirection) { healthPanel.transform.Rotate(Vector3.up, 180); }
            else if (DirectionType.East == inDirection) { healthPanel.transform.Rotate(Vector3.up, 270); }
            else if (DirectionType.West == inDirection) { healthPanel.transform.Rotate(Vector3.up, 90); }
            else if (DirectionType.NW == inDirection) { healthPanel.transform.Rotate(Vector3.up, 45); }
            else if (DirectionType.NE == inDirection) { healthPanel.transform.Rotate(Vector3.up, 315); }
            else if (DirectionType.SW == inDirection) { healthPanel.transform.Rotate(Vector3.up, 135); }
            else if (DirectionType.SE == inDirection) { healthPanel.transform.Rotate(Vector3.up, 225); }
        }

        private void UpdateCombat()
        {
            Collider[] enemies = targetScanner.GetEnemiesInSight(playerCharacter.transform);
            if (0 == enemies.Length)
            {
                combatModuleScript.StopModule();
                return;
            }
            combatModuleScript.StartModule();
            enemies = targetScanner.GetEnemiesInRange(playerCharacter.transform);
            combatModuleScript.SetEnemyInRange(0 == enemies.Length);
    }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            targetScanner.EditorGizmo(transform);
        }
#endif
    }
}
