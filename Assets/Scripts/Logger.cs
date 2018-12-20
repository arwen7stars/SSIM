using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Logger : MonoBehaviour {

    // the score canvas display
    public Text scoreText;

    // shots accuracy
    private List<float> shots = new List<float>();

    // accuracy mean
	private float mean = 0;

    // current score
    private int score = 0;

    public void LogShot(float acc) {

        // log
        shots.Add(acc);

		// update mean
		mean = (mean * (shots.Count - 1) + acc) / shots.Count;

		Debug.Log("Shots: " + shots.Count + ", Accuracy: " + mean);
	}

    public void IncreaseScore()
    {
        score++;
        scoreText.text = "Score: " + score;
    }
}
