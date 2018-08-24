using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace USComics_Camera {
	public class CameraManager : MonoBehaviour {
		private com.ootii.Cameras.CameraController cameraController;
		
		void Start() {			
			GameObject mainCharacter = GameObject.FindWithTag("MainCamera") as GameObject;
			if (null != mainCharacter) cameraController = mainCharacter.GetComponent<com.ootii.Cameras.CameraController>();
			if (null == cameraController) { Debug.LogError("CameraController.Start: cameraController is null."); }
			if (null == cameraController) { return; }
		}
		void Update() { }
	}
}