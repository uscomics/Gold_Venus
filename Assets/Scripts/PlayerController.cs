﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using USComics_Vision;
using USComics_User_Input;
using USComics_Environment;
using USComics_Message_Manager;

public class PlayerController : MonoBehaviour {
    public Direction direction = Direction.None;
    public bool movementToggledOn = false;

    private GameObject playerCharacter;
    private Rigidbody playerCharacterRigidbody;
    private Animator anim;
    private GameObject movementPadIndicator;
    private GameObject currentMovementTypePanel;
    private MovementTypeMenu movementTypeMenuScript;
    private Vector2 movementPadIndicatorOriginalPosition;
    private GameObject healthPanel;
    private Vector3 initialHelthPanelRotation;
    private MessageManager messageManager;

    private Vector3 previousVector = Vector3.zero;
    private float speed = 1.0f;
    private GameObject messageCanvas;

    void Start () {
        playerCharacter = GameObject.FindWithTag("PlayerCharacter") as GameObject;
        if (null != playerCharacter) playerCharacterRigidbody = playerCharacter.GetComponent<Rigidbody>();
        if (null != playerCharacter) anim = playerCharacter.GetComponent<Animator>();
        movementPadIndicator = GameObject.FindWithTag("MovementPadIndicator") as GameObject;
        currentMovementTypePanel = GameObject.FindWithTag("CurrentMovementType") as GameObject;
        if (null != currentMovementTypePanel) movementTypeMenuScript = currentMovementTypePanel.GetComponent<MovementTypeMenu>();
        messageCanvas = GameObject.FindWithTag("MessageCanvas") as GameObject;
        healthPanel = GameObject.FindWithTag("HealthGameObject") as GameObject;
        messageManager = GetComponent<MessageManager>();

        if (null == playerCharacter) { Debug.LogError("PlayerController.Start: playerCharacter is null."); }
        if (null == playerCharacterRigidbody) { Debug.LogError("PlayerController.Start: v is null."); }
        if (null == anim) { Debug.LogError("PlayerController.Start: anim is null."); }
        if (null == movementPadIndicator) { Debug.LogError("PlayerController.Start: movementPadIndicator is null."); }
        if (null == currentMovementTypePanel) { Debug.LogError("PlayerController.Start: currentMovementTypePanel is null."); }
        if (null == movementTypeMenuScript) { Debug.LogError("PlayerController.Start: movementTypeMenuScript is null."); }
        if (null == messageCanvas) { Debug.LogError("PlayerController.Start: messageCanvas is null."); }
        if (null == healthPanel) { Debug.LogError("PlayerController.Start: healthPanel is null."); }
        if (null == messageManager) { Debug.LogError("PlayerController.Start: messageManager is null."); }

        if (null == playerCharacter) { return; }
        if (null == playerCharacterRigidbody) { return; }
        if (null == anim) { return; }
        if (null == movementPadIndicator) { return; }
        if (null == currentMovementTypePanel) { return; }
        if (null == movementTypeMenuScript) { return; }
        if (null == messageCanvas) { return; }
        if (null == healthPanel) { return; }
        if (null == messageManager) { return; }

        movementPadIndicatorOriginalPosition = movementPadIndicator.transform.position;
        initialHelthPanelRotation = healthPanel.transform.eulerAngles;
    }

