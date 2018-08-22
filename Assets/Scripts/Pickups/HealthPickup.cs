using UnityEngine;
using USComics_Entity;

namespace USComics_Pickups {
	public class HealthPickup : AbstractPickup {
		public float health = 10.0f;
		public GameObject pointsModel;

		public override void ExecutePickup(EntityController target) {
			target.AddHealth(health);
			if (null != pointsModel)  spawnModel(pointsModel, target.transform.position + (target.transform.up * 2));
			PlaySound();
		}
	}
}
