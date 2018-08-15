using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using USComics_Movement;
using USComics_Environment;
using USComics_Message_Manager;
using USComics_Debug;

public class PlayerController : MonoBehaviour {
    // Player game objects
    private GameObject playerCharacter;
    private Rigidbody playerCharacterRigidbody;
    private GameObject healthPanel;
    private Vector3 initialHelthPanelRotation;

    // State variables
    private bool climbableInRange = false;
    private ModuleTypes mode;

    // Scripts
    private MovementTransitionManager movementTransitionManagerScript;
    private SimpleMovementModule simpleMovementScript;
    private ClimbMovementModule climbMovementScript;
    private MessageManager messageManagerScript;
    private DebugConsole debugConsoleScript;

    void Start () {
        playerCharacter = GameObject.FindWithTag("PlayerCharacter") as GameObject;
        if (null != playerCharacter) playerCharacterRigidbody = playerCharacter.GetComponent<Rigidbody>();
        if (null != playerCharacter) movementTransitionManagerScript = playerCharacter.GetComponent<MovementTransitionManager>();
        if (null != playerCharacter) simpleMovementScript = playerCharacter.GetComponent<SimpleMovementModule>();
        if (null != playerCharacter) climbMovementScript = playerCharacter.GetComponent<ClimbMovementModule>();
        healthPanel = GameObject.FindWithTag("HealthGameObject") as GameObject;
        messageManagerScript = GetComponent<MessageManager>();
        GameObject debugConsole = GameObject.FindWithTag("DebugConsole") as GameObject;
        if (null != debugConsole) debugConsoleScript = debugConsole.GetComponent<DebugConsole>();

        if (null == playerCharacter) { Debug.LogError("PlayerController.Start: playerCharacter is null."); }
        if (null == playerCharacterRigidbody) { Debug.LogError("PlayerController.Start: v is null."); }
        if (null == movementTransitionManagerScript) { Debug.LogError("PlayerController.Start: MovementTransitionManagerScript is null."); }
        if (null == simpleMovementScript) { Debug.LogError("PlayerController.Start: movementManagerScript is null."); }
        if (null == climbMovementScript) { Debug.LogError("PlayerController.Start: climbManagerScript is null."); }
        if (null == healthPanel) { Debug.LogError("PlayerController.Start: healthPanel is null."); }
        if (null == messageManagerScript) { Debug.LogError("PlayerController.Start: messageManager is null."); }
        if (null == debugConsoleScript) { Debug.LogError("PlayerController.Start: debugConsoleScript is null."); }

        if (null == playerCharacter) { return; }
        if (null == playerCharacterRigidbody) { return; }
        if (null == movementTransitionManagerScript) { return; }
        if (null == simpleMovementScript) { return; }
        if (null == climbMovementScript) { return; }
        if (null == healthPanel) { return; }
        if (null == messageManagerScript) { return; }
        if (null == debugConsoleScript) { return; }

        initialHelthPanelRotation = healthPanel.transform.eulerAngles;
        mode = ModuleTypes.Simple;
    }

    void Update () {
        if (ModuleTypes.Simple == mode)
        {
            if (!simpleMovementScript.IsRunning())
            {
                simpleMovementScript.StartModule();
                return;
            }
            Move currentMove = simpleMovementScript.CurrentMove;
            Vector3 currentVector = simpleMovementScript.CurrentVector;
            if (Vector3.zero != currentVector) playerCharacter.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(currentVector), 0.15F);
            playerCharacter.transform.Translate(currentVector * currentMove.Speed * Time.deltaTime, Space.World);
            SetHealthPosition(currentMove.Direction);
        }
        else if (ModuleTypes.Climbing == mode)
        {
            if (!climbMovementScript.IsRunning())
            {
                simpleMovementScript.StartModule();
                mode = ModuleTypes.Simple;
                return;
            }
        }
    }

    private void OnCollisionEnter(Collision collision) {
        Move currentMove = simpleMovementScript.CurrentMove;
        if ((int)LayerValues.TERRAIN != collision.gameObject.layer) simpleMovementScript.ForceStop();
        if ("Climbable" == collision.gameObject.tag) climbableInRange = true;
        if (climbableInRange)
        {
            Collider[] climbables = Environment.GetClimbables(playerCharacter.transform);
            if (0 != climbables.Length)
            {
                movementTransitionManagerScript.Transition = new MovementModulesTransition(ModuleTypes.Simple, ModuleTypes.Climbing );
                movementTransitionManagerScript.TransitionFromStarted();
                //climbManagerScript.StartModule();
                //movementManagerScript.StopModule();
                //mode = ModuleType.Climbing;
            }
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        // Debug.Log("STILL BANG! tag = " + collision.gameObject.tag);
    }
    private void OnCollisionExit(Collision collision)
    {
        if ("Climbable" == collision.gameObject.tag) climbableInRange = false;
        debugConsoleScript.SetOther1("climbableInRange: " + climbableInRange);
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
}
