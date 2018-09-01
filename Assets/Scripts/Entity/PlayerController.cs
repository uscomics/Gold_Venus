using UnityEngine;
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
            Setup();
            int msg = Random.Range(1, 11);
            if (1 == msg) MessageManagerScript.ShowImageMessage(Messages.MSG_HOW_TO_MOVE, 7);
            else if (2 == msg) MessageManagerScript.ShowImageMessage(Messages.MSG_HOW_TO_CLIMB, 7);
            else if (3 == msg) MessageManagerScript.ShowImageMessage(Messages.MSG_HOW_TO_SNEAK, 7);
            else if (4 == msg) MessageManagerScript.ShowImageMessage(Messages.MSG_HOW_TO_RUN, 7);
            else if (5 == msg) MessageManagerScript.ShowImageMessage(Messages.MSG_HOW_TO_WALK, 7);
            else if (6 == msg) MessageManagerScript.ShowImageMessage(Messages.MSG_HOW_TO_STOP, 7);
            else if (7 == msg) MessageManagerScript.ShowImageMessage(Messages.MSG_CONTROL_CAMERA, 7);
            else if (8 == msg) MessageManagerScript.ShowImageMessage(Messages.MSG_SUPER_BAR, 7);
            else if (9 == msg) MessageManagerScript.ShowImageMessage(Messages.MSG_HOW_TO_ATTACK, 7);
            else if (10 == msg) MessageManagerScript.ShowImageMessage(Messages.MSG_ATTACK_CONTROLS, 7);
        }

        void Update()
        {
            if (InitialUpdate)
            {
                SimpleMovementScript.StartModule();
                InitialUpdate = false;
            }
            UpdateBuffs();
            if (SimpleMovementScript.IsRunning())
            {
                Move currentMove = SimpleMovementScript.CurrentMove;
                Vector3 currentVector = SimpleMovementScript.CurrentVector;
                if (Vector3.zero != currentVector) Entity.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(currentVector), 0.15F);
                Entity.transform.Translate(currentVector * currentMove.Speed * Time.deltaTime, Space.World);
                if (DirectionType.Stop != currentMove.Direction) SetHealthPosition(currentMove.Direction);
                DebugConsoleScript.SetCurrentMove(currentMove);
                DebugConsoleScript.SetOther1("currentVector=" + currentVector);
            }
            else if (ClimbMovementScript.IsRunning())
            {
                ClimbMove currentMove = ClimbMovementScript.CurrentMove;
                Vector3 currentVector = ClimbMovementScript.CurrentVector;
                Entity.transform.Translate(currentVector * ClimbSpeed.GetSpeed(currentMove.Climb) * Time.deltaTime, Space.World);
                DebugConsoleScript.SetCurrentMove(currentMove);
                DebugConsoleScript.SetOther1("currentVector=" + currentVector);
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

        public Attack GetAttackAt(PlayerAttackIndex index) { return Attacks[(int)index]; }

        protected override bool Setup()
        {
            base.Setup();
            Entity = GameObject.FindWithTag("PlayerCharacter") as GameObject;
            if (null != Entity) MovementTransitionManagerScript = Entity.GetComponent<MovementTransitionManager>();
            if (null != Entity) SimpleMovementScript = Entity.GetComponent<SimpleMovementModule>();
            if (null != Entity) ClimbMovementScript = Entity.GetComponent<ClimbMovementModule>();

            if (null == Entity) { Debug.LogError("PlayerController.SetupEntity: playerCharacter is null."); }
            if (null == MovementTransitionManagerScript) { Debug.LogError("PlayerController.SetupEntity: MovementTransitionManagerScript is null."); }
            if (null == SimpleMovementScript) { Debug.LogError("PlayerController.SetupEntity: movementManagerScript is null."); }
            if (null == ClimbMovementScript) { Debug.LogError("EntityController.SetupEntity: climbManagerScript is null."); }

            if (null == Entity) { return false; }
            if (null == MovementTransitionManagerScript) { return false; }
            if (null == SimpleMovementScript) { return false; }
            if (null == ClimbMovementScript) { return false; }
            return true;
        }

        private void OnCollisionEnter(Collision collision)
        {
            Move currentMove = SimpleMovementScript.CurrentMove;
            if (collision.gameObject.CompareTag("Climbable")) ClimbableInRange = true;
            if (ClimbableInRange)
            {
                if (!ClimbMovementScript.IsRunning())
                {
                    Collider[] climbables = Environment.GetClimbables(Entity.transform);
                    if (0 != climbables.Length)
                    {
                        MovementTransitionManagerScript.StartTransitionFrom(ModuleTypes.Simple, ModuleTypes.Climbing);
                    }
                }
                else
                {
                    ClimbMovementScript.ForceStop();
                    MovementTransitionManagerScript.StartTransitionFrom(ModuleTypes.Climbing, ModuleTypes.Simple);

                }
            } else
            {
                //Debug.Log("BANG! tag = " + collision.gameObject.tag);
                if ((int)LayerValues.TERRAIN != collision.gameObject.layer) SimpleMovementScript.ForceStop();
            }
        }
        private void OnCollisionStay(Collision collision)
        {
            // Debug.Log("STILL BANG! tag = " + collision.gameObject.tag);
        }
        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("Climbable")) ClimbableInRange = false;
            // Debug.Log("BANG DONE! tag = " + collision.gameObject.tag);
        }

        private bool PlayerCanClimb()
        {
            Collider[] colliders = Environment.GetClimbables(Entity.transform);
            return (0 < colliders.Length);
        }

        private void SetHealthPosition(DirectionType inDirection)
        {
            HealthPanel.transform.localRotation = Quaternion.identity;
            if (DirectionType.South == inDirection) { HealthPanel.transform.Rotate(Vector3.up, 180); }
            else if (DirectionType.East == inDirection) { HealthPanel.transform.Rotate(Vector3.up, 270); }
            else if (DirectionType.West == inDirection) { HealthPanel.transform.Rotate(Vector3.up, 90); }
            else if (DirectionType.NW == inDirection) { HealthPanel.transform.Rotate(Vector3.up, 45); }
            else if (DirectionType.NE == inDirection) { HealthPanel.transform.Rotate(Vector3.up, 315); }
            else if (DirectionType.SW == inDirection) { HealthPanel.transform.Rotate(Vector3.up, 135); }
            else if (DirectionType.SE == inDirection) { HealthPanel.transform.Rotate(Vector3.up, 225); }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            EditorGizmo(transform);
        }
#endif
    }
}
