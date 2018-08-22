using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USComics_Entity;

namespace USComics_Combat
{
	[System.Serializable]
	public class DamageDoTEntityInfo : AbstractBuffInfo {
		public bool isDoT;
		public float damage;
		public DamageType damageType;
		public float duration;
		public float tickTime;
		public float lastTick;
		public GameObject damageModel;

		public override AbstractBuff GetBuff() { return new DamageDoTEntity(this); }
	}
}
