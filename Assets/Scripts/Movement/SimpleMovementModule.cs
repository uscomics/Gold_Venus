using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USComics_Debug;

namespace USComics_Movement
{
    public class SimpleMovementModule : AbstractMovementModule
    {
        public Move CurrentMove { get; set; }
        public Move PreviousMove { get; set; }
        public Vector3 CurrentVector { get; set; }
        public Vector3 PreviousVector { get; set; }

        private Animator Anim;
        private MovementTransitionManager MovementTransitionManagerScript;
        private GameObject speedBar;
        private SpeedBar SpeedBarScript;
        private MovementPad MovementPadScript;
        private Keyboard KeyboardScript;
        private DebugConsole debugConsoleScript;
        private bool moduleActive;

        // Use this for initialization
        void Start() {
            GameObject playerCharacter = GameObject.FindWithTag("PlayerCharacter") as GameObject;
            if (null != playerCharacter) Anim = playerCharacter.GetComponent<Animator>();
            if (null != playerCharacter) MovementTransitionManagerScript = playerCharacter.GetComponent<MovementTransitionManager>();
            GameObject movementPad = GameObject.FindWithTag("MovementPad") as GameObject;
            if (null != movementPad) MovementPadScript = movementPad.GetComponent<MovementPad>();
            if (null != movementPad) KeyboardScript = movementPad.GetComponent<Keyboard>();
            speedBar = GameObject.FindWithTag("SpeedBar") as GameObject;
            GameObject speedBarKnob = GameObject.FindWithTag("SpeedBarKnob") as GameObject;
            if (null != speedBarKnob) SpeedBarScript = speedBarKnob.GetComponent<SpeedBar>();
            GameObject debugConsole = GameObject.FindWithTag("DebugConsole") as GameObject;
            if (null != debugConsole) debugConsoleScript = debugConsole.GetComponent<DebugConsole>();

            if (null == Anim) { Debug.LogError("SimpleMovementModule.Start: Anim is null."); }
            if (null == MovementTransitionManagerScript) { Debug.LogError("SimpleMovementModule.Start: MovementTransitionManagerScript is null."); }
            if (null == MovementPadScript) { Debug.LogError("SimpleMovementModule.Start: MovementPadScript is null."); }
            if (null == KeyboardScript) { Debug.LogError("SimpleMovementModule.Start: KeyboardScript is null."); }
            if (null == speedBar) { Debug.LogError("SimpleMovementModule.Start: speedBar is null."); }
            if (null == SpeedBarScript) { Debug.LogError("SimpleMovementModule.Start: SpeedBarScript is null."); }
            if (null == debugConsoleScript) { Debug.LogError("SimpleMovementModule.Start: debugConsoleScript is null."); }

            if (null == Anim) { return; }
            if (null == MovementTransitionManagerScript) { return; }
            if (null == MovementPadScript) { return; }
            if (null == KeyboardScript) { return; }
            if (null == speedBar) { return; }
            if (null == SpeedBarScript) { return; }
            if (null == debugConsoleScript) { return; }


            CurrentMove = new Move(DirectionType.None, (float)MovementType.Standing);
            CurrentVector = DirectionUtilities.ConvertDirectionToVector(DirectionType.None, Vector3.zero);
            PreviousMove = new Move(DirectionType.None, (float)MovementType.Standing);
            PreviousVector = DirectionUtilities.ConvertDirectionToVector(DirectionType.None, Vector3.zero);
            moduleActive = false;
            MovementTransitionManagerScript.Register(this);
        }

        // Update is called once per frame
        void Update()
        {
            if (!moduleActive) return;
            DirectionType direction = GetDirection();
            float speed = SpeedBarScript.GetSpeed();
            if ((DirectionType.Stop != direction)
            && (DirectionType.None != direction)
            && (0 == speed)) {
                SpeedBarScript.SetSpeed(MovementSpeed.GetSpeed(MovementType.Walking));
                speed = SpeedBarScript.GetSpeed();
            }
            SetMove(direction, speed);
        }

