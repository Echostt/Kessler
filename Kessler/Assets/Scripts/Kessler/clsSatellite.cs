using UnityEngine;

public class clsSatellite : MonoBehaviour {
	public float posTheta = 0,  posPhi = 0,
				thetaRate = 0,  phiRate = 0, 
				dist = 0, angleRate = 0;
	public int collisionsRemaining;
    private const float cPI = Mathf.PI;

	//move object based on fixed angular rate around a sphere
	public void angleMove(){
        //reduction in angle movement to elim overflow after long idle
        if (posPhi >= 2 * cPI)
            posPhi -= 2 * cPI;
        else if (posPhi <= -2 * cPI)
            posPhi += 2 * cPI;
        if (posTheta >= 2 * cPI)
            posTheta -= 2 * cPI;
        else if (posTheta <= -2 * cPI)
            posTheta += 2 * cPI;

        // Add offsets to posPhi and posTheta for diff arrangements //TD ////////////////////
        this.posPhi += (angleRate * phiRate);
        this.posTheta += (angleRate * thetaRate);
		float sinTheta = Mathf.Sin(posTheta);
		this.gameObject.transform.position = (new Vector3(
			dist * sinTheta * Mathf.Cos(posPhi),
			dist * sinTheta * Mathf.Sin(posPhi),
			dist * Mathf.Cos(posTheta))
		);
        
        //this.gameObject.transform.RotateAround(Vector3.zero, Vector3.up, 100 * Time.deltaTime);
	}
		
    /*
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
			//GameObject.FindWithTag("SatelliteController").GetComponent<SatManager>().createDebris(this.gameObject);
			this.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
			//stop collided objects from moving
			angleRate = 0;
		}
	}
    */
}
