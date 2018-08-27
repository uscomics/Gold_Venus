using System.Collections;
using UnityEngine;
using USComics_Dynamic;
using USComics_Movement;
using USComics_Environment;

namespace USComics_FX {
    public class FX : AbstractFX {
        public GameObject Parent;
        public string[] AnimationNames;     // If multiple animation names are given, one will be randomly selected every time.
        public Animator AnimationEntity;
        public AudioSource AudioSource1;    // Plays Sound1
        public AudioSource AudioSource2;    // Plays Sound2
        public AudioClip Sound1;
        public AudioClip Sound2;
        public ParticleSystem[] ParticleSystems;    // All particle systems are played
        public Light[] Lights;              // All lights are used.
        public GameObject[] Spawn;
        public Vector3 SpawnPosition;
        public float SpawnAngleX = 0.0f;
        public float SpawnAngleY = 180.0f;
        public float SpawnAngleZ = 0.0f;
        public bool SpawnFirst;             // true = PlayAll spawns objects first and are possible force targets, false = PlayAll spawns objects last
        public DynamicObjectManager SpawnDynamicObjectManagerScript;
        public Vector3 ForceVector = Vector3.zero;  // Vector3.zero means no force
        public ForceMode ForceMode = ForceMode.Impulse;
        public bool ForceFromCenter;        // Set to true for an explosion-like (radial) force, or false for a linear force.
        public Transform ForceTransform;
        public float ForceRadius;
 
        private Animations _animations;
        private Sound _sound1;
        private Sound _sound2;
        private Particles _particleSystems;
        private Lights _lights;
        private Spawn _spawn;
        private Force _force;

        void Start() {
            if (null == Parent) {
                Debug.LogError("FX.Start: Parent is null.");
                return;
            }
            if (null != AnimationNames && 0 < AnimationNames.Length) {
                _animations = Parent.AddComponent<Animations>();
                _animations.AnimationNames = AnimationNames;
                _animations.AnimationEntity = AnimationEntity;
            }
            if (null != AudioSource1) {
                _sound1 = Parent.AddComponent<Sound>();
                _sound1.AudioSource = AudioSource1;
                _sound1.Clip = Sound1;
            }
            if (null != AudioSource2) {
                _sound2 = Parent.AddComponent<Sound>();
                _sound2.AudioSource = AudioSource2;
                _sound2.Clip = Sound2;
            }
            if (null != ParticleSystems && 0 < ParticleSystems.Length) {
                _particleSystems = Parent.AddComponent<Particles>();
                _particleSystems.ParticleSystems = ParticleSystems;
            }
            if (null != Lights && 0 < Lights.Length) {
                _lights = Parent.AddComponent<Lights>();
                _lights.Light = Lights;
            }
            if (null != Spawn) {
                _spawn = Parent.AddComponent<Spawn>();
                _spawn.Models = Spawn;
                _spawn.Position = SpawnPosition;
                _spawn.DynamicObjectManagerScript = SpawnDynamicObjectManagerScript;
                _spawn.AngleX = SpawnAngleX;
                _spawn.AngleY = SpawnAngleY;
                _spawn.AngleZ = SpawnAngleZ;
            }
            if (null != ForceTransform) {
                _force = Parent.AddComponent<Force>();
                _force.ForceVector = this.ForceVector;
                _force.ForceMode = ForceMode;
                _force.ForceFromCenter = ForceFromCenter;
                _force.Vision.DetectionRadius = ForceRadius;
            }
            if (PlayOnStart) StartCoroutine(Play());
        }

        public override IEnumerator Play() {
            if (SpawnFirst) SpawnModels();
            PlaySounds();
            PlayAnimation();
            PlayParticleSystems();
            PlayLights();
            ApplyForce();
            if (!SpawnFirst) SpawnModels();
            yield break;
        }
        public override bool IsPlaying() {
            return IsAnimationPlaying() || IsSoundPlaying() || AreParticleSystemsPlaying() || AreLightsPlaying() || AreModelsSpawning() || IsForcePlaying();
        }
        public override void Stop() {
            StopAnimation();
            StopSounds();
            StopParticleSystems();
            StopLights();
            StopSpawn();
            StopForce();
        }
        public override AbstractFX CreateNew(GameObject parent) { return parent.AddComponent<FX>(); }
        public void PlayAnimation() { if (null != _animations) StartCoroutine(_animations.Play()); }
        public bool IsAnimationPlaying() { return null != _animations && _animations.IsPlaying(); }
        public void StopAnimation() { if (null != _animations) _animations.Stop(); }
        public void PlaySounds() {
            if (null != _sound1) StartCoroutine(_sound1.Play());
            if (null != _sound2) StartCoroutine(_sound2.Play());
        }
        public bool IsSoundPlaying() {
            if (null != _sound1 && _sound1.IsPlaying()) return true;
            if (null != _sound2 && _sound2.IsPlaying()) return true;
            return false;
        }
        public void StopSounds() {
            if (null != _sound1 && _sound1.IsPlaying()) _sound1.Stop();
            if (null != _sound2 && _sound2.IsPlaying()) _sound2.Stop();
        }
        public void PlayParticleSystems() { if (null != _particleSystems) StartCoroutine(_particleSystems.Play()); }
        public bool AreParticleSystemsPlaying() { return null != _particleSystems && _particleSystems.IsPlaying(); }
        public void StopParticleSystems() { if (null != _particleSystems) _particleSystems.Stop(); }
        public void PlayLights() { if (null != _lights) StartCoroutine(_lights.Play()); }
        public bool AreLightsPlaying() { return null != _lights && _lights.IsPlaying(); }
        public void StopLights() { if (null != _lights) _lights.Stop(); }
        public void SpawnModels()  { if (null != _spawn) StartCoroutine(_spawn.Play()); }
        public bool AreModelsSpawning() { return null != _spawn && _spawn.IsPlaying(); }
        public void StopSpawn() { if (null != _spawn) _spawn.Stop(); }
        public void ApplyForce()  { if (null != _force) StartCoroutine(_force.Play()); }
        public bool IsForcePlaying() { return null != _force && _force.IsPlaying(); }
        public void StopForce() { if (null != _force) _force.Stop(); }
    }
}
