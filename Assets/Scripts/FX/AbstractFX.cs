using System.Collections;
using UnityEngine;

namespace USComics_FX {
	public abstract class AbstractFX : MonoBehaviour {
		public string Name;
		public bool PlayOnStart;
		public bool PlayOnUpdate;
		public bool PlayOnCollision;
		public bool StopOnCollision;
		public bool PlayOnTrigger;
		public bool StopOnTrigger;
		public bool PlayOnTriggerExit;
		public bool StopOnTriggerExit;

		void Start() { if (PlayOnStart) StartCoroutine(Play()); }
		void Update() { if (PlayOnUpdate) StartCoroutine(Play()); }
		void OnCollisionEnter(Collision collision) {
			if (PlayOnCollision) StartCoroutine(Play());
			if (StopOnCollision) Stop();
		}
		void OnTriggerEnter(Collider other) {
			if (PlayOnTrigger) StartCoroutine(Play());
			if (StopOnTrigger) Stop();
		}
		void OnTriggerExit(Collider other) {
			if (PlayOnTriggerExit) StartCoroutine(Play());
			if (StopOnTriggerExit) Stop();
		}
		public abstract IEnumerator Play();
		public void PlayCoroutine() { StartCoroutine(Play()); }
		public abstract bool IsPlaying();
		public abstract void Stop();
		public abstract AbstractFX CreateNew(GameObject parent);
		public void DestroySelf() { Destroy(this); }
	}
}

