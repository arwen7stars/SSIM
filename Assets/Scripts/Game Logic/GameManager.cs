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

	// index of round to maximize discount factor (minimum 1)
	public float disctFactorMaxRoundI;

	// goal for the player accuracy during adaptive gameplay
	public float adaptTargetAcc;

	// goal for the player reaction speed during adaptive gameplay
	public float adaptReactSpeed;

    // wait for new round
    public bool roundPause = false;

    // game stopped because of menu
    public bool gameStop = false;

    // game start
    public bool gameStart = false;

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

	// number of missed ducks
	private int missedDucks = 0;

	// mouse total travelled distance
	private float mouseTotalTravelledDistance = 0;

	// mouse travelled distance (per round)
	private float mouseTravelledDistance = 0;

    // current score
    private int score = 0;

    // key prefix for gamemode
    public const string GAME_MODE = "game_mode";

    // key for checking if this is the second game player has played the game
    public const string SECOND_GAME = "second_game";

    // Awake is always called before any Start functions
    void Awake()
    {
        LoadGameMode();

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

    public static void UpdateGameTracker()
    {
        if (PlayerPrefs.HasKey(GameManager.SECOND_GAME))
        {
            PlayerPrefs.SetInt(GameManager.SECOND_GAME, 1);         // 1 is true (second game)
        }
        else PlayerPrefs.SetInt(GameManager.SECOND_GAME, 0);        // 0 is false (first game)
    }

    // choose game mode
    public static void SetGameMode()
    {
        int game_mode = -1;

        if (!PlayerPrefs.HasKey(GameManager.SECOND_GAME))           // if this is the first game
        {
            game_mode = Random.Range(0, 2);
            PlayerPrefs.SetInt(GameManager.GAME_MODE, game_mode);   // 0 is linear, 1 is adaptive
        }
    }

    // update game mode at the end of the game and after filling form
    public static void UpdateGameMode()
    {
        int game_mode = -1;

        // update game mode
        if (PlayerPrefs.HasKey(GameManager.GAME_MODE))
        {
            int prevGameMode = PlayerPrefs.GetInt(GameManager.GAME_MODE);

            if (prevGameMode == 0) game_mode = 1;
            else game_mode = 0;

            PlayerPrefs.SetInt(GameManager.GAME_MODE, game_mode);
        }
    }

    // load game mode
    public void LoadGameMode()
    {
        int gameMode = 0;

        if (PlayerPrefs.HasKey(GAME_MODE))
            gameMode = PlayerPrefs.GetInt(GAME_MODE);

        if (gameMode == 0)      // 0 is linear mode
        {
            adaptive = false;
        } else
        {
            adaptive = true;
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
		if (acc < 0 && lifespan > 0) missedDucks++;
    } 

    // Increase score if red ducks are shot
    public void IncreaseScore(int increment)
    {
        // increase  score
        score += increment;

        if (score < 0) score = 0;

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

		Debug.Log("Round: #" + shotsPerRound.Count + ", Missed Ducks: " + missedDucks + ", Accuracy: " + (shotsMean * 100).ToString("F2") +
		"%, Reaction Time: " + ducksMean.ToString("F2") + "s, Mouse Travelled Distance: " + mouseTravelledDistance + "px.");

		// prepare next ite
		UpdateDifficulty(shotsMean);
		shots.Clear();
		ducks.Clear();
		mouseTravelledDistance = 0;
	}


	/**
	* Updates the game difficulty given the round stats.
	*
	* @param roundAccuracy Average accuracy of the previous round.
	*/
	void UpdateDifficulty(float roundAccuracy) {

		// TODO speed as well
		if (adaptive) {

			// discount factor isnt fully applied in the first N rounds
			float discFactorPercent;
			int iround = shotsPerRound.Count - 1;
			if (iround < disctFactorMaxRoundI)
			{
				discFactorPercent = iround / disctFactorMaxRoundI; 
			} else {
				discFactorPercent = 1.0f;
			}

			scalePercentIncrease = (1 - scaleDiscFactor * discFactorPercent) * (1 - roundAccuracy / adaptTargetAcc);

			// if user missed duck more than 1 duck, go back to previous speed stage
			if ((missedDucks > 1 && speedPercentIncrease > 0) ||
				(missedDucks < 2 && speedPercentIncrease < 0)) {
				speedPercentIncrease = -1 * speedPercentIncrease;
			}

			Debug.Log("Accuracy: " + roundAccuracy + ", Scale increase: " + (scalePercentIncrease * 100).ToString("F2") +
			"%, Speed increase: " + (speedPercentIncrease * 100).ToString("F2") + "%");
		} else
        {
            int iround = shotsPerRound.Count;

            if (iround > 2 && iround < 10)
            {
                speedPercentIncrease = speedPercentIncrease / 1.5f;
            } else if (iround > 10)
            {
                speedPercentIncrease = speedPercentIncrease / 2;
            }
        }
		
		foreach (DuckFactoryController factory in duckFactories)
		{
			factory.UpdateScaleAndSpeedBy(scalePercentIncrease, speedPercentIncrease);
		}
	}

    public int GetMissedDucks()
    {
        return missedDucks;
    }

    public void SetMissedDucks(int noDucks)
    {
        this.missedDucks = noDucks;
    }
}
