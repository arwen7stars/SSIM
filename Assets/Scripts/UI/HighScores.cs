using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScores : MonoBehaviour
{

    // score objects
    public GameObject[] scoresObj;

    // no scores message
    public GameObject noScores;

    // show scores
    public void ShowScores()
    {
        List<int> scoresList = PlayerHighscore.LoadScores();

        if (scoresList.Count == 0)
        {
            noScores.SetActive(true);
        }
        else
        {
            for (int i = 0; i < scoresList.Count; i++)
            {
                scoresObj[i].SetActive(true);                           // sets score object to active
                Text scoreText = scoresObj[i].GetComponent<Text>();     // gets its text component

                int tmpNumber = i + 1;                                  // number being shown on scores table

                scoreText.text = tmpNumber + ". " + scoresList[i];      // change text
            }
        }
    }
}
