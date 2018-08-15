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

    // Scripts
    private MovementManager movementManagerScript;
    private MessageManager messageManagerScript;
    private DebugConsole debugConsoleScript;

    void Start () {
        playerCharacter = GameObject.FindWithTag("PlayerCharacter") as GameObject;
        if (null != playerCharacter) playerCharacterRigidbody = playerCharacter.GetComponent<Rigidbody>();
        if (null != playerCharacter) movementManagerScript = playerCharacter.GetComponent<MovementManager>();
        healthPanel = GameObject.FindWithTag("HealthGameObject") as GameObject;
        messageManagerScript = GetComponent<MessageManager>();
        GameObject debugConsole = GameObject.FindWithTag("DebugConsole") as GameObject;
        if (null != debugConsole) debugConsoleScript = debugConsole.GetComponent<DebugConsole>();

        if (null == playerCharacter) { Debug.LogError("PlayerController.Start: playerCharacter is null."); }
        if (null == playerCharacterRigidbody) { Debug.LogError("PlayerController.Start: v is null."); }
        if (null == movementManagerScript) { Debug.LogError("PlayerController.Start: movementManagerScript is null."); }
        if (null == healthPanel) { Debug.LogError("PlayerController.Start: healthPanel is null."); }
        if (null == messageManagerScript) { Debug.LogError("PlayerController.Start: messageManager is null."); }
        if (null == debugConsoleScript) { Debug.LogError("PlayerController.Start: debugConsoleScript is null."); }

        if (null == playerCharacter) { return; }
        if (null == playerCharacterRigidbody) { return; }
        if (null == movementManagerScript) { return; }
        if (null == healthPanel) { return; }
        if (null == messageManagerScript) { return; }
        if (null == debugConsoleScript) { return; }

        initialHelthPanelRotation = healthPanel.transform.eulerAngles;
    }

    void Update () {
        Move currentMove = movementManagerScript.CurrentMove;
        Vector3 currentVector = movementManagerScript.CurrentVector;
        if (Vector3.zero != currentVector) playerCharacter.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(currentVector), 0.15F);
        playerCharacter.transform.Translate(currentVector * currentMove.Speed * Time.deltaTime, Space.World);
        debugConsoleScript.SetOther4("4 current speed=" + currentMove.Speed + ", current direction=" + currentMove.Direction);
    }

    private void OnCollisionEnter(Collision collision) {
        Move currentMove = movementManagerScript.CurrentMove;
        if ((int)LayerValues.TERRAIN != collision.gameObject.layer) movementManagerScript.ForceStop();
        if ("Climbable" == collision.gameObject.tag) climbableInRange = true;
        if (climbableInRange)
        {
            //Collider[] climbables = Environment.GetClimbables(playerCharacter.transform);
            //if (0 != climbables.Length) {
            //    movementManagerScript.SetMovementType(MovementType.Climbing_Mount);
            //}
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

    public bool PlayerCanClimb()
    {
        Collider[] colliders = Environment.GetClimbables(playerCharacter.transform);
        return (0 < colliders.Length);
    }

    void SetHealthPosition(DirectionType inDirection)
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
