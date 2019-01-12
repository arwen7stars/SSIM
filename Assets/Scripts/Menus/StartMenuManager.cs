using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{

    // default menu
    public GameObject defaultMenu;

    // high scores screen
    public GameObject highScoresMenu;

    // get high scores
    public HighScores highScores;

    // int corresponding to level scene
    private const int LEVEL_SCENE = 1;

    // choose game mode
    public void SetGameMode()
    {
        int game_mode = -1;

        if (PlayerPrefs.HasKey(GameManager.GAME_MODE))
        {
            int prevGameMode = PlayerPrefs.GetInt(GameManager.GAME_MODE);

            if (prevGameMode == 0) game_mode = 1;
            else game_mode = 0;

            PlayerPrefs.SetInt(GameManager.GAME_MODE, game_mode);
        } else
        {
            game_mode = Random.Range(0, 2);
            PlayerPrefs.SetInt(GameManager.GAME_MODE, game_mode);   // 0 is linear, 1 is adaptive
        }

        Debug.Log(game_mode);
    }

    // play game
    public void PlayGame()
    {
        SetGameMode();
        SceneManager.LoadScene(LEVEL_SCENE);
    }

    // show high scores
    public void HighScores()
    {
        defaultMenu.SetActive(false);
        highScoresMenu.SetActive(true);

        highScores.ShowScores();
    }

    // back to main menu from high scores
    public void BackToMenuFromHighscores()
    {
        defaultMenu.SetActive(true);
        highScoresMenu.SetActive(false);
    }

    // exit game
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
