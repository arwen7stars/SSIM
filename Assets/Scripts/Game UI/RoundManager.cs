using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour
{

    // ducks failed text UI
    public Text duckFailTxt;

    // game object with failed ducks message
    public GameObject duckFail;

    // game object with ducks hit message
    public GameObject duckHit;

    // time showing messages
    private const float SHOWING_MSG = 2.0f;

    // already showing message or not
    private bool showing = false;

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.roundPause) {
            showing = false;
            return;
        }

        if (!showing)
        {
            showing = true;
            int missedDucks = GameManager.instance.GetMissedDucks();
            GameManager.instance.SetMissedDucks(0);

            StartCoroutine(ShowMessage(missedDucks));
        }
    }

    IEnumerator ShowMessage(int missedDucks)
    {
        if (missedDucks > 0)
        {
            // show ducks failed message on Canvas
            duckFailTxt.text = "You didn't hit " + missedDucks + " evil ducks :(";

            duckFail.SetActive(true);

            yield return new WaitForSeconds(SHOWING_MSG);

            duckFail.SetActive(false);
        }
        else
        {
            duckHit.SetActive(true);

            yield return new WaitForSeconds(SHOWING_MSG);

            duckHit.SetActive(false);
        }

    }
}
