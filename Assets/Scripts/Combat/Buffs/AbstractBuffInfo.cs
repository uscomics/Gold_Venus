using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace USComics_Combat {
	public abstract class AbstractBuffInfo : MonoBehaviour {
		void Start() { }
		void Update() { }
	    public abstract AbstractBuff GetBuff();	
	}
}	
