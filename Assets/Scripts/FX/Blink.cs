using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using JetBrains.Annotations;
using UnityEngine;

namespace USComics_FX {
	public class Blink : AbstractFX {
		public float BlinkTimeOn;
		public float BlinkTimeOff;
		public int BlinkCount;		// -1 to blink forever.
		public AbstractFX FX;
		
		private bool IsBlinkOn;
		private int BlinkCounter;
		private bool Playing;

		public override IEnumerator Play() {
			Playing = true;
			IsBlinkOn = false;
			BlinkCounter = 0;
			while (-1 == BlinkCount || BlinkCounter < BlinkCount) {
				if (!IsBlinkOn) {
					IsBlinkOn = true;
					yield return StartCoroutine(FX.Play());
					yield return new WaitForSeconds(BlinkTimeOn);
				} else {
					FX.Stop();
					BlinkCounter++;
					IsBlinkOn = false;
					yield return new WaitForSeconds(BlinkTimeOff);
				}
			}
			Playing = false;
		}
		public override bool IsPlaying() { return Playing; }
		public override void Stop() { FX.Stop(); Playing = false; }
		public override AbstractFX CreateNew(GameObject parent) { return parent.AddComponent<Blink>(); }
	}
}
