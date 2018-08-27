using UnityEngine;
using System.Collections;

namespace USComics_FX {
	public class Animations : AbstractFX {
		public Animator AnimationEntity;
		public string[] AnimationNames;     // If multiple animation names are given, one will be randomly selected every time.

		public override IEnumerator Play() {
			if (null != AnimationNames && 0 != AnimationNames.Length && null != AnimationEntity) {
				int randomAniation = Random.Range(0, AnimationNames.Length);
				AnimationEntity.Play(AnimationNames[randomAniation]);
			}
			yield break;
		}
		public override bool IsPlaying() {
			if (null == AnimationNames || 0 == AnimationNames.Length) return false;
			Animator animator = AnimationEntity.GetComponent<Animator>();
			if (animator.GetCurrentAnimatorStateInfo(0).length > animator.GetCurrentAnimatorStateInfo(0).normalizedTime) return true;
			return false;
		}
		public override void Stop() { }
		public override AbstractFX CreateNew(GameObject parent) { return parent.AddComponent<Animations>(); }
	}
}