using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace USComics_Combat {
	[System.Serializable]
	public class Damage {
		public float DamagePoints;
		public GameObject PointsObject;
		public DamageType DamageType;
		public bool IsAoE;
		public float RadiusAoE;
		public bool IsDoT;
		public float DamagePointsDoT;
		public DamageType DamageTypeDoT;
		public float DurationDoT;
		public float TickTimeDoT;
		public float LastTickDoT;
		public GameObject PointsObjectDoT;

		public Damage() { }
		public Damage(Damage from) {
			DamagePoints = from.DamagePoints;
			PointsObject = from.PointsObject;
			DamageType = from.DamageType;
			IsAoE = from.IsAoE;
			RadiusAoE = from.RadiusAoE;
			IsDoT = from.IsDoT;
			DamagePointsDoT = from.DamagePointsDoT;
			DamageTypeDoT = from.DamageTypeDoT;
			DurationDoT = from.DurationDoT;
			TickTimeDoT = from.TickTimeDoT;
			LastTickDoT = from.LastTickDoT;
			PointsObjectDoT = from.PointsObjectDoT;
		}
	}
}
