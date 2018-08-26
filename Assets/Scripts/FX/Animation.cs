using UnityEngine;
using System.Collections;

namespace USComics_FX {
	public class Animation : AbstractFX {
		public Animator[] Targets;
		public string[] AnimationNames;     // If multiple animation names are given, one will be randomly selected every time.

		public override IEnumerator Play() {
			if (null != AnimationNames && 0 != AnimationNames.Length && null != Targets && 0 != Targets.Length) {
				foreach (var target in Targets) {
					if (null == target) continue;
					int randomAniation = Random.Range(0, AnimationNames.Length);
					target.Play(AnimationNames[randomAniation]);
				}
			}
			yield break;
		}
		public override bool IsPlaying() {
			if (null == AnimationNames || 0 == AnimationNames.Length) return false;
			foreach (var target in Targets) {
				Animator animator = target.GetComponent<Animator>();
				if (animator.GetCurrentAnimatorStateInfo(0).length > animator.GetCurrentAnimatorStateInfo(0).normalizedTime) return true;
			}
			return false;
		}
		public override void Stop() { }
		public override AbstractFX CreateNew(GameObject parent) { return parent.AddComponent<Animation>(); }
	}
}