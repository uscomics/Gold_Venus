using UnityEngine;
using System.Collections;

namespace USComics_FX {
	public class Sound : AbstractFX {
		public AudioSource AudioSource;
		public AudioClip Clip;

		public override IEnumerator Play() {
			if (null != AudioSource) {
				if (null != Clip) AudioSource.PlayOneShot(Clip);
			}
			yield break;
		}
		public override bool IsPlaying() {
			if (null == AudioSource) return false;
			return AudioSource.isPlaying;
		}
		public override void Stop() { if (null != AudioSource && AudioSource.isPlaying) AudioSource.Stop(); }
		public override AbstractFX CreateNew(GameObject parent) { return parent.AddComponent<Sound>(); }
	}
}