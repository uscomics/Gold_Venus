using UnityEngine;
using System.Collections;

namespace USComics_FX {
	public class Particles : AbstractFX {
		public ParticleSystem[] ParticleSystems;

		public override IEnumerator Play() {
			if (null != ParticleSystems && 0 < ParticleSystems.Length) {
				foreach (var particleSystem in ParticleSystems) { if (null != particleSystem) particleSystem.Play(); }
			}
			yield break;
		}
		public override bool IsPlaying() {
			if (null == ParticleSystems) return false;
			foreach (var particleSystem in ParticleSystems) { if (null != particleSystem && particleSystem.isPlaying) return true; }
			return false;
			
		}
		public override void Stop() {
			if (null == ParticleSystems) return;
			foreach (var particleSystem in ParticleSystems) { if (null != particleSystem) particleSystem.Stop(); }
		}
		public override AbstractFX CreateNew(GameObject parent) { return parent.AddComponent<Particles>(); }
	}
}