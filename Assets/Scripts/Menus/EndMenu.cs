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

    // int corresponding to start menu scene
    private const int START_SCENE = 0;

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

            if (secondGame == 0)        // 0 is false, ie first game
            {
                secondMessage.SetActive(false);
                firstMessage.SetActive(true);
            } else
            {
                firstMessage.SetActive(false);
                secondMessage.SetActive(true);
            }
        }
	}


    public void BackToMenu()
    {
        SceneManager.LoadScene(START_SCENE);
    }
}
