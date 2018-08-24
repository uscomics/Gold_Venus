using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using USComics_Debug;
using USComics_Combat;
using USComics_Movement;
using USComics_Environment;
using USComics_Message_Manager;

namespace USComics_Entity
{
    public class PlayerController : EntityController
    {
        void Start()
        {
            SetupEntity();
            int msg = Random.Range(1, 11);
            if (1 == msg) messageManagerScript.ShowImageMessage(Messages.MSG_HOW_TO_MOVE, 7);
            else if (2 == msg) messageManagerScript.ShowImageMessage(Messages.MSG_HOW_TO_CLIMB, 7);
            else if (3 == msg) messageManagerScript.ShowImageMessage(Messages.MSG_HOW_TO_SNEAK, 7);
            else if (4 == msg) messageManagerScript.ShowImageMessage(Messages.MSG_HOW_TO_RUN, 7);
            else if (5 == msg) messageManagerScript.ShowImageMessage(Messages.MSG_HOW_TO_WALK, 7);
            else if (6 == msg) messageManagerScript.ShowImageMessage(Messages.MSG_HOW_TO_STOP, 7);
            else if (7 == msg) messageManagerScript.ShowImageMessage(Messages.MSG_CONTROL_CAMERA, 7);
            else if (8 == msg) messageManagerScript.ShowImageMessage(Messages.MSG_SUPER_BAR, 7);
            else if (9 == msg) messageManagerScript.ShowImageMessage(Messages.MSG_HOW_TO_ATTACK, 7);
            else if (10 == msg) messageManagerScript.ShowImageMessage(Messages.MSG_ATTACK_CONTROLS, 7);
        }

        void Update()
        {
            if (initialUpdate)
            {
                simpleMovementScript.StartModule();
                initialUpdate = false;
            }
            UpdateBuffs();
            if (simpleMovementScript.IsRunning())
            {
                Move currentMove = simpleMovementScript.CurrentMove;
                Vector3 currentVector = simpleMovementScript.CurrentVector;
                if (Vector3.zero != currentVector) entity.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(currentVector), 0.15F);
                entity.transform.Translate(currentVector * currentMove.Speed * Time.deltaTime, Space.World);
                if (DirectionType.Stop != currentMove.Direction) SetHealthPosition(currentMove.Direction);
                debugConsoleScript.SetCurrentMove(currentMove);
                debugConsoleScript.SetOther1("currentVector=" + currentVector);
            }
            else if (climbMovementScript.IsRunning())
            {
                ClimbMove currentMove = climbMovementScript.CurrentMove;
                Vector3 currentVector = climbMovementScript.CurrentVector;
                entity.transform.Translate(currentVector * ClimbSpeed.GetSpeed(currentMove.Climb) * Time.deltaTime, Space.World);
                debugConsoleScript.SetCurrentMove(currentMove);
                debugConsoleScript.SetOther1("currentVector=" + currentVector);
            }
        }
        public override bool IsPlayer() { return true; }

        public PlayerAttackIndex ConvertAttackTypeToPlayerAttackIndex(AttackType attack)
        {
            if (AttackType.Punch == attack) return PlayerAttackIndex.Punch;
            else if (AttackType.Kick == attack) return PlayerAttackIndex.Kick;
            else if (AttackType.Block == attack) return PlayerAttackIndex.Block;
            else if (AttackType.Jumpkick == attack) return PlayerAttackIndex.Jumpkick;
            else if (AttackType.Super == attack) return PlayerAttackIndex.Super;
            return PlayerAttackIndex.None;
        }

        public Attack GetAttackAt(PlayerAttackIndex index) { return attacks[(int)index]; }

        protected override bool SetupEntity()
        {
            base.SetupEntity();
            entity = GameObject.FindWithTag("PlayerCharacter") as GameObject;
            if (null != entity) movementTransitionManagerScript = entity.GetComponent<MovementTransitionManager>();
            if (null != entity) simpleMovementScript = entity.GetComponent<SimpleMovementModule>();
            if (null != entity) climbMovementScript = entity.GetComponent<ClimbMovementModule>();

            if (null == entity) { Debug.LogError("PlayerController.SetupEntity: playerCharacter is null."); }
            if (null == movementTransitionManagerScript) { Debug.LogError("PlayerController.SetupEntity: MovementTransitionManagerScript is null."); }
            if (null == simpleMovementScript) { Debug.LogError("PlayerController.SetupEntity: movementManagerScript is null."); }
            if (null == climbMovementScript) { Debug.LogError("EntityController.SetupEntity: climbManagerScript is null."); }

            if (null == entity) { return false; }
            if (null == movementTransitionManagerScript) { return false; }
            if (null == simpleMovementScript) { return false; }
            if (null == climbMovementScript) { return false; }
            return true;
        }

        private void OnCollisionEnter(Collision collision)
        {
            Move currentMove = simpleMovementScript.CurrentMove;
            if (collision.gameObject.CompareTag("Climbable")) climbableInRange = true;
            if (climbableInRange)
            {
                if (!climbMovementScript.IsRunning())
                {
                    Collider[] climbables = Environment.GetClimbables(entity.transform);
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
                //Debug.Log("BANG! tag = " + collision.gameObject.tag);
                if ((int)LayerValues.TERRAIN != collision.gameObject.layer) simpleMovementScript.ForceStop();
            }
        }
        private void OnCollisionStay(Collision collision)
        {
            // Debug.Log("STILL BANG! tag = " + collision.gameObject.tag);
        }
        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("Climbable")) climbableInRange = false;
            // Debug.Log("BANG DONE! tag = " + collision.gameObject.tag);
        }

        private bool PlayerCanClimb()
        {
            Collider[] colliders = Environment.GetClimbables(entity.transform);
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

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            EditorGizmo(transform);
        }
#endif
    }

    [System.Serializable]
    public enum PlayerAttackIndex {
        None = -1,
        Punch = 0,
        Kick = 1,
        Block = 2,
        Jumpkick = 3,
        Super = 4
    };
}
