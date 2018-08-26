using UnityEngine;
using System.Collections;

namespace USComics_FX {
    public class RotateGameObject : AbstractFX {
        public bool RotateX = true;
        public bool RotateY = false;
        public bool RotateZ = false;
        public float Speed = 5.0f;

        private bool IsRunning;

        public override IEnumerator Play() {
            IsRunning = true;         
            Rigidbody r = GetComponent<Rigidbody>();
            float x = (RotateX) ? r.angularVelocity.x + Speed : r.angularVelocity.x;
            float y = (RotateY) ? r.angularVelocity.y + Speed : r.angularVelocity.y;
            float z = (RotateZ) ? r.angularVelocity.z + Speed : r.angularVelocity.z;
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
        public override AbstractFX CreateNew(GameObject parent) { return parent.AddComponent<RotateGameObject>(); }
    }
}