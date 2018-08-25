﻿using UnityEngine;
using System.Collections;
using USComics_Dynamic;

namespace USComics_FX {
	public class Spawn : AbstractFX {
		public GameObject[] Models;
		public DynamicObjectManager DynamicObjectManagerScript;
		public Vector3 position;
		public float AngleX;
		public float AngleY;
		public float AngleZ;

		private bool playing;

		public override IEnumerator Play() {
			if (null != Models) {
				playing = true;
				foreach (var model in Models) { if (null != model) DynamicObjectManagerScript.Clone(model, position, AngleX, AngleY, AngleZ); }
				playing = false;
			}
			yield break;
		}
		public override bool IsPlaying() { return playing; }
		public override void Stop() { }
		public override AbstractFX CreateNew(GameObject parent) { return parent.AddComponent<Spawn>(); }
	}
}