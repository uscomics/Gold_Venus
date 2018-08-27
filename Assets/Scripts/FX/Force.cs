using System.Collections;
using UnityEngine;
using USComics;

namespace USComics_FX {
	public class Force : AbstractTargetedFX {
		public Vector3 ForceVector = Vector3.zero;  // Vector3.zero means no force
		public ForceMode ForceMode = ForceMode.Impulse;
		public bool ForceFromCenter;        		 // Set to true for an explosion-like (radial) force, or false for a linear force.

		private bool Playing;

		public override IEnumerator Play() {
			Playing = true;
			if (null != Origin) ApplyForce();
			Playing = false;
			yield break;
		}

		public override bool IsPlaying() { return Playing;}
		public override void Stop() { Playing = false; }
		public override AbstractFX CreateNew(GameObject parent) { return parent.AddComponent<Force>(); }
		protected void ApplyForce() {
			if (ForceVector == Vector3.zero) return;
			GameObject[] targets = GetTargets();
			Rigidbody[] rigidTargets = GameObjectUtilities.GetRigidbodies(targets);
			foreach (var target in rigidTargets) {
				if (ForceFromCenter) target.AddForceAtPosition(ForceVector, transform.position, ForceMode);
				else target.AddForce(ForceVector, ForceMode);
			}
		}
	}
}
