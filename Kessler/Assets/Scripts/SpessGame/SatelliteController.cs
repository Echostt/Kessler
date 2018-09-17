using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SatelliteController : MonoBehaviour {
    public List<GameObject> satPrefabs;
    public GameObject txtNumSats;
    private bool isCreatingSats = false;

	private List<clsSatellite> sats = new List<clsSatellite>();

	private void Update(){
        GameObject.FindGameObjectWithTag("FPSText").GetComponent<Text>().text = " FPS: " + (1.0f / Time.smoothDeltaTime);
        if (sats.Count > 0) {
			for (int i = 0; i < sats.Count; ++i) {
                sats[i].GetComponent<clsSatellite>().angleMove();
			}
		}
	}

    /// <summary>
    /// Creates a randomly positioned and angled satellite in stable orbit.
    /// </summary>
	public void addSatGen(int satPos){
		float randOffset = Random.Range(0, 2000);

		int randPhiRate = Random.Range(1, 3);
		int randThetaRate = Random.Range(1, 3);
		//throw in some chaotic motion
		int randDirection = Random.Range(0, 20);
		if(randDirection > 10) {
			randPhiRate *= -1;
		}
		if(randDirection % 2 == 0) {
			randThetaRate *= -1;
		}
        //chaotic starting positions
        float randStartPhi = Random.Range(-Mathf.PI, Mathf.PI);
        float randStartTheta = Random.Range(-Mathf.PI, Mathf.PI);
        float dist = 5.5f + (randOffset / 1000);
		float sinTheta = Mathf.Sin(randStartTheta);

		Vector3 startPos = new Vector3(
			dist * sinTheta * Mathf.Cos(randStartPhi),
			dist * sinTheta * Mathf.Sin(randStartPhi),
			dist * Mathf.Cos(randStartTheta));

		GameObject s = GameObject.Instantiate(satPrefabs[satPos - 1], startPos, Quaternion.identity);
		//s.hideFlags = HideFlags.HideInHierarchy;
		clsSatellite c = s.GetComponent<clsSatellite>();
		//distance is from center of the earth

		c.dist = dist;
		c.posPhi = randStartPhi;
        c.posTheta = randStartTheta;
        //c.posTheta = c.posPhi;

		c.phiRate = randPhiRate;
        //c.thetaRate = randThetaRate;
        c.thetaRate = c.phiRate * 2;
        c.angleRate = 0.0005f;

        sats.Add(c);
        txtNumSats.GetComponent<Text>().text = "Sats: " + sats.Count;
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().addSat();
	}

    public void addSatSpecific(int satPos) {

        float phiRate = float.Parse(GameObject.Find("inputPhi").GetComponent<InputField>().text);
        float thetaRate = float.Parse(GameObject.Find("inputTheta").GetComponent<InputField>().text);
        float dist = float.Parse(GameObject.Find("inputDist").GetComponent<InputField>().text);

        //chaotic starting positions
        /*
        float randStartPhi = Random.Range(-Mathf.PI, Mathf.PI);
        float randStartTheta = Random.Range(-Mathf.PI, Mathf.PI);
        */
        float randStartPhi = 0;
        float randStartTheta = 0;
        float sinTheta = Mathf.Sin(randStartTheta);

        Vector3 startPos = new Vector3(
            dist * sinTheta * Mathf.Cos(randStartPhi),
            dist * sinTheta * Mathf.Sin(randStartPhi),
            dist * Mathf.Cos(randStartTheta));

        GameObject s = GameObject.Instantiate(satPrefabs[satPos - 1], startPos, Quaternion.identity);
        //s.hideFlags = HideFlags.HideInHierarchy;
        clsSatellite c = s.GetComponent<clsSatellite>();
        //distance is from center of the earth

        c.dist = dist;
        c.posPhi = randStartPhi;
        c.posTheta = randStartTheta;
        //c.posTheta = c.posPhi;

        c.phiRate = phiRate;
        c.thetaRate = thetaRate;
        //c.angleRate = 0.0005f;
        c.angleRate = 0.005f;

        sats.Add(c);
        txtNumSats.GetComponent<Text>().text = "Sats: " + sats.Count;
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().addSat();
    }

    public void addSat1 () {
        //addSatGen(1);
        addSatSpecific(1);
    }
    public void addSat2 () {
        //addSatGen(2);
        addSatSpecific(2);
    }

    public IEnumerator createPersistentSats () {
        while (true) {
            addSatSpecific(1);
            yield return new WaitForSeconds(0.3f);
        }
    }

    public void startPersistentSatsCreation () {
        if (!isCreatingSats) {
            isCreatingSats = true;
            StartCoroutine("createPersistentSats");
        } else {
            isCreatingSats = false;
            StopCoroutine("createPersistentSats");
        }
    }

}
