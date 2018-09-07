using UnityEditor.Build;
using UnityEngine;
using USComics_Entity;

namespace USComics_Movement {
	public class FallMovementModule : AbstractMovementModule {
		public float MaximumFallTime = 5.0f;
		
		private bool _moduleActive;
		private FallPad _fallPadScript;
		private Animator _anim;
		private MovementTransitionManager _movementTransitionManagerScript;
		private PlayerController _playerControllerScript;
		private Vector3 _fallStartPosition;
		private float _fallStartTime;

		void Start() {
			GameObject playerCharacter = GameObject.FindWithTag("PlayerCharacter") as GameObject;
			if (null != playerCharacter) _anim = playerCharacter.GetComponent<Animator>();
			if (null != playerCharacter) _movementTransitionManagerScript = playerCharacter.GetComponent<MovementTransitionManager>();
			if (null != playerCharacter) _playerControllerScript = playerCharacter.GetComponent<PlayerController>();
			GameObject fallingPanel = GameObject.FindWithTag("Falling Panel") as GameObject;
			if (null != fallingPanel) _fallPadScript = fallingPanel.GetComponent<FallPad>();
			
			if (null == _anim) { Debug.LogError("FallMovementModule.Start: _anim is null."); }
			if (null == _movementTransitionManagerScript) { Debug.LogError("FallMovementModule.Start: _movementTransitionManagerScript is null."); }
			if (null == _playerControllerScript) { Debug.LogError("FallMovementModule.Start: _entityControllerScript is null."); }
			if (null == _fallPadScript) { Debug.LogError("FallMovementModule.Start: _fallPadScript is null."); }

			if (null == _anim) { return; }
			if (null == _movementTransitionManagerScript) { return; }
			if (null == _playerControllerScript) { return; }
			if (null == _fallPadScript) { return; }
			
			_movementTransitionManagerScript.Register(this);
		}

		void Update() {
			if (!IsRunning()) return;
			_anim.Play("Falling_Loop");
			if (Time.time -_fallStartTime < MaximumFallTime) return;
			_playerControllerScript.DoDeath(null);
		}

		public override ModuleTypes ModuleType() { return ModuleTypes.Falling; }
		public override string GetTransitionToAnimationName() { return "Falling_Loop"; }
		public override string GetTransitionFromAnimationName() { return "FlightFallLanding"; }
		public override void StartModule() {
			if (IsRunning()) return;
			_moduleActive = true;
			_fallPadScript.ShowFallingUI();
			_fallStartPosition = _playerControllerScript.Entity.transform.position;
			_fallStartTime = Time.time;
		}
		public override void StopModule() {
			_moduleActive = false;
			_fallPadScript.HideFallingUI();
		}
		public override bool IsRunning() { return _moduleActive; }
		public Vector3 GetFallStartPosition() { return _fallStartPosition; }
		public float GetFallStartTime() { return _fallStartTime; }
	}
}
