using UnityEngine;
using USComics;
using USComics_FX;

namespace USComics_Entity {
	public class SpawnPoint : MonoBehaviour {
		public bool PlayerVisited;
		public AbstractFX[] SpawnEffects;

		public void PlaySpawnFX() {
			if (null != SpawnEffects) { foreach (var fx in SpawnEffects) fx.PlayCoroutine(); }
		}
#if UNITY_EDITOR
		public void EditorGizmo(Transform inTransform) {
			UnityEditor.Handles.color = Color.yellow;
			UnityEditor.Handles.DrawSolidDisc(inTransform.position, inTransform.up, 0.5f);
		}

		protected void OnDrawGizmosSelected() { EditorGizmo(transform); }
#endif
		protected virtual void OnTriggerEnter(Collider other) {
			GameObject go = GameObjectUtilities.GetGameObject(other);
			PlayerController player = go.GetComponent<PlayerController>();
			if (null == player) return;
			PlayerVisited = true;
		}
	}
}
