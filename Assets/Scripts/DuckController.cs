using UnityEngine;
using System.Collections.Generic;

public class DuckController : MonoBehaviour {

    // movement speed
    public float speed;

	// rotation speed
	public float rotspeed;

	// rotation bound
	public float rotbound;

	// right bound
	public float rbound;

	// left bound
	public float lbound;

	// center of target (normalized text coords)
	public Vector2 targetCenter;

	// radius of the target (normalized text coods)
	public float radius;

	// true if duck wasnt hit this loop
	private bool alive;

	// initial rotation
	private Quaternion irotation;

    // water splash sound
    private AudioSource splashSound;

    // Use this for initialization
    void Start () {
		alive = true;
		irotation = transform.rotation;
        splashSound = GetComponent<AudioSource>();
	}
	

	// Update is called once per frame
	void Update () {

		// move in line
		transform.Translate(speed * Time.deltaTime, 0, 0, Space.World);

        // reset if out of vision
        if (speed > 0)
        {
            if (transform.localPosition.x > rbound)
            {
                alive = true;
                transform.localPosition = new Vector3(lbound, transform.localPosition.y, transform.localPosition.z);
                transform.rotation = irotation;
            }
        }
        else
        {
            if (transform.localPosition.x < lbound)
            {
                alive = true;
                transform.localPosition = new Vector3(rbound, transform.localPosition.y, transform.localPosition.z);
                transform.rotation = irotation;
            }
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
	* Called when this object is hit.
	*
	* @param target True if the object hit was the target, false if it was the duck.
	* @param pixelUV Normalized text coords of the hit point.
	*/
	public void Hit(bool target, Vector2 pixelUV) {
		alive = false;

		// accuracy baseline for a duck hit is 0.5
		float accuracy = 0.5f;

		// if target was hit, update its accuracy (up to 1)
		if (target) {
			accuracy += 0.5f * (1 - Vector2.Distance(pixelUV, targetCenter) / radius);
		}

        GameManager.instance.LogShot(accuracy);
        GameManager.instance.IncreaseScore();
	}
}
