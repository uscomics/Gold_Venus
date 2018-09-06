using UnityEngine;
using System.Collections;

namespace USComics_FX {
	public class PointAtCamera : AbstractFX {
		private GameObject _camera;
		private bool _playing;

		void Start() {
			_camera = GameObject.FindWithTag("MainCamera") as GameObject;
			if (PlayOnStart) StartCoroutine(Play());
		}
		public override IEnumerator Play() {
			_playing = true;         
			Transform t = GetComponent<Transform>();
			t.LookAt(_camera.transform);
			yield break;
		}
		public override bool IsPlaying() {
			return _playing;
		}
		public override void Stop() {
			_playing = false;
			Rigidbody r = GetComponent<Rigidbody>();
			r.angularVelocity = Vector3.zero;
		}
		public override AbstractFX CreateNew(GameObject parent) { return parent.AddComponent<PointAtCamera>(); }
	}
}