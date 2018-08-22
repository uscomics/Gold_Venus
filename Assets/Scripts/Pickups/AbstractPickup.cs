using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USComics_Entity;
using USComics_Dynamic;

namespace USComics_Pickups {
	public abstract class AbstractPickup : MonoBehaviour {
		public AudioSource audioSource;
		public AudioClip sound;

		public abstract void ExecutePickup(EntityController target);
		protected void PlaySound() {
			if (null == audioSource) return;
			if (null == sound) return;
			audioSource.PlayOneShot(sound);
		}

		protected bool spawnModel(GameObject model, Vector3 position, float xAngle = 0.0f, float yAngle = 180.0f, float zAngle = 0.0f) {
			DynamicObjectManager DynamicObjectManagerScript;
			GameObject dynamicObjects = GameObject.FindWithTag("DynamicObjects") as GameObject;
			if (null != dynamicObjects) {
				DynamicObjectManagerScript = dynamicObjects.GetComponent<DynamicObjectManager>();
				if (null == DynamicObjectManagerScript) {
					Debug.LogError("DamageDoTEntity.SetupBuff: DynamicObjectManagerScript is null.");
					return false;
				} else {
					DynamicObjectManagerScript.Clone(model, position, xAngle, yAngle, zAngle);
				}
			}
			return true;
		}
	}
}
