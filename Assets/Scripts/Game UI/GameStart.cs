using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour {
    // find hammer and platform message
    public GameObject tutorial;

    // game start message
    public GameObject gameStartMsg;

    // time until tutorial shown
    private const float TUTORIAL_DELAY = 4.0f;

    // time until game start
    private const float START_DELAY = 2.0f;

    void Start()
    {
        StartCoroutine(ShowTutorial());
        StartCoroutine(StartGame());
    }

    IEnumerator ShowTutorial()
    {
        // show tutorial message
        tutorial.SetActive(true);
        yield return new WaitForSeconds(TUTORIAL_DELAY);
        tutorial.SetActive(false);
    }

    // show game start messages
    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(TUTORIAL_DELAY-START_DELAY);

        // show game start message
        gameStartMsg.SetActive(true);
        yield return new WaitForSeconds(START_DELAY);
        gameStartMsg.SetActive(false);

        // open curtains and start game
        CurtainsController.OpenCurtains();
    }

}
