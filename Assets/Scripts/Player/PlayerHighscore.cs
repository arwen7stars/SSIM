using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerHighscore : MonoBehaviour
{

    // Highscores' list max size
    private const int SCORES_SIZE = 5;

    // key prefix for highscores
    private const string KEY_PREFIX = "highscore";

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Add score to highscores' list
    public static void AddScore(int score)
    {
        if (score > 0)
        {
            List<int> loadedScores = LoadScores();

            if (loadedScores.Count > 0)
            {
                loadedScores.Add(score);                    // add new score to list of loaded scores

                List<int> highScores = RemoveDuplicates(loadedScores);

                highScores.Sort();                        // sort scores
                highScores.Reverse();

                int listSize = SCORES_SIZE;
                if (highScores.Count <= SCORES_SIZE)        // no more than 5 high scores
                {
                    listSize = highScores.Count;
                }

                for (int i = 0; i < listSize; i++)
                {
                    PlayerPrefs.SetInt(KEY_PREFIX + i, highScores[i]);
                }
            }
            else
            {
                PlayerPrefs.SetInt(KEY_PREFIX + 0, score);
            }
        }
    }

    // Get scores' list
    public static List<int> LoadScores()
    {
        List<int> scores = new List<int>();

        for (int i = 0; i < SCORES_SIZE; i++)
        {
            if (PlayerPrefs.HasKey(KEY_PREFIX + i))
            {
                int currScore = PlayerPrefs.GetInt(KEY_PREFIX + i);
                scores.Add(currScore);
            }
        }

        return scores;
    }

    public static List<int> RemoveDuplicates(List<int> list)
    {
        List<int> unique = new List<int>();

        foreach (int item in list)
        {
            if (!unique.Any(x => x.Equals(item)))
            {
                unique.Add(item);
            }
        }

        return unique;
    }
}