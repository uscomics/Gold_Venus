﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USComics_Debug;

namespace USComics_Movement
{
    public class SimpleMovementModule : AbstractMovementModule {
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

            if (null == Anim) { Debug.LogError("SimpleMovementModule.Start: Anim is null."); }
            if (null == MovementTransitionManagerScript) { Debug.LogError("SimpleMovementModule.Start: MovementTransitionManagerScript is null."); }
            if (null == MovementPadScript) { Debug.LogError("SimpleMovementModule.Start: MovementPadScript is null."); }
            if (null == KeyboardScript) { Debug.LogError("SimpleMovementModule.Start: KeyboardScript is null."); }
            if (null == speedBar) { Debug.LogError("SimpleMovementModule.Start: speedBar is null."); }
            if (null == SpeedBarScript) { Debug.LogError("SimpleMovementModule.Start: SpeedBarScript is null."); }

            if (null == Anim) { return; }
            if (null == MovementTransitionManagerScript) { return; }
            if (null == MovementPadScript) { return; }
            if (null == KeyboardScript) { return; }
            if (null == speedBar) { return; }
            if (null == SpeedBarScript) { return; }


            CurrentMove = new Move(DirectionType.None, (float)MovementType.Standing);
            CurrentVector = Direction.ConvertDirectionToVector(DirectionType.None, Vector3.zero);
            PreviousMove = new Move(DirectionType.None, (float)MovementType.Standing);
            PreviousVector = Direction.ConvertDirectionToVector(DirectionType.None, Vector3.zero);
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
        public override string GetTransitionToAnimationName() { return "Idle 0"; }
        public override string GetTransitionFromAnimationName() { return "Walk 0"; }

        public override void StartModule()
        {
            ForceStop();
            moduleActive = true;
            MovementPadScript.ShowMovementUI();
        }

        public override bool IsRunning()
        {
            return moduleActive;
        }

        public override void StopModule()
        {
            moduleActive = false;
            MovementPadScript.HideMovementUI();
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
            CurrentVector = Direction.ConvertDirectionToVector(direction, tempVector);
            PreviousMove = CurrentMove;
            CurrentMove = new Move(direction, speed);
            UpdateAnimation(speed);
            DebugConsole.INSTANCE.SetCurrentMove(CurrentMove);
            DebugConsole.INSTANCE.SetPreviousMove(PreviousMove);
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
            Anim.Play("Idle");
        }
        private void Sneaking(float speed)
        {
            Anim.Play("Crouch_Walk");
        }
        private void Walking(float speed)
        {
            Anim.Play("Walk");
        }
        private void Running(float speed)
        {
            Anim.Play("Run");
        }
    }
}
