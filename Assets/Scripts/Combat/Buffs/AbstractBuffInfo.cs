using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USComics_Entity;

namespace USComics_Combat
{
	[System.Serializable]
	public abstract class AbstractBuffInfo : ScriptableObject {
		public abstract AbstractBuff GetBuff();
	}
}
