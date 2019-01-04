using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurtainsController : MonoBehaviour {

	// speed at which the curtains scale
	public float speed;
	
	// how much the curtains are allowed to scale
	public float limit;

	// left side curtain
	private Transform left;

	// right side curtain
	private Transform right;

	// total amount courtains have scaled so far
	private float total;

	// true if animation is playing, false otherwise
	private bool animating;

    // true if curtains are closing, false otherwise
    private bool closing;

	// the instance
	static CurtainsController self;

	
	void Start () {
		total = 0;
		left = transform.Find("left wing");
		right = transform.Find("right wing");
		animating = false;
		self = this;
	}

	
	void Update () {
		if (animating && total < limit) {
			float delta = speed * Time.deltaTime;
			total += Mathf.Abs(delta);
			left.transform.localScale += new Vector3(delta, 0, 0);
			right.transform.localScale -= new Vector3(delta, 0, 0);
		} else {
            // if curtains were opening until they reached the limit...
            if (animating && !closing)
            {
                // ducks start spawning again
                GameManager.instance.roundPause = false;
            }

			animating = false;
		}		
	}


	// Open the curtains
	public static void OpenCurtains() {
		if (self == null) return;

        self.closing = false;
		self.animating = true;
		self.total = 0;
		self.speed = self.speed > 0 ? -1 * self.speed : self.speed;
	}


	// Close the curtains
	public static void CloseCurtains() {
		if (self == null) return;

        self.closing = true;
		self.animating = true;
		self.total = 0;
		self.speed = self.speed < 0 ? -1 * self.speed : self.speed;

        // curtains close and player reloads gun while game is on pause
        GameManager.instance.EndRound();
    }
}
