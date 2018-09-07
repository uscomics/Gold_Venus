using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace USComics_Movement {
	public class FallMovementModule : AbstractMovementModule {
		private bool _moduleActive;
		private FallPad _fallPadScript;
		private Animator _anim;
		private MovementTransitionManager _movementTransitionManagerScript;
		private float _fallStartLocation;

		void Start() {
			GameObject playerCharacter = GameObject.FindWithTag("PlayerCharacter") as GameObject;
			if (null != playerCharacter) _anim = playerCharacter.GetComponent<Animator>();
			if (null != playerCharacter) _movementTransitionManagerScript = playerCharacter.GetComponent<MovementTransitionManager>();
			GameObject fallingPanel = GameObject.FindWithTag("Falling Panel") as GameObject;
			if (null != fallingPanel) _fallPadScript = fallingPanel.GetComponent<FallPad>();
			
			if (null == _anim) { Debug.LogError("FallMovementModule.Start: _anim is null."); }
			if (null == _movementTransitionManagerScript) { Debug.LogError("FallMovementModule.Start: _movementTransitionManagerScript is null."); }
			if (null == _fallPadScript) { Debug.LogError("FallMovementModule.Start: _fallPadScript is null."); }

			if (null == _anim) { return; }
			if (null == _movementTransitionManagerScript) { return; }
			if (null == _fallPadScript) { return; }
			
			_movementTransitionManagerScript.Register(this);
		}

		void Update() {
			if (IsRunning()) _anim.Play("Falling_Loop");
		}

		public override ModuleTypes ModuleType() { return ModuleTypes.Falling; }
		public override string GetTransitionToAnimationName() { return "Falling_Loop"; }
		public override string GetTransitionFromAnimationName() { return "FlightFallLanding"; }
		public override void StartModule() {
			_moduleActive = true;
			_fallPadScript.ShowFallingUI();
		}
		public override void StopModule() {
			_moduleActive = false;
			_fallPadScript.HideFallingUI();
		}
		public override bool IsRunning() { return _moduleActive; }
		public float GetFallStartLocation() { return _fallStartLocation; }
	}
}
