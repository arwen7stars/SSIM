using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour {

    // adaptive link
    public GameObject adaptiveLink;

    // linear link
    public GameObject linearLink;

    // first message
    public GameObject firstMessage;

    // second message
    public GameObject secondMessage;

    // end menu btn text
    public Text btnTxt;

    // int corresponding to scene to go back to
    private int backScene = 0;

    // Use this for initialization
    void Awake () {
		if (PlayerPrefs.HasKey(GameManager.GAME_MODE))
        {
            int gameMode = PlayerPrefs.GetInt(GameManager.GAME_MODE);

            if (gameMode == 0)      // 0 is linear mode
            {
                adaptiveLink.SetActive(false);
                linearLink.SetActive(true);
            } else
            {
                linearLink.SetActive(false);
                adaptiveLink.SetActive(true);
            }
        }

        if (PlayerPrefs.HasKey(GameManager.SECOND_GAME))
        {
            int secondGame = PlayerPrefs.GetInt(GameManager.SECOND_GAME);

            if (secondGame == 0)        // 0 is false, ie this is first game player played
            {
                secondMessage.SetActive(false);
                firstMessage.SetActive(true);
                btnTxt.text = "PLAY AGAIN";

                // will play the game after clicking on button
                backScene = 1;
            } else
            {
                firstMessage.SetActive(false);
                secondMessage.SetActive(true);
                btnTxt.text = "BACK";

                // will go back to main menu
                backScene = 0;
            }
        }
	}

    public void OpenAdaptiveURL()
    {
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLScowbzmMvS71oPN12jQi90phShIpEoPyKllXpf0JeQyza5TIg/viewform");
    }

    public void OpenLinearURL()
    {
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSfD_2BSpXdPfZtYkvz4aS9Jf6uAdmgGZKIqYUIqznMTCG_wDg/viewform");
    }


    public void BackToMenu()
    {
        // if next scene to load is the game
        if (backScene == 1)
        {
            GameManager.SetGameMode();
        }

        SceneManager.LoadScene(backScene);
    }
}