        public override ModuleTypes ModuleType() { return ModuleTypes.Simple; }
        public override string StartAnimationVariable() { return "SimpleStart"; }
        public override string StopAnimationVariable() { return "SimpleStop"; }

        public override void StartModule()
        {
            ForceStop();
            moduleActive = true;
            if (!speedBar.activeSelf) speedBar.SetActive(true);
        }

        public override bool IsRunning()
        {
            return moduleActive;
        }

        public override void StopModule()
        {
            moduleActive = false;
            if (speedBar.activeSelf) speedBar.SetActive(false);
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

        private void SetMove(DirectionType direction, float speed)
        {
            if ((CurrentMove.Direction == direction) && (CurrentMove.Speed == speed)) return;

            Vector3 tempVector = PreviousVector;
            PreviousVector = CurrentVector;
            CurrentVector = DirectionUtilities.ConvertDirectionToVector(direction, tempVector);
            PreviousMove = CurrentMove;
            CurrentMove = new Move(direction, speed);
            UpdateAnimation(speed);
            debugConsoleScript.SetCurrentMove(CurrentMove);
            debugConsoleScript.SetPreviousMove(PreviousMove);
        }

        private void UpdateAnimation(float speed)
        {
            MovementType movementType = MovementSpeed.GetMovementType(speed);
            if (MovementType.Standing == movementType) Standing(speed);
            else if (MovementType.Sneaking == movementType) Sneaking(speed);
            else if (MovementType.Walking == movementType) Walking(speed);
            else if (MovementType.Running == movementType) Running(speed);
        }
        private void Standing(float speed)
        {
            Anim.SetBool("Sneak", false);
            Anim.SetBool("Climb", false);
            Anim.SetBool("Fall", false);
            Anim.SetBool("Stand", true);
            Anim.SetFloat("Speed", speed);
        }
        private void Sneaking(float speed)
        {
            Anim.SetBool("Sneak", true);
            Anim.SetBool("Climb", false);
            Anim.SetBool("Fall", false);
            Anim.SetBool("Stand", false);
            Anim.SetFloat("Speed", speed);
        }
        private void Walking(float speed)
        {
            Anim.SetBool("Sneak", false);
            Anim.SetBool("Climb", false);
            Anim.SetBool("Fall", false);
            Anim.SetBool("Stand", false);
            Anim.SetFloat("Speed", speed);
        }
        private void Running(float speed)
        {
            Anim.SetBool("Sneak", false);
            Anim.SetBool("Climb", false);
            Anim.SetBool("Fall", false);
            Anim.SetBool("Stand", false);
            Anim.SetFloat("Speed", speed);
        }
    }
    [System.Serializable]
    public enum MovementType
    {
        Sneaking,
        Walking,
        Running,
        Standing,
        None
    }

    [System.Serializable]
    public class MovementSpeed
    {
        public const float SNEAK_SPEED = 1.0f;
        public const float WALK_SPEED = 5.0f;
        public const float RUN_SPEED = 10.0f;
        public const float STANDING_SPEED = 0.0f;
        public static float GetSpeed(MovementType movementType)
        {
            if (MovementType.Sneaking == movementType) return MovementSpeed.SNEAK_SPEED;
            if (MovementType.Walking == movementType) return MovementSpeed.WALK_SPEED;
            if (MovementType.Running == movementType) return MovementSpeed.RUN_SPEED;
            if (MovementType.Standing == movementType) return MovementSpeed.STANDING_SPEED;
            return 0.0f;
        }
        public static MovementType GetMovementType(float speed)
        {
            if (MovementSpeed.SNEAK_SPEED > speed) return MovementType.Standing;
            if (MovementSpeed.WALK_SPEED > speed) return MovementType.Sneaking;
            if (MovementSpeed.RUN_SPEED > speed) return MovementType.Walking;
            return MovementType.Running;
        }
    }

    [System.Serializable]
    public class Move
    {
        public Move(DirectionType directionType, float speed)
        {
            Direction = directionType;
            Speed = speed;
        }
        public DirectionType Direction { get; set; }
        public float Speed { get; set; }
    }
}
