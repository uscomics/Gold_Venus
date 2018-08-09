using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using USComics_User_Input;

public class PlayerController : MonoBehaviour {
    public Direction direction = Direction.None;
    public bool movementToggledOn = false;

    private GameObject playerCharacter;
    private Animator anim;
    private GameObject movementPadIndicator;
    private GameObject currentMovementTypePanel;
    private MovementTypeMenu movementTypeMenuScript;
    private Vector2 movementPadIndicatorOriginalPosition;
    private GameObject healthPanel;
    private Vector3 initialHelthPanelLocalPosition;
    private Quaternion initialHelthPanelRotation;

    private Vector3 previousVector = Vector3.zero;
    private float speed = 1.0f;
    private GameObject messageCanvas;

    void Start () {
        playerCharacter = GameObject.FindWithTag("PlayerCharacter") as GameObject;
        if (null != playerCharacter) anim = playerCharacter.GetComponent<Animator>();
        movementPadIndicator = GameObject.FindWithTag("MovementPadIndicator") as GameObject;
        currentMovementTypePanel = GameObject.FindWithTag("CurrentMovementType") as GameObject;
        if (null != currentMovementTypePanel) movementTypeMenuScript = currentMovementTypePanel.GetComponent<MovementTypeMenu>();
        messageCanvas = GameObject.FindWithTag("MessageCanvas") as GameObject;
        healthPanel = GameObject.FindWithTag("HealthGameObject") as GameObject;

        if (null == playerCharacter) { Debug.LogError("PlayerController.Start: playerCharacter is null."); }
        if (null == anim) { Debug.LogError("PlayerController.Start: anim is null."); }
        if (null == movementPadIndicator) { Debug.LogError("PlayerController.Start: movementPadIndicator is null."); }
        if (null == currentMovementTypePanel) { Debug.LogError("PlayerController.Start: currentMovementTypePanel is null."); }
        if (null == movementTypeMenuScript) { Debug.LogError("PlayerController.Start: movementTypeMenuScript is null."); }
        if (null == messageCanvas) { Debug.LogError("PlayerController.Start: messageCanvas is null."); }
        healthPanel = GameObject.FindWithTag("HealthGameObject") as GameObject;

        if (null == playerCharacter) { return; }
        if (null == anim) { return; }
        if (null == movementPadIndicator) { return; }
        if (null == currentMovementTypePanel) { return; }
        if (null == movementTypeMenuScript) { return; }
        if (null == messageCanvas) { return; }
        if (null == healthPanel) { return; }

        movementPadIndicatorOriginalPosition = movementPadIndicator.transform.position;
        initialHelthPanelLocalPosition = healthPanel.transform.localPosition;
        initialHelthPanelRotation = healthPanel.transform.rotation;
    }

    void Update () {
        MovementType movementType = Keyboard.GetMovementType();
        if (MovementType.None != movementType)  {
            speed = movementTypeMenuScript.SetMovementType(movementType);
        } else {
            speed = MovementSpeed.GetSpeed(MovementTypeMenu.currentMovementType);
        }

        Direction direction = Keyboard.GetDirection();
        if (Direction.None == direction) { direction = MovementPad.GetDirection(); }

        if (PlayerShouldStop(direction)) {
            movementToggledOn = false;
            anim.SetFloat("Speed", 0.0f);
        } else if (PlayerShouldGo(direction)) {
            setMovementTypeValues();
            previousVector = MovementPad.ConvertDirectionToVector(direction, previousVector);
            movementToggledOn = true;

            playerCharacter.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(previousVector), 0.15F);
            playerCharacter.transform.Translate (previousVector * speed * Time.deltaTime, Space.World);
        }
        if (Direction.None != direction) {
            movementPadIndicator.transform.position = MovementPad.GetIndicatorPosition(direction, movementPadIndicatorOriginalPosition);
            if (Direction.None != direction && Direction.Stop != direction) healthPanel.transform.localPosition = GetHealthPosition(direction);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        Debug.Log("BANG!");
        Keyboard.forceStop = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("NO BANG!");
        Keyboard.forceStop = true;
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
                anim.SetFloat("Speed", MovementSpeed.GetSpeed(MovementType.Sneaking));
                break;
            case MovementType.Walking:
                anim.SetBool("Sneak", false);
                anim.SetFloat("Speed", MovementSpeed.GetSpeed(MovementType.Walking));
                break;
            case MovementType.Running:
                anim.SetBool("Sneak", false);
                anim.SetFloat("Speed", MovementSpeed.GetSpeed(MovementType.Running));
                break;
            case MovementType.Climbing:
                anim.SetBool("Sneak", false);
                anim.SetFloat("Speed", MovementSpeed.GetSpeed(MovementType.Climbing));
                break;
        }
    }

    Vector3 GetHealthPosition(Direction inDirection)
    {
        if (Direction.South == inDirection) return new Vector3(initialHelthPanelLocalPosition.x, initialHelthPanelLocalPosition.y, -initialHelthPanelLocalPosition.z);
        if (Direction.SW == inDirection) return new Vector3(initialHelthPanelLocalPosition.x, initialHelthPanelLocalPosition.y, -initialHelthPanelLocalPosition.z);
        if (Direction.SE == inDirection) return new Vector3(initialHelthPanelLocalPosition.x, initialHelthPanelLocalPosition.y, -initialHelthPanelLocalPosition.z);
        return initialHelthPanelLocalPosition;
    }
}
