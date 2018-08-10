using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace USComics_User_Input
{
    [System.Serializable]
    public enum MovementType
    {
        Sneaking,
        Walking,
        Running,
        Climbing,
        None
    }

    public class MovementSpeed
    {
        public const float SNEAK_SPEED = 1.0f;
        public const float WALK_SPEED = 5.0f;
        public const float RUN_SPEED = 10.0f;
        public const float CLIMBING_SPEED = 1.0f;
        public static float GetSpeed(MovementType movementType)
        {
            if (MovementType.Sneaking == movementType) return MovementSpeed.SNEAK_SPEED;
            if (MovementType.Walking == movementType) return MovementSpeed.WALK_SPEED;
            if (MovementType.Running == movementType) return MovementSpeed.RUN_SPEED;
            if (MovementType.Climbing == movementType) return MovementSpeed.CLIMBING_SPEED;
            return 0.0f;
        }
    }

    public class MovementTypeMenu : MonoBehaviour
    {
        public Button currentMovementTypeButton;
        public Image sneakingIcon;
        public Image walkingIcon;
        public Image runningIcon;
        public Image climbingIcon;
        public static MovementType currentMovementType;

        private GameObject movementTypeList;

        void Start()
        {
            currentMovementType = MovementType.Walking;
            movementTypeList = GameObject.FindWithTag("MovementTypeList") as GameObject;
            if (null == movementTypeList) { Debug.LogError("MovementTypeController.Start: movementTypeList is null."); }

            if (null == movementTypeList) { return; }
            if (null == currentMovementTypeButton) { return; }

            ToggleMovementTypeList();
        }
        void Update() { }
        public void ToggleMovementTypeList() { movementTypeList.SetActive(!movementTypeList.activeSelf); }
        public void ShowMovementTypeList() { movementTypeList.SetActive(true); }
        public void HideMovementTypeList() { movementTypeList.SetActive(false); }
        public void Sneaking()
        {
            MovementTypeMenu.currentMovementType = MovementType.Sneaking;
            currentMovementTypeButton.image.sprite = sneakingIcon.sprite;
            HideMovementTypeList();
        }
        public void Walking()
        {
            MovementTypeMenu.currentMovementType = MovementType.Walking;
            currentMovementTypeButton.image.sprite = walkingIcon.sprite;
            HideMovementTypeList();
        }
        public void Running()
        {
            MovementTypeMenu.currentMovementType = MovementType.Running;
            currentMovementTypeButton.image.sprite = runningIcon.sprite;
            HideMovementTypeList();
        }
        public void Climbing()
        {
            MovementTypeMenu.currentMovementType = MovementType.Climbing;
            currentMovementTypeButton.image.sprite = climbingIcon.sprite;
            HideMovementTypeList();
        }
        public float SetMovementType(MovementType movementType)
        {
            if (MovementType.Sneaking == movementType) Sneaking();
            else if (MovementType.Walking == movementType) Walking();
            else if (MovementType.Running == movementType) Running();
            else if (MovementType.Sneaking == movementType) Climbing();
            return MovementSpeed.GetSpeed(movementType);
        }
    }
}
