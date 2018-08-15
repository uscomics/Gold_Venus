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

        private Animator Anim;
        private MovementTransitionManager MovementTransitionManagerScript;
        private MovementPad MovementPadScript;
        private Keyboard KeyboardScript;
        private DebugConsole debugConsoleScript;
        private bool moduleActive;

        // Use this for initialization
        void Start()
        {
            GameObject playerCharacter = GameObject.FindWithTag("PlayerCharacter") as GameObject;
            if (null != playerCharacter) Anim = playerCharacter.GetComponent<Animator>();
            if (null != playerCharacter) MovementTransitionManagerScript = playerCharacter.GetComponent<MovementTransitionManager>();
            GameObject movementPad = GameObject.FindWithTag("MovementPad") as GameObject;
            if (null != movementPad) MovementPadScript = movementPad.GetComponent<MovementPad>();
            if (null != movementPad) KeyboardScript = movementPad.GetComponent<Keyboard>();
            GameObject speedBarKnob = GameObject.FindWithTag("SpeedBar") as GameObject;
            GameObject debugConsole = GameObject.FindWithTag("DebugConsole") as GameObject;
            if (null != debugConsole) debugConsoleScript = debugConsole.GetComponent<DebugConsole>();

            if (null == Anim) { Debug.LogError("ClimbManager.Start: Anim is null."); }
            if (null == MovementTransitionManagerScript) { Debug.LogError("MovementManager.Start: MovementTransitionManagerScript is null."); }
            if (null == MovementPadScript) { Debug.LogError("ClimbManager.Start: MovementPadScript is null."); }
            if (null == KeyboardScript) { Debug.LogError("ClimbManager.Start: KeyboardScript is null."); }
            if (null == debugConsoleScript) { Debug.LogError("ClimbManager.Start: debugConsoleScript is null."); }

            if (null == Anim) { return; }
            if (null == MovementTransitionManagerScript) { return; }
            if (null == MovementPadScript) { return; }
            if (null == KeyboardScript) { return; }
            if (null == debugConsoleScript) { return; }

            CurrentMove = new ClimbMove(DirectionType.None, ClimbType.None);
            CurrentVector = DirectionUtilities.ConvertDirectionToVector(DirectionType.None, Vector3.zero);
            PreviousMove = new ClimbMove(DirectionType.None, ClimbType.None);
            PreviousVector = DirectionUtilities.ConvertDirectionToVector(DirectionType.None, Vector3.zero);
            Speed = 0.0f;
            moduleActive = false;
            MovementTransitionManagerScript.Register(this);
        }

        // Update is called once per frame
        void Update()
        {
            if (!moduleActive) return;
            DirectionType direction = GetDirection();
            if (DirectionType.Stop == direction) Speed = 0.0f;
            else if ((DirectionType.North != direction) 
            && (DirectionType.South != direction)
            && (DirectionType.None != direction))
            {
                StopModule();
                return;
            }
            ClimbType climbType = GetClimbType();
            SetMove(direction, climbType);
        }

        public override ModuleTypes ModuleType() { return ModuleTypes.Climbing;  }
        public override string StartAnimationVariable() { return "ClimbStart"; }
        public override string StopAnimationVariable() { return "ClimbStop"; }

        public override void StartModule()
        {
            moduleActive = true;
        }

        public override bool IsRunning()
        {
            return moduleActive;
        }

        public override void StopModule()
        {
            moduleActive = false;
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
            if (Anim.GetBool("Climb")) return ClimbType.Climbing;
            return ClimbType.None;
        }

        private void SetMove(DirectionType direction, ClimbType climbType)
        {
            if ((CurrentMove.Direction == direction) && (CurrentMove.Climb == climbType)) return;

            Vector3 tempVector = PreviousVector;
            PreviousVector = CurrentVector;
            CurrentVector = DirectionUtilities.ConvertDirectionToVector(direction, tempVector);
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
            Anim.SetBool("ClimbMount", false);
            Anim.SetBool("ClimbDismount", false);
            Anim.SetBool("Fall", false);
            Anim.SetBool("Stand", false);
            Anim.SetFloat("Speed", ClimbSpeed.GetSpeed(ClimbType.Climbing));
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
