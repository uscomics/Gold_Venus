using System.Collections;
using UnityEngine;
using USComics_Entity;

namespace USComics_Combat {
	[System.Serializable]
	public class OutOfCombatHeal : AbstractBuff {
		public float TimeSinceLastAttack = 15.0f;
		public int HealAmount = 5;
		public float TimeBetweenHealTicks = 1f;

		protected class HealHelper : MonoBehaviour {
			public void Heal(OutOfCombatHeal healInfo, EntityController entity) {
				StartCoroutine(HealCoroutine(healInfo, entity));
			}
			public IEnumerator HealCoroutine(OutOfCombatHeal healInfo, EntityController entity) {
				while (true) {
					if ((Time.time > healInfo.TimeSinceLastAttack)
					&& (entity.GetHealth() < entity.GetMaxHealth())
					&& (Time.time - entity.GetLastTimeAttacked() > healInfo.TimeSinceLastAttack)) {
						entity.AddHealth(healInfo.HealAmount);
					}
					yield return new WaitForSeconds(healInfo.TimeBetweenHealTicks);
				}
			}
		}

		public OutOfCombatHeal() { }
		public OutOfCombatHeal(OutOfCombatHeal buff) : base(buff) {
			TimeSinceLastAttack = buff.TimeSinceLastAttack;
			HealAmount = buff.HealAmount;
			TimeBetweenHealTicks = buff.TimeBetweenHealTicks;
		}
		public override AbstractBuff Clone() { return new OutOfCombatHeal(this); }
		public override Attack Buff(Attack attack) { return attack; }
		public override EntityController Buff(EntityController entity) {
			if (0.0f == StartTime) {
				StartTime = Time.time;
				HealHelper healHelper = Target.Entity.AddComponent<HealHelper>();
				healHelper.Heal(this, Target);
			}
			return entity;
		}
	}
}
