using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USComics_Combat;
using USComics_Entity;

namespace USComics_FX {
    public class AttackAdapter : AbstractFX {
        public Attack attack;
        public EntityController target;

        private bool Playing;
        
        public override IEnumerator Play() {
            Playing = true;
            attack.DoAttack(target);
            Playing = false;
            yield break;
        }
        public override bool IsPlaying() { return Playing;}
        public override void Stop() { attack.StopAll(); }
        public override AbstractFX CreateNew(GameObject parent) { return parent.AddComponent<AttackAdapter>(); }
    }
}
