using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    // static instance of GameManager which allows it to be accessed by any other script.
    public static GameManager instance = null;

    // the score canvas display
    public Text scoreText;

	// list of all target poles
	public List<GameObject> targetPoles;

	// list of all duck factories
	private List<DuckFactoryController> duckFactories;

	// percentage to increase speed per round (linearly)
	public float speedPercentIncrease;

	// percentage to increase scale per round (linearly)
	public float scalePercentIncrease;

	// discount factor for the scale increase
	public float scaleDiscFactor;

	// number of the rounds before discount factor is maxed
	public float disctFactorMaxRoundN;

	// goal for the player accuracy during adaptive gameplay
	public float adaptTargetAcc;

    // wait for new round
    public bool roundPause = false;

    // game stopped because of menu
    public bool gameStop = false;

    // timer over
    public bool timerOver = false;

    // game over
    public bool gameOver = false;

	// true if difficulty is adaptive, false if linear
	public bool adaptive;

    // list of shots' accuracy
    private List<float> shots = new List<float>();

	// list of shots' accuracy (average) per round
	private List<float> shotsPerRound = new List<float>();

	// list of ducks' lifespan
	private List<float> ducks = new List<float>();

	// list of ducks' lifespan (average) per round
	private List<float> ducksPerRound = new List<float>();

	// mouse total travelled distance
	private float mouseTotalTravelledDistance = 0;

	// mouse travelled distance (per round)
	private float mouseTravelledDistance = 0;

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

	void Start()
	{
		duckFactories = new List<DuckFactoryController>();
		foreach (GameObject pole in targetPoles)
		{
			duckFactories.Add(pole.GetComponent<DuckFactoryController>());
		}
	}

    public void LogShot(float acc, float lifespan)
    {
        if (acc >= 0) shots.Add(acc);
		if (lifespan >= 0) ducks.Add(lifespan);
    }

    // Increase score if red ducks are shot
    public void IncreaseScore(int increment)
    {
        // increase  score
        score += increment;

        // show score on Canvas
        scoreText.text = "Score: " + score;
    }

    public int GetScore()
    {
        return score;
    }

	public void LogMouse(float distance)
	{
		mouseTotalTravelledDistance += distance;
		mouseTravelledDistance += distance;
	}

	public void EndRound()
	{
		// pause the round
		roundPause = true;

		// calculate rounds accuracy and store it
		float shotsMean = 0;
		foreach (float acc in shots)
		{
			shotsMean += acc;
		}
		shotsMean = shotsMean / shots.Count;
		shotsPerRound.Add(shotsMean);

		float ducksMean = 0;
		foreach (float lifespan in ducks)
		{
			ducksMean += lifespan;		
		}
		ducksMean = ducksMean / ducks.Count;
		ducksPerRound.Add(ducksMean);

		Debug.Log("Round: #" + shotsPerRound.Count + ", Accuracy: " + (shotsMean * 100).ToString("F2") +
		"%, Reaction Time: " + ducksMean.ToString("F2") + "s, Mouse Travelled Distance: " + mouseTravelledDistance + "px.");

		// prepare next ite
		shots.Clear();
		ducks.Clear();
		mouseTravelledDistance = 0;
		UpdateDifficulty(shotsMean, ducksMean);
	}


	/**
	* Updates the game difficulty given the round stats.
	*
	* @param roundAccuracy Average accuracy of the previous round.
	* @param roundReactionTime Average reaction time of the previous round.
	*/
	void UpdateDifficulty(float roundAccuracy, float roundReactionTime) {

		// TODO speed as well
		if (adaptive) {

			// discount factor isnt fully applied in the first N rounds
			float discFactorPercent;
			int nround = shotsPerRound.Count - 1;
			if (nround < disctFactorMaxRoundN)
			{
				discFactorPercent = nround / disctFactorMaxRoundN; 
			} else {
				discFactorPercent = 1.0f;
			}

			scalePercentIncrease = (1 - scaleDiscFactor * discFactorPercent) * (1 - roundAccuracy / adaptTargetAcc);
			Debug.Log("Accuracy: " + roundAccuracy + ", Scale increase: " + (scalePercentIncrease * 100).ToString("F2") + "%");
		}
		
		foreach (DuckFactoryController factory in duckFactories)
		{
			if (adaptive)
			{
				factory.UpdateScaleAndSpeedBy(scalePercentIncrease, 0);
			}
			else
			{
				factory.UpdateScaleAndSpeedLinearlyBy(scalePercentIncrease, speedPercentIncrease);
			}
		}
	}
}
