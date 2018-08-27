using System.Collections;
using UnityEngine;
using USComics;
using USComics_Combat;
using USComics_Entity;
using USComics_Environment;

namespace USComics_FX {
    public class AttackAdapter : AbstractTargetedFX {
        public GameObject Attacker;
        public Attack Attack;

        private bool Playing;
      
        public override IEnumerator Play() {
            GameObject[] targets = GetTargets();
            if (null != targets) {
                Playing = true;
                foreach (var target in targets) {
                    if (null != target) {
                        EntityController targetEntityController = target.GetComponent<EntityController>();
                        Attack.entity = Attacker;
                        if (null != targetEntityController) Attack.DoAttack(targetEntityController);
                    }
                }
                Playing = false;
            }
            yield break;
        }
        public override bool IsPlaying() { return Playing;}
        public override void Stop() { Attack.StopAll(); Playing = false; }
        public override AbstractFX CreateNew(GameObject parent) { return parent.AddComponent<AttackAdapter>(); }
    }
}
