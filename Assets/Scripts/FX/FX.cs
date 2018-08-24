using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using USComics_Dynamic;
using USComics_Movement;

namespace USComics_FX {
    public class FX : MonoBehaviour {
        public string FXName;
        public string[] AnimationNames;     // If multiple animation names are given, one will be randomly selected every time.
        public AudioSource AudioSource1;    // Plays Sound1
        public AudioSource AudioSource2;    // Plays Sound2
        public AudioClip Sound1;
        public AudioClip Sound2;
        public ParticleSystem[] ParticleSystems;    // All particle systems are played
        public Light[] Lights;              // All lights are used.
        public GameObject[] Spawn;
        public bool SpawnFirst;             // true = PlayAll spawns objects first and are possible force targets, false = PlayAll spawns objects last
        public DynamicObjectManager DynamicObjectManagerScript;
        public Vector3 ForceVector = Vector3.zero;  // Vector3.zero means no force
        public ForceMode ForceMode = ForceMode.Impulse;
        public bool ForceFromCenter;        // Set to true for an explosion-like (radial) force, or false for a linear force.

        void Start() { }
        void Update() { }
        public Collider[] GetTargets(Transform transform, float radius) { return DirectionUtilities.GetObjectsInRadius(transform.position, radius); }
        public void PlayAll(Transform transform, float radius, float angleX = 0.0f, float angleY = 180.0f, float angleZ = 0.0f) {
            if (SpawnFirst) SpawnModels(transform.position, angleX, angleY, angleZ);
            Collider[] targets = GetTargets(transform, radius);
            PlaySounds();
            PlayAnimation(targets);
            PlayParticleSystems();
            PlayLights();
            ApplyForce(transform, targets);
            if (!SpawnFirst) SpawnModels(transform.position, angleX, angleY, angleZ);
        }
        public void StopAll() {
            StopSounds();
            StopParticleSystems();
            StopLights();
        }
        public void PlaySounds() {
            if (null == AudioSource1) return;
            if (null == Sound1) return;
            AudioSource1.PlayOneShot(Sound1);
            if (null == Sound2) return;
            if (null == AudioSource2) {
                Debug.LogError("Need two audio sources to play two audio clips.");
                return;
            }
            AudioSource2.PlayOneShot(Sound2);
        }
        public void StopSounds() {
            if (null != AudioSource1 && AudioSource1.isPlaying) AudioSource1.Stop();
            if (null != AudioSource2 && AudioSource2.isPlaying) AudioSource2.Stop();
        }
        public void PlayAnimation(Collider[] targets) {
            if (null == AnimationNames || 0 == AnimationNames.Length) return;
            foreach (var target in targets) {
                if (null == target) continue;
                Animator animator = target.GetComponent<Animator>();
                if (null == animator) continue;
                int randomAniation = Random.Range(0, AnimationNames.Length);
                animator.Play(AnimationNames[randomAniation]);
            }
        }

        public void PlayParticleSystems() {
            if (null == ParticleSystems) return;
            foreach (var particleSystem in ParticleSystems) { if (null != particleSystem) particleSystem.Play(); }
        }

        public void StopParticleSystems() {
            if (null == ParticleSystems) return;
            foreach (var particleSystem in ParticleSystems) { if (null != particleSystem) particleSystem.Stop(); }
        }

        public void PlayLights() {
            if (null == Lights) return;
            foreach (var light in Lights) { if (null != light) light.enabled = true; }
        }

        public void StopLights() {
            if (null == Lights) return;
            foreach (var light in Lights) { if (null != light) light.enabled = false; }
        }
        public void SpawnModels(Vector3 position, float angleX = 0.0f, float angleY = 180.0f, float angleZ = 0.0f) {
            if (null == Spawn) return;
            foreach (var model in Spawn) { if (null != model) DynamicObjectManagerScript.Clone(model, position, angleX, angleY, angleZ); }            
        }
        public void ApplyForce(Transform transform, float radius) {
            if (ForceVector == Vector3.zero) return;
            Collider[] targets = DirectionUtilities.GetObjectsInRadius(transform.position, radius);
            ApplyForce(transform, targets);
        }
        public void ApplyForce(Transform transform, Collider[] targets) {
            if (ForceVector == Vector3.zero) return; 
            ApplyForce(transform, DirectionUtilities.GetGameObjects(targets));
        }
        public void ApplyForce(Transform transform, GameObject[] targets) {
            if (ForceVector == Vector3.zero) return; 
            ApplyForce(transform, DirectionUtilities.GetRigidbodies(targets));
        }
        public void ApplyForce(Transform transform, Rigidbody[] targets) {
            if (ForceVector == Vector3.zero) return;
            foreach (var target in targets) {
                if (ForceFromCenter) target.AddForceAtPosition(ForceVector, transform.position, ForceMode);
                else target.AddForce(ForceVector, ForceMode);
            }
        }
    }
}
