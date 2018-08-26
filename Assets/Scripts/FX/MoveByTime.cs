using UnityEngine;
using System.Collections;

namespace USComics_FX {
    public class MoveByTime : AbstractFX {
        public float Speed = 1.0f;
        public Vector3 Direction = Vector3.zero;

        private bool Playing;

        public override IEnumerator Play() {
            Playing = true;         
            Rigidbody r = GetComponent<Rigidbody>();
            if (Vector3.zero == Direction) Direction = transform.forward;
            r.velocity = Direction * Speed;
            yield break;
        }
        public override bool IsPlaying() {
            return Playing;
        }
        public override void Stop() {
            Playing = false;
            Rigidbody r = GetComponent<Rigidbody>();
            r.velocity = Vector3.zero * 0;
        }
        public override AbstractFX CreateNew(GameObject parent) { return parent.AddComponent<MoveByTime>(); }
    }
}
