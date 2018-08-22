using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USComics_Entity;
using USComics_Dynamic;

namespace USComics_Combat {
	[System.Serializable]
	public class DamageDoTEntity : AbstractBuff {
		public DamageDoTEntityInfo dotInfo;

		private DynamicObjectManager DynamicObjectManagerScript;

		public DamageDoTEntity() { }
		public DamageDoTEntity(DamageDoTEntity buff) : base(buff) {
			dotInfo.damage = buff.dotInfo.damage;
			dotInfo.duration = buff.dotInfo.duration;
			dotInfo.damageType = buff.dotInfo.damageType;
			dotInfo.tickTime = buff.dotInfo.tickTime;
			dotInfo.lastTick = buff.dotInfo.lastTick;
			dotInfo.damageModel = buff.dotInfo.damageModel;
		}
		public DamageDoTEntity(DamageDoTEntityInfo buff) {
			dotInfo.damage = buff.damage;
			dotInfo.duration = buff.duration;
			dotInfo.damageType = buff.damageType;
			dotInfo.tickTime = buff.tickTime;
			dotInfo.lastTick = buff.lastTick;
			dotInfo.damageModel = buff.damageModel;
		}
		public override AbstractBuff Clone() { return new DamageDoTEntity(this); }
		public void FromAttack(Attack attack, EntityController attacker, EntityController target) {
			dotInfo.damage = attack.dotInfo.damage;
			dotInfo.duration = attack.dotInfo.duration;
			dotInfo.damageType = attack.dotInfo.damageType;
			dotInfo.tickTime = attack.dotInfo.tickTime;
			dotInfo.lastTick = attack.dotInfo.lastTick;
			dotInfo.damageModel = attack.dotInfo.damageModel;
			Attacker = attacker;
			Target = target;
		}
		public override Attack Buff(Attack attack) { return attack; }
		public override EntityController Buff(EntityController entity) {
			if (Expired) return entity;
			if ((0 != dotInfo.lastTick) && (Time.time - dotInfo.lastTick < dotInfo.tickTime)) return entity;
			entity.healthScript.health -= dotInfo.damage;
			SpawnPoints(entity);
			if (0 >= entity.healthScript.health) {
				entity.healthScript.health = 0;
				entity.DoDeath(Attacker);
			}
			if (0 == StartTime) StartTime = Time.time;
			dotInfo.lastTick = Time.time;
			if (dotInfo.lastTick - StartTime >= dotInfo.duration) Expired = true;
			return entity;
		}
		protected override bool SetupBuff() {
			base.SetupBuff();
			GameObject dynamicObjects = GameObject.FindWithTag("DynamicObjects") as GameObject;
			if (null != dynamicObjects) DynamicObjectManagerScript = dynamicObjects.GetComponent<DynamicObjectManager>();
			if (null == DynamicObjectManagerScript) { Debug.LogError("DamageDoTEntity.SetupBuff: DynamicObjectManagerScript is null."); }
			if (null == DynamicObjectManagerScript) { return false; }
			return true;
		}
		private void SpawnPoints(EntityController target)
		{
			if (null == dotInfo.damageModel) return;
			DynamicObjectManagerScript.Clone(dotInfo.damageModel, target.transform.position + (target.transform.up * 2), 0.0f, 180.0f, 0.0f);
		}
	}
}
