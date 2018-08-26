using UnityEngine;
using System.Collections;
using USComics_Dynamic;

namespace USComics_FX {
	public class Spawn : AbstractFX {
		public GameObject[] Models;
		public DynamicObjectManager DynamicObjectManagerScript;
		public Vector3 Position;
		public float AngleX;
		public float AngleY;
		public float AngleZ;

		private bool Playing;

		public override IEnumerator Play() {
			if (null != Models) {
				Playing = true;
				foreach (var model in Models) { if (null != model) DynamicObjectManagerScript.Clone(model, Position, AngleX, AngleY, AngleZ); }
				Playing = false;
			}
			yield break;
		}
		public override bool IsPlaying() { return Playing; }
		public override void Stop() { }
		public override AbstractFX CreateNew(GameObject parent) { return parent.AddComponent<Spawn>(); }
	}
}