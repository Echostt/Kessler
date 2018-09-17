using UnityEngine;
using UnityEngine.UI;

public class SliderTextController : MonoBehaviour {
    //Updates the text when the slider is updated
    public void updatePhi () {
        GameObject.Find("inputPhi").GetComponent<InputField>().text = 
        GameObject.Find("SliderPhi").GetComponent<Slider>().value.ToString();
    }
    //Updates the slider when the text is updated
    public void updatePhiSliderPos () {
        GameObject.Find("SliderPhi").GetComponent<Slider>().value =
        float.Parse(GameObject.Find("inputPhi").GetComponent<InputField>().text);
    }

    public void updateTheta () {
        GameObject.Find("inputTheta").GetComponent<InputField>().text =
        GameObject.Find("SliderTheta").GetComponent<Slider>().value.ToString();
    }

    public void updateThetaSliderPos () {
        GameObject.Find("SliderTheta").GetComponent<Slider>().value =
        float.Parse(GameObject.Find("inputTheta").GetComponent<InputField>().text);
    }

    public void updateDist () {
        GameObject.Find("inputDist").GetComponent<InputField>().text =
        GameObject.Find("SliderDist").GetComponent<Slider>().value.ToString();
    }

    public void updatedistSliderPos () {
        GameObject.Find("SliderDist").GetComponent<Slider>().value =
        float.Parse(GameObject.Find("inputDist").GetComponent<InputField>().text);
    }
}
