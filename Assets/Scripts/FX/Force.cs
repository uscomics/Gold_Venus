using UnityEngine;
using System.Collections;
using USComics_Movement;
using USComics_Environment;

namespace USComics_FX {
	public class Force : AbstractFX {
		public Vector3 ForceVector = Vector3.zero;  // Vector3.zero means no force
		public ForceMode ForceMode = ForceMode.Impulse;
		public bool ForceFromCenter;        // Set to true for an explosion-like (radial) force, or false for a linear force.
		public Transform ForceTransform;    // Works with TargetRadius, ColliderTargets, GameObjectTargets, or RigidbodyTargets.
		public float TargetRadius;
		public Collider[] ColliderTargets;
		public GameObject[] GameObjectTargets;
		public Rigidbody[] RigidbodyTargets;

		private bool Playing;

		public override IEnumerator Play() {
			Playing = true;
			if (null != ForceTransform) {
				if (null != RigidbodyTargets && 0 < RigidbodyTargets.Length) { ApplyForce(ForceTransform, RigidbodyTargets); }
				else if (null != GameObjectTargets && 0 < GameObjectTargets.Length) { ApplyForce(ForceTransform, GameObjectTargets); }
				else if (null != ColliderTargets && 0 < ColliderTargets.Length) { ApplyForce(ForceTransform, ColliderTargets); }
				else { ApplyForce(ForceTransform, TargetRadius); }
			}
			Playing = false;
			yield break;
		}

		public override bool IsPlaying() { return Playing;}
		public override void Stop() { Playing = false; }
		public override AbstractFX CreateNew(GameObject parent) { return parent.AddComponent<Force>(); }
		public void ApplyForce(Transform transform, float radius) {
			if (ForceVector == Vector3.zero) return;
			Collider[] targets = Environment.GetObjectsInRadius(transform.position, radius);
			ApplyForce(transform, targets);
		}
		public void ApplyForce(Transform transform, Collider[] targets) {
			if (ForceVector == Vector3.zero) return; 
			ApplyForce(transform, GameObjectUtilities.GetGameObjects(targets));
		}
		public void ApplyForce(Transform transform, GameObject[] targets) {
			if (ForceVector == Vector3.zero) return; 
			ApplyForce(transform, GameObjectUtilities.GetRigidbodies(targets));
		}
		public void ApplyForce(Transform transform, Rigidbody[] targets) {
			if (ForceVector == Vector3.zero) return;
			foreach (var target in targets) {
				if (ForceFromCenter) target.AddForceAtPosition(ForceVector, transform.position, ForceMode);
				else target.AddForce(ForceVector, ForceMode);
			}
		}
	}
}
