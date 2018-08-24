using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace USComics_FX {
	public class Blink : MonoBehaviour {
		public float BlinkTimeOn;
		public float BlinkTimeOff;
		public int BlinkCount;		// -1 to blink forever.
		public FX FX;
		public Transform Transform;
		public float Radius;
		public float AngleX = 0.0f;
		public float AngleY = 180.0f;
		public float AngleZ = 0.0f;
		public bool IsBlinkRunning { get; protected set; }
		
		private bool IsBlinkOn;
		private int BlinkCounter;
		void Start() { }

		public IEnumerator PlayAll() {
			IsBlinkRunning = true;
			IsBlinkOn = false;
			BlinkCounter = 0;
			while (-1 == BlinkCount || BlinkCounter < BlinkCount) {
				if (!IsBlinkOn) {
					FX.PlayAll(Transform, Radius, AngleX, AngleY, AngleZ);
					IsBlinkOn = true;
					yield return new WaitForSeconds(BlinkTimeOn);
				} else {
					FX.StopAll();
					BlinkCounter++;
					IsBlinkOn = false;
					yield return new WaitForSeconds(BlinkTimeOff);
				}
			}
			IsBlinkRunning = false;
		}
		void OnTriggerEnter(Collider other) { Debug.Log("BOMB!"); StartCoroutine(PlayAll()); }
	}
}
