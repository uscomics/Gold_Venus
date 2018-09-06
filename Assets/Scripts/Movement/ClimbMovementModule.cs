using UnityEngine;

namespace USComics_Movement {
    public class ClimbMovementModule : AbstractMovementModule {
        public ClimbMove CurrentMove { get; set; }
        public ClimbMove PreviousMove { get; set; }
        public Vector3 CurrentVector { get; set; }
        public Vector3 PreviousVector { get; set; }
        public float Speed { get; set; }

        private Rigidbody _rigidBody;
        private Animator _anim;
        private MovementTransitionManager _movementTransitionManagerScript;
        private ClimbPad _climbPadScript;
        private GameObject _climbingPanel;
        private Keyboard _keyboardScript;
        private bool _moduleActive;

        // Use this for initialization
        void Start() {
            GameObject playerCharacter = GameObject.FindWithTag("PlayerCharacter") as GameObject;
            if (null != playerCharacter) _anim = playerCharacter.GetComponent<Animator>();
            if (null != playerCharacter) _movementTransitionManagerScript = playerCharacter.GetComponent<MovementTransitionManager>();
            if (null != playerCharacter) _rigidBody = playerCharacter.GetComponent<Rigidbody>();
            GameObject climbPad = GameObject.FindWithTag("ClimbPad") as GameObject;
            if (null != climbPad) _climbPadScript = climbPad.GetComponent<ClimbPad>();
            if (null != climbPad) _keyboardScript = climbPad.GetComponent<Keyboard>();
            _climbingPanel = GameObject.FindWithTag("ClimbingPanel") as GameObject;

            if (null == _anim) { Debug.LogError("ClimbMovementModule.Start: _anim is null."); }
            if (null == _movementTransitionManagerScript) { Debug.LogError("ClimbMovementModule.Start: _movementTransitionManagerScript is null."); }
            if (null == _rigidBody) { Debug.LogError("ClimbMovementModule.Start: _rigidBody is null."); }
            if (null == _climbPadScript) { Debug.LogError("ClimbMovementModule.Start: _climbPadScript is null."); }
            if (null == _climbingPanel) { Debug.LogError("ClimbMovementModule.Start: _climbingPanel is null."); }
            if (null == _keyboardScript) { Debug.LogError("ClimbMovementModule.Start: _keyboardScript is null."); }

            if (null == _anim) { return; }
            if (null == _movementTransitionManagerScript) { return; }
            if (null == _rigidBody) { return; }
            if (null == _climbPadScript) { return; }
            if (null == _climbingPanel) { return; }
            if (null == _keyboardScript) { return; }

            CurrentMove = new ClimbMove(DirectionType.None, ClimbType.None);
            CurrentVector = ConvertForClimbing(Direction.ConvertDirectionToVector(DirectionType.None, Vector3.zero));
            PreviousMove = new ClimbMove(DirectionType.None, ClimbType.None);
            PreviousVector = ConvertForClimbing(Direction.ConvertDirectionToVector(DirectionType.None, Vector3.zero));
            Speed = 0.0f;
            _moduleActive = false;
            _movementTransitionManagerScript.Register(this);
            _climbPadScript.HideClimbingUI();
        }

        // Update is called once per frame
        void Update() {
            if (!_moduleActive) return;
            DirectionType direction = GetDirection();
            if ((DirectionType.Stop != direction)
            && (DirectionType.North != direction) 
            && (DirectionType.South != direction)
            && (DirectionType.None != direction)) {
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

        public override void StartModule() {
            _moduleActive = true;
            _rigidBody.useGravity = false;
            _climbPadScript.ShowClimbingUI();
            ForceStop();
        }
        public override bool IsRunning() { return _moduleActive; }
        public override void StopModule() {
            _moduleActive = false;
            _rigidBody.useGravity = true;
            _climbPadScript.HideClimbingUI();
        }
        public void ForceStop() {
            BufferedDirection stop = new BufferedDirection(DirectionType.Stop);
            _keyboardScript.directionBuffer.queue.Enqueue(stop);
        }
        public void TransitionOutOfClimb() { _movementTransitionManagerScript.StartTransitionFrom(ModuleTypes.Climbing, ModuleTypes.Simple); }
        
        private DirectionType GetDirection() {
            DirectionType direction = _climbPadScript.CurrentDirection;
            return direction;
        }
        private Vector3 ConvertForClimbing(Vector3 vector) {
            if (Vector3.forward == vector) return Vector3.up;
            if (-Vector3.forward == vector) return -Vector3.up;
            if (-Vector3.zero == vector) return Vector3.zero;
            return Vector3.zero;
        }
        private ClimbType GetClimbType()  { return ClimbType.Climbing; }
        private void SetMove(DirectionType direction, ClimbType climbType) {
            if ((CurrentMove.Direction == direction) && (CurrentMove.Climb == climbType)) return;

            Vector3 tempVector = PreviousVector;
            PreviousVector = CurrentVector;
            CurrentVector = ConvertForClimbing(Direction.ConvertDirectionToVector(direction, tempVector));
            PreviousMove = CurrentMove;
            CurrentMove = new ClimbMove(direction, climbType);
            UpdateAnimation(climbType);
        }
        private void UpdateAnimation(ClimbType climbType) {
            if (ClimbType.Climbing == climbType) Climbing();
            else if (ClimbType.Idle == climbType) Idle();
        }
        private void Climbing() { _anim.Play("LadderClimb"); }
        private void Idle() { _anim.Play("EdgeGrab_Idle"); }
    }
}
