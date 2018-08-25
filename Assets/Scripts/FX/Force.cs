using UnityEngine;
using System.Collections;
using USComics_Movement;

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

		private bool playing;

		public override IEnumerator Play() {
			playing = true;
			if (null != ForceTransform) {
				if (null != RigidbodyTargets) ApplyForce(ForceTransform, RigidbodyTargets);
				else if (null != GameObjectTargets) ApplyForce(ForceTransform, GameObjectTargets);
				else if (null != ColliderTargets) ApplyForce(ForceTransform, ColliderTargets);
				else ApplyForce(ForceTransform, TargetRadius);
			}
			playing = false;
			yield break;
		}

		public override bool IsPlaying() { return playing;}
		public override void Stop() { }
		public override AbstractFX CreateNew(GameObject parent) { return parent.AddComponent<Force>(); }
		public void ApplyForce(Transform transform, float radius) {
			if (ForceVector == Vector3.zero) return;
			Collider[] targets = Direction.GetObjectsInRadius(transform.position, radius);
			ApplyForce(transform, targets);
		}
		public void ApplyForce(Transform transform, Collider[] targets) {
			if (ForceVector == Vector3.zero) return; 
			ApplyForce(transform, Direction.GetGameObjects(targets));
		}
		public void ApplyForce(Transform transform, GameObject[] targets) {
			if (ForceVector == Vector3.zero) return; 
			ApplyForce(transform, Direction.GetRigidbodies(targets));
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
