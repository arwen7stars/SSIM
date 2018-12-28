using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    // static instance of GameManager which allows it to be accessed by any other script.
    public static GameManager instance = null;

    // the score canvas display
    public Text scoreText;

    // wait for new round
    public bool gamePause = false;

    // list of shots' accuracy
    private List<float> shots = new List<float>();

    // accuracy mean
    private float mean = 0;

    // current score
    private int score = 0;

    // Awake is always called before any Start functions
    void Awake()
    {
        // check if instance already exists
        if (instance == null)
        {
            // if not, set instance to this
            instance = this;
        }
        // if instance already exists and it's not this:
        else if (instance != this)
        {
            // then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }
    }

    public void LogShot(float acc)
    {
        // add new shot accuracy
        shots.Add(acc);

        // update mean
        mean = (mean * (shots.Count - 1) + acc) / shots.Count;

        Debug.Log("Shots: " + shots.Count + ", Accuracy: " + mean);
    }

    // Increase score if red ducks are shot
    public void IncreaseScore()
    {
        // increase  score
        score++;

        // show score on Canvas
        scoreText.text = "Score: " + score;
    }
}
