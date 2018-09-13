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
		if (sats.Count > 0) {
			for (int i = 0; i < sats.Count; ++i) {
                sats[i].GetComponent<clsSatellite>().angleMove();
			}
		}
	}

	public void addSatGeneric(){
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
        float randStartPhi = Random.Range(-Mathf.PI, Mathf.PI);
        float randStartTheta = Random.Range(-Mathf.PI, Mathf.PI);
        float dist = 5 + (randOffset / 1000);
		float sinTheta = Mathf.Sin(randStartTheta);

		Vector3 startPos = new Vector3(
			dist * sinTheta * Mathf.Cos(randStartPhi),
			dist * sinTheta * Mathf.Sin(randStartPhi),
			dist * Mathf.Cos(randStartTheta));

		GameObject s = GameObject.Instantiate(satPrefabs[0], startPos, Quaternion.identity);
		//s.hideFlags = HideFlags.HideInHierarchy;
		clsSatellite c = s.GetComponent<clsSatellite>();
		//distance is from center of the earth
		//dist = radius + minimum distance + randomvalue scaled to be between 500-700km
		c.dist = dist;
		c.posPhi = randStartPhi;
        c.posTheta = randStartTheta;
        //c.posTheta = c.posPhi;

		c.phiRate = randPhiRate;
        //c.thetaRate = randThetaRate;
        c.thetaRate = c.phiRate * 2;
        c.angleRate = 0.005f;

        sats.Add(c);
        txtNumSats.GetComponent<Text>().text = "Sats: " + sats.Count;
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().addSat();
	}

    public IEnumerator createPersistentSats () {
        while (true) {
            addSatGeneric();
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
