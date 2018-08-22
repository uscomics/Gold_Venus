using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USComics_Entity;
using USComics_Dynamic;

namespace USComics_Combat {
	[System.Serializable]
	public class DamageDoTEntity : AbstractBuff {
		public float damageDoT;
		public float durationDoT;
		public DamageType damageTypeDoT;
		public float tickTimeDoT;
		public float lastUsedDoT;
		public GameObject damageModelDoT;

		private DynamicObjectManager DynamicObjectManagerScript;

		public DamageDoTEntity() { }
		public DamageDoTEntity(DamageDoTEntity buff) : base(buff) {
			damageDoT = buff.damageDoT;
			durationDoT = buff.durationDoT;
			damageTypeDoT = buff.damageTypeDoT;
			tickTimeDoT = buff.tickTimeDoT;
			lastUsedDoT = buff.lastUsedDoT;
			damageModelDoT = buff.damageModelDoT;
		}
		public override AbstractBuff Clone() { return new DamageDoTEntity(this); }
		public void FromAttack(Attack attack, EntityController attacker, EntityController target) {
			damageDoT = attack.damageDoT;
			durationDoT = attack.durationDoT;
			damageTypeDoT = attack.damageTypeDoT;
			tickTimeDoT = attack.tickTimeDoT;
			lastUsedDoT = attack.lastUsedDoT;
			damageModelDoT = attack.damageModelDoT;
			Attacker = attacker;
			Target = target;
		}
		public override Attack Buff(Attack attack) { return attack; }
		public override EntityController Buff(EntityController entity) {
			if (Expired) return entity;
			if ((0 != lastUsedDoT) && (Time.time - lastUsedDoT < tickTimeDoT)) return entity;
			Debug.Log("DoT");
			entity.healthScript.health -= damageDoT;
			SpawnPoints(entity);
			if (0 >= entity.healthScript.health) {
				entity.healthScript.health = 0;
				entity.DoDeath(Attacker);
			}
			if (0 == StartTime) StartTime = Time.time;
			lastUsedDoT = Time.time;
			if (lastUsedDoT - StartTime >= durationDoT) Expired = true;
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
			if (null == damageModelDoT) return;
			DynamicObjectManagerScript.Clone(damageModelDoT, target.transform.position + (target.transform.up * 2), 0.0f, 180.0f, 0.0f);
		}
	}
}
