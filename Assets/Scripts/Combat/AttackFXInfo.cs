using UnityEngine;
using USComics_FX;
using USComics_Dynamic;

namespace USComics_Combat {
	[System.Serializable]
	public class AttackFXInfo {
		public GameObject Parent;
		public string[] AnimationNames; // If multiple animation names are given, one will be randomly selected every time.
		public Animator AnimationEntity;
		public AudioSource AudioSource1; // Plays Sound1
		public AudioSource AudioSource2; // Plays Sound2
		public AudioClip Sound1;
		public AudioClip Sound2;
		public ParticleSystem[] ParticleSystems; // All particle systems are played
		public Light[] Lights; // All lights are used.
		public GameObject[] Spawn;
		public Vector3 SpawnPosition;
		public float SpawnAngleX = 0.0f;
		public float SpawnAngleY = 180.0f;
		public float SpawnAngleZ = 0.0f;
		public bool SpawnFirst; // true = PlayAll spawns objects first and are possible force targets, false = PlayAll spawns objects last
		public DynamicObjectManager SpawnDynamicObjectManagerScript;
		public Vector3 ForceVector = Vector3.zero; // Vector3.zero means no force
		public ForceMode ForceMode = ForceMode.Impulse;
		public bool ForceFromCenter; // Set to true for an explosion-like (radial) force, or false for a linear force.
		public Transform ForceTransform;
		public float ForceRadius;

		public void CopyTo(FX fx) {
			fx.Parent = Parent;
			fx.AnimationNames = AnimationNames;
			fx.AnimationEntity = AnimationEntity;
			fx.AudioSource1 = AudioSource1;
			fx.AudioSource2 = AudioSource2;
			fx.Sound1 = Sound1;
			fx.Sound2 = Sound2;
			fx.ParticleSystems = ParticleSystems;
			fx.Spawn = Spawn;
			fx.SpawnPosition = SpawnPosition;
			fx.SpawnAngleX = SpawnAngleX;
			fx.SpawnAngleY = SpawnAngleY;
			fx.SpawnAngleZ = SpawnAngleZ;
			fx.SpawnFirst = SpawnFirst;
			fx.SpawnDynamicObjectManagerScript = SpawnDynamicObjectManagerScript;
			fx.ForceVector = ForceVector;
			fx.ForceMode = ForceMode;
			fx.ForceFromCenter = ForceFromCenter;
			fx.ForceTransform = ForceTransform;
			fx.ForceRadius = ForceRadius;
		}
	}
}
