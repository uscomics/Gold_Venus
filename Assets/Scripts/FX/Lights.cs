using UnityEngine;
using System.Collections;

namespace USComics_FX {
	public class Lights : AbstractFX {
		public Light[] Light;

		void Start() {
			Stop();
			if (PlayOnStart) StartCoroutine(Play());
		}
		public override IEnumerator Play() {
			if (null != Light) {
				foreach (var l in Light) { if (null != l) l.enabled = true; }
			}
			yield break;
		}
		public override bool IsPlaying() {
			if (null == Light) return false;
			foreach (var light in Light) { if (null != light && light.enabled) return true; }
			return false;
		}
		public override void Stop() {            
			if (null == Light) return;
			foreach (var light in Light) { if (null != light) light.enabled = false; }
		}
		public override AbstractFX CreateNew(GameObject parent) { return parent.AddComponent<Lights>(); }
	}
}