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
	public float angleStep = Mathf.PI / 16384; //2 ^ 14(16384)  * 2
	public int satLimit;
	private List<List<GameObject>> satMaster = new List<List<GameObject>>();

	private int currSatSet = 0;

	private Text txtNumSats, txtNumDebris;
	//private Random rand;
	private int numDebris = 0;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < 10; i++){
			satMaster.Add(new List<GameObject>());
		}
		fillInitSats();
		txtNumSats = GameObject.Find("TextNumSats").GetComponent<Text>();
		txtNumDebris = GameObject.Find("TextNumDebris").GetComponent<Text>();
	}

	void Update () {
		for (int j = 0; j < satMaster[currSatSet % satMaster.Count].Count; ++j){
			satMaster[currSatSet % satMaster.Count][j].GetComponent<clsSatellite>().angleMove();
		}
		currSatSet += 1;
		txtNumSats.text = "Num Sats: " + numSats + " FPS: " + (1.0f/Time.smoothDeltaTime);
	}

	void fillInitSats(){
		for (int i = 0; i < satLimit; ++i){
			addSat();
		}
	}
		

	//adds a new satellite in orbit at a random position travelling in a random direction
	void addSat(){
		float randOffset = Random.Range(0, 3000);

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
		//s.hideFlags = HideFlags.HideInHierarchy;
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
		satMaster[numSats % satMaster.Count].Add(s);
	}

	//takes in a gameobject that has recently collided, and uses it to generate debris
	public void createDebris(GameObject sat){
		int randDebrisCount = Random.Range(2, 10);
		if(sat.tag != "Debris")
			numSats -= 1;
		for(int i = 0; i < randDebrisCount; ++i) {
			if(sat.gameObject.tag == "Debris") {
				createDebrisSat(new Vector4(1.0f, 0.5f, 1.0f, 0), sat); // pink
			} else {
				createDebrisSat(new Vector4(1.0f, 0.5f, 0, 0), sat); // orange
			}
			txtNumDebris.text = "Debris: " + numDebris;
		}
	}

	void createDebrisSat(Vector4 passColor, GameObject sat){
		clsSatellite clsSat = sat.GetComponent<clsSatellite>();

		float sinTheta = Mathf.Sin(clsSat.posTheta);
		int randSteps = Random.Range(3, 30);

		Vector3 startNewPos = new Vector3(
			clsSat.dist * (sinTheta + (clsSat.thetaRate * angleStep * randSteps)) * Mathf.Cos(clsSat.posPhi + (clsSat.phiRate * angleStep * randSteps)),
			clsSat.dist * (sinTheta + (clsSat.thetaRate * angleStep * randSteps)) * Mathf.Sin(clsSat.posPhi + (clsSat.phiRate * angleStep * randSteps)),
			clsSat.dist * Mathf.Cos(clsSat.posTheta + (clsSat.thetaRate * angleStep * randSteps))
		);
		GameObject s = GameObject.Instantiate(sat, startNewPos, Quaternion.identity);
		s.hideFlags = HideFlags.HideInHierarchy;
		s.transform.localScale = new Vector3(2, 2, 2);
		s.GetComponent<SphereCollider>().radius /= 2;
		s.tag = "Debris";
		clsSatellite sClsSat = s.GetComponent<clsSatellite>();
		float randOffset = Random.Range(0, 3000);
		sClsSat.dist = earthRadius + 5 + (randOffset / 1000);
		int randThetaOffset = Random.Range(5, 75);
		int randPhiOffset = Random.Range(5, 75);
		sClsSat.angleRate = angleStep;
		sClsSat.posPhi += angleStep * 2 * randPhiOffset;
		sClsSat.posTheta += angleStep * randThetaOffset;
		s.GetComponent<Renderer>().material.color = passColor; //orange
		numDebris++;
		satMaster[currSatSet % satMaster.Count].Add(s);
		s.GetComponent<Rigidbody>().detectCollisions = true;
	}
}
