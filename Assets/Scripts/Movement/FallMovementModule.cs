using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace USComics_Movement {
	public class FallMovementModule : AbstractMovementModule {
		private bool _moduleActive;
		private FallPad _fallPadScript;

		void Start() {
			GameObject fallingPanel = GameObject.FindWithTag("FallingPanel") as GameObject;
			if (null != fallingPanel) _fallPadScript = fallingPanel.GetComponent<FallPad>();
			
			if (null == _fallPadScript) { Debug.LogError("FallMovementModule.Start: _fallPadScript is null."); }

			if (null == _fallPadScript) { return; }
		}
		void Update() { }

		public override ModuleTypes ModuleType() { return ModuleTypes.Falling; }
		public override string GetTransitionToAnimationName() { return "ClimbStart"; }
		public override string GetTransitionFromAnimationName() { return "ClimbStart"; }
		public override void StartModule() {
			_moduleActive = true;
			_fallPadScript.ShowFallingUI();
		}
		public override void StopModule() {
			_moduleActive = false;
			_fallPadScript.HideFallingUI();
		}
		public override bool IsRunning() { return _moduleActive; }
	}
}
