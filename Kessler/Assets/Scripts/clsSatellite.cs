using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clsSatellite : MonoBehaviour {
	public float posTheta = Mathf.PI, posPhi = Mathf.PI,
				thetaRate = 0,  phiRate = 0, 
				dist = 0, angleRate = 0;
	public int collisionsRemaining;

	//move object based on fixed angular rate around a sphere
	public void angleMove(){
		this.posPhi += (angleRate * phiRate);
		this.posTheta += (angleRate * thetaRate);
		float sinTheta = Mathf.Sin(posTheta);
		this.gameObject.transform.SetPositionAndRotation(new Vector3(
			dist * sinTheta * Mathf.Cos(posPhi),
			dist * sinTheta * Mathf.Sin(posPhi),
			dist * Mathf.Cos(posTheta)),
			Quaternion.identity
		);
	}
		
	public void OnTriggerEnter(Collider col){
		this.collisionsRemaining -= 1;
		if(this.collisionsRemaining > 0) {
			if(col.gameObject.tag == "Debris") {
				this.GetComponent<Renderer>().material.color = new Vector4(0, 0, 1.0f, 0); //blue
			} else {
				this.GetComponent<Renderer>().material.color = new Vector4(1.0f, 0, 0, 0); //red
			}
			//remove collision detection 
			this.GetComponent<Rigidbody>().detectCollisions = false;
			//pass current gameobject to create debris from position/orbit
			GameObject.FindWithTag("GameController").GetComponent<SatManager>().createDebris(this.gameObject);
			this.transform.localScale = new Vector3(4, 4, 4);
			//stop collided objects from moving
			angleRate = 0;
		}
	}

}
