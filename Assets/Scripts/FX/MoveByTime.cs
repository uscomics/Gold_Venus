using UnityEngine;
using System.Collections;

namespace USComics_FX {
    public class MoveByTime : AbstractFX {
        public float speed = 1.0f;
        public Vector3 direction = Vector3.zero;

        private bool IsRunning;

        public override IEnumerator Play() {
            IsRunning = true;         
            Rigidbody r = GetComponent<Rigidbody>();
            if (Vector3.zero == direction) direction = transform.forward;
            r.velocity = direction * speed;
            yield break;
        }
        public override bool IsPlaying() {
            return IsRunning;
        }
        public override void Stop() {
            IsRunning = false;
            Rigidbody r = GetComponent<Rigidbody>();
            r.velocity = Vector3.zero * 0;
        }
    }
}
