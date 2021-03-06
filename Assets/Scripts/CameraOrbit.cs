﻿using UnityEngine;
using System.Collections;
 
public class CameraOrbit : MonoBehaviour {
	[Range(4.0f,10.0f) ] public float RotateAmount = 8f;
	public Transform rotationPoint;
	private Vector3 startPos;
	private Quaternion startRot;
	
	void Start(){
		startPos = GetComponent<Transform>().position;
		startRot = GetComponent<Transform>().rotation;
	}

	// when rendering camera always use late update.
	void LateUpdate() {
		if (Input.GetMouseButton(1)) {
			OrbitCamera();
		}
		else if (Input.GetKeyDown(KeyCode.C) || Input.GetMouseButton(2)){
			GetDefaultCameraView();	
		}
	}

	
	public void OrbitCamera(){
		Vector3 target = rotationPoint.transform.position ;
		float x_rotate = Input.GetAxis("Mouse X") * RotateAmount;
		OrbitCamera(target, x_rotate,0.0f);
	}
	
	
	//returns the camera to the initial view.
	public void GetDefaultCameraView(){
			gameObject.transform.position = startPos;
			gameObject.transform.rotation = startRot;
	}
	
	public void OrbitCamera(Vector3 target, float y_rotate, float x_rotate) {
		transform.RotateAround(target, Vector3.up, y_rotate);
		transform.LookAt(target);
	}
}