﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clsSatellite : MonoBehaviour {
	public float posTheta = Mathf.PI, posPhi = Mathf.PI,
				thetaRate = 0,  phiRate = 0, 
				dist = 0, angleRate = 0;

	//move object based on fixed angular rate around a sphere
	public void angleMove(){
		this.posPhi += angleRate * phiRate;
		this.posTheta += angleRate * thetaRate;
		float sinTheta = Mathf.Sin(posTheta);
		this.gameObject.transform.SetPositionAndRotation(new Vector3(
			dist * sinTheta * Mathf.Cos(posPhi),
			dist * sinTheta * Mathf.Sin(posPhi),
			dist * Mathf.Cos(posTheta)),
			Quaternion.identity
		);
	}
		
	public void OnTriggerEnter(Collider col){
		if(col.gameObject.tag == "Debris") {
			//set collided objects to bright red as detection
			this.GetComponent<Renderer>().material.color = new Vector4(0, 0, 1.0f, 0);
			this.transform.localScale = new Vector3(4, 4, 4);
			//remove collision detection 
			this.GetComponent<Rigidbody>().detectCollisions = false;
			//pass current gameobject to create debris from position/orbit
			GameObject.FindWithTag("GameController").GetComponent<SatManager>().createDebris(this.gameObject);
			//stop collided objects from moving
			angleRate = 0;
		} else {
			//set collided objects to bright red as detection
			this.GetComponent<Renderer>().material.color = new Vector4(1.0f, 0, 0, 0);
			this.transform.localScale = new Vector3(4, 4, 4);
			//remove collision detection 
			this.GetComponent<Rigidbody>().detectCollisions = false;
			//pass current gameobject to create debris from position/orbit
			GameObject.FindWithTag("GameController").GetComponent<SatManager>().createDebris(this.gameObject);
			//stop collided objects from moving
			angleRate = 0;
		}
	}

	public void Update(){
		angleMove();
	}

}
