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
            GameObject speedBarKnob = GameObject.FindWithTag("SpeedBar") as GameObject;
            GameObject debugConsole = GameObject.FindWithTag("DebugConsole") as GameObject;
            if (null != debugConsole) debugConsoleScript = debugConsole.GetComponent<DebugConsole>();

            if (null == Anim) { Debug.LogError("ClimbMovementModule.Start: Anim is null."); }
            if (null == movementTransitionManagerScript) { Debug.LogError("ClimbMovementModule.Start: MovementTransitionManagerScript is null."); }
            if (null == rigidBody) { Debug.LogError("ClimbMovementModule.Start: rigidBody is null."); }
            if (null == MovementPadScript) { Debug.LogError("ClimbMovementModule.Start: MovementPadScript is null."); }
            if (null == KeyboardScript) { Debug.LogError("ClimbMovementModule.Start: KeyboardScript is null."); }
            if (null == debugConsoleScript) { Debug.LogError("ClimbMovementModule.Start: debugConsoleScript is null."); }

            if (null == Anim) { return; }
            if (null == movementTransitionManagerScript) { return; }
            if (null == rigidBody) { return; }
            if (null == MovementPadScript) { return; }
            if (null == KeyboardScript) { return; }
            if (null == debugConsoleScript) { return; }

            CurrentMove = new ClimbMove(DirectionType.None, ClimbType.None);
            CurrentVector = ConvertForClimbing(DirectionUtilities.ConvertDirectionToVector(DirectionType.None, Vector3.zero));
            PreviousMove = new ClimbMove(DirectionType.None, ClimbType.None);
            PreviousVector = ConvertForClimbing(DirectionUtilities.ConvertDirectionToVector(DirectionType.None, Vector3.zero));
            Speed = 0.0f;
            moduleActive = false;
            movementTransitionManagerScript.Register(this);
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
                movementTransitionManagerScript.Transition = new MovementModulesTransition(ModuleTypes.Climbing, ModuleTypes.Simple);
                movementTransitionManagerScript.TransitionFromStarted();
                return;
            }
            ClimbType climbType = GetClimbType();
            if (DirectionType.Stop == direction) { Speed = 0.0f; }
            else { Speed = ClimbSpeed.GetSpeed(climbType); }
            SetMove(direction, climbType);
        }

        public override ModuleTypes ModuleType() { return ModuleTypes.Climbing;  }
        public override string StartAnimationVariable() { return "ClimbStart"; }
        public override string StopAnimationVariable() { return "ClimbStop"; }

        public override void StartModule()
        {
            moduleActive = true;
            rigidBody.useGravity = false;
        }

        public override bool IsRunning()
        {
            return moduleActive;
        }

        public override void StopModule()
        {
            moduleActive = false;
            rigidBody.useGravity = true;
        }

        public void ForceStop()
        {
            BufferedDirection stop = new BufferedDirection(DirectionType.Stop);
            KeyboardScript.directionBuffer.queue.Enqueue(stop);
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
        }
        private void Climbing()
        {
            Anim.SetBool("Sneak", false);
            Anim.SetBool("Climb", true);
            Anim.SetBool("Fall", false);
            Anim.SetBool("Stand", false);
            Anim.SetFloat("Speed", Speed);
        }
    }

    [System.Serializable]
    public enum ClimbType
    {
        Climbing,
        None
    }

    [System.Serializable]
    public class ClimbSpeed
    {
        public const float CLIMB_SPEED = 1.0f;
        public static float GetSpeed(ClimbType climbType)
        {
            if (ClimbType.Climbing == climbType) return ClimbSpeed.CLIMB_SPEED;
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
