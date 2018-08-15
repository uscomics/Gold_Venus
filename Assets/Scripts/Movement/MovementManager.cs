using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USComics_Debug;

namespace USComics_Movement
{
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
        public Move(DirectionType directionType, float speed) {
            Direction = directionType;
            Speed = speed;
        }
        public DirectionType Direction { get; set; }
        public float Speed { get; set; }
    }

    public class MovementManager : MonoBehaviour
    {
        public Move CurrentMove { get; set; }
        public Move PreviousMove { get; set; }
        public Vector3 CurrentVector { get; set; }
        public Vector3 PreviousVector { get; set; }

        private Animator Anim;
        private SpeedBar SpeedBarScript;
        private MovementPad MovementPadScript;
        private KeyboardManager KeyboardScript;
        private DebugConsole debugConsoleScript;

        // Use this for initialization
        void Start() {
            GameObject playerCharacter = GameObject.FindWithTag("PlayerCharacter") as GameObject;
            if (null != playerCharacter) Anim = playerCharacter.GetComponent<Animator>();
            GameObject movementPad = GameObject.FindWithTag("MovementPad") as GameObject;
            if (null != movementPad) MovementPadScript = movementPad.GetComponent<MovementPad>();
            if (null != movementPad) KeyboardScript = movementPad.GetComponent<KeyboardManager>();
            GameObject speedBarKnob = GameObject.FindWithTag("SpeedBar") as GameObject;
            if (null != speedBarKnob) SpeedBarScript = speedBarKnob.GetComponent<SpeedBar>();
            GameObject debugConsole = GameObject.FindWithTag("DebugConsole") as GameObject;
            if (null != debugConsole) debugConsoleScript = debugConsole.GetComponent<DebugConsole>();

            if (null == Anim) { Debug.LogError("MovementManager.Start: Anim is null."); }
            if (null == MovementPadScript) { Debug.LogError("MovementManager.Start: MovementPadScript is null."); }
            if (null == KeyboardScript) { Debug.LogError("MovementManager.Start: KeyboardScript is null."); }
            if (null == SpeedBarScript) { Debug.LogError("MovementManager.Start: SpeedBarScript is null."); }
            if (null == debugConsoleScript) { Debug.LogError("MovementManager.Start: debugConsoleScript is null."); }

            if (null == Anim) { return; }
            if (null == MovementPadScript) { return; }
            if (null == KeyboardScript) { return; }
            if (null == SpeedBarScript) { return; }
            if (null == debugConsoleScript) { return; }


            CurrentMove = new Move(DirectionType.None, (float)MovementType.Standing);
            CurrentVector = DirectionUtilities.ConvertDirectionToVector(DirectionType.None, Vector3.zero);
            PreviousMove = new Move(DirectionType.None, (float)MovementType.Standing);
            PreviousVector = DirectionUtilities.ConvertDirectionToVector(DirectionType.None, Vector3.zero);
        }

        // Update is called once per frame
        void Update()
        {
            DirectionType direction = GetDirection();
            float speed = SpeedBarScript.GetSpeed();
            SetMove(direction, speed);
        }

        DirectionType GetDirection()
        {
            DirectionType direction = KeyboardScript.GetDirection();
            if (DirectionType.None == direction) { direction = MovementPadScript.CurrentDirection; }
            return direction;
        }

        public void ForceStop()
        {
            BufferedDirection stop = new BufferedDirection(DirectionType.Stop);
            KeyboardScript.directionBuffer.queue.Enqueue(stop);
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
            debugConsoleScript.SetOther5("5 CurrentVector=" + CurrentVector.ToString() + "PreviousVector=" + PreviousVector.ToString());
            debugConsoleScript.SetOther6("6 CurrentDirection=" + MovementPadScript.CurrentDirection);
        }

        private void UpdateAnimation(float speed)
        {
            MovementType movementType = MovementSpeed.GetMovementType(speed);
            if (MovementType.Standing == movementType) Standing();
            else if (MovementType.Sneaking == movementType) Sneaking();
            else if (MovementType.Walking == movementType) Walking();
            else if (MovementType.Running == movementType) Running();
        }
        private void Standing()
        {
            Anim.SetBool("Sneak", false);
            Anim.SetBool("Climb", false);
            Anim.SetBool("ClimbMount", false);
            Anim.SetBool("ClimbDismount", false);
            Anim.SetBool("Fall", false);
            Anim.SetBool("Stand", true);
            Anim.SetFloat("Speed", MovementSpeed.GetSpeed(MovementType.Standing));
        }
        private void Sneaking()
        {
            Anim.SetBool("Sneak", true);
            Anim.SetBool("Climb", false);
            Anim.SetBool("ClimbMount", false);
            Anim.SetBool("ClimbDismount", false);
            Anim.SetBool("Fall", false);
            Anim.SetBool("Stand", false);
            Anim.SetFloat("Speed", MovementSpeed.GetSpeed(MovementType.Sneaking));
        }
        private void Walking()
        {
            Anim.SetBool("Sneak", false);
            Anim.SetBool("Climb", false);
            Anim.SetBool("ClimbMount", false);
            Anim.SetBool("ClimbDismount", false);
            Anim.SetBool("Fall", false);
            Anim.SetBool("Stand", false);
            Anim.SetFloat("Speed", MovementSpeed.GetSpeed(MovementType.Walking));
        }
        private void Running()
        {
            Anim.SetBool("Sneak", false);
            Anim.SetBool("Climb", false);
            Anim.SetBool("ClimbMount", false);
            Anim.SetBool("ClimbDismount", false);
            Anim.SetBool("Fall", false);
            Anim.SetBool("Stand", false);
            Anim.SetFloat("Speed", MovementSpeed.GetSpeed(MovementType.Running));
        }
    }
}
