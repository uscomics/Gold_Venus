using UnityEngine;
using System.Collections;

namespace USComics_FX {
    public class RotateGameObject : AbstractFX {
        public bool rotateX = true;
        public bool rotateY = false;
        public bool rotateZ = false;
        public float speed = 5.0f;

        private bool IsRunning;

        public override IEnumerator Play() {
            IsRunning = true;         
            Rigidbody r = GetComponent<Rigidbody>();
            float x = (rotateX) ? r.angularVelocity.x + speed : r.angularVelocity.x;
            float y = (rotateY) ? r.angularVelocity.y + speed : r.angularVelocity.y;
            float z = (rotateZ) ? r.angularVelocity.z + speed : r.angularVelocity.z;
            r.angularVelocity = new Vector3(x, y, z);
            yield break;
        }
        public override bool IsPlaying() {
            return IsRunning;
        }
        public override void Stop() {
            IsRunning = false;
            Rigidbody r = GetComponent<Rigidbody>();
            r.angularVelocity = Vector3.zero;
        }
    }
}