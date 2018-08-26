using System.Collections;
using UnityEngine;
using USComics_Dynamic;
using USComics_Movement;
using USComics_Environment;

namespace USComics_FX {
    public class FX : AbstractFX {
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
        public Transform Transform;
        public float Radius;
        public float AngleX = 0.0f;
        public float AngleY = 180.0f;
        public float AngleZ = 0.0f;
        
        public Collider[] GetTargets(Transform transform, float radius) { return Environment.GetObjectsInRadius(transform.position, radius); }
        public override IEnumerator Play() {
            if (SpawnFirst) SpawnModels(transform.position, AngleX, AngleY, AngleZ);
            Collider[] targets = GetTargets(Transform, Radius);
            PlaySounds();
            PlayAnimation(targets);
            PlayParticleSystems();
            PlayLights();
            ApplyForce(transform, targets);
            if (!SpawnFirst) SpawnModels(Transform.position, AngleX, AngleY, AngleZ);
            yield break;
        }
        public override bool IsPlaying() {
            Collider[] targets = GetTargets(Transform, Radius);
            return IsSoundPlaying() || IsAnimationPlaying(targets) || AreParticleSystemsPlaying();
        }
        public override void Stop() {
            StopSounds();
            StopParticleSystems();
            StopLights();
        }
        public override AbstractFX CreateNew(GameObject parent) { return parent.AddComponent<FX>(); }
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
        public bool IsSoundPlaying() {
            bool result = false;
            if (null != AudioSource1) result = AudioSource1.isPlaying;
            if (null != AudioSource2) result |= AudioSource2.isPlaying;
            return result;
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
        public bool IsAnimationPlaying(Collider[] targets) {
            if (null == AnimationNames || 0 == AnimationNames.Length) return false;
            foreach (var target in targets) {
                Animator animator = target.GetComponent<Animator>();
                if (animator.GetCurrentAnimatorStateInfo(0).length > animator.GetCurrentAnimatorStateInfo(0).normalizedTime) return true;
            }
            return false;
        }
        public void PlayParticleSystems() {
            if (null == ParticleSystems) return;
            foreach (var particleSystem in ParticleSystems) { if (null != particleSystem) particleSystem.Play(); }
        }
        public bool AreParticleSystemsPlaying() {
            if (null == ParticleSystems) return false;
            foreach (var particleSystem in ParticleSystems) { if (null != particleSystem && particleSystem.isPlaying) return true; }
            return false;
        }
        public void StopParticleSystems() {
            if (null == ParticleSystems) return;
            foreach (var particleSystem in ParticleSystems) { if (null != particleSystem) particleSystem.Stop(); }
        }
        public void PlayLights() {
            if (null == Lights) return;
            foreach (var light in Lights) { if (null != light) light.enabled = true; }
        }
        public bool AreLightsPlaying() {
            if (null == Lights) return false;
            foreach (var light in Lights) { if (null != light && light.enabled) return true; }
            return false;
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
            Collider[] targets = Environment.GetObjectsInRadius(transform.position, radius);
            ApplyForce(transform, targets);
        }
        public void ApplyForce(Transform transform, Collider[] targets) {
            if (ForceVector == Vector3.zero) return; 
            ApplyForce(transform, GameObjectUtilities.GetGameObjects(targets));
        }
        public void ApplyForce(Transform transform, GameObject[] targets) {
            if (ForceVector == Vector3.zero) return; 
            ApplyForce(transform, GameObjectUtilities.GetRigidbodies(targets));
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
