using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USComics_Dynamic;

namespace USComics_FX {
	public abstract class AbstractFX : MonoBehaviour {
		public string Name;
		public bool PlayOnStart;
		public bool PlayOnUpdate;
		public bool PlayOnCollision;
		public bool PlayOnTrigger;

		void Start() { if (PlayOnStart) StartCoroutine(Play()); }
		void Update() { if (PlayOnUpdate) StartCoroutine(Play()); }
		void OnCollisionEnter(Collision collision) { if (PlayOnCollision) StartCoroutine(Play()); }
		void OnTriggerEnter(Collider other) { if (PlayOnTrigger) StartCoroutine(Play()); }
		public abstract IEnumerator Play();
		public void PlayCoroutine() { StartCoroutine(Play()); }
		public abstract bool IsPlaying();
		public abstract void Stop();
		public abstract AbstractFX CreateNew(GameObject parent);
		public void DestroySelf() { Destroy(this); }
	}
}

