using System.Collections;
using UnityEngine;

namespace USComics_FX {
    public abstract class TargetedAnimation : AbstractTargetedFX {
        public string[] AnimationNames;     // If multiple animation names are given, one will be randomly selected every time.

        public override IEnumerator Play() {
            GameObject[] targets = GetTargets();
            if (null != AnimationNames && 0 != AnimationNames.Length && null != targets && 0 != targets.Length) {
                foreach (var target in targets) {
                    if (null == target) continue;
                    int randomAniation = Random.Range(0, AnimationNames.Length);
                    Animator animator = target.GetComponent<Animator>();
                    if (null == animator) continue;
                    animator.Play(AnimationNames[randomAniation]);
                }
            }
            yield break;
        }
        public override bool IsPlaying() {
            GameObject[] targets = GetTargets();
            if (null == AnimationNames || 0 == AnimationNames.Length) return false;
            foreach (var target in targets) {
                Animator animator = target.GetComponent<Animator>();
                if (animator.GetCurrentAnimatorStateInfo(0).length > animator.GetCurrentAnimatorStateInfo(0).normalizedTime) return true;
            }
            return false;
        }
        public override void Stop() { }
        public override AbstractFX CreateNew(GameObject parent) { return parent.AddComponent<TargetedAnimation>(); }
    }
}
