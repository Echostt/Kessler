using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
	public int cameraSpeed;
	private Vector3 vecDirection;
	void Update(){
		if(Input.GetKey(KeyCode.UpArrow))
			vecDirection = Vector3.left;
		if(Input.GetKey(KeyCode.RightArrow))
			vecDirection = Vector3.down;
		if(Input.GetKey(KeyCode.DownArrow))
			vecDirection = Vector3.right;
		if(Input.GetKey(KeyCode.LeftArrow))
			vecDirection = Vector3.up;
		if(Input.GetKey(KeyCode.Space))
			vecDirection = Vector3.zero;
		transform.RotateAround(Vector3.zero, vecDirection, cameraSpeed * Time.deltaTime);
	}
}
