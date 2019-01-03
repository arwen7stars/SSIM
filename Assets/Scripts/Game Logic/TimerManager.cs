using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerManager : MonoBehaviour {

    // timer in seconds
    public const int TIMER_SECONDS = 180;

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

    // if score has been saved or not
    private bool savedScore = false;

    // game object with timer over message
    public GameObject timerOverMsg;

    // time showing message
    private const float SHOWING_MSG = 3.0f;

    // int corresponding to start menu scene
    private const int START_MENU_SCENE = 0;

    // Use this for initialization
    void Start () {
        timeLeft = TIMER_SECONDS;
        tmpTime = TIMER_SECONDS;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (timeLeft > 0)
        {
            // ignore timer if game is switching between rounds
            if (GameManager.instance.roundPause) return;

            ConvertTime();
            textTimer.text = minutes + ":" + seconds;
        } else
        {
            GameManager.instance.timerOver = true;

            if (GameManager.instance.roundPause) {
                if (!savedScore)
                {
                    GameManager.instance.gameOver = true;

                    // game over
                    int finalScore = GameManager.instance.GetScore();
                    PlayerHighscore.AddScore(finalScore);

                    savedScore = true;

                    StartCoroutine(ShowTimerOverMsg());
                }
            }
        }

    }

    // Show timer over message
    IEnumerator ShowTimerOverMsg()
    {

        timerOverMsg.SetActive(true);

        yield return new WaitForSeconds(SHOWING_MSG);

        timerOverMsg.SetActive(false);

        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        SceneManager.LoadScene(START_MENU_SCENE);
    }

    // Convert timeLeft to minutes and seconds
    private void ConvertTime()
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
