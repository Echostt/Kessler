using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public Text txtMoneyTotal;
    public Text txtMoneyOverTime;

    private int numSats1 = 0;
    private int sat1Val = 1;
    private int money = 0;
    private int moneyOverTime = 0;

	// Update is called once per frame
	void Update () {
        //only update once per second //////TD
        money += numSats1 * sat1Val;
        txtMoneyTotal.text = money.ToString();
        txtMoneyOverTime.text = (numSats1 * sat1Val).ToString();
	}

    public void addSat () {
        numSats1 += 1;
    }
}
