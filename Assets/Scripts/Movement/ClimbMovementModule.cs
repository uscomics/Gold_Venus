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
        private MovementPad MovementPadScript;
        private GameObject climbingPanel;
        private Keyboard KeyboardScript;
        private DebugConsole debugConsoleScript;
        private bool moduleActive;

        // Use this for initialization
        void Start()
        {
            GameObject playerCharacter = GameObject.FindWithTag("PlayerCharacter") as GameObject;
            if (null != playerCharacter) Anim = playerCharacter.GetComponent<Animator>();
            if (null != playerCharacter) movementTransitionManagerScript = playerCharacter.GetComponent<MovementTransitionManager>();
            if (null != playerCharacter) rigidBody = playerCharacter.GetComponent<Rigidbody>();
            GameObject movementPad = GameObject.FindWithTag("MovementPad") as GameObject;
            if (null != movementPad) MovementPadScript = movementPad.GetComponent<MovementPad>();
            if (null != movementPad) KeyboardScript = movementPad.GetComponent<Keyboard>();
            climbingPanel = GameObject.FindWithTag("ClimbingPanel") as GameObject;
            GameObject debugConsole = GameObject.FindWithTag("DebugConsole") as GameObject;
            if (null != debugConsole) debugConsoleScript = debugConsole.GetComponent<DebugConsole>();

            if (null == Anim) { Debug.LogError("ClimbMovementModule.Start: Anim is null."); }
            if (null == movementTransitionManagerScript) { Debug.LogError("ClimbMovementModule.Start: MovementTransitionManagerScript is null."); }
            if (null == rigidBody) { Debug.LogError("ClimbMovementModule.Start: rigidBody is null."); }
            if (null == MovementPadScript) { Debug.LogError("ClimbMovementModule.Start: MovementPadScript is null."); }
            if (null == climbingPanel) { Debug.LogError("ClimbMovementModule.Start: climbingPanel is null."); }
            if (null == KeyboardScript) { Debug.LogError("ClimbMovementModule.Start: KeyboardScript is null."); }
            if (null == debugConsoleScript) { Debug.LogError("ClimbMovementModule.Start: debugConsoleScript is null."); }

            if (null == Anim) { return; }
            if (null == movementTransitionManagerScript) { return; }
            if (null == rigidBody) { return; }
            if (null == MovementPadScript) { return; }
            if (null == climbingPanel) { return; }
            if (null == KeyboardScript) { return; }
            if (null == debugConsoleScript) { return; }

            CurrentMove = new ClimbMove(DirectionType.None, ClimbType.None);
            CurrentVector = ConvertForClimbing(DirectionUtilities.ConvertDirectionToVector(DirectionType.None, Vector3.zero));
            PreviousMove = new ClimbMove(DirectionType.None, ClimbType.None);
            PreviousVector = ConvertForClimbing(DirectionUtilities.ConvertDirectionToVector(DirectionType.None, Vector3.zero));
            Speed = 0.0f;
            moduleActive = false;
            movementTransitionManagerScript.Register(this);
            climbingPanel.SetActive(false);
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
            if (!climbingPanel.activeSelf) climbingPanel.SetActive(true);
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
            if (climbingPanel.activeSelf) climbingPanel.SetActive(false);
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
            DirectionType direction = MovementPadScript.CurrentDirection;
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
            CurrentVector = ConvertForClimbing(DirectionUtilities.ConvertDirectionToVector(direction, tempVector));
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

    [System.Serializable]
    public enum ClimbType
    {
        Climbing,
        Idle,
        None
    }

    [System.Serializable]
    public class ClimbSpeed
    {
        public const float CLIMB_SPEED = 1.0f;
        public const float IDLE_SPEED = 1.0f;
        public static float GetSpeed(ClimbType climbType)
        {
            if (ClimbType.Climbing == climbType) return ClimbSpeed.CLIMB_SPEED;
            if (ClimbType.Idle == climbType) return ClimbSpeed.IDLE_SPEED;
            return 0.0f;
        }
    }

    [System.Serializable]
    public class ClimbMove
    {
        public ClimbMove(DirectionType directionType, ClimbType climbType)
        {
            Direction = directionType;
            Climb = climbType;
        }
        public DirectionType Direction { get; set; }
        public ClimbType Climb { get; set; }
    }
}
