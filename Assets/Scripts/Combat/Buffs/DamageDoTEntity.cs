using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USComics_Entity;
using USComics_Dynamic;

namespace USComics_Combat {
	[System.Serializable]
	public class DamageDoTEntity : AbstractBuff {
		public bool IsDoT;
		public float Damage;
		public DamageType DamageType;
		public float Duration;
		public float TickTime;
		public float LastTick;
		public GameObject DamageModel;

		public DamageDoTEntity() { }
		public DamageDoTEntity(DamageDoTEntity buff) : base(buff) {
			Damage = buff.Damage;
			Duration = buff.Duration;
			DamageType = buff.DamageType;
			TickTime = buff.TickTime;
			LastTick = buff.LastTick;
			DamageModel = buff.DamageModel;
		}
		public override AbstractBuff Clone() { return new DamageDoTEntity(this); }
		public void FromAttack(Attack attack, EntityController attacker, EntityController target) {
			Damage = attack.AttackInfo.Damage.DamagePointsDoT;
			Duration = attack.AttackInfo.Damage.DurationDoT;
			DamageType = attack.AttackInfo.Damage.DamageType;
			TickTime = attack.AttackInfo.Damage.TickTimeDoT;
			LastTick = attack.AttackInfo.Damage.LastTickDoT;
			DamageModel = attack.AttackInfo.Damage.PointsObjectDoT;
			Attacker = attacker;
			Target = target;
		}
		public override Attack Buff(Attack attack) { return attack; }
		public override EntityController Buff(EntityController entity) {
			if (Expired) return entity;
			if ((0 != LastTick) && (Time.time - LastTick < TickTime)) return entity;
			entity.HealthScript.HealthPoints -= Damage;
			SpawnPoints(entity);
			if (0 >= entity.HealthScript.HealthPoints) {
				entity.HealthScript.HealthPoints = 0;
				entity.DoDeath(Attacker);
			}
			if (0 == StartTime) StartTime = Time.time;
			LastTick = Time.time;
			if (LastTick - StartTime >= Duration) Expired = true;
			return entity;
		}
		private void SpawnPoints(EntityController target) {
			if (null == DamageModel) return;
			DynamicObjectManager.INSTANCE.Clone(DamageModel, target.transform.position + (target.transform.up * 2), 0.0f, 180.0f, 0.0f);
		}
	}
}
