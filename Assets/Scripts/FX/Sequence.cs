using System.Collections;
using UnityEngine;

namespace USComics_FX {
	public class Sequence : AbstractFX {
		public float TimeBetweenFX;
		public bool Loop;
		public AbstractFX[] FX;
		
		private int SequenceCounter;
		private bool Playing;

		public override IEnumerator Play() {
			Playing = true;
			SequenceCounter = 0;
			while (Playing && FX.Length > SequenceCounter) {
				if (Playing) yield return StartCoroutine(FX[SequenceCounter].Play());
				if (Playing) yield return new WaitForSeconds(TimeBetweenFX);
				if (Playing) FX[SequenceCounter].Stop();
				SequenceCounter++;
				if (Playing && Loop && FX.Length <= SequenceCounter) SequenceCounter = 0;
			}
			Playing = false;
		}
		public override bool IsPlaying() { return Playing; }
		public override void Stop() {
			if (null == FX) {
				foreach (var fx in FX) {
					if (null != fx && fx.IsPlaying()) fx.Stop();
				}
			}
			Playing = false;
		}
		public override AbstractFX CreateNew(GameObject parent) { return parent.AddComponent<Sequence>(); }
	}
}
