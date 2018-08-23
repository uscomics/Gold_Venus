using UnityEngine;
using System.Collections;

namespace USComics_FX {
    public class DeactivateByTime : MonoBehaviour {
        public float Lifetime;
        public bool Activate;
        public bool FlipAndRepeat;

        private float StartTime;

        void Start() { StartTime = Time.time; }
        void Update() {
            if (Time.time >= StartTime + Lifetime) { gameObject.SetActive(Activate); }
            if (FlipAndRepeat) {
                Activate = !Activate;
                ResetTime();
            }
        }
        public void ResetTime() { StartTime = Time.time; }
        public void ResetObjectsActivateState() { gameObject.SetActive(!Activate); }
    }
}
