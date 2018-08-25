using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractFX : MonoBehaviour {

	public bool PlayOnStart;
	public bool PlayOnUpdate;
	public bool PlayOnCollision;
	public bool PlayOnTrigger;
	public Transform Transform;
	public float Radius;
	public float AngleX = 0.0f;
	public float AngleY = 180.0f;
	public float AngleZ = 0.0f;
	
	void Start () { if (PlayOnStart) StartCoroutine(Play()); }
	void Update () { if (PlayOnUpdate) StartCoroutine(Play()); }
	void OnCollisionEnter(Collision collision) { if (PlayOnCollision) StartCoroutine(Play()); }
	void OnTriggerEnter(Collider other) { if (PlayOnTrigger) StartCoroutine(Play()); }
	public abstract IEnumerator Play();
	public abstract bool IsPlaying();
	public abstract void Stop();
}