    void Update () {
        MovementType movementType = Keyboard.GetMovementType();
        if ((MovementType.Climbing == movementType) && (!PlayerCanClimb())) {
            messageManager.ShowMessage(Messages.MSG_NOTHING_TO_CLIMB);
            movementType = MovementType.None;
        }

        if (MovementType.None != movementType)  {
            speed = movementTypeMenuScript.SetMovementType(movementType);
        } else {
            speed = MovementSpeed.GetSpeed(MovementTypeMenu.currentMovementType);
        }

        Direction direction = Keyboard.GetDirection();
        if (Direction.None == direction) { direction = MovementPad.GetDirection(); }
        if (Direction.None != direction) { movementTypeMenuScript.HideMovementTypeList(); }

        if (PlayerShouldStop(direction)) {
            movementToggledOn = false;
            anim.SetFloat("Speed", 0.0f);
        } else if (PlayerShouldGo(direction)) {
            setMovementTypeValues();
            previousVector = ConvertForClimbing(MovementTypeMenu.currentMovementType, Vision.ConvertDirectionToVector(direction, previousVector));
            movementToggledOn = true;

            if (MovementType.Climbing != MovementTypeMenu.currentMovementType) {
                playerCharacter.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(previousVector), 0.15F);
            }
            playerCharacter.transform.Translate (previousVector * speed * Time.deltaTime, Space.World);
        }
        if (Direction.None != direction) {
            movementPadIndicator.transform.position = MovementPad.GetIndicatorPosition(direction, movementPadIndicatorOriginalPosition);
            if (Direction.None != direction && Direction.Stop != direction) SetHealthPosition(direction);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        // Debug.Log("BANG! tag = " + collision.gameObject.tag + ", layer = " + collision.gameObject.layer + ", TERRAIN = " + LayerValues.TERRAIN);
        if ((int)LayerValues.TERRAIN != collision.gameObject.layer) Keyboard.forceStop = true;
        if (((int)LayerValues.TERRAIN == collision.gameObject.layer)&& (MovementType.Climbing == MovementTypeMenu.currentMovementType)) Keyboard.forceStop = true;
    }
    private void OnCollisionStay(Collision collision)
    {
        // Debug.Log("STILL BANG! tag = " + collision.gameObject.tag);
    }
    private void OnCollisionExit(Collision collision)
    {
        // Debug.Log("BANG DONE! tag = " + collision.gameObject.tag);
    }

    public bool PlayerCanClimb() {
        Collider[] colliders = Environment.GetClimbables(playerCharacter.transform);
        return (0 < colliders.Length);
    }
    bool PlayerShouldStop(Direction direction) { return ((movementToggledOn) && (Direction.Stop == direction)); }

    bool PlayerShouldGo(Direction direction) {
       if (Direction.Stop == direction) return false;
       if ((!movementToggledOn) && (Direction.None == direction)) return false;
       return true;
    }

    void setMovementTypeValues() {
        switch (MovementTypeMenu.currentMovementType)
        {
            case MovementType.Sneaking:
                anim.SetBool("Sneak", true);
                anim.SetBool("Climb", false);
                anim.SetFloat("Speed", MovementSpeed.GetSpeed(MovementType.Sneaking));
                break;
            case MovementType.Walking:
                anim.SetBool("Sneak", false);
                anim.SetBool("Climb", false);
                anim.SetFloat("Speed", MovementSpeed.GetSpeed(MovementType.Walking));
                break;
            case MovementType.Running:
                anim.SetBool("Sneak", false);
                anim.SetBool("Climb", false);
                anim.SetFloat("Speed", MovementSpeed.GetSpeed(MovementType.Running));
                break;
            case MovementType.Climbing:
                anim.SetBool("Sneak", false);
                anim.SetBool("Climb", true);
                anim.SetFloat("Speed", MovementSpeed.GetSpeed(MovementType.Climbing));
                break;
        }
    }

    Vector3 ConvertForClimbing(MovementType type, Vector3 movement)
    {
        Debug.Log("ConvertForClimbing! type " + type);
        if (MovementType.Climbing != type) {
            playerCharacterRigidbody.useGravity = true;
            return movement;
        }
        playerCharacterRigidbody.useGravity = false;
        if (Vector3.forward == movement)
        {
            Debug.Log("UP!");
            return Vector3.up;
        }
        if (-Vector3.forward == movement)
        {
            Debug.Log("DOWN!");
            return -Vector3.up;
        }
        return movement;
    }

    void SetHealthPosition(Direction inDirection)
    {
        healthPanel.transform.localRotation = Quaternion.identity;
        if (Direction.South == inDirection) { healthPanel.transform.Rotate(Vector3.up, 180); }
        else if (Direction.East == inDirection) { healthPanel.transform.Rotate(Vector3.up, 270); }
        else if (Direction.West == inDirection) { healthPanel.transform.Rotate(Vector3.up, 90); }
        else if (Direction.NW == inDirection) { healthPanel.transform.Rotate(Vector3.up, 45); }
        else if (Direction.NE == inDirection) { healthPanel.transform.Rotate(Vector3.up, 315); }
        else if (Direction.SW == inDirection) { healthPanel.transform.Rotate(Vector3.up, 135); }
        else if (Direction.SE == inDirection) { healthPanel.transform.Rotate(Vector3.up, 225); }
    }

}
