using System.Collections;
using UnityEngine;

namespace USComics_FX {
	public class ChanceFX : AbstractFX {
		public AbstractFX FX;
		[Range(0, 100)] public int Chance;
		
		public override IEnumerator Play() {
			if ((null != FX) 
			&& (Random.Range(0, 100) < Chance)) {
				yield return StartCoroutine(FX.Play());
			}
		}

		public override bool IsPlaying() { return null != FX && FX.IsPlaying(); }
		public override void Stop() { if (null != FX) FX.Stop(); }
		public override AbstractFX CreateNew(GameObject parent) { return parent.AddComponent<ChanceFX>(); }
	}
}
