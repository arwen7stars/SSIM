using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour {

    // timer in seconds
    public const int TIMER_SECONDS = 240;

    // timer text UI
    public Text textTimer;

    // how much time left in the timer
    private int timeLeft;

    // timer aux
    private float tmpTime;

    // minutes in string
    private string minutes;

    // seconds in string
    private string seconds;

    // Use this for initialization
    void Start () {
        timeLeft = TIMER_SECONDS;
        tmpTime = TIMER_SECONDS;
    }
	
	// Update is called once per frame
	void Update () {
        // ignore timer if game is switching between rounds
        if (GameManager.instance.gamePause) return;

        if (timeLeft > 0)
        {
            convertTime();
            textTimer.text = minutes + ":" + seconds;
        }

    }

    // Convert timeLeft to minutes and seconds
    private void convertTime()
    {
        tmpTime -= Time.deltaTime;
        timeLeft = (int)tmpTime;

        int minutesInt = timeLeft / 60;
        int secondsInt = timeLeft - minutesInt * 60;

        if (minutesInt < 10) { minutes = "0" + minutesInt; }
        else { minutes = minutesInt.ToString(); }

        if (secondsInt < 10) { seconds = "0" + secondsInt; }
        else { seconds = secondsInt.ToString(); }
    }
}
