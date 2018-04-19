using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//vPython script scaled down x100000
//6350000 -> 63.5
public class SatManager : MonoBehaviour {
	public int numSats = 0;
	public GameObject satPrefab;
	public float earthRadius = 63.5f;
	public float angleStep = Mathf.PI / 16384; //2 ^ 14 
	public int satLimit;

	private Text txtNumSats, txtNumDebris;
	private Random rand;
	private int numDebris = 0;

	// Use this for initialization
	void Start () {
		//sats = new List<GameObject>();
		rand = new Random();
		fillInitSats();
		txtNumSats = GameObject.Find("TextNumSats").GetComponent<Text>();
		txtNumDebris = GameObject.Find("TextNumDebris").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		//angleMoveHandler();
		if(numSats < satLimit) {
			addSat();
		}
		txtNumSats.text = "Num Sats: " + numSats + " FPS: " + (1.0f/Time.smoothDeltaTime);
	}

	void fillInitSats(){
		for (int i = 0; i < satLimit; ++i){
			addSat();
		}
	}
		

	//adds a new satellite in orbit at a random position travelling in a random direction
	void addSat(){
		float randOffset = Random.Range(0, 2000);

		int randPhiRate = Random.Range(2, 6);
		int randThetaRate = Random.Range(2, 6);
		//throw in some chaotic motion
		int randDirection = Random.Range(0, 20);
		if(randDirection > 10) {
			randPhiRate *= -1;
		}
		if(randDirection % 2 == 0) {
			randThetaRate *= -1;
		}
		//chaotic starting positions
		int randStartPhi = Random.Range(1, 16364);
		int randStartTheta = Random.Range(1, 16384);
		float dist = earthRadius + 5 + (randOffset / 1000);
		float sinTheta = Mathf.Sin(randStartTheta);

		Vector3 startPos = new Vector3(
			dist * sinTheta * Mathf.Cos(randStartPhi),
			dist * sinTheta * Mathf.Sin(randStartPhi),
			dist * Mathf.Cos(randStartTheta));

		GameObject s = GameObject.Instantiate(satPrefab, startPos, Quaternion.identity);
		s.hideFlags = HideFlags.HideInHierarchy;
		clsSatellite c = s.GetComponent<clsSatellite>();
		//distance is from center of the earth
		//dist = radius + minimum distance + randomvalue scaled to be between 500-700km
		c.dist = dist;
		c.posPhi = randStartPhi;
		c.posTheta = randStartTheta;

		c.phiRate = randPhiRate;
		c.thetaRate = randThetaRate;
		c.angleRate = angleStep;
		numSats++;
	}

	//takes in a gameobject that has recently collided, and uses it to generate debris
	public void createDebris(GameObject sat){
		float colliderRad = sat.GetComponent<SphereCollider>().radius;
		int numDebrisSpawned = Random.Range(2, 8);
		clsSatellite clsSat = sat.GetComponent<clsSatellite>();

		float sinTheta = Mathf.Sin(clsSat.posTheta);

		Vector3 startPos = new Vector3(
			clsSat.dist * sinTheta * Mathf.Cos(clsSat.posPhi),
			clsSat.dist * sinTheta * Mathf.Sin(clsSat.posPhi),
			clsSat.dist * Mathf.Cos(clsSat.posTheta));

		Vector3 startNewPos = new Vector3(
			clsSat.dist * (sinTheta + (clsSat.thetaRate * angleStep * 4)) * Mathf.Cos(clsSat.posPhi + (clsSat.phiRate * angleStep * 4)),
			clsSat.dist * (sinTheta + (clsSat.thetaRate * angleStep * 4)) * Mathf.Sin(clsSat.posPhi + (clsSat.phiRate * angleStep * 4)),
			clsSat.dist * Mathf.Cos(clsSat.posTheta + (clsSat.thetaRate * angleStep * 4))
		);
			
		for(int i = 0; i < numDebrisSpawned; ++i) {
			GameObject s = GameObject.Instantiate(sat, startNewPos, Quaternion.identity);
			clsSatellite sClsSat = s.GetComponent<clsSatellite>();
			int randThetaOffset = Random.Range(3, 10);
			int randPhiOffset = Random.Range(3, 10);
			sClsSat.angleRate = angleStep;
			sClsSat.posPhi += angleStep * 2 * randPhiOffset;
			sClsSat.posTheta += angleStep * randThetaOffset;
			sClsSat.delayInitCollision();
			s.GetComponent<Renderer>().material.color = new Vector4(1.0f, 0.5f, 0, 0);
			numDebris++;
			Debug.Log("NEW DEBRIS AT: Phi: " + sClsSat.posPhi + " Theta: " + sClsSat.posTheta);
		}
		txtNumDebris.text = "Debris: " + numDebris;
	}
}
