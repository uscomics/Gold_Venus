using USComics_Entity;

namespace USComics_Pickups {
	public class ExtraLifePickup : AbstractPickup {
		public override void ExecutePickup(EntityController target) {
			target.AddLife();
			PlaySound();
		}
	}
}
