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

    // play game
    public void PlayGame()
    {
        GameManager.SetGameMode();
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
