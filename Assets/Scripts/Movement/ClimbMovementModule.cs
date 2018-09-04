using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USComics_Debug;

namespace USComics_Movement
{
    public class ClimbMovementModule
        : AbstractMovementModule
    {
        public ClimbMove CurrentMove { get; set; }
        public ClimbMove PreviousMove { get; set; }
        public Vector3 CurrentVector { get; set; }
        public Vector3 PreviousVector { get; set; }
        public float Speed { get; set; }

        private Rigidbody rigidBody;
        private Animator Anim;
        private MovementTransitionManager movementTransitionManagerScript;
        private ClimbPad ClimbPadScript;
        private GameObject climbingPanel;
        private Keyboard KeyboardScript;
        private bool moduleActive;

        // Use this for initialization
        void Start()
        {
            GameObject playerCharacter = GameObject.FindWithTag("PlayerCharacter") as GameObject;
            if (null != playerCharacter) Anim = playerCharacter.GetComponent<Animator>();
            if (null != playerCharacter) movementTransitionManagerScript = playerCharacter.GetComponent<MovementTransitionManager>();
            if (null != playerCharacter) rigidBody = playerCharacter.GetComponent<Rigidbody>();
            GameObject climbPad = GameObject.FindWithTag("ClimbPad") as GameObject;
            if (null != climbPad) ClimbPadScript = climbPad.GetComponent<ClimbPad>();
            if (null != climbPad) KeyboardScript = climbPad.GetComponent<Keyboard>();
            climbingPanel = GameObject.FindWithTag("ClimbingPanel") as GameObject;

            if (null == Anim) { Debug.LogError("ClimbMovementModule.Start: Anim is null."); }
            if (null == movementTransitionManagerScript) { Debug.LogError("ClimbMovementModule.Start: MovementTransitionManagerScript is null."); }
            if (null == rigidBody) { Debug.LogError("ClimbMovementModule.Start: rigidBody is null."); }
            if (null == ClimbPadScript) { Debug.LogError("ClimbMovementModule.Start: ClimbPadScript is null."); }
            if (null == climbingPanel) { Debug.LogError("ClimbMovementModule.Start: climbingPanel is null."); }
            if (null == KeyboardScript) { Debug.LogError("ClimbMovementModule.Start: KeyboardScript is null."); }

            if (null == Anim) { return; }
            if (null == movementTransitionManagerScript) { return; }
            if (null == rigidBody) { return; }
            if (null == ClimbPadScript) { return; }
            if (null == climbingPanel) { return; }
            if (null == KeyboardScript) { return; }

            CurrentMove = new ClimbMove(DirectionType.None, ClimbType.None);
            CurrentVector = ConvertForClimbing(Direction.ConvertDirectionToVector(DirectionType.None, Vector3.zero));
            PreviousMove = new ClimbMove(DirectionType.None, ClimbType.None);
            PreviousVector = ConvertForClimbing(Direction.ConvertDirectionToVector(DirectionType.None, Vector3.zero));
            Speed = 0.0f;
            moduleActive = false;
            movementTransitionManagerScript.Register(this);
            ClimbPadScript.HideClimbingUI();
        }

        // Update is called once per frame
        void Update()
        {
            if (!moduleActive) return;
            DirectionType direction = GetDirection();
            if ((DirectionType.Stop != direction)
            && (DirectionType.North != direction) 
            && (DirectionType.South != direction)
            && (DirectionType.None != direction))
            {
                TransitionOutOfClimb();
                return;
            }
            ClimbType climbType = GetClimbType();
            if (DirectionType.Stop == direction) { climbType = ClimbType.Idle; }
            Speed = ClimbSpeed.GetSpeed(climbType);
            SetMove(direction, climbType);
        }

        public override ModuleTypes ModuleType() { return ModuleTypes.Climbing;  }
        public override string GetTransitionToAnimationName() { return "ClimbStart"; }
        public override string GetTransitionFromAnimationName() { return "ClimbStop"; }

        public override void StartModule()
        {
            moduleActive = true;
            rigidBody.useGravity = false;
            ClimbPadScript.ShowClimbingUI();
            ForceStop();
        }

        public override bool IsRunning()
        {
            return moduleActive;
        }

        public override void StopModule()
        {
            moduleActive = false;
            rigidBody.useGravity = true;
            ClimbPadScript.HideClimbingUI();
        }

        public void ForceStop()
        {
            BufferedDirection stop = new BufferedDirection(DirectionType.Stop);
            KeyboardScript.directionBuffer.queue.Enqueue(stop);
        }

        public void TransitionOutOfClimb()
        {
            movementTransitionManagerScript.StartTransitionFrom(ModuleTypes.Climbing, ModuleTypes.Simple);
        }

        private DirectionType GetDirection()
        {
            DirectionType direction = ClimbPadScript.CurrentDirection;
            return direction;
        }

        private Vector3 ConvertForClimbing(Vector3 vector)
        {
            if (Vector3.forward == vector) return Vector3.up;
            if (-Vector3.forward == vector) return -Vector3.up;
            if (-Vector3.zero == vector) return Vector3.zero;
            return Vector3.zero;
        }

        private ClimbType GetClimbType()
        {
            return ClimbType.Climbing;
        }

        private void SetMove(DirectionType direction, ClimbType climbType)
        {
            if ((CurrentMove.Direction == direction) && (CurrentMove.Climb == climbType)) return;

            Vector3 tempVector = PreviousVector;
            PreviousVector = CurrentVector;
            CurrentVector = ConvertForClimbing(Direction.ConvertDirectionToVector(direction, tempVector));
            PreviousMove = CurrentMove;
            CurrentMove = new ClimbMove(direction, climbType);
            UpdateAnimation(climbType);
        }

        private void UpdateAnimation(ClimbType climbType)
        {
            if (ClimbType.Climbing == climbType) Climbing();
            else if (ClimbType.Idle == climbType) Idle();
        }
        private void Climbing()
        {
            Anim.Play("LadderClimb");
        }
        private void Idle()
        {
            Anim.Play("EdgeGrab_Idle");
        }
    }
}
