using UnityEngine;
using System.Collections;

namespace USComics_FX {
    public class DeactivateByTime : AbstractFX {
        public float Lifetime;
        public bool Activate;
        public bool FlipAndRepeat;

        private float StartTime;
        private bool IsRunning;

        void Start() {
            StartTime = Time.time;
            if (PlayOnStart) StartCoroutine(Play());
        }
        public override IEnumerator Play() {
            IsRunning = true;         
            if (Time.time >= StartTime + Lifetime) { gameObject.SetActive(Activate); }
            if (FlipAndRepeat) {
                Activate = !Activate;
                ResetTime();
            }
            yield break;
        }
        public override bool IsPlaying() {
            return IsRunning;
        }
        public override void Stop() {
            IsRunning = false;
        }
        public void ResetTime() { StartTime = Time.time; }
        public void ResetObjectsActivateState() { gameObject.SetActive(!Activate); }
    }
}
