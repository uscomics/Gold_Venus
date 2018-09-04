using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USComics_Entity;
using USComics_Dynamic;

namespace USComics_Combat {
	[System.Serializable]
	public class DamageDoTEntity : AbstractBuff {
		public bool isDoT;
		public float damage;
		public DamageType damageType;
		public float duration;
		public float tickTime;
		public float lastTick;
		public GameObject damageModel;

		public DamageDoTEntity() { }
		public DamageDoTEntity(DamageDoTEntity buff) : base(buff) {
			damage = buff.damage;
			duration = buff.duration;
			damageType = buff.damageType;
			tickTime = buff.tickTime;
			lastTick = buff.lastTick;
			damageModel = buff.damageModel;
		}
		public override AbstractBuff Clone() { return new DamageDoTEntity(this); }
		public void FromAttack(Attack attack, EntityController attacker, EntityController target) {
			damage = attack.AttackInfo.Damage.DamagePointsDoT;
			duration = attack.AttackInfo.Damage.DurationDoT;
			damageType = attack.AttackInfo.Damage.DamageType;
			tickTime = attack.AttackInfo.Damage.TickTimeDoT;
			lastTick = attack.AttackInfo.Damage.LastTickDoT;
			damageModel = attack.AttackInfo.Damage.PointsObjectDoT;
			Attacker = attacker;
			Target = target;
		}
		public override Attack Buff(Attack attack) { return attack; }
		public override EntityController Buff(EntityController entity) {
			if (Expired) return entity;
			if ((0 != lastTick) && (Time.time - lastTick < tickTime)) return entity;
			entity.HealthScript.HealthPoints -= damage;
			SpawnPoints(entity);
			if (0 >= entity.HealthScript.HealthPoints) {
				entity.HealthScript.HealthPoints = 0;
				entity.DoDeath(Attacker);
			}
			if (0 == StartTime) StartTime = Time.time;
			lastTick = Time.time;
			if (lastTick - StartTime >= duration) Expired = true;
			return entity;
		}
		private void SpawnPoints(EntityController target)
		{
			if (null == damageModel) return;
			DynamicObjectManager.INSTANCE.Clone(damageModel, target.transform.position + (target.transform.up * 2), 0.0f, 180.0f, 0.0f);
		}
	}
}
