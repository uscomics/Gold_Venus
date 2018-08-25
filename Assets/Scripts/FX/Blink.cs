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
		public bool IsBlinkRunning { get; protected set; }
		
		private bool IsBlinkOn;
		private int BlinkCounter;

		public override IEnumerator Play() {
			IsBlinkRunning = true;
			IsBlinkOn = false;
			BlinkCounter = 0;
			while (-1 == BlinkCount || BlinkCounter < BlinkCount) {
				if (!IsBlinkOn) {
					FX.Transform = Transform;
					FX.Radius = Radius;
					FX.AngleX = AngleX;
					FX.AngleY = AngleY;
					FX.AngleZ = AngleZ;
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
			IsBlinkRunning = false;
		}
		public override bool IsPlaying() { return IsBlinkRunning; }
		public override void Stop() { FX.Stop(); }
	}
}
