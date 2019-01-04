using UnityEngine;
using System.Collections.Generic;

public class DuckController : MonoBehaviour {

    // movement speed
    private float speed;

	// rotation speed
	public float rotspeed;

	// rotation bound
	public float rotbound;

	// x bound
	private float xbound;

	// center of target (normalized text coords)
	public Vector2 targetCenter;

	// radius of the target (normalized text coods)
	public float radius;

	// chance of going red (per second) [0, 100]
	public float redchance;

	// true if duck wasnt hit this loop
	private bool alive;

	// initial rotation
	private Quaternion irotation;

    // water splash sound
    private AudioSource splashSound;

    // duck color
    private Color color;

    // Use this for initialization
    void Start () {
		alive = true;
		irotation = transform.rotation;

        // Get splash sound effect from scene
        splashSound = GameObject.Find("Targets").GetComponent<AudioSource>();

        // Find duck renderer
        Renderer rend = transform.Find("duck").GetComponent<MeshRenderer>();

        int rand = Random.Range(0, 100);
		if (rand < redchance) {
            rend.material.color = Color.red;
		}

        // Set duck color
        color = rend.material.color;
	}
	

	// Update is called once per frame
	void Update () {

		// move in line
		transform.Translate(speed * Time.deltaTime, 0, 0, Space.World);

		// kill self
		if (speed < 0 && transform.localPosition.x <= xbound || speed > 0 && transform.localPosition.x >= xbound) {
			Destroy(gameObject);
		}

        // all ducks die switching rounds! :D
        if (GameManager.instance.roundPause)
        {
            alive = false;
        }

		// dying animation
		if (!alive && transform.eulerAngles.z != rotbound) {
			Quaternion from = transform.rotation;
			Quaternion to = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, rotbound);
			float step = rotspeed * Time.deltaTime;

			transform.rotation = Quaternion.RotateTowards(from, to, step);

            splashSound.Play();
        }		
	}

	
	/**
	* Initialize this object
	*/
	public void Init(float speed, float xbound) {
		this.speed = speed;
		this.xbound = xbound;
		if (speed < 0) {
			transform.Find("duck").Rotate(0, 180, 0);
		}
	}


	/**
	* Called when this object is hit.
	*
	* @param target True if the object hit was the target, false if it was the duck.
	* @param pixelUV Normalized text coords of the hit point.
	*/
	public void Hit(bool target, Vector2 pixelUV) {
        // duck is no longer alive
		alive = false;

        if (color != Color.red)
        {
            // If duck hit isn't red, it doesn't count
            GameManager.instance.LogShot(0);
        }
        else
        {
            // accuracy baseline for a duck hit is 0.5
            float accuracy = 0.5f;

            // if target was hit, update its accuracy (up to 1)
            if (target)
            {
                accuracy += 0.5f * (1 - Vector2.Distance(pixelUV, targetCenter) / radius);
            }

            GameManager.instance.LogShot(accuracy);

            // score increment varies depending on accuracy
            int increment = (int)(accuracy * 10) - 4;

            // increase score only if duck is red
            GameManager.instance.IncreaseScore(increment);
        }
        
    }
}
