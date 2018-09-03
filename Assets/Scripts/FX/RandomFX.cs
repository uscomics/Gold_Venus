using System.Collections;
using UnityEngine;

namespace USComics_FX {
	public class RandomFX : AbstractFX {

		public AbstractFX[] FXArray;

		protected int _selectedFX = -1;

		public override IEnumerator Play() {
			if (null != FXArray) {
				_selectedFX = SelectRandomFX();
				if ((FXArray.Length > _selectedFX)
				&& (null != FXArray[_selectedFX])) {
					if (IsPlaying()) Stop();
					yield return StartCoroutine(FXArray[_selectedFX].Play());
				} else { _selectedFX = -1; }
			}
		}

		public override bool IsPlaying() { return -1 != _selectedFX && _selectedFX < FXArray.Length && FXArray[_selectedFX].IsPlaying(); }
		public override void Stop() { if (-1 != _selectedFX && _selectedFX < FXArray.Length) FXArray[_selectedFX].Stop(); }
		public override AbstractFX CreateNew(GameObject parent) { return parent.AddComponent<RandomFX>(); }
		
		protected virtual int SelectRandomFX() { return Random.Range(0, FXArray.Length); }
	}
}
